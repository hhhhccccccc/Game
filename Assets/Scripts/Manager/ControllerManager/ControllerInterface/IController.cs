
public interface IController<in T> where T : MessageModel
{
    void Handle(T msg);
}
