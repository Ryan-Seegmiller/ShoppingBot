using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MallTile
{
    public GameObject tile;
    [Range(0f,1f)] public float spawnChance = .5f;


    [HideInInspector] public float prevoiusSpawnChance = .5f;

    public static MallTile[] Balance(MallTile[] tiles)
    {
        float sum = 0;
        foreach (MallTile t in tiles) { sum += t.spawnChance; }

        if (sum > 1)
        {
            float num = (sum - 1) / tiles.Length;
            foreach (MallTile t in tiles)
            {
                t.spawnChance = Mathf.Clamp01(t.spawnChance - num);
            }
        } else if (sum < 1)
        {
            float num = (1 - sum) / tiles.Length;
            foreach (MallTile t in tiles)
            {
                t.spawnChance = Mathf.Clamp01(t.spawnChance + num);
            }
        }
        return tiles;
    }
    public static GameObject GetRandomTile(MallTile[] tiles)
    {
        float num = Random.Range(0f,1f);
        float sum = 0;
        for (int i = 0; i < tiles.Length; i++)
        {
            sum += tiles[i].spawnChance;
            if (sum >= num)
            {
                return tiles[i].tile;
            }
        }
        return null;
    }
}
