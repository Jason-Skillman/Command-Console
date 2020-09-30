using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConsoleOpener : MonoBehaviour
{
    public TMP_InputField commandInput;
    public GameObject consoleCanvas;
    
    //private Canvas _consoleCanvas;
    private CanvasGroup _consoleGroup;

    private void Start()
    {
        //_consoleCanvas = consoleCanvas.GetComponent<Canvas>();
        _consoleGroup = consoleCanvas.GetComponent<CanvasGroup>();
    }
    
    private void OnOpenConsole()
    {
        var consoleEnabled = !_consoleGroup.interactable;
        
        //_consoleCanvas.enabled = consoleEnabled;
        _consoleGroup.alpha = consoleEnabled ? 1f : 0f;
        _consoleGroup.interactable = consoleEnabled;
        _consoleGroup.blocksRaycasts = consoleEnabled;

        if (consoleEnabled)
        {
            commandInput.Select();
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
            commandInput.OnDeselect(null);
        }
    }
}
