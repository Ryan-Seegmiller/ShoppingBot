using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class LevelManagerTests
{
    public static GameObject level;

    public GameObject InstanceLevel()
    {
        string[] result = AssetDatabase.FindAssets("Level", new[] { "Assets/Level-Gen/Prefabs" });
        string assetPath = AssetDatabase.GUIDToAssetPath(result[0]);
        GameObject levelPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
        return GameObject.Instantiate(levelPrefab);
    }

    [UnityTest]
    public IEnumerator MapGeneration()
    {
        if (level == null) { level = InstanceLevel(); }
        yield return new WaitForSeconds(1);

        Assert.IsTrue(level.transform.childCount > 0);

        yield return null;
    }
}
