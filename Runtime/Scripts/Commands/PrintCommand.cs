namespace DebugCommandConsole.Commands {
    public class PrintCommand : ICommand {
        public void Action(string[] args) {
            CommandConsole.Instance.Log(args);
        }

        public string[] SuggestedArgs(string[] args) {
            return new[] {
                "<text>"
            };
        }

        public string Label => "print";
    }
}
