namespace DebugCommandConsole.Commands {
    public class AdditionCommand : ICommand {
        public string Label => "add";

        public string[] SuggestedArgs(string[] args) {
            return new[] {
                "<num 1>",
                "<num 2>"
            };
        }

        public void Action(string[] args) {
            int num1 = int.Parse(args[0]);
            int num2 = int.Parse(args[1]);
            int answer = num1 + num2;

            CommandConsole.Instance.Log("The answer is " + answer);
        }
    }
}
