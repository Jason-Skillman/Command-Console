namespace JasonSkillman.Console.Tests {
    using UnityEngine;

    public class Driver : MonoBehaviour {

        void Update() {
            if(Input.GetKeyDown(KeyCode.Alpha1)) {
                CommandConsole.Instance.Log("Hello World");
            }

            if(Input.GetKeyDown(KeyCode.Alpha2)) {
                CommandConsole.Instance.LogWarning("Hello World");
            }

            if(Input.GetKeyDown(KeyCode.Alpha3)) {
                CommandConsole.Instance.LogError("Hello World");
            }
        }
    }
}