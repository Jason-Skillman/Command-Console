namespace JasonSkillman.Console.Commands {
    public class PrintCommand : ICommand {
        public string Label => "print";

        public string[] SuggestedArgs(string[] args) {
            return new[] {
                "<text>"
            };
        }

        public void Action(string[] args) {
            CommandConsole.Instance.Log(args);
        }
    }
}
