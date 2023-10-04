using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using PlayerContoller;

public class PlayerControllerTests : MonoBehaviour
{
    public void IsLaserIneracting()
    {
        //Find asset is database
        string[] result = AssetDatabase.FindAssets("Rob Door");
        //Get Filepath
        string assetPath = AssetDatabase.GUIDToAssetPath(result[0]);

        GameObject GO = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

        Assert.IsTrue(GO.GetComponent<Laser>() != null);

    }
}

