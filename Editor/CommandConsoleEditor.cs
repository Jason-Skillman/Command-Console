using DebugCommandConsole;
using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CommandConsoleEditor {

    [MenuItem("GameObject/Console/CommandConsole", false, 10)]
    static void CreateCustomPrimitiveGameObject(MenuCommand menuCommand) {
        //Check if the console has already been created
        CommandConsole existingConsole = GameObject.FindObjectOfType<CommandConsole>();

        if(existingConsole != null) {
            Debug.LogWarning("Command console has already been created.");
            Selection.activeObject = existingConsole;
            return;
        }

        //Use the asset database to fetch the console prefab
        GameObject consolePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
            "Packages/com.jasonskillman.commandconsole/Runtime/Prefabs/CommandConsole.prefab");

        //Instantiate the prefab in the hierarchy
        PrefabUtility.InstantiatePrefab(consolePrefab);

        Selection.activeObject = consolePrefab;
    }

}
