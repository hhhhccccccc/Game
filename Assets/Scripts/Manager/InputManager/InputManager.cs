using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class InputManager : ManagerBase, IInitRootAfter, IUpdate
{
    [Inject] private IMessageManager MessageManager;
    [Inject] private IPoolManager PoolManager;
    [Inject] private ILogManager LogManager;
    private bool BattleInputValid;

    protected override IEnumerator OnInit()
    {
        BattleInputValid = false;
        yield break;
    }
    public void SetBattleInputValid(bool value) => BattleInputValid = value;

    private void BattleInputListen()
    {
        if (Input.GetMouseButtonDown(0)) //检测鼠标左键点击
        { 
            MessageManager.DispatchMsg<MouseClickEventModel>(null);
        }

        if (Input.anyKey)
        {
            for (int key = 97; key <= 122; key++)
            {
                var keyCode = (KeyCode)key;
                if (Input.GetKeyDown(keyCode))
                {
                    var model = PoolManager.GetClass<KeyCodeClickEventModel>();
                    model.KeyCode = keyCode;
                    model.ClickType = ClickKeyCodeType.KeyDown;
                    MessageManager.DispatchMsg(model);
                    PoolManager.RecycleClass(model);
                }
                
                if (Input.GetKey(keyCode))
                {
                    var model = PoolManager.GetClass<KeyCodeClickEventModel>();
                    model.KeyCode = keyCode;
                    model.ClickType = ClickKeyCodeType.KeyOn;
                    MessageManager.DispatchMsg(model);
                    PoolManager.RecycleClass(model);
                }
                
                if (Input.GetKeyUp(keyCode))
                {
                    var model = PoolManager.GetClass<KeyCodeClickEventModel>();
                    model.KeyCode = keyCode;
                    model.ClickType = ClickKeyCodeType.KeyUp;
                    MessageManager.DispatchMsg(model);
                    PoolManager.RecycleClass(model);
                }
            }
        }
    }

    public void OnUpdate(float dt)
    {
        if (BattleInputValid)
            BattleInputListen();
    }
}