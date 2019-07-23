using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 通用界面UI
/// </summary>
public class CommonUI : MonoBehaviour
{
    /// <summary>
    /// 每关结束界面
    /// </summary>
    Transform final;
    Button resetBtn;
    Button nextBtn;

    /// <summary>
    /// 重做\下一关事件
    /// </summary>
    public event System.Action redoClickEvent, nextClickEvent;

    private void Start()
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.offsetMax = Vector2.zero;
        rt.offsetMin = Vector2.zero;
        final = transform.Find("Final");
        final.gameObject.SetActive(false);
        resetBtn = final.Find("reset").GetComponent<Button>();
        resetBtn.onClick.AddListener(BtnResetClick);
        nextBtn = final.Find("next").GetComponent<Button>();
        nextBtn.onClick.AddListener(NextBtnClick);
    }
    public void BtnResetClick()
    {
        final.gameObject.SetActive(false);
        if (redoClickEvent != null)
        {
            redoClickEvent();
        }
    }
    public void NextBtnClick()
    {
        final.gameObject.SetActive(false);
        if (nextClickEvent != null)
        {
            nextClickEvent();
        }
    }

    /// <summary>
    /// 下一关和重做按钮的显示隐藏
    /// </summary>
    public void ShowFinalUI()
    {
        UIManager.Instance.SetUIDepthTop("CommonUI");
        final.gameObject.SetActive(true);
    }

    public void HideFinalUI()
    {
        final.gameObject.SetActive(false);
    }

    Text topTitle;
    public void SetComUITitle(string title)
    {
        if (topTitle == null)
        {
            topTitle = transform.Find("top/title/text").GetComponent<Text>();
        }
        topTitle.text = title;
    }
    private void OnDestroy()
    {
        if (resetBtn != null)
        {
            resetBtn.onClick.RemoveListener(BtnResetClick);
        }
        if (nextBtn != null)
        {
            nextBtn.onClick.RemoveListener(NextBtnClick);
        }
    }
}
