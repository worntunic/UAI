using System;
using System.Collections;
using System.Collections.Generic;
using UAI.Demo.Terrain;
using UAI.Utils.DataStructures;
using UnityEngine;
using System.Diagnostics;

namespace UAI.GeneralAI
{
    [System.Serializable]
    public class MapNode : IHeapItem<MapNode>
    {
        public Vector3 worldPoint;
        public bool passable;
        public int x;
        public int y;
        public MapNode parent;
        public int gCost;
        public int hCost;
        public int fCost { get { return gCost + hCost; } }
        private int _heapIndex;
        public int HeapIndex { get => _heapIndex; set => _heapIndex = value; }
        public MapNode(int x, int y, Vector3 worldPoint, bool passable)
        {
            this.worldPoint = worldPoint;
            this.passable = passable;
            this.x = x;
            this.y = y;
            gCost = 0;
            hCost = 0;
        }

        public int CompareTo(MapNode other)
        {
            int compare = fCost.CompareTo(other.fCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(other.hCost);
            }
            return -compare;
        }
    }
    public class MapGrid
    {
        public MapNode[,] nodes;
        public int Width { get { return nodes.GetLength(0); } }
        public int Height { get { return nodes.GetLength(1); } }
        public Vector2 gridWorldSize;
        public MapInfo mapInfo;
        public int TileCount { get { return Width * Height; } }
        public int mapEdgeWidth = 1;
        public MapGrid(MapInfo mapInfo)
        {
            this.mapInfo = mapInfo;
            gridWorldSize = new Vector2(mapInfo.Width * mapInfo.tileScale, mapInfo.Height * mapInfo.tileScale);
            nodes = new MapNode[mapInfo.Width, mapInfo.Height];
            for (int y = mapInfo.Height - 1; y >= 0; y--)
            {
                for (int x = 0; x < mapInfo.Width; x++)
                {
                    int flippedY = mapInfo.Height - y;
                    Vector3 worldPos = new Vector3(mapInfo.tileScale * x - gridWorldSize.x / 2, mapInfo.tiles[x, y].height, mapInfo.tileScale * flippedY - gridWorldSize.y / 2);
                    nodes[x, y] = new MapNode(x, y, worldPos, mapInfo.GetTileTerrain(x, y).passable);
                }
            }
        }

        public MapNode GetFromWorldPos(Vector3 worldPos)
        {
            float percentX = Mathf.Clamp01((worldPos.x + (gridWorldSize.x / 2)) / gridWorldSize.x);
            float percentY = Mathf.Clamp01((worldPos.z + (gridWorldSize.y / 2)) / gridWorldSize.y);
            float flippedPercentY = 1 - percentY;
            int x = Mathf.RoundToInt((Width) * percentX);
            int y = Mathf.RoundToInt((Height) * flippedPercentY);
            return nodes[x, y];
        }

        public List<MapNode> GetNeighbours(MapNode node)
        {
            List<MapNode> neighbours = new List<MapNode>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }
                    int checkX = node.x + x;
                    int checkY = node.y + y;
                    if (IsInsideMapEdges(checkX, checkY))
                    {
                        neighbours.Add(nodes[checkX, checkY]);
                    }
                }
            }
            return neighbours;
        }

        public bool IsInsideGrid(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }
        public bool IsInsideMapEdges(int x, int y)
        {
            return x >= mapEdgeWidth && x < Width - mapEdgeWidth && y >= mapEdgeWidth && y < Height - mapEdgeWidth;
        }

        public MapNode GetRandomPassableNode()
        {
            MapNode randomNode = null;
            while (randomNode == null || !randomNode.passable)
            {
                randomNode = nodes[UnityEngine.Random.Range(mapEdgeWidth, Width - mapEdgeWidth), UnityEngine.Random.Range(mapEdgeWidth, Height - mapEdgeWidth)];
            }
            return randomNode;
        }

    }
    public class Pathfinding
    {
        public MapInfo mapInfo;
        public MapGrid mapGrid;
        public int diagonalMoveCost = 10;
        public int normalMoveCost = 10;
        public List<MapNode> path;

        public void AssignMap(MapInfo mapInfo)
        {
            this.mapInfo = mapInfo;
            mapGrid = new MapGrid(mapInfo);
        }
        public List<MapNode> FindPathToRandomPoint(Vector3 startPos)
        {
            MapNode randomNode = null;
            while (randomNode == null || mapGrid.GetFromWorldPos(startPos) == randomNode)
            {
                randomNode = mapGrid.GetRandomPassableNode();
            }
            return FindPath(startPos, randomNode.worldPoint);
        }
        public List<MapNode> FindPath(Vector3 startPos, Vector3 targetPos)
        {
            /*Stopwatch sw = new Stopwatch();
            sw.Start();*/
            Heap<MapNode> openSet = new Heap<MapNode>(mapGrid.TileCount);
            HashSet<MapNode> closedSet = new HashSet<MapNode>();

            MapNode start = mapGrid.GetFromWorldPos(startPos);
            MapNode target = mapGrid.GetFromWorldPos(targetPos);
            start.gCost = 0;
            start.hCost = 0;

            openSet.Add(start);

            while (openSet.Count > 0)
            {
                MapNode current = openSet.RemoveFirstItem();
                closedSet.Add(current);

                if (current == target)
                {
                    /*sw.Stop();
                    UnityEngine.Debug.Log($"{sw.ElapsedTicks}ticks");*/
                    return RetracePath(start, target);
                }
                List<MapNode> neighbours = mapGrid.GetNeighbours(current);
                foreach (MapNode neighbour in neighbours)
                {
                    if (!neighbour.passable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMoveCostToNeighbour = current.gCost + GetDistance(current, neighbour);
                    if (newMoveCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMoveCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, target);
                        neighbour.parent = current;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }
            }
            return new List<MapNode> { start };
        }

        private List<MapNode> RetracePath(MapNode start, MapNode end)
        {
            path = new List<MapNode>();
            MapNode current = end;
            while (current != start)
            {
                path.Add(current);
                current = current.parent;
            }
            path.Reverse();
            return path;
        }

        private int GetDistance(MapNode nodeA, MapNode nodeB)
        {
            int dstX = Mathf.Abs(nodeA.x - nodeB.x);
            int dstY = Mathf.Abs(nodeA.y - nodeB.y);


            if (dstX > dstY)
            {
                return dstY * normalMoveCost + (dstX - dstY) * normalMoveCost;
            } else
            {
                return dstX * normalMoveCost + (dstY - dstX) * normalMoveCost;
            }
        }
        public int GetTileDistance(MapNode nodeA, MapNode nodeB)
        {
            int dstX = Mathf.Abs(nodeA.x - nodeB.x);
            int dstY = Mathf.Abs(nodeA.y - nodeB.y);


            return (dstX > dstY) ? dstX : dstY;
        }
        public int GetTileDistance(Vector3 worldPointA, Vector3 worldPointB)
        {
            return GetTileDistance(mapGrid.GetFromWorldPos(worldPointA), mapGrid.GetFromWorldPos(worldPointB));
        }
    }
}

