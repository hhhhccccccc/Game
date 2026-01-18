using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapNode : MonoBehaviour
{
    public int layerIndex;
    public int nodeIndex;
    public List<int> nextLayerConnectedNodes = new();
    public List<int> previousLayerConnectedNodes = new();
    public Image image;
    public RoomType NodeType;
    public bool isActive;
    private float maxConnectedNodesProbability = 0.1f;

    public void SetMapNodeType(RoomType roomType)
    {
        NodeType = roomType;
    }

    public void SetMapNodeIndex(int layerIndex, int nodeIndex)
    {
        this.layerIndex = layerIndex;
        this.nodeIndex = nodeIndex;
    }
}

public enum RoomType
{
    None,
    Enemy,
    Elite,
    Shop,
    Treasure,
    Event,
    Rest,
    Boss
}