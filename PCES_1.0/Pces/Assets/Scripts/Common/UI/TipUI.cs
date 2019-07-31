using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[ExecuteInEditMode]
public class TipUI : MonoBehaviour
{
    Text text;
    //RectTransform rt;
    // Use this for initialization
    void Awake()
    {
        text = GetComponentInChildren<Text>();
        //rt = transform.Find("bg").GetComponent<RectTransform>();
    }
    /// <summary>
    /// 设置提示框的信息
    /// </summary>
    /// <param name="tip"></param>
    public void SetTipMessage(string tip)
    {
        transform.SetAsLastSibling();
        text.text = tip;
        gameObject.SetActive(true);
        Invoke("Hide", 2f);
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }
}
