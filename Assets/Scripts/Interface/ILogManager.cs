
using System;

public interface ILogManager : IManager
{ 
    void Debug(string msg);
    void Error(string msg);
    void Error(Exception e);
}
