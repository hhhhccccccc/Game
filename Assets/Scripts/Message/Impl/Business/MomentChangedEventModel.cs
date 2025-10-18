public class MomentChangedEventModel : MessageModel
{
    public int Moment;

    public override void Recycle()
    {
        base.Recycle();
        Moment = 0;
    }
}
