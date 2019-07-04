﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TestPaperItem : MonoBehaviour
{
    public PaperItem pItem;
    ToggleGroup tg;
    Toggle[] togs;
    private void Awake()
    {
        tg = GetComponent<ToggleGroup>();
    }
    public void Init(PaperItem _pItem)
    {
        pItem = _pItem;
        transform.Find("question").GetComponent<Text>().text = pItem.question;
        togs = transform.GetComponentsInChildren<Toggle>();
        for (int j = 0; j < togs.Length; j++)
        {
            togs[j].transform.Find("Label").GetComponent<Text>().text = pItem.options[j];
            string answer = pItem.answer;
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
            Text txt = transform.Find("question").GetComponent<Text>();
            string str = txt.text + answer;
            txt.text = str;
            for (int i = 0; i < togs.Length; i++)
            {
                togs[i].interactable = false;
            }
        }
    }
}
