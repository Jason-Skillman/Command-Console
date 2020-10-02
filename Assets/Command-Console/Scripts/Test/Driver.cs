using UnityEngine;
using System.Collections;
using CommandConsole.Console;

public class Driver : MonoBehaviour {

    void Update() {
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            CommandConsole.Console.CommandConsole.Instance.Log("Hello World");
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)) {
            CommandConsole.Console.CommandConsole.Instance.LogWarning("Hello World");
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)) {
            CommandConsole.Console.CommandConsole.Instance.LogError("Hello World");
        }
    }

}