using Sirenix.Serialization;

public class AutoTargetEffect
{
    [OdinSerialize] public TargetMode TargetMode { get; private set; }
    [OdinSerialize] public Effect Effect { get; private set; }
}