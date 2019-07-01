using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsBgCtrl : MonoBehaviour
{
    private Image bg;
    public Text tips;
    void Awake()
    {
        bg = transform.GetComponent<Image>();
    }

    void Update()
    {
        if (bg != null)
        {
            bg.enabled = string.IsNullOrEmpty(tips.text) ? false : true;
        }
    }
}
