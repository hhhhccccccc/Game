using cfg;

public class WeatherChangedEventModel : MessageModel
{
    public int OldWeatherID;
    public WeatherType OldWeatherType;
    public string OldWeatherDes;
    public int OldWeatherFilter;
    
    public int NewWeatherID;
    public WeatherType NewWeatherType;
    public string NewWeatherDes;
    public int NewWeatherFilter;

    public override void Recycle()
    {
        base.Recycle();
        OldWeatherID = 0;
        OldWeatherType = 0;
        OldWeatherDes = string.Empty;
        OldWeatherFilter = 0;
        NewWeatherID = 0;
        NewWeatherType = 0;
        NewWeatherDes = string.Empty;
        NewWeatherFilter = 0;
    }
}
