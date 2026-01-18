using System;

public class RandomSystem : Singleton<RandomSystem>
{
    private Random random;

    public Random GetRandomGenerator(ModuleType moduleType)
    {
        int seed = (int)DateTime.Now.Ticks + (int)moduleType;
        random = new Random(seed);
        return random;
    }
}

public enum ModuleType
{
    Map
}