using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelGen
{
    public class LevelManager : MonoBehaviour
    {
        #region Singleton
        public static LevelManager instance;
        private void Awake()
        {
            if (instance != null)
            {
                instance = this;
            } else
            {
                Debug.LogWarning("LevelManager.Awake() :: Another instance of GameManager attempted to exist.", GameManager.instance);
                Destroy(gameObject);
            }
        }
        #endregion

        [SerializeField] public MapData mapData;
        [SerializeField] internal bool doGenerate = true;

        private int[] specialTiles = new int[1] {0};

        #region Level Instantiation

        #region Methods

        public void DeleteLevel(bool includeShaft = false)
        {
            if (!includeShaft && transform.childCount <= MapData.maxFloors) { return; }
            int lastIndex = (!includeShaft) ? MapData.maxFloors : 0;
            for (int i = transform.childCount - 1; i >= lastIndex; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
        private void SelectSpecialTiles()
        {
            specialTiles = new int[mapData.floorNum];
            specialTiles[0] = 0;
            for (int i = 1; i < mapData.floorNum; i++)
            {
                bool available = false;
                do
                {
                    specialTiles[i] = Random.Range(1, mapData.mapSize - 1);
                    available = specialTiles[i] == specialTiles[i - 1];
                } while (available);
            }
        }
        private bool IsSpecialTile(int floor, int x, int z)
        {
            if (floor >= specialTiles.Length) { return false; }
            // check specialTiles array if the specified tile is special
            return (specialTiles[floor] == x && specialTiles[floor] == z);
        }
        #endregion

        public void InstanceElevatorShaft()
        {
            for (int i = 0; i < MapData.maxFloors; i++)
            {
                if (i == 0)
                {
                    Instantiate(mapData.elevator, Vector3.zero, Quaternion.identity, transform);
                } else
                {
                    Instantiate(mapData.elevatorShaft, new Vector3(0, i * MapData.tileHeight, 0), Quaternion.identity, transform);
                }
            }
        }
        public void InstanceMall()
        {
            SelectSpecialTiles();
            int floorRamp = (mapData.floorNum > 1) ? Random.Range(1, mapData.mapSize-1) : 0;
            for (int i = 0; i < mapData.floorNum + 1; i++) // loops through the floors
            {
                for (int j = 0; j < mapData.mapSize; j++) // loops through the x axis of a single floor
                {
                    for (int k = 0; k < mapData.mapSize; k++) // loops through the z axis of a single floor
                    {
                        if (i == mapData.floorNum)
                        {
                            Instantiate(mapData.roof, new Vector3(j * MapData.tileSize, i * MapData.tileHeight, k * MapData.tileSize), Quaternion.identity, transform);
                            continue;
                        }
                        // SPAWN TILES
                        bool specialTile = IsSpecialTile(i+1, j, k); //(j == 0 && k == 0) || (j == floorRamp && k == floorRamp);
                        if (j + k == 0)
                        {
                            // Do nothing. This is the elevator shaft
                        } else if (specialTile)
                        {
                            int randRot = 90 * Random.Range(0, 4);
                            Instantiate(mapData.ramp, new Vector3(j * MapData.tileSize, i * MapData.tileHeight, k * MapData.tileSize), Quaternion.Euler(0, randRot, 0), transform);
                        }
                        else if (!IsSpecialTile(i, j, k))
                        {
                            GameObject tile = null;
                            if (mapData.mallTiles.Length == 0)
                            {
                                Debug.LogError("LevelManager.InstanceMall() :: No mall tiles in MapData", this);
                            } else if (mapData.mallTiles.Length == 1)
                            {
                                tile = mapData.mallTiles[0].tile;
                            } else
                            {
                                tile = MallTile.GetRandomTile(mapData.mallTiles);
                            }
                            int randRot = 90 * Random.Range(0, 4);
                            Instantiate(tile, new Vector3(j * MapData.tileSize, i * MapData.tileHeight, k * MapData.tileSize), Quaternion.Euler(0, randRot, 0), transform);
                        }

                        // SPAWN EXTERIOR MALL WALLS
                        if (j == 0 && k != 0)
                        {
                            Instantiate(mapData.wall, new Vector3(j * MapData.tileSize, i * MapData.tileHeight, k * MapData.tileSize), Quaternion.identity, transform);
                        }
                        else if (j == mapData.mapSize - 1)
                        {
                            Instantiate(mapData.wall, new Vector3(j * MapData.tileSize, i * MapData.tileHeight, k * MapData.tileSize), Quaternion.Euler(0, 180, 0), transform);
                        }
                        if (k == 0 && j != 0)
                        {
                            Instantiate(mapData.wall, new Vector3(j * MapData.tileSize, i * MapData.tileHeight, k * MapData.tileSize), Quaternion.Euler(0, -90, 0), transform);
                        }
                        else if (k == mapData.mapSize - 1)
                        {
                            Instantiate(mapData.wall, new Vector3(j * MapData.tileSize, i * MapData.tileHeight, k * MapData.tileSize), Quaternion.Euler(0, 90, 0), transform);
                        }
                    }
                }
            }
        }
        #endregion

        private void Start()
        {
            if (doGenerate)
            {
                InstanceElevatorShaft();
                InstanceMall();
            }
        }
    }
}
