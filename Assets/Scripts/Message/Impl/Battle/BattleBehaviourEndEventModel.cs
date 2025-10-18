using System.Collections.Generic;

public class BattleBehaviourEndEventModel : MessageModel
{
    public List<int> BattleBehaviourIDList { get; set; } = new List<int>();

    public override void Recycle()
    {
        base.Recycle();
        BattleBehaviourIDList.Clear();
    }
}
