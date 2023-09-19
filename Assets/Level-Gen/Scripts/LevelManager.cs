using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] internal MapData mapData;

        #region Level Instantiation
        public void InstanceMall()
        {
            for (int i = 0; i < mapData.mapSize; i++)
            {
                for (int j = 0; j < mapData.mapSize; j++)
                {
                    GameObject go;
                    if (i == 0 && j == 0)
                    {
                        go = Instantiate(mapData.elevatorTile, new Vector3(i * MapData.tileSize, 0, j * MapData.tileSize), Quaternion.identity, transform);
                    } else
                    {
                        int mallTileIndex = (mapData.mallTiles.Length <= 1) ? 0 : Random.Range(0, mapData.mallTiles.Length);
                        int randRot = 90 * Random.Range(0, 4);
                        go = Instantiate(mapData.mallTiles[mallTileIndex], new Vector3(i * MapData.tileSize, 0, j * MapData.tileSize), Quaternion.Euler(0, randRot, 0), transform);
                    }
                    // spawn mall exterior walls
                    if (i == 0)
                    {
                        GameObject wall = Instantiate(mapData.wall, new Vector3(i * MapData.tileSize, 0, j * MapData.tileSize), Quaternion.identity, transform);
                    } else if (i == mapData.mapSize - 1)
                    {
                        GameObject wall = Instantiate(mapData.wall, new Vector3(i * MapData.tileSize, 0, j * MapData.tileSize), Quaternion.Euler(0, 180, 0), transform);
                    }
                    if (j == 0)
                    {
                        GameObject wall = Instantiate(mapData.wall, new Vector3(i * MapData.tileSize, 0, j * MapData.tileSize), Quaternion.Euler(0, -90, 0), transform);
                    } else if (j == mapData.mapSize - 1)
                    {
                        GameObject wall = Instantiate(mapData.wall, new Vector3(i * MapData.tileSize, 0, j * MapData.tileSize), Quaternion.Euler(0, 90, 0), transform);
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
