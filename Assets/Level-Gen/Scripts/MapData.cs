using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    [CreateAssetMenu(menuName = "LevelGen/MapTiles")]
    internal class MapData : ScriptableObject
    {
        public const int tileSize = 10;
        public const int tileHeight = 5;
        public const int maxSize = 20;
        public const int maxFloors = 5;
        [Range(5, maxSize)] public int mapSize = 5;
        [Range(1, maxFloors)] public int floorNum = 1;

        public GameObject elevator;
        public GameObject elevatorShaft;
        public GameObject wall;
        public GameObject ramp;
        public GameObject roof;
        public GameObject[] mallTiles;
    }
}
