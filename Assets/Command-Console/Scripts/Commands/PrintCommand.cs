using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandConsole;

public class PrintCommand : ICommand {
    public void Execute(string args) {
        CommandConsole.CommandConsole.Log(args);
    }

    public string Suggest(string args) {
        return "";
    }

    public string Label => "print";
    public string HelpText => $"{Label} <color=red><string></color>";
}
