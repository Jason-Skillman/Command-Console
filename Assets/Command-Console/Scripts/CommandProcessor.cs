using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Console.Scripts
{
    public class CommandProcessor : MonoBehaviour
    {
        public CommandList commands;

        public TMP_Text suggestionText;
    
        private TMP_InputField _input;
    
        private void Start()
        {
            _input = GetComponent<TMP_InputField>();
            
            _input.onValueChanged.AddListener(UpdateSuggestion);
            
            _input.onEndEdit.AddListener(commandText =>
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    RunCommand(commandText);
                }
            });
            _input.onEndEdit.AddListener(_ =>
            {
                _input.Select();
                _input.ActivateInputField();
            });
        
            commands.LoadCommands();
        }
        
        private ICommand FindCommand(string label)
        {
            return commands.LoadedCommands.FirstOrDefault(loadedCommand => loadedCommand.Label.Equals(label.ToLower(), StringComparison.CurrentCultureIgnoreCase));
        }
    
        private void RunCommand(string commandString)
        {
            var label = commandString;
            var args = "";
        
            if (commandString.IndexOf(' ') > -1)
            {
                label = commandString.Substring(0, commandString.IndexOf(' '));
                args = commandString.Substring(commandString.IndexOf(' ') + 1);
            }

            var command = FindCommand(label);

            if (command != null)
            {
                command.Execute(args);
            }
            else
            {
                CommandConsole.Log($"<color=red>Unknown command</color> <color=#FF6666>\"{label}\"</color>");
            }

            _input.text = "";
        }

        private void UpdateSuggestion(string commandString)
        {
            var label = commandString;
            var args = "";
        
            if (commandString.IndexOf(' ') > -1)
            {
                label = commandString.Substring(0, commandString.IndexOf(' '));
                args = commandString.Substring(commandString.IndexOf(' ') + 1);
            }

            var suggestion = "";
            
            var command = FindCommand(label);

            if (command != null)
            {
                suggestion = command.Suggest(args);
            }
            else if(label != "" && args == "")
            {
                var suggestedCommand = commands.LoadedCommands.FirstOrDefault(loadedCommand => loadedCommand.Label.StartsWith(label.ToLower(), StringComparison.CurrentCultureIgnoreCase));
                if (suggestedCommand != null)
                {
                    if (suggestedCommand.Label.Length > commandString.Length)
                    {
                        suggestion = suggestedCommand.Label.Substring(commandString.Length);
                    }
                }
            }
            
            suggestionText.text = commandString + suggestion;
        }
    }
}
