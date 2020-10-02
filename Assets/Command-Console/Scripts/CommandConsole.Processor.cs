using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace CommandConsole.Console {
    public partial class CommandConsole : MonoBehaviour {

        /// <summary>
        /// Stores all of the loaded commands.
        /// </summary>
        protected readonly List<ICommand> loadedCommands = new List<ICommand>();

        private void Processor_Start() {
            inputField.onValueChanged.AddListener(UpdateSuggestion);

            inputField.onEndEdit.AddListener(commandText => {
                if(Input.GetKeyDown(KeyCode.Return)) {
                    RunCommand(commandText);
                }
            });
            inputField.onEndEdit.AddListener(_ => {
                inputField.Select();
                inputField.ActivateInputField();
            });

            LoadCommands();
        }

        /// <summary>
        /// Collects and stores all of the possible commands.
        /// </summary>
        public void LoadCommands() {
            loadedCommands.Clear();

            //Using C# reflection, find all of the commands in the current assembly
            IEnumerable<Type> commandTypes = Assembly.GetAssembly(typeof(ICommand)).GetTypes()
                .Where(t => t != typeof(ICommand) && typeof(ICommand).IsAssignableFrom(t));

            //Print out all of the loaded commands
            Log($"Loading {commandTypes.Count()} commands");
            foreach(Type type in commandTypes) {
                Log($" - {type.FullName}");

                //Create an instance of the command and add it to the loaded commands list
                ICommand commandInstance = (ICommand)Activator.CreateInstance(type);
                loadedCommands.Add(commandInstance);
            }
        }

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

        private void UpdateSuggestion(string commandString) {
            var label = commandString;
            var args = "";

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

    }
}
