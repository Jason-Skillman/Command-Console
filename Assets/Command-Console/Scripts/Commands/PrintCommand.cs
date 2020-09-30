using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DebugCommandConsole;

public class PrintCommand : ICommand {
    public void Execute(string args) {
       CommandConsole.Instance.Log(args);
    }

    public string Suggest(string args) {
        return "";
    }

    public string Label => "print";
    public string HelpText => $"{Label} <color=red><string></color>";
}
