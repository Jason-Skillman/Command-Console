using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace CommandConsole.Console {
    public partial class CommandConsole : MonoBehaviour {

        [Header("Processor References")]
        public TMP_Text suggestionText;
        public TMP_InputField _input;

        /// <summary>
        /// Stores all of the loaded commands.
        /// </summary>
        protected readonly List<ICommand> loadedCommands = new List<ICommand>();

        private void Processor_Start() {
            _input.onValueChanged.AddListener(UpdateSuggestion);

            _input.onEndEdit.AddListener(commandText => {
                if(Input.GetKeyDown(KeyCode.Return)) {
                    RunCommand(commandText);
                }
            });
            _input.onEndEdit.AddListener(_ => {
                _input.Select();
                _input.ActivateInputField();
            });

            LoadCommands();
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

            _input.text = "";
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

        /// <summary>
        /// Collects and stores all of the possible commands.
        /// Uses C# Reflection.
        /// </summary>
        public void LoadCommands() {
            //Todo: delete
            loadedCommands.Clear();

            IEnumerable<Type> commandTypes = Assembly.GetAssembly(typeof(ICommand)).GetTypes()
                .Where(t => t != typeof(ICommand) && typeof(ICommand).IsAssignableFrom(t));

            CommandConsole.Instance.Log($"Loading {commandTypes.Count()} commands");

            var objects = new List<ICommand>();
            foreach(var type in commandTypes) {
                CommandConsole.Instance.Log($" - {type.FullName}");
                var commandInstance = (ICommand)Activator.CreateInstance(type);

                loadedCommands.Add(commandInstance);
            }
        }

    }
}
