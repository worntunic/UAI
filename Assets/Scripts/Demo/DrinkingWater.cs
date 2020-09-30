using System.Collections;
using System.Collections.Generic;
using UAI.Demo.Terrain;
using UAI.GeneralAI;
using UAI.Utils.DataStructures;
using UnityEngine;

namespace UAI.Demo
{
    public class DrinkingWater : MonoBehaviour
    {
        public List<MapNode> tilesToDrink;
        public MapGrid mapGrid;

        public void Init(MapGrid mapGrid)
        {
            this.mapGrid = mapGrid;
            FindWater();
        }
        private void FindWater()
        {
            tilesToDrink = new List<MapNode>();
            for (int y = 0; y < mapGrid.Height; y++)
            {
                for (int x = 0; x < mapGrid.Width; x++)
                {
                    MapNode node = mapGrid.nodes[x, y];
                    if (mapGrid.mapInfo.GetTileTerrain(x, y).drinkingWater)
                    {
                        List<MapNode> neighbourTiles = mapGrid.GetNeighbours(node);
                        for (int i = 0; i < neighbourTiles.Count; i++)
                        {
                            if (neighbourTiles[i].passable && !tilesToDrink.Contains(neighbourTiles[i]))
                            {
                                tilesToDrink.Add(neighbourTiles[i]);
                            }
                        }
                    }
                }
            }
        }

        private class DrinkTile : IHeapItem<DrinkTile>
        {
            public MapNode node;
            public float distance;
            public DrinkTile(MapNode node, MapNode fromNode)
            {
                this.node = node;
                this.distance = Starter.PathFinder.GetTileDistance(node, fromNode);
            }
            private int _heapIndex;
            public int HeapIndex { get => _heapIndex; set => _heapIndex = value; }

            public int CompareTo(DrinkTile other)
            {
                return Mathf.FloorToInt(other.distance - distance);
            }
        }
        public List<MapNode> GetClosestDrinkTiles(Vector3 startPos, int count)
        {
            MapNode start = Starter.PathFinder.mapGrid.GetFromWorldPos(startPos);
            List<MapNode> nodes = new List<MapNode>();
            Heap<DrinkTile> heapNodes = new Heap<DrinkTile>(Starter.PathFinder.mapGrid.Width * Starter.PathFinder.mapGrid.Height);
            for (int i = 0; i < tilesToDrink.Count; i++)
            {
                heapNodes.Add(new DrinkTile(tilesToDrink[i], start));
            }
            count = (count > heapNodes.Count) ? heapNodes.Count : count;
            for (int i = 0; i < count; i++)
            {
                nodes.Add(heapNodes.RemoveFirstItem().node);
            }
            return nodes;
        }
    }
}

