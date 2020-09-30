using System.Collections.Generic;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DebugCommandConsole {
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

        private bool isPlaying = false;
        private Thread mainThread;

        private static List<string> newLogMessages = new List<string>();

        public bool IsOpen { get; private set; }

        private void Awake() {
            if(Instance == null) Instance = this;
            else Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
            
            isPlaying = Application.isPlaying;
            mainThread = Thread.CurrentThread;
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
            }

            if(newLogMessages.Count <= 0)
                return;

            var newLogs = newLogMessages;
            newLogMessages = new List<string>();

            foreach(var log in newLogs) {
                AddToGameConsole(log);
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

        #region DebugLogs

        public void AddToGameConsole(string str) {
            if(Thread.CurrentThread == mainThread) {
                if(isPlaying) {
                    var output = Instantiate(outputPrefab, commandOutput);
                    output.GetComponentInChildren<TMP_Text>().text = str;

                    LayoutRebuilder.ForceRebuildLayoutImmediate(commandOutput);
                }
            } else {
                newLogMessages.Add(str);
            }
        }

        /// <summary>
        /// Used to simple log text to the console
        /// </summary>
        /// <param name="args"></param>
        public void Log(params object[] args) {
            StringBuilder sb = new StringBuilder();

            for(var i = 0; i < args.Length; i++) {
                if(i != 0) {
                    sb.Append(" ");
                }
                sb.Append(args[i]);
            }

            AddToGameConsole(sb.ToString());
        }

        /*public static void Warn(params object[] args) {
            var sb = new StringBuilder();
            sb.Append("<color=yellow>[WARN] ");
            for(var i = 0; i < args.Length; i++) {
                if(i != 0) {
                    sb.Append(" ");
                }
                sb.Append(args[i]);
            }
            sb.Append("</color>");

            Log(sb.ToString());
        }*/

        /*public static void Error(params object[] args) {
            var sb = new StringBuilder();
            sb.Append("<color=red>[ERROR] ");
            for(var i = 0; i < args.Length; i++) {
                if(i != 0) {
                    sb.Append(" ");
                }
                sb.Append(args[i]);
            }
            sb.Append("</color>");

            Log(sb.ToString());
        }*/

        #endregion

        

    }
}