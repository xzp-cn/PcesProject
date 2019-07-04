using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromptHelper : SingleTon<PromptHelper>{
    public Text promptUI;

    public void SetText(string txt)
    {
        promptUI.text = txt;
    }
}
