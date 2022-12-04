namespace JasonSkillman.Console.Editor {
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.EventSystems;
    
    public class CreateCommandConsoleEditor {

        [MenuItem("GameObject/Console/Command Console", false, 10)]
        static void CreateCommandConsole(MenuCommand menuCommand) {
            //Check if the console has already been created
            CommandConsole existingConsole = Object.FindObjectOfType<CommandConsole>();

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


            //Instantiate an EventSystem if one does not exist
            GameObject eventSystem = GameObject.Find("EventSystem");
            if(eventSystem != null) return;

            eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }
    }
}
