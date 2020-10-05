using System.Collections;
using System.Collections.Generic;
using UAI.Demo;
using UAI.Demo.Terrain;
using UnityEngine;

namespace UAI.GeneralAI
{
    public class Starter : MonoBehaviour
    {
        public MapGenerator mapGenerator;
        public PlantSpawner plants;
        public DrinkingWater drinkingWater;
        public BunnySpawner bunnySpawner;
        private static Pathfinding _pathfinder;
        public static Pathfinding PathFinder { get { return _pathfinder; } }
        public bool drawPassability = true;

        private void Awake()
        {
            mapGenerator.GenerateMap();
            _pathfinder = new Pathfinding();
            _pathfinder.AssignMap(mapGenerator.MapInfo);
            bunnySpawner.Init();
            drinkingWater.Init(_pathfinder.mapGrid);
            plants.Init(_pathfinder.mapGrid);

        }

        /*private void OnEnable()
        {
            MapGenerator.OnMapGenerated += OnMapGenerated;
        }

        private void OnDisable()
        {
            MapGenerator.OnMapGenerated -= OnMapGenerated;
        }

        private void OnMapGenerated(MapInfo mapInfo)
        {
            if (_pathfinder == null)
            {
                _pathfinder = new Pathfinding();
            }
            _pathfinder.AssignMap(mapInfo);
        }*/

        private void OnDrawGizmos()
        {
            if (drawPassability && PathFinder != null && PathFinder.mapGrid != null)
            {
                Gizmos.DrawWireCube(transform.position, new Vector3(PathFinder.mapGrid.Width, 1, PathFinder.mapGrid.Height));

                foreach (MapNode n in PathFinder.mapGrid.nodes)
                {
                    Gizmos.color = n.passable ? Color.white : Color.black;
                    /*if (PathFinder.path != null && PathFinder.path.Contains(n))
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawCube(n.worldPoint, Vector3.one * PathFinder.mapInfo.tileScale);

                    }*/
                    if (n.passable)
                    Gizmos.DrawCube(n.worldPoint, Vector3.one * PathFinder.mapInfo.tileScale);
                }
            }
        }
    }
}

