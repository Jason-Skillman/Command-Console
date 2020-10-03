using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DebugCommandConsole {
    public partial class CommandConsole : MonoBehaviour {

        private void OnEnable() {
            inputField.onValueChanged.AddListener(InputField_OnValueChanged);
            inputField.onEndEdit.AddListener(InputField_OnEndEdit);
        }

        private void OnDisable() {
            inputField.onValueChanged.RemoveListener(InputField_OnValueChanged);
            inputField.onEndEdit.RemoveListener(InputField_OnEndEdit);
        }

        #region EventListeners

        /// <summary>
        /// Updates the text suggestion when input value changes
        /// </summary>
        /// <param name="commandString"></param>
        private void InputField_OnValueChanged(string commandString) {
            string label = commandString;
            List<string> args = new List<string>();

            //Parse the command label and args
            if(commandString.IndexOf(' ') > -1) {
                label = commandString.Substring(0, commandString.IndexOf(' '));
                args.AddRange(commandString.Substring(commandString.IndexOf(' ') + 1).Split(' '));

                //Check if the last arg is empty
                if(args[args.Count - 1].Equals(string.Empty)) {
                    //Remove the last arg if it is empty
                    args.RemoveAt(args.Count - 1);
                    //print("last arg is empty");
                }
            }

            //Setup the suggestion builder
            suggestionBuilder.Clear();
            suggestionBuilder.Append(label);
            suggestionBuilder.Append(" ");
            //Add in all of the args
            args.ForEach(a => {
                suggestionBuilder.Append(a);
                suggestionBuilder.Append(" ");
            });


            //Search for a command based on the label
            ICommand command = FindCommand(label);

            //Was a command found?
            if(command != null) {
                //Get all of the suggested args from the command
                string[] allSuggestedArgs = command.SuggestedArgs(args.ToArray());

                //Append all of the args together
                string attachment = string.Empty;
                for(int i = 0; i < allSuggestedArgs.Length; i++) {
                    //Skip the arg if it is already being used in the commandString
                    if(i <= args.Count - 1) continue;
                    
                    attachment += allSuggestedArgs[i];

                    //Append a space between args
                    if(i != allSuggestedArgs.Length - 1) attachment += " ";
                }

                //Add the args strings to the suggestion builder
                suggestionBuilder.Append(attachment);
            } else if(label != string.Empty) {
                //Predic what the next command could be based on input
                ICommand suggestedCommand = loadedCommands.FirstOrDefault(loadedCommand => loadedCommand.Label
                    .StartsWith(label.ToLower(), StringComparison.CurrentCultureIgnoreCase));
                
                if(suggestedCommand != null) {
                    if(suggestedCommand.Label.Length > commandString.Length) {
                        suggestionBuilder.Append(suggestedCommand.Label.Substring(commandString.Length));
                    }
                }
            }

            //Append the final suggestion
            suggestionText.text = suggestionBuilder.ToString();
        }

        private void InputField_OnEndEdit(string commandString) {
            if(Input.GetKeyDown(KeyCode.Return)) {
                RunCommand(commandString);
            }

            inputField.Select();
            inputField.ActivateInputField();
        }

        #endregion

        /// <summary>
        /// Finds a command that matches the label
        /// </summary>
        /// <param name="label">The command label to search for</param>
        /// <returns>The command instance</returns>
        private ICommand FindCommand(string label) {
            return loadedCommands.FirstOrDefault(loadedCommand => loadedCommand.Label.ToLower().Equals(label.ToLower(), 
                StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Runs the command with args
        /// </summary>
        /// <param name="commandString">The entire command to run as a string</param>
        private void RunCommand(string commandString) {
            string label = commandString;
            List<string> args = new List<string>();

            //Parse the command label and args
            if(commandString.IndexOf(' ') > -1) {
                label = commandString.Substring(0, commandString.IndexOf(' '));
                args.AddRange(commandString.Substring(commandString.IndexOf(' ') + 1).Split(' '));

                //Check if the last arg is empty
                if(args[args.Count - 1].Equals(string.Empty)) {
                    //Remove the last arg if it is empty
                    args.RemoveAt(args.Count - 1);
                    //print("last arg is empty");
                }
            }

            //Find the suggestion text
            suggestionBuilder.Clear();
            ICommand command = FindCommand(label);


            //Run the command if one was found
            if(command != null) {
                command.Execute(args.ToArray());
            } else {
                Log($"<color=red>Unknown command</color> <color=#FF6666>\"{label}\"</color>");
            }

            //Clear the input field
            inputField.text = string.Empty;
        }

    }
}
