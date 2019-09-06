using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectUI : MonoBehaviour
{
    Button selectBtn, okBtn, closeBtn;
    Toggle[] toggles;
    Transform tip;
    public int selectedObjIndex = -1;
    int lastIndex = -1;
    public event System.Action<int> okEvent;
    public event System.Action selectStageEvent, closeEvent;
    Dictionary<string, Texture> spDic;
    ToggleGroup tgroup;
    Transform panel;
    private void Awake()
    {
        panel = transform.Find("bg");
        tgroup = GetComponentInChildren<ToggleGroup>();
        tgroup.allowSwitchOff = true;
        ToggleGroupOff();
        panel.gameObject.SetActive(false);
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
        tip = transform.Find("bg/panel/tip");
        tip.gameObject.SetActive(false);
        okBtn = transform.Find("bg/panel/okBtn").GetComponent<Button>();
        closeBtn = transform.Find("bg/panel/closeBtn").GetComponent<Button>();
        selectBtn = transform.Find("selectBtn").GetComponent<Button>();
        toggles = transform.Find("bg/panel/toggles").GetComponentsInChildren<Toggle>();
        for (int i = 0; i < toggles.Length; i++)
        {
            int index = i + 1;
            toggles[i].onValueChanged.AddListener(delegate (bool isOn)
            {
                OnValueChanged(isOn, index);
            });
        }
        okBtn.onClick.AddListener(OnOkBtnClick);
        closeBtn.onClick.AddListener(OnCloseBtnClick);
        selectBtn.onClick.AddListener(OnSelectBtnClick);
    }

    void OnSelectBtnClick()
    {
        //Debug.Log("click");
        panel.gameObject.SetActive(true);
        if (selectStageEvent != null)
        {
            selectStageEvent();
        }
    }
    void OnOkBtnClick()
    {
        if (selectedObjIndex >= 0)
        {
            panel.gameObject.SetActive(false);
            if (okEvent != null)
            {
                //if (lastIndex != selectedObjIndex)
                //{
                //    lastIndex = selectedObjIndex;
                //    okEvent(selectedObjIndex);
                //}
                okEvent(selectedObjIndex);
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
        panel.gameObject.SetActive(false);
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
        panel.gameObject.SetActive(true);
    }
    private void OnDestroy()
    {
        //toggleEvent = null;
        okEvent = null;
        closeEvent = null;

    }
}
