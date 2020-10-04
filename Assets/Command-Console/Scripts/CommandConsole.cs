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

        [SerializeField]
        [Tooltip("~ key to open. Uses Unity's old input manager.")]
        private bool useTildeToOpen = true;
        [SerializeField]
        [Tooltip("Shows all of the commands that were succesfully loaded into the console on start.")]
        private bool showLoadedCommandsOnStart = true;

        [SerializeField]
        [Header("References")]
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
        private string previousCommandText;

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
            //Todo: Fixes a bug where the "`" key is present on the input field when opening the console for the first time
            //Clear the input field of the key used to open/close the console
            inputField.text = inputField.text.Replace("`", string.Empty);

            //Use tilde key to open/close the console
            if(useTildeToOpen && Input.GetKeyDown(KeyCode.BackQuote)) {
                Toggle();
            }
            //Use tab to fill in the command text
            else if(Input.GetKeyDown(KeyCode.Tab)) {
                FillInByCurrentSuggestion();
            }
            //Use up arrow to use the previously used command
            else if(Input.GetKeyDown(KeyCode.UpArrow)) {
                PreviousCommand();
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
            if(showLoadedCommandsOnStart) {
                Log($"Loading {commandTypes.Count()} commands");
                foreach(Type type in commandTypes) {
                    Log($" - {type.FullName}");

                    //Create an instance of the command and add it to the loaded commands list
                    ICommand commandInstance = (ICommand)Activator.CreateInstance(type);
                    loadedCommands.Add(commandInstance);
                }
            }
        }

        /// <summary>
        /// Fills in the rest of the command based on the current suggestion text
        /// </summary>
        public void FillInByCurrentSuggestion() {
            string newText = suggestionBuilder.ToString(); ;

            if(!newText.Equals(string.Empty)) {
                inputField.text = suggestionBuilder.ToString();

                inputField.text = inputField.text.Replace(" ", string.Empty);
                inputField.text += " ";

                inputField.caretPosition = newText.ToCharArray().Length;
            }
        }

        /// <summary>
        /// Fills in the input field text with the last used command
        /// </summary>
        public void PreviousCommand() {
            if(previousCommandText.Equals(string.Empty)) return;

            inputField.text = previousCommandText;
            inputField.caretPosition = previousCommandText.ToCharArray().Length;
        }

        #region OpenAndClose

        /// <summary>
        /// Opens the console.
        /// </summary>
        public void Open() {
            IsOpen = true;

            //Enable the canvas group
            canvasGroup.alpha = 1.0f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            //Enable the input field
            inputField.interactable = true;
            inputField.Select();

            //Clear the input field of the key used to open/close the console
            inputField.text = inputField.text.Replace("`", string.Empty);
        }

        /// <summary>
        /// Closes the console.
        /// </summary>
        public void Close() {
            IsOpen = false;

            //Disable the canvas group
            canvasGroup.alpha = 0.0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            //Disable the input field
            inputField.interactable = false;
            inputField.OnDeselect(null);
            EventSystem.current.SetSelectedGameObject(null);

            //Clear the input field of the key used to open/close the console
            inputField.text = inputField.text.Replace("`", string.Empty);
        }

        /// <summary>
        /// Toggles opening and closing the console.
        /// </summary>
        public void Toggle() {
            if(IsOpen) Close();
            else Open();
        }

        #endregion

        #region Logs

        /// <summary>
        /// Prints a log message to the console.
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
        /// Prints a warning message to the console.
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
        /// Prints an error message to the console.
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