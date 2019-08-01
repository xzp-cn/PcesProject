using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 调试模块
/// </summary>
public class DebugView : MonoBehaviour {
    public Dropdown qhwListUI;

    public Dropdown negQhwListUI;

    private void Awake()
    {
        GlobalEntity.GetInstance().AddListener(DebugModule.GEvt.Show, OnShow);
        GlobalEntity.GetInstance().AddListener(DebugModule.GEvt.Hide, OnHide);

        List<Dropdown.OptionData> qhwdropList = new List<Dropdown.OptionData>();
        qhwdropList.Add(new Dropdown.OptionData { text = "巧克力" });
        qhwdropList.Add(new Dropdown.OptionData { text = "海苔饼干" });
        qhwdropList.Add(new Dropdown.OptionData { text = "薯片" });
        qhwdropList.Add(new Dropdown.OptionData { text = "小汽车" });
        qhwListUI.options.Clear();
        qhwListUI.AddOptions(qhwdropList);
        qhwListUI.onValueChanged.AddListener(OnQHWValueChange);

        List<Dropdown.OptionData> negdropList = new List<Dropdown.OptionData>();
        negdropList.Add(new Dropdown.OptionData { text = "香蕉" });
        negdropList.Add(new Dropdown.OptionData { text = "苹果" });
        negdropList.Add(new Dropdown.OptionData { text = "雪饼" });

        negQhwListUI.options.Clear();
        negQhwListUI.AddOptions(qhwdropList);
        negQhwListUI.onValueChanged.AddListener(OnNegQHWValueChange);
    }

    private void OnQHWValueChange(int val)
    {
        UnityEngine.Debug.Log("DebugView::OnQHWValueChange():" + val);
        DebugModule.GetInstance().ChoiceIndex = val;
    }

    private void OnNegQHWValueChange(int val)
    {
        UnityEngine.Debug.Log("DebugView::OnNegQHWValueChange():" + val);
    }

    void Start () {

    }

    void Update () {

    }

    public void Dispose()
    {

    }

    private void OnDestroy()
    {
        GlobalEntity.GetInstance().RemoveListener(DebugModule.GEvt.Show, OnShow);
        GlobalEntity.GetInstance().RemoveListener(DebugModule.GEvt.Hide, OnHide);
    }

    private void OnHide()
    {
        this.gameObject.SetActive(false);
    }

    private void OnShow()
    {
        this.gameObject.SetActive(true);
    }
}
