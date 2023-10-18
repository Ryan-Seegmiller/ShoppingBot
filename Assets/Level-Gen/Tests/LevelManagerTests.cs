using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using LevelGen;
using Items;

public class LevelManagerTests
{
    #region Scene Instancing
    public static GameObject InstanceLevel()
    {
        string[] result = AssetDatabase.FindAssets("Level", new[] { "Assets/Level-Gen/Prefabs" });
        string assetPath = AssetDatabase.GUIDToAssetPath(result[0]);
        GameObject levelPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
        return GameObject.Instantiate(levelPrefab);
    }
    public static GameObject InstanceItemManager()
    {
        string[] result = AssetDatabase.FindAssets("ItemManager", new[] { "Assets/Item/Prefabs" });
        string assetPath = AssetDatabase.GUIDToAssetPath(result[0]);
        GameObject itemManagerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
        return GameObject.Instantiate(itemManagerPrefab);
    }
    #endregion

    [UnityTest]
    public IEnumerator IsGenerating()
    {
        #region Instance Check
        if (ItemManager.instance == null)
        {
            InstanceItemManager();
            yield return new WaitForSeconds(.5f);
        }
        if (LevelManager.instance == null)
        {
            InstanceLevel();
            yield return new WaitForSeconds(1);
        }
        #endregion

        Assert.IsTrue(LevelManager.instance.transform.childCount > 0);

        yield return null;
    }
    [UnityTest]
    public IEnumerator ElevatorGenerating()
    {
        #region Instance Check
        if (ItemManager.instance == null)
        {
            InstanceItemManager();
            yield return new WaitForSeconds(.5f);
        }
        if (LevelManager.instance == null)
        {
            InstanceLevel();
            yield return new WaitForSeconds(1);
        }
        #endregion
        int elevator = MapData.maxFloors;

        LevelManager.instance.DeleteLevel(true);
        Assert.AreEqual(0, LevelManager.instance.transform.childCount);
        yield return new WaitForSeconds(.1f);

        LevelManager.instance.DeleteLevel(true);
        LevelManager.instance.InstanceElevatorShaft();
        Assert.AreEqual(elevator, LevelManager.instance.transform.childCount);

        yield return null;
    }
    [UnityTest]
    public IEnumerator MallGenerating()
    {
        #region Instance Check
        if (ItemManager.instance == null)
        {
            InstanceItemManager();
            yield return new WaitForSeconds(.5f);
        }
        if (LevelManager.instance == null)
        {
            InstanceLevel();
            yield return new WaitForSeconds(1);
        }
        #endregion
        int elevator = MapData.maxFloors;
        int grid = LevelManager.instance.mapData.mapSize * LevelManager.instance.mapData.mapSize * (LevelManager.instance.mapData.floorNum + 1);
        int walls = (LevelManager.instance.mapData.mapSize * 4 * LevelManager.instance.mapData.floorNum) - (elevator * 2);
        int mall = grid /*+ walls*/;

        LevelManager.instance.DeleteLevel(true);
        Assert.AreEqual(0, LevelManager.instance.transform.childCount);
        yield return new WaitForSeconds(.1f);

        LevelManager.instance.DeleteLevel(true);
        LevelManager.instance.InstanceMall();
        Assert.AreEqual(mall, LevelManager.instance.transform.childCount);

        yield return null;
    }
    [UnityTest]
    public IEnumerator FullMapGenerating()
    {
        #region Instance Check
        if (ItemManager.instance == null)
        {
            InstanceItemManager();
            yield return new WaitForSeconds(.5f);
        }
        if (LevelManager.instance == null)
        {
            InstanceLevel();
            yield return new WaitForSeconds(1);
        }
        #endregion
        int elevator = MapData.maxFloors;
        int grid = LevelManager.instance.mapData.mapSize * LevelManager.instance.mapData.mapSize * (LevelManager.instance.mapData.floorNum + 1);
        int walls = (LevelManager.instance.mapData.mapSize * 4 * LevelManager.instance.mapData.floorNum) - (elevator * 2);
        int mall = grid + walls;
        int expectedChildCount = mall + elevator;

        LevelManager.instance.DeleteLevel(true);
        Assert.AreEqual(0, LevelManager.instance.transform.childCount);
        yield return new WaitForSeconds(.1f);

        LevelManager.instance.DeleteLevel(true);
        LevelManager.instance.InstanceElevatorShaft();
        LevelManager.instance.InstanceMall();
        Assert.AreEqual(expectedChildCount, LevelManager.instance.transform.childCount);

        yield return null;
    }
}
