using CommandConsole.Console;

public class AdditionCommand : ICommand {
    public void Execute(string[] args) {
        //CommandConsole.Console.CommandConsole.Instance.Log(args);
    }

    public string[] SuggestedArgs(string[] args) {
        return new[] { "<num 1>", "<num 2>" };
    }

    public string Label => "add";
    public string HelpText => $"{Label} <color=red><string></color>";
}
