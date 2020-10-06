namespace DebugCommandConsole {
    public interface ICommand {
        /// <summary>
        /// The action to execute when the command is run.
        /// </summary>
        /// <param name="args">The arguments to use with the action.</param>
        void Action(string[] args);

        /// <summary>
        /// A list of suggested arguments to give to the command. Arguments should be in order.
        /// </summary>
        /// <param name="args">The arguments currently being used if any.</param>
        /// <returns></returns>
        string[] SuggestedArgs(string[] args = null);

        /// <summary>
        /// The command's label/header.
        /// </summary>
        string Label { get; }
    }
}
