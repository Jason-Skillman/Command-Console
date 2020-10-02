using System.Collections.Generic;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CommandConsole.Console {
    public partial class CommandConsole : MonoBehaviour {

        public static CommandConsole Instance { get; private set; }

        [Header("References")]
        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]
        private TMP_InputField inputField;
        [SerializeField]
        private RectTransform commandOutput;
        [SerializeField]
        private GameObject outputPrefab;

        private StringBuilder sb;
        private static List<string> newLogMessages = new List<string>();

        public bool IsOpen { get; private set; }

        private void Awake() {
            if(Instance == null) Instance = this;
            else Destroy(gameObject);
            DontDestroyOnLoad(gameObject);

            sb = new StringBuilder();
        }

        private void Start() {
            //Start the console closed
            Close();

            //Call the second half of the start method
            Processor_Start();
        }

        private void Update() {
            //Toggle the command console
            if(Input.GetKeyDown(KeyCode.BackQuote)) {
                Toggle();

                inputField.interactable = false;
            } else if(Input.GetKeyUp(KeyCode.BackQuote)) {
                inputField.interactable = true;
                inputField.text = string.Empty;
            }
        }

        #region OpenAndClose

        public void Open() {
            IsOpen = true;

            canvasGroup.alpha = 1.0f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            //Select the input field
            inputField.Select();
        }

        public void Close() {
            IsOpen = false;

            canvasGroup.alpha = 0.0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            //Deselect the input field
            inputField.OnDeselect(null);
            EventSystem.current.SetSelectedGameObject(null);
        }

        public void Toggle() {
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