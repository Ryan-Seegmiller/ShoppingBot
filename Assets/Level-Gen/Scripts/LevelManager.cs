using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] internal MapData mapData;

        #region Level Instantiation
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
        public void DeleteLevel(bool includeShaft = false)
        {
            if (!includeShaft && transform.childCount <= MapData.maxFloors) { return; }
            int lastIndex = (!includeShaft) ? MapData.maxFloors: 0;
            for (int i = transform.childCount - 1; i >= lastIndex; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
        public void InstanceMall()
        {
            int floorRamp = (mapData.floorNum > 1) ? Random.Range(1, mapData.mapSize-1) : 0;
            for (int i = 0; i < mapData.floorNum; i++) // loops through the floors
            {
                for (int j = 0; j < mapData.mapSize; j++) // loops through the x axis of a single floor
                {
                    for (int k = 0; k < mapData.mapSize; k++) // loops through the z axis of a single floor
                    {
                        // SPAWN TILES
                        bool specialTile = (j == 0 && k == 0) || (j == floorRamp && k == floorRamp);
                        if (j == 0 && k == 0)
                        {
                            // Do nothing. This is the elevator shaft
                        } else if (j == floorRamp && k == floorRamp && mapData.floorNum - i > 1)
                        {
                            int randRot = 90 * Random.Range(0, 4);
                            Instantiate(mapData.ramp, new Vector3(j * MapData.tileSize, i * MapData.tileHeight, k * MapData.tileSize), Quaternion.Euler(0, randRot, 0), transform);
                        }
                        else if (!specialTile)
                        {
                            int mallTileIndex = (mapData.mallTiles.Length <= 1) ? 0 : Random.Range(0, mapData.mallTiles.Length);
                            int randRot = 90 * Random.Range(0, 4);
                            Instantiate(mapData.mallTiles[mallTileIndex], new Vector3(j * MapData.tileSize, i * MapData.tileHeight, k * MapData.tileSize), Quaternion.Euler(0, randRot, 0), transform);
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
            InstanceElevatorShaft();
            InstanceMall();
        }
    }
}
