using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectionUI : MonoBehaviour
{
    Button okBtn;
    Toggle[] toggles;
    Transform tip;
    int selectedObjIndex = -1;
    public event System.Action<int> okEvent;
    Dictionary<string, Texture> spDic;
    private void Awake()
    {
        gameObject.SetActive(false);
        //RectTransform rt = GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        selectedObjIndex = -1;
    }
    private void Start()
    {
        tip = transform.Find("panel/tip");
        tip.gameObject.SetActive(false);
        okBtn = transform.Find("panel/okBtn").GetComponent<Button>();
        toggles = transform.Find("panel/toggles").GetComponentsInChildren<Toggle>();
        if (spDic == null)
        {
            spDic = new Dictionary<string, Texture>();
            RawImage[] rws = transform.GetComponentInChildren<ToggleGroup>().GetComponentsInChildren<RawImage>();
            for (int i = 0; i < 5; i++)
            {
                //toggles[i].name = i.ToString();
                string path = "Images/tuka/tuka_" + ((PropsTag)i).ToString();
                Texture texture = ResManager.GetTexture(path);
                spDic.Add(((PropsTag)i).ToString(), texture);
                rws[i].texture = texture;
            }
        }
        for (int i = 0; i < toggles.Length; i++)
        {
            int index = i;
            toggles[i].onValueChanged.AddListener(delegate (bool isOn)
            {
                OnValueChanged(isOn, index);
            });
        }
        okBtn.onClick.AddListener(OnOkBtnClick);
    }
    /// <summary>
    /// 3D物体显示
    /// </summary>
    void ShowObj()
    {
        //for (int i = 0; i < length; i++)
        //{

        //}
    }
    void OnOkBtnClick()
    {
        if (selectedObjIndex >= 0)
        {
            //Debug.Log(((PropsTag)selectedObjIndex).ToString());
            gameObject.SetActive(false);
            if (okEvent != null)
            {
                okEvent(selectedObjIndex);
            }
        }
        else
        {
            tip.gameObject.SetActive(true);
            Invoke("HideTipText", 0.5f);
        }
    }
    void OnValueChanged(bool isOn, int ObjIndex)
    {
        if (isOn)
        {
            selectedObjIndex = ObjIndex;
            //Debug.Log(selectedObjName);
        }
    }
    void HideTipText()
    {
        tip.gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    private void OnDestroy()
    {
        //toggleEvent = null;
    }
}
