using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CommandConsoleEditor : MonoBehaviour {

    [MenuItem("GameObject/Console/CommandConsole", false, 10)]
    static void CreateCustomPrimitiveGameObject(MenuCommand menuCommand) {
        //Use the asset database to fetch the console prefab
        GameObject consolePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
            "Packages/com.jasonskillman.commandconsole/Runtime/Prefabs/CommandConsole.prefab");

        //Instantiate the prefab in the hierarchy
        PrefabUtility.InstantiatePrefab(consolePrefab);

        Selection.activeObject = consolePrefab;
    }

}
