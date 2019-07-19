using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TestPaperItem : MonoBehaviour
{
    public PaperItem pItem;
    public Sprite right;
    public Sprite wrong;
    ToggleGroup tg;
    Toggle[] togs;
    public int item_right = 0;//统计正确
    public int item_wrong = 0;//统计错误
    private void Awake()
    {
        tg = GetComponent<ToggleGroup>();
        tg.allowSwitchOff = true;
    }
    public void Init(PaperItem _pItem)
    {
        pItem = _pItem;
        transform.Find("question").GetComponent<Text>().text = pItem.question;
        transform.Find("question/answer").GetComponent<Text>().text = string.Empty;
        togs = transform.GetComponentsInChildren<Toggle>();
        for (int j = 0; j < togs.Length; j++)
        {
            togs[j].transform.Find("Label").GetComponent<Text>().text = pItem.options[j];
            string answer = pItem.answer;//正确答案
            string tName = togs[j].name;
            togs[j].onValueChanged.AddListener(delegate (bool isOn)
            {
                OnValueChanged(isOn, answer, tName);
            });
        }
    }
    void OnValueChanged(bool isOn, string answer, string togName)
    {
        if (isOn)
        {
            Text txt = transform.Find("question/answer").GetComponent<Text>();
            txt.text = answer;
            Image img = transform.Find(togName + "/Background/Checkmark").GetComponent<Image>();
            img.sprite = answer == togName ? right : wrong;
            if (answer == togName)
            {
                item_right = 1;
            }
            else
            {
                item_wrong = 1;
            }
            img.SetNativeSize();
            for (int i = 0; i < togs.Length; i++)
            {
                togs[i].interactable = false;
            }
        }
    }
    public void ResetAll()//重置数据
    {
        for (int i = 0; i < togs.Length; i++)
        {
            transform.Find("question/answer").GetComponent<Text>().text = string.Empty;
            togs[i].isOn = false;
            togs[i].interactable = true;
        }
        item_right = 0;
        item_wrong = 0;
    }
}
