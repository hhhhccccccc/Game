public class PlayerSys : SingleArchiveModel
{
    public int Guid { get; set; }
    public override void Init()
    {
        base.Init();
        if (Guid == 0)
        {
            Guid = System.Guid.NewGuid().GetHashCode();
        }
    }
}
