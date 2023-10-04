using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using gamemanager;

public class GameManagerTestScript
{
    // A Test behaves as an ordinary method
    [Test]
    public void GameManagerTestScriptSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator GameManagerTestScriptWithEnumeratorPasses()
    {
        Assert.IsFalse(false);

        yield return null;
    }
}
