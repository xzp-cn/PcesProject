using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectUI : MonoBehaviour
{
    Button okBtn, closeBtn;
    Toggle[] toggles;
    Transform tip;
    int selectedObjIndex = -1;
    int lastIndex = -1;
    public event System.Action<int> okEvent;
    public event System.Action closeEvent;
    Dictionary<string, Texture> spDic;
    ToggleGroup tgroup;
    private void Awake()
    {
        gameObject.SetActive(false);
        tgroup = GetComponentInChildren<ToggleGroup>();
        tgroup.allowSwitchOff = true;
        ToggleGroupOff();
        //RectTransform rt = GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        //ToggleGroupOff();
        transform.SetAsLastSibling();
    }
    private void Start()
    {
        lastIndex = selectedObjIndex = -1;
        tip = transform.Find("panel/tip");
        tip.gameObject.SetActive(false);
        okBtn = transform.Find("panel/okBtn").GetComponent<Button>();
        closeBtn = transform.Find("panel/closeBtn").GetComponent<Button>();
        toggles = transform.Find("panel/toggles").GetComponentsInChildren<Toggle>();
        for (int i = 0; i < toggles.Length; i++)
        {
            int index = i;
            toggles[i].onValueChanged.AddListener(delegate (bool isOn)
            {
                OnValueChanged(isOn, index);
            });
        }
        okBtn.onClick.AddListener(OnOkBtnClick);
        closeBtn.onClick.AddListener(OnCloseBtnClick);
    }

    void OnOkBtnClick()
    {
        if (selectedObjIndex >= 0)
        {
            gameObject.SetActive(false);
            if (okEvent != null)
            {
                if (lastIndex != selectedObjIndex)
                {
                    lastIndex = selectedObjIndex;
                    okEvent(selectedObjIndex);
                }
            }
        }
        else
        {
            tip.gameObject.SetActive(true);
            Invoke("HideTipText", 0.5f);
        }
    }
    void OnCloseBtnClick()
    {
        gameObject.SetActive(false);
        if (closeEvent != null)
        {
            closeEvent();
        }
    }
    void OnValueChanged(bool isOn, int ObjIndex)
    {
        if (isOn)
        {
            selectedObjIndex = ObjIndex;
        }
        else
        {
            selectedObjIndex = -1;
        }
    }
    void HideTipText()
    {
        tip.gameObject.SetActive(false);
    }
    void ToggleGroupOff()
    {
        tgroup.SetAllTogglesOff();
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    private void OnDestroy()
    {
        //toggleEvent = null;
        okEvent = null;
        closeEvent = null;

    }
}
