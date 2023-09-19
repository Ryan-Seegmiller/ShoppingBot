using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    [CreateAssetMenu(menuName = "LevelGen/MapTiles")]
    internal class MapData : ScriptableObject
    {
        public const int tileSize = 10;
        [Range(5, 20)] public int mapSize = 5;
        [Range(1, 5)] public int floorNum = 1;

        public GameObject elevatorTile;
        public GameObject wall;
        public GameObject[] mallTiles;
    }
}
