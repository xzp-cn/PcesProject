using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 第四阶段--句型表达
/// </summary>
public class SpeakUpView : MonoBehaviour
{
    //1-3关控制器
    SpeakUpCtrlA spaCtrl;
    SpeakUpCtrlB spbCtrl;
    SpeakUpCtrlC spcCtrl;

    TestPaperView tpv; //测试题

    void Start()
    {
        ClickDispatcher.Inst.SetCurrentCamera(Camera.main);
        UnityEngine.Debug.Log("SpeakUpView::Awake(): 第四阶段 句型表达");

        CommonUI com = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        com.SetComUITitle("第四阶段 句型表达");

        UnityEngine.Debug.Log("SpeakUpView::Start(): 第四阶段 第一关 句型表达");
        //spaCtrl = ResManager.GetPrefab("Prefabs/SpeakUp/SpeakUpA").GetComponent<SpeakUpCtrlA>();
        //spaCtrl.evtFinished += OnSpaCtrlFinished;
        //spaCtrl.evtRedo += OnSpaCtrlRedo;

        //spbCtrl = ResManager.GetPrefab("Prefabs/SpeakUp/SpeakUpB").GetComponent<SpeakUpCtrlB>();
        //spbCtrl.evtFinished += OnSpbCtrlFinished;
        //spbCtrl.evtRedo += OnSpbCtrlRedo;

        //spcCtrl = ResManager.GetPrefab("Prefabs/SpeakUp/SpeakUpC").GetComponent<SpeakUpCtrlC>();
        //spcCtrl.evtFinished += OnSpcCtrlFinished;
        //spcCtrl.evtRedo += OnSpcCtrlRedo;

        InitPersonsState();
    }

    private void OnSpaCtrlRedo()
    {
        spaCtrl.evtFinished -= OnSpaCtrlFinished;
        spaCtrl.evtRedo -= OnSpaCtrlRedo;
        spaCtrl.Dispose();

        spaCtrl = ResManager.GetPrefab("Prefabs/SpeakUp/SpeakUpA").GetComponent<SpeakUpCtrlA>();
        spaCtrl.evtFinished += OnSpaCtrlFinished;
        spaCtrl.evtRedo += OnSpaCtrlRedo;
    }

    private void OnSpaCtrlFinished()
    {
        Debug.Log("SpeakUpView.OnSpaCtrlFinished(): 第四阶段 第一关 句型表达 下一关!!!");
        spaCtrl.evtFinished -= OnSpaCtrlFinished;
        spaCtrl.evtRedo -= OnSpaCtrlRedo;
        spaCtrl.Dispose();

        spbCtrl = ResManager.GetPrefab("Prefabs/SpeakUp/SpeakUpB").GetComponent<SpeakUpCtrlB>();
        spbCtrl.evtFinished += OnSpbCtrlFinished;
        spbCtrl.evtRedo += OnSpbCtrlRedo;
    }

    private void OnSpbCtrlRedo()
    {
        spbCtrl.evtFinished -= OnSpbCtrlFinished;
        spbCtrl.evtRedo -= OnSpbCtrlRedo;
        spbCtrl.Dispose();

        spbCtrl = ResManager.GetPrefab("Prefabs/SpeakUp/SpeakUpB").GetComponent<SpeakUpCtrlB>();
        spbCtrl.evtFinished += OnSpbCtrlFinished;
        spbCtrl.evtRedo += OnSpbCtrlRedo;
    }

    private void OnSpbCtrlFinished()
    {
        spbCtrl.evtFinished -= OnSpbCtrlFinished;
        spbCtrl.evtRedo -= OnSpbCtrlRedo;
        spbCtrl.Dispose();

        spcCtrl = ResManager.GetPrefab("Prefabs/SpeakUp/SpeakUpC").GetComponent<SpeakUpCtrlC>();
        spcCtrl.evtFinished += OnSpcCtrlFinished;
        spcCtrl.evtRedo += OnSpcCtrlRedo;
    }

    private void OnSpcCtrlRedo()
    {
        spcCtrl.evtFinished -= OnSpcCtrlFinished;
        spcCtrl.evtRedo -= OnSpcCtrlRedo;
        spcCtrl.Dispose();

        spcCtrl = ResManager.GetPrefab("Prefabs/SpeakUp/SpeakUpC").GetComponent<SpeakUpCtrlC>();
        spcCtrl.evtFinished += OnSpcCtrlFinished;
        spcCtrl.evtRedo += OnSpcCtrlRedo;
    }

    private void OnSpcCtrlFinished()
    {
        spcCtrl.evtFinished -= OnSpcCtrlFinished;
        spcCtrl.evtRedo -= OnSpcCtrlRedo;
        spcCtrl.Dispose();

        tpv = ResManager.GetPrefab("Prefabs/UI/TestPaperView").GetComponent<TestPaperView>();
        tpv.evtFinished += OnTestPaperFinished;
        tpv.evtRedo += OnTestPaperRedo;
    }

    void OnTestPaperRedo()
    {
        tpv.evtFinished -= OnTestPaperFinished;
        tpv.evtRedo -= OnTestPaperRedo;
        tpv.Dispose();
        tpv = ResManager.GetPrefab("Prefabs/UI/TestPaperView").GetComponent<TestPaperView>();
        tpv.evtFinished += OnTestPaperFinished;
        tpv.evtRedo += OnTestPaperRedo;
    }
    void OnTestPaperFinished()
    {
        tpv.evtFinished -= OnTestPaperFinished;
        tpv.evtRedo -= OnTestPaperRedo;
        tpv.Dispose();
        //通知当前阶段完成
        OnSpeakUpFinished();
    }

    void OnSpeakUpFinished()
    {
        //第四阶段完成
        GlobalEntity.GetInstance().Dispatch<ModelTasks>(FlowModel.mEvent.FlowStepFinished, ModelTasks.SpeakUp);
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

    public void Dispose()
    {
        if (spaCtrl != null)
        {
            spaCtrl.Dispose();
        }
        if (spbCtrl != null)
        {
            spbCtrl.Dispose();
        }
        if (spcCtrl != null)
        {
            spcCtrl.Dispose();
        }
        if (tpv != null)
        {
            tpv.Dispose();
        }
        Destroy(gameObject);
    }
}
