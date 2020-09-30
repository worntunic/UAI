using System.Collections;
using System.Collections.Generic;
using UAI.GeneralAI;
using UnityEngine;

public class MapGridTester : MonoBehaviour
{
    /*[System.Serializable]
    public struct PosData
    {
        public Vector3 worldPos;
        public int x, y;
    }*/
    public Vector3 worldPos;
    public int x, y;
    //public PosData init, calculated;
    public bool drawByWorldPos;

    /*private void Update()
    {
        InsertWay(x, y);
        CalcWay(init.worldPos);
    }*/
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (Starter.PathFinder != null)
        {
            if (drawByWorldPos)
            {
                MapNode node = Starter.PathFinder.mapGrid.GetFromWorldPos(worldPos);
                x = node.x;
                y = node.y;
                Gizmos.DrawCube(node.worldPoint, Vector3.one);
            }
            else
            {
                MapNode node = Starter.PathFinder.mapGrid.nodes[x, y];
                worldPos = node.worldPoint;
                Gizmos.DrawCube(node.worldPoint, Vector3.one);
            }

        }

    }
    /*private void InsertWay(int fromX, int fromY)
    {
        MapGrid mapGrid = Starter.PathFinder.mapGrid;
        int flippedY = mapGrid.mapInfo.Height - fromY;
        Vector3 worldPos = new Vector3(mapGrid.mapInfo.tileScale * fromX - mapGrid.gridWorldSize.x / 2, mapGrid.mapInfo.tiles[fromX, fromY].height, mapGrid.mapInfo.tileScale * flippedY - mapGrid.gridWorldSize.y / 2);
        MapNode node = new MapNode(fromX, fromY, worldPos, mapGrid.mapInfo.GetTileTerrain(fromX, fromY).passable);
        init.worldPos = worldPos;
        init.x = fromX;
        init.y = fromY;
    }
    private void CalcWay(Vector3 point)
    {
        MapGrid mapGrid = Starter.PathFinder.mapGrid;
        float percentX = Mathf.Clamp01((point.x + (mapGrid.gridWorldSize.x / 2)) / mapGrid.gridWorldSize.x);
        float percentY = Mathf.Clamp01((point.z + (mapGrid.gridWorldSize.y / 2)) / mapGrid.gridWorldSize.y);
        float flippedPercentY = 1 - percentY;
        int newX = Mathf.RoundToInt((mapGrid.Width) * percentX);
        int newY = Mathf.RoundToInt((mapGrid.Height) * flippedPercentY);
        calculated.worldPos = mapGrid.nodes[newX, newY].worldPoint;
        calculated.x = newX;
        calculated.y = newY;
    }*/
}
