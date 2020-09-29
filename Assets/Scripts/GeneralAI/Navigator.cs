using System;
using System.Collections;
using System.Collections.Generic;
using UAI.Demo.Terrain;
using UnityEngine;

namespace UAI.GeneralAI
{
    public class MapNode
    {
        public Vector3 worldPoint;
        public bool passable;
        public int x;
        public int y;
        public MapNode parent;
        public int gCost;
        public int hCost;
        public int fCost { get { return gCost + hCost; } }

        public MapNode(int x, int y, Vector3 worldPoint, bool passable)
        {
            this.worldPoint = worldPoint;
            this.passable = passable;
            this.x = x;
            this.y = y;
            gCost = 0;
            hCost = 0;
        }

    }
    public class MapGrid
    {
        public MapNode[,] nodes;
        public int Width { get { return nodes.GetLength(0); } }
        public int Height { get { return nodes.GetLength(1); } }
        private Vector2 gridWorldSize;
        public MapInfo mapInfo;

        public MapGrid(MapInfo mapInfo)
        {
            this.mapInfo = mapInfo;
            gridWorldSize = new Vector2(mapInfo.Width * mapInfo.tileScale, mapInfo.Height * mapInfo.tileScale);
            nodes = new MapNode[mapInfo.Width, mapInfo.Height];
            for (int y = mapInfo.Height - 1; y >= 0; y--)
            {
                for (int x = 0; x < mapInfo.Width; x++)
                {
                    int flippedY = mapInfo.Height - 1 - y;
                    Vector3 worldPos = new Vector3(1 + mapInfo.tileScale * x - gridWorldSize.x / 2, mapInfo.tiles[x, y].height, mapInfo.tileScale * flippedY - gridWorldSize.y / 2);
                    nodes[x, y] = new MapNode(x, y, worldPos, mapInfo.GetTileTerrain(x, y).passable);
                }
            }
        }

        public MapNode GetFromWorldPos(Vector3 worldPos)
        {
            float percentX = Mathf.Clamp01((worldPos.x + (gridWorldSize.x / 2) - 1) / gridWorldSize.x);
            float percentY = Mathf.Clamp01((worldPos.z + (gridWorldSize.y / 2)) / gridWorldSize.y);
            float flippedPercentY = 1 - percentY;
            int x = Mathf.RoundToInt((Width - 1) * percentX);
            int y = Mathf.RoundToInt((Height - 1) * flippedPercentY);
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
                    if (IsInsideGrid(checkX, checkY))
                    {
                        neighbours.Add(nodes[checkX, checkY]);
                    }
                }
            }
            return neighbours;
        }

        private bool IsInsideGrid(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        public MapNode GetRandomPassableNode()
        {
            MapNode randomNode = null;
            while (randomNode == null || !randomNode.passable)
            {
                randomNode = nodes[UnityEngine.Random.Range(0, Width), UnityEngine.Random.Range(0, Height)];
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
            List<MapNode> openSet = new List<MapNode>();
            HashSet<MapNode> closedSet = new HashSet<MapNode>();

            MapNode start = mapGrid.GetFromWorldPos(startPos);
            MapNode target = mapGrid.GetFromWorldPos(targetPos);
            start.gCost = 0;
            start.hCost = 0;

            openSet.Add(start);

            while (openSet.Count > 0)
            {
                MapNode current = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < current.fCost || openSet[i].fCost == current.fCost && openSet[i].hCost < current.hCost)
                    {
                        current = openSet[i];
                    }
                }

                openSet.Remove(current);
                closedSet.Add(current);

                if (current == target)
                {
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
    }
}

