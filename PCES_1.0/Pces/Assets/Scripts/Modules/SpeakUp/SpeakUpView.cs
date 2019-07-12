using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 第四阶段--句型表达
/// </summary>
public class SpeakUpView : MonoBehaviour {
    //1-3关控制器
    SpeakUpCtrlA spaCtrl;
    SpeakUpCtrlB spbCtrl;
    SpeakUpCtrlC spcCtrl;

    void Start () {
        ClickDispatcher.Inst.SetCurrentCamera(Camera.main);
        UnityEngine.Debug.Log("SpeakUpView::Awake(): 第四阶段 句型表达");

        CommonUI com = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        com.SetComUITitle("第四阶段 句型表达");

        UnityEngine.Debug.Log("SpeakUpView::Start(): 第四阶段 第一关 句型表达");
        spaCtrl = ResManager.GetPrefab("Prefabs/SpeakUp/SpeakUpA").GetComponent<SpeakUpCtrlA>();
        spaCtrl.evtFinished += OnSpaCtrlFinished;
        spaCtrl.evtRedo += OnSpaCtrlRedo;

        //dpcCtrl = ResManager.GetPrefab("Prefabs/DistinguishPicture/DistinguishPictureC").GetComponent<DistinguishPictureCtrlC>();
        //dpcCtrl.evtFinished += OnDpcCtrlFinished;

        InitPersonsState();
    }

    private void OnSpaCtrlRedo()
    {
    }

    private void OnSpaCtrlFinished()
    {
    }

    /// <summary>
    /// 初始化人物状态
    /// </summary>
    void InitPersonsState()
    {
        ///小华初始状态
        GameObject xiaohuaGo = PeopleManager.Instance.GetPeople("XH_BD");
        xiaohuaGo.transform.localPosition = Vector3.zero;


        ///辅导老师
        PeopleManager.Instance.GetPeople("FDLS_BD");

        ///初始化老师状态
    }
}
