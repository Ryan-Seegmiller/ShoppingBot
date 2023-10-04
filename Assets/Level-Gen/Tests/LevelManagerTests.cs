using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class LevelManagerTests
{
    [UnityTest]
    public IEnumerator MapGeneration()
    {
        string[] result = AssetDatabase.FindAssets("Level", new[] { "Assets/Level-Gen/Prefabs" });
        string assetPath = AssetDatabase.GUIDToAssetPath(result[0]);
        GameObject levelPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
        GameObject level = GameObject.Instantiate(levelPrefab);

        yield return new WaitForSeconds(1);

        Assert.IsTrue(level.transform.childCount > 0);

        yield return null;
    }
}
