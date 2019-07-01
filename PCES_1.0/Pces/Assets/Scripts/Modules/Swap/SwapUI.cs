using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapUI : MonoBehaviour
{
    public System.Action chooseEvent, speakEvent;
    Button chooseBtn;
    Button microButton;
    public GameObject GetChooseBtn
    {
        get
        {
            return chooseBtn.gameObject;
        }
    }
    public GameObject GetMicroBtn
    {
        get
        {
            return microButton.gameObject;
        }
    }
    private void Awake()
    {
        chooseBtn = transform.Find("chooseButton").GetComponent<Button>();
        microButton = transform.Find("microphone").GetComponent<Button>();
        chooseBtn.onClick.AddListener(OnChooseBtnClick);
        microButton.onClick.AddListener(OnSpeakBtnClick);
        RectTransform rt = GetComponent<RectTransform>();
        rt.offsetMax = Vector2.zero;
        rt.offsetMin = Vector2.zero;
    }
    void OnChooseBtnClick()
    {

        if (chooseEvent != null)
        {
            chooseEvent();
        }
    }
    void OnSpeakBtnClick()
    {
        if (speakEvent != null)
        {
            speakEvent();
        }
    }
    public void SetButtonVisiable(BtnName _name, bool show)
    {
        if (_name == BtnName.chooseButton)
        {
            chooseBtn.gameObject.SetActive(show);
        }
        else if (_name == BtnName.microButton)
        {
            microButton.gameObject.SetActive(show);
        }
    }
    public enum BtnName
    {
        /// <summary>
        /// 选择强化物按钮
        /// </summary>
        chooseButton = 0,
        /// <summary>
        /// 麦克风按钮
        /// </summary>
        microButton = 1
    }

    private void OnDestroy()
    {
        if (chooseEvent != null)
        {
            chooseEvent = null;
        }

        if (speakEvent != null)
        {
            speakEvent = null;
        }
    }
}
