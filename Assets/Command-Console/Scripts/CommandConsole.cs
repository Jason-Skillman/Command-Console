using System.Collections.Generic;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Reflection;
using System.Linq;

namespace DebugCommandConsole {
    public partial class CommandConsole : MonoBehaviour {

        public static CommandConsole Instance { get; private set; }

        [Header("References")]
        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]
        private TMP_InputField inputField;
        [SerializeField]
        public TMP_Text suggestionText;
        [SerializeField]
        private RectTransform commandOutput;
        [SerializeField]
        private GameObject outputPrefab;
        
        private StringBuilder sb;
        private StringBuilder suggestionBuilder;

        public bool IsOpen { get; private set; }

        /// <summary>
        /// Stores all of the loaded commands.
        /// </summary>
        protected readonly List<ICommand> loadedCommands = new List<ICommand>();

        private void Awake() {
            if(Instance == null) Instance = this;
            else Destroy(gameObject);
            DontDestroyOnLoad(gameObject);

            sb = new StringBuilder();
            suggestionBuilder = new StringBuilder();
        }

        private void Start() {
            Close();
            LoadCommands();
        }

        private void Update() {
            //Handle opening and closing the
            if(Input.GetKeyDown(KeyCode.BackQuote)) {
                Toggle();

                //Is the console closed
                if(!IsOpen) {
                    inputField.interactable = false;
                    inputField.OnDeselect(null);
                    EventSystem.current.SetSelectedGameObject(null);

                    //Cut off the last key (The key used to close the console)
                    if(inputField.text.Length - 1 >= 0)
                        inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
                }
            } else if(Input.GetKeyUp(KeyCode.BackQuote)) {
                //Is the console open
                if(IsOpen) {
                    inputField.interactable = true;
                    inputField.Select();
                }
            }
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

        #region OpenAndClose

        private void Open() {
            IsOpen = true;

            canvasGroup.alpha = 1.0f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        private void Close() {
            IsOpen = false;

            canvasGroup.alpha = 0.0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        private void Toggle() {
            if(IsOpen) Close();
            else Open();
        }

        #endregion

        #region Logs

        /// <summary>
        /// Prints a log message to the console
        /// </summary>
        /// <param name="args"></param>
        public void Log(params object[] args) {
            sb.Clear();

            for(var i = 0; i < args.Length; i++) {
                //Adds a space between multible args
                if(i != 0) sb.Append(" ");
                
                sb.Append(args[i]);
            }

            //Create the text card and set the text
            GameObject outputCard = Instantiate(outputPrefab, commandOutput);
            outputCard.GetComponentInChildren<TMP_Text>().text = sb.ToString();

            LayoutRebuilder.ForceRebuildLayoutImmediate(commandOutput);
        }

        /// <summary>
        /// Prints a warning message to the console
        /// </summary>
        /// <param name="args"></param>
        public void LogWarning(params object[] args) {
            sb.Clear();

            sb.Append("<color=yellow>[Warning] ");
            for(var i = 0; i < args.Length; i++) {
                if(i != 0) {
                    sb.Append(" ");
                }
                sb.Append(args[i]);
            }
            sb.Append("</color>");

            Log(sb.ToString());
        }

        /// <summary>
        /// Prints an error message to the console
        /// </summary>
        /// <param name="args"></param>
        public void LogError(params object[] args) {
            sb.Clear();

            sb.Append("<color=red>[Error] ");
            for(var i = 0; i < args.Length; i++) {
                if(i != 0) {
                    sb.Append(" ");
                }
                sb.Append(args[i]);
            }
            sb.Append("</color>");

            Log(sb.ToString());
        }

        #endregion

    }
}