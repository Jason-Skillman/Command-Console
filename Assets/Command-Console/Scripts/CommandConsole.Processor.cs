using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CommandConsole.Console {
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

            //Find the suggestion text
            suggestionBuilder.Clear();
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
                ICommand suggestedCommand = loadedCommands.FirstOrDefault(loadedCommand => loadedCommand.Label
                    .StartsWith(label.ToLower(), StringComparison.CurrentCultureIgnoreCase));
                
                if(suggestedCommand != null) {
                    if(suggestedCommand.Label.Length > commandString.Length) {
                        suggestionBuilder.Append(suggestedCommand.Label.Substring(commandString.Length));
                    }
                }
            }

            suggestionText.text = commandString + " " + suggestionBuilder.ToString();
        }

        private void InputField_OnEndEdit(string commandString) {
            if(Input.GetKeyDown(KeyCode.Return)) {
                RunCommand(commandString);
            }

            inputField.Select();
            inputField.ActivateInputField();
        }

        #endregion

        private ICommand FindCommand(string label) {
            return loadedCommands.FirstOrDefault(loadedCommand => loadedCommand.Label.ToLower().Equals(label.ToLower(), 
                StringComparison.CurrentCultureIgnoreCase));
        }

        private void RunCommand(string commandString) {
            var label = commandString;
            var args = "";

            if(commandString.IndexOf(' ') > -1) {
                label = commandString.Substring(0, commandString.IndexOf(' '));
                args = commandString.Substring(commandString.IndexOf(' ') + 1);
            }

            var command = FindCommand(label);

            if(command != null) {
                //command.Execute(args);
            } else {
                Log($"<color=red>Unknown command</color> <color=#FF6666>\"{label}\"</color>");
            }

            inputField.text = "";
        }

    }
}
