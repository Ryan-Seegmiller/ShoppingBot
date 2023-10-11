using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using LevelGen;

public class LevelManagerTests
{
    public static GameObject level;

    public static GameObject InstanceLevel()
    {
        string[] result = AssetDatabase.FindAssets("Level", new[] { "Assets/Level-Gen/Prefabs" });
        string assetPath = AssetDatabase.GUIDToAssetPath(result[0]);
        GameObject levelPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
        return GameObject.Instantiate(levelPrefab);
    }

    [UnityTest]
    public IEnumerator MapTilesExist()
    {
        if (level == null) 
        { 
            level = InstanceLevel();
            yield return new WaitForSeconds(1);
        }

        
    }

    [UnityTest]
    public IEnumerator TileBalance()
    {
        if (level == null)
        {
            level = InstanceLevel();
            yield return new WaitForSeconds(1);
        }

        LevelManager manager = level.GetComponent<LevelManager>();
        GameObject[] tiles = new GameObject[100];
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = MallTile.GetRandomTile(manager.mapData.mallTiles);
            yield return null;
        }

        // TODO: check if the tile spawn chances are properly weighted

        Assert.IsTrue(true);
        yield return null;
    }

    [UnityTest]
    public IEnumerator MapIsGenerating()
    {
        if (level == null)
        {
            level = InstanceLevel();
            yield return new WaitForSeconds(1);
        }

        Assert.IsTrue(level.transform.childCount > 0);

        yield return null;
    }
}
