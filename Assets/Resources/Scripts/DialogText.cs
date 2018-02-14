using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogText : MonoBehaviour
{
    private Text text;
    private int dialogLine = 0;
	// Use this for initialization
	void Start ()
    {
        text = GetComponentInChildren<Text>();
        TextData.AddDialogText(this);
	}

    public void NextLine()
    {
        dialogLine++;
        if(dialogLine > 2) dialogLine = 1;

        text.text = TextData.GetText("dialog_" + dialogLine.ToString("0"));
    }

    public void UpdateDialogLine()
    {
        text.text = TextData.GetText("dialog_" + dialogLine.ToString("0"));
    }
}
