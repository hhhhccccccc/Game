public enum PanelLayerType
{
    BackGround,
    MidGround,
    ForeGround,
    Top,
    Pop,
}

public enum JobPriority
{
    Low = 1,
    Mid = 2,
    High = 3
}

public enum BattleObjType
{
    Role = 0
}

public enum BattleState
{
    None = 0,
    RoundStart = 1,
    PreDoDesition = 2,//预先行动
    ForceDoDesition = 3,//强制预先行动
    ActionWheelLogicCalculate = 4,//行动息逻辑计算
}

public enum InputType
{
    Keyboard = 0,
    Mouse = 1,
}

public enum BattleBehaviourType
{
    Jump = 0,
    Skill = 1
}

public enum BattleClickType
{
    Entity = 0,
    Skill = 1,
    Cancel = 2
}

public enum ClickKeyCodeType
{
    None = 0,
    KeyDown = 1,
    KeyOn = 2,
    KeyUp = 3
}