using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class MapGenerate : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private GameObject nodePrefab;
    [SerializeField]
    private GameObject linePrefab;
    [SerializeField]
    private RectTransform mapNodeParentRect;
    [SerializeField]
    private Vector2Int nodeSize;
    [SerializeField]
    private Vector2Int nodeOffset;
    [SerializeField]
    private int nodeAnglesOffset;
    /// <summary>
    ///     最顶部和最底部的空间
    /// </summary>
    [SerializeField]
    private Vector2Int padding;

    [Header("Map Node Database")]
    [SerializeField]
    private MapConfig mapConfig;

    private Vector2Int layerCount;
    /// <summary>
    ///     行：第几层
    ///     列：第几个房间
    /// </summary>
    private MapNode[,] mapNodeArray;

    private Random mapRandom;

    private void Initialize()
    {
        //从种子管理器获取随机数生成器
        mapRandom = RandomSystem.Instance.GetRandomGenerator(ModuleType.Map);

        layerCount = mapConfig.layerCount;
        mapNodeArray = new MapNode[layerCount.x, layerCount.y];
    }

    public void GenerateMap()
    {
        // 初始化地图生成器
        Initialize();

        SetContentView();
        // 创建地图房间
        CreateMap();

        // 随机生成路线
        GenerateRouteLoop();

        // 删除未相连的房间
        DeleteInactiveNode();

        // 设置房间连接线可视化
        SetNodeLineVisual();

        MapSystem.Instance.SetMapData(mapNodeArray);
    }

    #region 设置地图视图
    private void SetContentView()
    {
        Vector2 size = mapNodeParentRect.sizeDelta;
        size.x = nodeSize.x * (layerCount.x - 1) + padding.x * 2;
        size.y += padding.y * 2;
        mapNodeParentRect.sizeDelta = size;
    }
    
    private void CreateMap()
    {
        int offsetX = padding.x;

        for (int i = 0; i < mapNodeArray.GetLength(0); i++)
        {
            int offsetY = -((layerCount.y - 1) * nodeSize.y) / 2;
            for (int j = 0; j < mapNodeArray.GetLength(1); j++)
            {
                MapNode node = Instantiate(nodePrefab, mapNodeParentRect).GetComponent<MapNode>();
                node.transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-nodeAnglesOffset, nodeAnglesOffset));
                mapNodeArray[i, j] = node;

                node.SetMapNodeIndex(i, j);

                int roomOffsetY = offsetY + UnityEngine.Random.Range(-nodeOffset.x, nodeOffset.x);
                float roomOffsetX;

                if (i != 0 && i != layerCount.x - 1)
                {
                    roomOffsetX = offsetX + UnityEngine.Random.Range(-nodeOffset.y, nodeOffset.y);
                }
                else
                {
                    roomOffsetX = offsetX;
                }

                node.transform.localPosition = new Vector3(roomOffsetX, roomOffsetY);
                node.gameObject.SetActive(true);

                offsetY += nodeSize.y;
            }

            offsetX += nodeSize.x;
        }
    }
    #endregion

    #region 设置房间连接
    private void GenerateRouteLoop()
    {
        List<int> originRoomList = new();
        int repetitionCount = mapConfig.repetitionCount;
        for (int i = 0; i < repetitionCount; i++)
        {
            int originRoom = mapRandom.Next(0, repetitionCount);
            if (i == 1)
            {
                int safetyCounter = 0;
                while (originRoom == originRoomList[0] && safetyCounter < 100)
                {
                    originRoom = mapRandom.Next(0, repetitionCount);
                    safetyCounter++;
                }

                if (safetyCounter >= 100) originRoom = originRoomList[0];
            }

            originRoomList.Add(originRoom);
            SetRoute(originRoom);
        }
    }

    private void SetRoute(int originRoom)
    {
        int currentRoom = originRoom;

        RoomType currentRoomType = RoomType.None;

        for (int i = 0; i < mapNodeArray.GetLength(0); i++)
        {
            MapNode currentNode = mapNodeArray[i, currentRoom];
            currentNode.isActive = true;

            RoomType previousRoomType = currentRoomType;
            currentRoomType = SetRoomType(i, previousRoomType);

            // 设置新的房间类型
            currentNode.SetMapNodeType(currentRoomType);

            // 如果已经是最后一层，不需要设置下一个连接点
            if (i == mapNodeArray.Length - 1)
                break;

            int minIndex = 0;
            int maxIndex = layerCount.y - 1;

            // 检查前一层节点约束 (只在i>0时有效)
            if (currentRoom > 0)
            {
                MapNode previousNode = mapNodeArray[i, currentRoom - 1];
                if (previousNode.nextLayerConnectedNodes.Count > 0)
                    minIndex = previousNode.nextLayerConnectedNodes.Max();
            }

            // 检查下一层节点约束
            if (currentRoom < layerCount.y - 1) // 确保下一层和下下层都存在
            {
                MapNode nextNode = mapNodeArray[i, currentRoom + 1];
                if (nextNode.nextLayerConnectedNodes.Count > 0) maxIndex = nextNode.nextLayerConnectedNodes.Min();
            }

            minIndex = Mathf.Max(minIndex, currentRoom - 1);
            maxIndex = Mathf.Min(maxIndex, currentRoom + 1);

            int nextRoomIndex = mapRandom.Next(minIndex, maxIndex + 1);

            List<int> nextLayerConnectedNodes = currentNode.nextLayerConnectedNodes;
            if (!nextLayerConnectedNodes.Contains(nextRoomIndex)) nextLayerConnectedNodes.Add(nextRoomIndex);

            currentRoom = nextRoomIndex;
        }
    }

    private void DeleteInactiveNode()
    {
        foreach (MapNode node in mapNodeArray)
        {
            if (!node) continue;
            node.gameObject.SetActive(node.isActive);
        }
    }

    private void SetNodeLineVisual()
    {
        for (int i = 0; i < mapNodeArray.GetLength(0) - 1; i++)
        {
            for (int j = 0; j < mapNodeArray.GetLength(1); j++)
            {
                MapNode node = mapNodeArray[i, j];
                Vector2 startPosition = node.transform.localPosition;
                if (node.nextLayerConnectedNodes.Count <= 0) continue;

                List<int> connectedNodes = node.nextLayerConnectedNodes;
                foreach (int t in connectedNodes)
                {
                    MapNode connectedNode = mapNodeArray[i + 1, t];
                    Vector2 endPosition = connectedNode.transform.localPosition;

                    GameObject lineInstance = Instantiate(linePrefab, mapNodeParentRect);
                    lineInstance.SetActive(true);

                    SetNodeLinePosition(lineInstance, startPosition, endPosition);
                }
            }
        }
    }

    private void SetNodeLinePosition(GameObject lineInstance, Vector2 startPosition, Vector2 endPosition)
    {
        RectTransform rectTransform = lineInstance.GetComponent<RectTransform>();
        rectTransform.localPosition = (startPosition + endPosition) / 2;

        Vector2 size = rectTransform.sizeDelta;
        size.x = Vector2.Distance(startPosition, endPosition) - 70;
        rectTransform.sizeDelta = size;

        Vector2 direction = endPosition - startPosition;
        rectTransform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
    }
    #endregion

    #region 随机房间类型
    private RoomType SetRoomType(int layerIndex, RoomType previousRoomType)
    {
        switch (layerIndex)
        {
            // 1. 处理固定类型层级
            case 0:
                return RoomType.Enemy;
            case 7:
                return RoomType.Treasure;
        }

        if (layerIndex == layerCount.x - 1) return RoomType.Rest;

        // 2. 确定当前层级的限制条件
        List<RoomType> excludedTypes = new() { RoomType.None };
        bool avoidDuplicates = previousRoomType != RoomType.Enemy && previousRoomType != RoomType.Event;

        switch (layerIndex)
        {
            case 1:
                excludedTypes.AddRange(new[] { RoomType.Shop, RoomType.Elite, RoomType.Rest });
                break;
            // 2-5层不要精英、休息点
            case > 1 and < 6:
                excludedTypes.AddRange(new[] { RoomType.Elite, RoomType.Rest });
                break;
            default:
            {
                if (layerIndex == layerCount.x - 2)
                    excludedTypes.Add(RoomType.Rest);
                break;
            }
        }

        // 3. 生成符合条件的节点类型
        return GetValidNodeType(previousRoomType, excludedTypes, avoidDuplicates);
    }

    private RoomType GetValidNodeType(RoomType previousType, List<RoomType> excludedTypes, bool avoidDuplicates)
    {
        const int maxAttempts = 100;
        int attempts = 0;
        RoomType nodeType;

        do
        {
            nodeType = mapConfig.GetRandomMapNodeType(mapRandom).NodeType;
            attempts++;

            // 如果尝试次数过多，返回Enemy作为默认类型
            if (attempts >= maxAttempts) return RoomType.Enemy;

            // 检查是否符合条件：不在排除列表中，且不重复（如需要）
        } while (excludedTypes.Contains(nodeType) ||
                 avoidDuplicates && nodeType == previousType);

        return nodeType;
    }
    #endregion
}