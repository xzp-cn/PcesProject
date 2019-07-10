﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentenceExpressionView : MonoBehaviour
{
    public SentenceCtrlA stACtrl;
    public SentenceCtrlB stBCtrl;
    public SentenceCtrlC stCCtrl;
    public SentenceCtrlD stDCtrl;
    public TestPaperView tpv;
    CommonUI com;
    private void Awake()
    {
        ClickDispatcher.Inst.SetCurrentCamera(Camera.main);
        UnityEngine.Debug.Log("SentenceExpressionView::Awake(): 第六阶段 增强自发性沟通");
        this.name = "SentenceExpressionView";
    }
    private void Start()
    {
        com = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        com.SetComUITitle("第六阶段 增强自发性沟通");
        Canvas canvas = FindObjectOfType<Canvas>();
        com.transform.SetParent(canvas.transform);
        //stACtrl = ResManager.GetPrefab("Prefabs/SentenceExpression/SentenceCtrlA").GetComponent<SentenceCtrlA>();
        //stACtrl.transform.SetParent(transform);
        //stACtrl.evtFinished += OnstACtrlFinished;
        //stACtrl.evtRedo += OnstACtrlRedo;
        //stBCtrl = ResManager.GetPrefab("Prefabs/SentenceExpression/SentenceCtrlB").GetComponent<SentenceCtrlB>();
        //stBCtrl.transform.SetParent(transform);
        //stBCtrl.evtFinished += OnstBCtrlFinished;
        //stBCtrl.evtRedo += OnstBCtrlRedo;
        stCCtrl = ResManager.GetPrefab("Prefabs/SentenceExpression/SentenceCtrlC").GetComponent<SentenceCtrlC>();
        stCCtrl.transform.SetParent(transform);
        stCCtrl.evtFinished += OnstCCtrlFinished;
        stCCtrl.evtRedo += OnstCCtrlRedo;
        //tpv = ResManager.GetPrefab("Prefabs/UI/TestPaperView").GetComponent<TestPaperView>();
        //tpv.transform.SetParent(transform);
        //tpv.evtFinished += OnTestPaperFinished;
        //tpv.evtRedo += OnTestPaperRedo;
    }
    void OnstACtrlFinished()
    {
        stACtrl.evtFinished -= OnstACtrlFinished;
        stACtrl.evtRedo -= OnstACtrlRedo;
        stACtrl.Dispose();
        stBCtrl = ResManager.GetPrefab("Prefabs/SentenceExpression/SentenceCtrlB").GetComponent<SentenceCtrlB>();
        stBCtrl.transform.SetParent(transform);
        stBCtrl.evtFinished += OnstBCtrlFinished;
        stBCtrl.evtRedo += OnstBCtrlRedo;
    }
    void OnstACtrlRedo()
    {
        stACtrl.evtFinished -= OnstACtrlFinished;
        stACtrl.evtRedo -= OnstACtrlRedo;
        stACtrl.Dispose();
        stACtrl = ResManager.GetPrefab("Prefabs/SentenceExpression/SentenceCtrlA").GetComponent<SentenceCtrlA>();
        stACtrl.transform.SetParent(transform);
        stACtrl.evtFinished += OnstACtrlFinished;
        stACtrl.evtRedo += OnstACtrlRedo;
    }
    private void OnstBCtrlFinished()
    {
        stBCtrl.evtFinished -= OnstBCtrlFinished;
        stBCtrl.evtRedo -= OnstBCtrlRedo;
        stBCtrl.Dispose();
        stCCtrl = ResManager.GetPrefab("Prefabs/SentenceExpression/SentenceCtrlC").GetComponent<SentenceCtrlC>();
        stCCtrl.transform.SetParent(transform);
        stCCtrl.evtFinished += OnstCCtrlFinished;
        stCCtrl.evtRedo += OnstCCtrlRedo;
    }
    void OnstBCtrlRedo()
    {
        stBCtrl.evtFinished -= OnstBCtrlFinished;
        stBCtrl.evtRedo -= OnstBCtrlRedo;
        stBCtrl.Dispose();
        stBCtrl = ResManager.GetPrefab("Prefabs/SentenceExpression/SentenceCtrlB").GetComponent<SentenceCtrlB>();
        stBCtrl.transform.SetParent(transform);
        stBCtrl.evtFinished += OnstBCtrlFinished;
        stBCtrl.evtRedo += OnstBCtrlRedo;
    }
    private void OnstCCtrlFinished()
    {
        stCCtrl.evtFinished -= OnstCCtrlFinished;
        stCCtrl.evtRedo -= OnstCCtrlRedo;
        stCCtrl.Dispose();
        stDCtrl = ResManager.GetPrefab("Prefabs/SentenceExpression/SentenceCtrlD").GetComponent<SentenceCtrlD>();
        stDCtrl.transform.SetParent(transform);
        stDCtrl.evtFinished += OnecDCtrlFinished;
        stDCtrl.evtRedo += OnecDCtrlRedo;
    }
    private void OnstCCtrlRedo()
    {
        stCCtrl.evtFinished -= OnstCCtrlFinished;
        stCCtrl.evtRedo -= OnstCCtrlRedo;
        stCCtrl.Dispose();
        stCCtrl = ResManager.GetPrefab("Prefabs/SentenceExpression/SentenceCtrlC").GetComponent<SentenceCtrlC>();
        stCCtrl.transform.SetParent(transform);
        stCCtrl.evtFinished += OnstCCtrlFinished;
        stCCtrl.evtRedo += OnstCCtrlRedo;
    }
    private void OnecDCtrlFinished()
    {
        stDCtrl.evtFinished -= OnecDCtrlFinished;
        stDCtrl.evtRedo -= OnecDCtrlRedo;
        stDCtrl.Dispose();
        tpv = ResManager.GetPrefab("Prefabs/UI/TestPaperView").GetComponent<TestPaperView>();
        tpv.evtFinished += OnTestPaperFinished;
        tpv.evtRedo += OnTestPaperRedo;
    }
    private void OnecDCtrlRedo()
    {
        stDCtrl.evtFinished -= OnecDCtrlFinished;
        stDCtrl.evtRedo -= OnecDCtrlRedo;
        stDCtrl.Dispose();
        stDCtrl = ResManager.GetPrefab("Prefabs/SentenceExpression/SentenceCtrlD").GetComponent<SentenceCtrlD>();
        stDCtrl.transform.SetParent(transform);
        stDCtrl.evtFinished += OnstCCtrlFinished;
        stDCtrl.evtRedo += OnstCCtrlRedo;
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
        GlobalEntity.GetInstance().Dispatch<ModelTasks>(FlowModel.mEvent.FlowStepFinished, ModelTasks.EnhanceCommunity);
    }
    void RemoveListens()
    {
        //com.redoClickEvent -= SwapModel.GetInstance().Redo;
        //com.nextClickEvent -= SwapModel.GetInstance().NextLevel;
    }
    public void Dispose()
    {
        //销毁、资源释放、监听移除            
        RemoveListens();
        Destroy(gameObject);
    }
}
