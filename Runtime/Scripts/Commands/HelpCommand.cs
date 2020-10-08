namespace DebugCommandConsole.Commands {
    public class HelpCommand : ICommand {
        public string Label => "help";

        public string[] SuggestedArgs(string[] args) {
            return null;
        }

        public void Action(string[] args) {
            CommandConsole.Instance.LogAllCommands();
        }
    }
}
