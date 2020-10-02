using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandConsole.Console;

public class PrintCommand : ICommand {
    public void Execute(string[] args) {
        CommandConsole.Console.CommandConsole.Instance.Log(args);
    }

    public string[] SuggestedArgs(string[] args) {
        return new[] { "<text>" };
    }

    public string Label => "print";
    public string HelpText => $"{Label} <color=red><string></color>";
}
