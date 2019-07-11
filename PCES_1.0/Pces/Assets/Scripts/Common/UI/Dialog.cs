using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 对话提示
/// </summary>
public class Dialog : MonoBehaviour
{
    Text upText, contentText;
    private void Awake()
    {
        upText = GetComponent<Text>();
        contentText = transform.Find("Image/Text").GetComponent<Text>();
    }
    public void Show(bool _show)
    {
        gameObject.SetActive(_show);
    }
    public void SetDialogMessage(string msg)
    {
        Show(true);
        upText.text = msg;
        contentText.text = msg;
    }

}
