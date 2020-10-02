using DebugCommandConsole;

public class PrintCommand : ICommand {
    public void Execute(string[] args) {
        CommandConsole.Instance.Log(args);
    }

    public string[] SuggestedArgs(string[] args) {
        return new[] { 
            "<text>" 
        };
    }

    public string Label => "print";
    public string HelpText => $"{Label} <color=red><string></color>";
}
