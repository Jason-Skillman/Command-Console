using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Updates the text suggestions when input value changes
        /// </summary>
        /// <param name="commandString"></param>
        private void InputField_OnValueChanged(string commandString) {
            string label = commandString;
            string args = "";

            if(commandString.IndexOf(' ') > -1) {
                label = commandString.Substring(0, commandString.IndexOf(' '));
                args = commandString.Substring(commandString.IndexOf(' ') + 1);
            }

            var suggestion = "";

            var command = FindCommand(label);

            if(command != null) {
                suggestion = command.Suggest(args);
            } else if(label != "" && args == "") {
                var suggestedCommand = loadedCommands.FirstOrDefault(loadedCommand => loadedCommand.Label.StartsWith(label.ToLower(), StringComparison.CurrentCultureIgnoreCase));
                if(suggestedCommand != null) {
                    if(suggestedCommand.Label.Length > commandString.Length) {
                        suggestion = suggestedCommand.Label.Substring(commandString.Length);
                    }
                }
            }

            suggestionText.text = commandString + suggestion;
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
            return loadedCommands.FirstOrDefault(loadedCommand => loadedCommand.Label.Equals(label.ToLower(), StringComparison.CurrentCultureIgnoreCase));
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
                command.Execute(args);
            } else {
                Log($"<color=red>Unknown command</color> <color=#FF6666>\"{label}\"</color>");
            }

            inputField.text = "";
        }

    }
}
