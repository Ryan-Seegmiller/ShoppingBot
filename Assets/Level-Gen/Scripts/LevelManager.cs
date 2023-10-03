using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] internal MapData mapData;

        #region Level Instantiation
        public void ElevatorShaft()
        {

        }
        public void DeleteLevel()
        {

        }
        public void InstanceMall()
        {
            int floorRamp = (mapData.floorNum > 1) ? Random.Range(1, mapData.mapSize-1) : 0;
            for (int i = 0; i < mapData.floorNum; i++)
            {
                for (int j = 0; j < mapData.mapSize; j++)
                {
                    for (int k = 0; k < mapData.mapSize; k++)
                    {
                        GameObject go;
                        bool specialTile = (j == 0 && k == 0) || (j == floorRamp && k == floorRamp);
                        if (j == 0 && k == 0)
                        {
                            go = Instantiate(mapData.elevatorTile, new Vector3(j * MapData.tileSize, i * 5, k * MapData.tileSize), Quaternion.identity, transform);
                        } else if (j == floorRamp && k == floorRamp && mapData.floorNum - i > 1)
                        {
                            int randRot = 90 * Random.Range(0, 4);
                            go = Instantiate(mapData.ramp, new Vector3(j * MapData.tileSize, i * 5, k * MapData.tileSize), Quaternion.Euler(0, randRot, 0), transform);
                        }
                        else if (!specialTile)
                        {
                            int mallTileIndex = (mapData.mallTiles.Length <= 1) ? 0 : Random.Range(0, mapData.mallTiles.Length);
                            int randRot = 90 * Random.Range(0, 4);
                            go = Instantiate(mapData.mallTiles[mallTileIndex], new Vector3(j * MapData.tileSize, i * 5, k * MapData.tileSize), Quaternion.Euler(0, randRot, 0), transform);
                        }
                        // spawn mall exterior walls
                        if (j == 0)
                        {
                            GameObject wall = Instantiate(mapData.wall, new Vector3(j * MapData.tileSize, i * 5, k * MapData.tileSize), Quaternion.identity, transform);
                        }
                        else if (j == mapData.mapSize - 1)
                        {
                            GameObject wall = Instantiate(mapData.wall, new Vector3(j * MapData.tileSize, i * 5, k * MapData.tileSize), Quaternion.Euler(0, 180, 0), transform);
                        }
                        if (k == 0)
                        {
                            GameObject wall = Instantiate(mapData.wall, new Vector3(j * MapData.tileSize, i * 5, k * MapData.tileSize), Quaternion.Euler(0, -90, 0), transform);
                        }
                        else if (k == mapData.mapSize - 1)
                        {
                            GameObject wall = Instantiate(mapData.wall, new Vector3(j * MapData.tileSize, i * 5, k * MapData.tileSize), Quaternion.Euler(0, 90, 0), transform);
                        }
                    }
                }
            }
        }
        #endregion

        private void Start()
        {
            InstanceMall();
        }
    }
}
