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
    public void SetPos(Vector3? pos=null)
    {
        if (pos!=null)
        {
            transform.localPosition =(Vector3)pos;                
        }
        transform.localPosition = new Vector3();
    }
    public void Show(bool _show = false)
    {
        gameObject.SetActive(_show);
    }
    public void SetDialogMessage(string msg)
    {
        Show(true);
        upText.text = msg;
        contentText.text = msg;
        HideCtrl(2);
    }
    /// <summary>
    /// 对话框等待一定时间消失。
    /// </summary>
    public void HideCtrl(float delay)
    {
        Invoke("Hide", delay);
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }
}
