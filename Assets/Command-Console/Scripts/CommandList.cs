using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Console.Scripts
{
    [CreateAssetMenu(menuName = "Console/Command List")]
    public class CommandList : ScriptableObject
    {
        
        //#if UNITY_EDITOR
        //public List<MonoScript> commands;
    
        [NonSerialized]
        public readonly List<ICommand> LoadedCommands = new List<ICommand>();
    
        public void LoadCommands()
        {
            LoadedCommands.Clear();

            var commandTypes = Assembly.GetAssembly(typeof(ICommand)).GetTypes().Where(t => t != typeof(ICommand) && 
                                                                                            typeof(ICommand).IsAssignableFrom(t));
                //Assembly.GetAssembly(typeof(ICommand)).GetTypes()
                //.Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(ICommand)));
            
            CommandConsole.Log($"Loading {commandTypes.Count()} commands");
            
            var objects = new List<ICommand>();
            foreach (var type in commandTypes)
            {
                CommandConsole.Log($"Loading command {type.FullName}");
                var commandInstance = (ICommand) Activator.CreateInstance(type);
            
                LoadedCommands.Add(commandInstance);
            }
            
            /*foreach (var command in commands)
            {
                var commandInstance = (ICommand) Activator.CreateInstance(command.GetClass());
            
                LoadedCommands.Add(commandInstance);
            }*/
        }
        //#else
        /*
        [NonSerialized]
        public readonly List<ICommand> LoadedCommands = new List<ICommand> {
            new GenerateCommand(),
            new PrintCommand(),
            new ConnectCommand(),
            new SetUsernameCommand(),
            new GiveSkillCommand()
        };
    
        public void LoadCommands()
        {
        }
        //#endif
        */
    }
}
