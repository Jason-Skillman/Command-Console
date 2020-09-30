using System.Collections;
using System.Collections.Generic;
using Console.Scripts;
using UnityEngine;

public class PrintCommand : ICommand
{
    public void Execute(string args)
    {
        CommandConsole.Log(args);
    }

    public string Suggest(string args)
    {
        return "";
    }

    public string Label => "print";
    public string HelpText => $"{Label} <color=red><string></color>";
}
