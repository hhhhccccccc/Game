using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = System.Random;

[CreateAssetMenu(menuName = "Config/Map")]
public class MapConfig : SerializedScriptableObject
{
    [SerializeField] public MapNode mapNode;
    [SerializeField] public Vector2Int layerCount;
    [SerializeField] public int repetitionCount;
    [SerializeField] private List<RoomType> roomTypes = new();
    
    public MapNode GetRandomMapNodeType(Random random)
    {
        int index = random.Next(0, roomTypes.Count);
        MapNode node = Instantiate(mapNode).GetComponent<MapNode>();
        node.SetMapNodeType(roomTypes[index]);
        return node;
    }
}