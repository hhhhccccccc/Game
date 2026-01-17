public class MapSystem : Singleton<MapSystem>
{
    public MapGenerate mapGenerate;
    public MapNode[,] mapNodes;

    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        mapGenerate.GenerateMap();
    }

    public void SetMapData(MapNode[,] mapNodeArray)
    {
        mapNodes = mapNodeArray;
    }
}