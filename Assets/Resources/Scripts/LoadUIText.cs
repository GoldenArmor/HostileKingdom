using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadUIText : MonoBehaviour
{
    private Text text;
    public string key;
    private void Start()
    {
        TextData.AddUIText(this);
    }

    public void LoadText()
    {
        if(text == null) text = GetComponent<Text>();
        text.text = TextData.GetText(key);
    }
}
