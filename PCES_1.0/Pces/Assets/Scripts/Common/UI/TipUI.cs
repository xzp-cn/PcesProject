using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TipUI : MonoBehaviour
{
    Text text;
    // Use this for initialization
    void Awake()
    {
        text = GetComponentInChildren<Text>();
    }
    /// <summary>
    /// 设置提示框的信息
    /// </summary>
    /// <param name="tip"></param>
    public void SetTipMessage(string tip)
    {

        text.text = tip;
        gameObject.SetActive(true);
        Invoke("Hide", 2f);
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }
}
