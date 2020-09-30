using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace CommandConsole {
    [CreateAssetMenu(menuName = "Console/Command List")]
    public class CommandList : ScriptableObject {

        [NonSerialized]
        public readonly List<ICommand> LoadedCommands = new List<ICommand>();

        public void LoadCommands() {
            LoadedCommands.Clear();

            var commandTypes = Assembly.GetAssembly(typeof(ICommand)).GetTypes()
                .Where(t => t != typeof(ICommand) && typeof(ICommand).IsAssignableFrom(t));
            
            CommandConsole.Log($"Loading {commandTypes.Count()} commands");

            var objects = new List<ICommand>();
            foreach(var type in commandTypes) {
                CommandConsole.Log($"Loading command {type.FullName}");
                var commandInstance = (ICommand)Activator.CreateInstance(type);

                LoadedCommands.Add(commandInstance);
            }
        }
        
    }
}
