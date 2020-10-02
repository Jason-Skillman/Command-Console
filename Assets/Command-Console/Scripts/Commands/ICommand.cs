namespace CommandConsole.Console {
    public interface ICommand {
        void Execute(string[] args);
        string[] SuggestedArgs(string[] args);

        string Label { get; }
        string HelpText { get; }
    }
}
