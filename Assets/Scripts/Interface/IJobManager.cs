using System.Collections;

public interface IJobManager : IManager, IUpdate
{
    public void SetJobState(JobPriority priority, bool state);

    public void AddJob(JobPriority priority, IEnumerator enumerator);
}
