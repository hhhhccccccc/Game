using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobManager : ManagerBase, IJobManager
{
    private Dictionary<int, CoroutineJob> Jobs;

    protected override IEnumerator OnInit()
    {
        Jobs = new();
        CreateCoroutineJob(JobPriority.Low, 1, 1);
        CreateCoroutineJob(JobPriority.Mid, 1, 3);
        CreateCoroutineJob(JobPriority.High, 1, 5);
        yield break;
    }

    public void SetJobState(JobPriority priority, bool state)
    {
        Jobs[(int)priority].IsPaused = state;
    }

    public void AddJob(JobPriority priority, IEnumerator enumerator)
    {
        Jobs[(int)priority].AddJob(enumerator);
    }
    
    #region 任务数据

    private void CreateCoroutineJob(JobPriority priority, int frameInterval, int doCount)
    {
        Jobs.Add((int)priority, new CoroutineJob
        {
            Priority = priority,
            FrameInterval = frameInterval, 
            CurrFrame = 0,
            DoCount = doCount, 
            Tasks = new Queue<IEnumerator>(),
            IsPaused = false,
            RunningCoroutine = null
        });
    }
    
    public void OnUpdate(float dt)
    {
        ProcessJob(JobPriority.High);
        ProcessJob(JobPriority.Mid);
        ProcessJob(JobPriority.Low);
    }
    
    private void ProcessJob(JobPriority priority)
    {
        Jobs[(int)priority].DoJob();
    }

    #endregion
    
    #region 单个任务管理

    class CoroutineJob 
    {
        public JobPriority Priority;
        public int FrameInterval;
        public int DoCount;
        public Queue<IEnumerator> Tasks;
        public bool IsPaused;
        private Stack<IEnumerator> ParentCoroutine = new();
        public IEnumerator RunningCoroutine; // 当前运行的协程
        public int CurrFrame;
        private float WaitTime;
        public void DoJob()
        {
            if (IsPaused)
                return;
            CurrFrame++;
            if (CurrFrame % FrameInterval == 0)
            {
                if (Tasks.Count <= 0 && RunningCoroutine == null)
                    return;
                //Debug.Log($"第 -- {CurrFrame} -- 帧 ");
                for (int i = 0; i < DoCount; i++)
                {
                    if (WaitTime > 0)
                    {
                        WaitTime -= Time.deltaTime;
                        continue;
                    }
                    
                    if (RunningCoroutine == null)
                    {
                        if (ParentCoroutine.Count > 0)
                        {
                            RunningCoroutine = ParentCoroutine.Pop();
                        }
                        else if (Tasks.Count > 0)
                        {
                            RunningCoroutine = Tasks.Dequeue();
                        }
                    }

                    MoveNext();
                }
            }
        }

        private void MoveNext()
        {
            if (!RunningCoroutine.MoveNext())
            {
                if (ParentCoroutine.Count > 0)
                {
                    RunningCoroutine = ParentCoroutine.Pop();
                }
                else
                {
                    RunningCoroutine = null;
                }
            }
            else if (RunningCoroutine.Current is IEnumerator current)
            {
                ParentCoroutine.Push(RunningCoroutine);
                RunningCoroutine = current;
            }
            else if (RunningCoroutine.Current is WaitTimeModel time)
            {
                WaitTime = time.Time;
            }
        }

        public void AddJob(IEnumerator enumerator)
        {
            Tasks.Enqueue(enumerator);
        }
    }

    #endregion
}