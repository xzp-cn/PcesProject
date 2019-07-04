using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPaperItem : MonoBehaviour
{
    public PaperItem pItem;
    private void Awake()
    {

    }
    public void Init(PaperItem _pItem)
    {
        pItem = _pItem;
        temp.Find("question").GetComponent<Text>().text = paper.subList[i].question;
        Toggle[] togs = temp.GetComponentsInChildren<Toggle>();
        for (int j = 0; j < togs.Length; j++)
        {
            temp.Find("a/Label").GetComponent<Text>().text = sub.subList[i].options[j];
            togs[j].onValueChanged.AddListener(delegate (bool isOn)
            {
                OnValueChanged(isOn, answer);
            });
        }
    }
    void OnValueChanged(bool isOn, string answer)
    {
        if (isOn)
        {

        }
    }
}
