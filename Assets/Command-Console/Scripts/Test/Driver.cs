using UnityEngine;
using System.Collections;
using DebugCommandConsole;

public class Driver : MonoBehaviour {

    void Update() {
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            CommandConsole.Instance.Log("Hello world");
        }
    }

}