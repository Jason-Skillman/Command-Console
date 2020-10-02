using DebugCommandConsole;
using System;

public class AdditionCommand : ICommand {
    public void Execute(string[] args) {
        int num1 = int.Parse(args[0]);
        int num2 = int.Parse(args[1]);
        int answer = num1 + num2;

        CommandConsole.Instance.Log("The answer is " + answer);
    }

    public string[] SuggestedArgs(string[] args) {
        return new[] { 
            "<num 1>", 
            "<num 2>" 
        };
    }

    public string Label => "add";
    public string HelpText => $"{Label} <color=red><string></color>";
}
