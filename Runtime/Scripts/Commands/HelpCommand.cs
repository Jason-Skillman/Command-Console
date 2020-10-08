namespace DebugCommandConsole.Commands {
    public class HelpCommand : ICommand {
        public void Action(string[] args) {
            CommandConsole.Instance.LogAllCommands();
        }

        public string[] SuggestedArgs(string[] args) {
            return null;
        }

        public string Label => "help";
    }
}
