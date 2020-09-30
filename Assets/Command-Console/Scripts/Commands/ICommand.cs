namespace DebugCommandConsole {
    public interface ICommand {
        void Execute(string args);
        string Suggest(string args);

        string Label { get; }
        string HelpText { get; }
    }
}
