using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentenceExpressionView : MonoBehaviour
{
    //public SentenceCtrlA stACtrl;
    //public SentenceCtrlB stBCtrl;
    //public SentenceCtrlC stCCtrl;
    //public SentenceCtrlD stDCtrl;
    //public TestPaperView tpv;
    //CommonUI com;
    //private void Awake()
    //{
    //    ClickDispatcher.Inst.SetCurrentCamera(Camera.main);
    //    UnityEngine.Debug.Log("EnhanceCommunityView::Awake(): 第二阶段 增强自发性沟通");
    //    this.name = "EnhanceCommunityView";

    //}
    //private void Start()
    //{
    //    com = UIManager.Instance.GetUI<CommonUI>("CommonUI");
    //    com.SetComUITitle("第二阶段 增强自发性沟通");
    //    Canvas canvas = FindObjectOfType<Canvas>();
    //    com.transform.SetParent(canvas.transform);
    //    //stACtrl = ResManager.GetPrefab("Prefabs/EnhanceCommunity/EnhanceA").GetComponent<SentenceCtrlA>();
    //    //stACtrl.transform.SetParent(transform);
    //    //stACtrl.evtFinished += OnstACtrlFinished;
    //    //stACtrl.evtRedo += OnstACtrlRedo;
    //    //stBCtrl = ResManager.GetPrefab("Prefabs/EnhanceCommunity/EnhanceB").GetComponent<SentenceCtrlB>();
    //    //stBCtrl.transform.SetParent(transform);
    //    //stBCtrl.evtFinished += OnstBCtrlFinished;
    //    //stBCtrl.evtRedo += OnstBCtrlRedo;
    //    //ecCCtrl = ResManager.GetPrefab("Prefabs/EnhanceCommunity/EnhanceC").GetComponent<SentenceCtrlC>();
    //    //ecCCtrl.transform.SetParent(transform);
    //    //ecCCtrl.evtFinished += OnecCCtrlFinished;
    //    //ecCCtrl.evtRedo += OnecCCtrlRedo;
    //    //tpv = ResManager.GetPrefab("Prefabs/UI/TestPaperView").GetComponent<TestPaperView>();
    //    //tpv.transform.SetParent(transform);
    //    //tpv.evtFinished += OnTestPaperFinished;
    //    //tpv.evtRedo += OnTestPaperRedo;
    //}
    //void OnstACtrlFinished()
    //{
    //    stACtrl.evtFinished -= OnstACtrlFinished;
    //    stACtrl.evtRedo -= OnstACtrlRedo;
    //    stACtrl.Dispose();
    //    stBCtrl = ResManager.GetPrefab("Prefabs/EnhanceCommunity/EnhanceB").GetComponent<SentenceCtrlB>();
    //    stBCtrl.transform.SetParent(transform);
    //    stBCtrl.evtFinished += OnstBCtrlFinished;
    //    stBCtrl.evtRedo += OnstBCtrlRedo;
    //}
    //void OnstACtrlRedo()
    //{
    //    stACtrl.evtFinished -= OnstACtrlFinished;
    //    stACtrl.evtRedo -= OnstACtrlRedo;
    //    stACtrl.Dispose();
    //    stACtrl = ResManager.GetPrefab("Prefabs/EnhanceCommunity/EnhanceA").GetComponent<SentenceCtrlA>();
    //    stACtrl.transform.SetParent(transform);
    //    stACtrl.evtFinished += OnstACtrlFinished;
    //    stACtrl.evtRedo += OnstACtrlRedo;
    //}
    //private void OnstBCtrlFinished()
    //{
    //    stBCtrl.evtFinished -= OnstBCtrlFinished;
    //    stBCtrl.evtRedo -= OnstBCtrlRedo;
    //    stBCtrl.Dispose();
    //    ecCCtrl = ResManager.GetPrefab("Prefabs/EnhanceCommunity/EnhanceC").GetComponent<SentenceCtrlC>();
    //    ecCCtrl.transform.SetParent(transform);
    //    ecCCtrl.evtFinished += OnecCCtrlFinished;
    //    ecCCtrl.evtRedo += OnecCCtrlRedo;
    //}
    //void OnstBCtrlRedo()
    //{
    //    stBCtrl.evtFinished -= OnstBCtrlFinished;
    //    stBCtrl.evtRedo -= OnstBCtrlRedo;
    //    stBCtrl.Dispose();
    //    stBCtrl = ResManager.GetPrefab("Prefabs/EnhanceCommunity/EnhanceB").GetComponent<SentenceCtrlB>();
    //    stBCtrl.transform.SetParent(transform);
    //    stBCtrl.evtFinished += OnstBCtrlFinished;
    //    stBCtrl.evtRedo += OnstBCtrlRedo;
    //}
    //private void OnecCCtrlFinished()
    //{
    //    ecCCtrl.evtFinished -= OnecCCtrlFinished;
    //    ecCCtrl.evtRedo -= OnecCCtrlRedo;
    //    ecCCtrl.Dispose();
    //    stDCtrl = ResManager.GetPrefab("Prefabs/EnhanceCommunity/EnhanceC").GetComponent<SentenceCtrlD>();
    //    ecCCtrl.transform.SetParent(transform);
    //    ecCCtrl.evtFinished += OnecCCtrlFinished;
    //    ecCCtrl.evtRedo += OnecCCtrlRedo;
    //}
    //private void OnecCCtrlRedo()
    //{
    //    ecCCtrl.evtFinished -= OnecCCtrlFinished;
    //    ecCCtrl.evtRedo -= OnecCCtrlRedo;
    //    ecCCtrl.Dispose();
    //    ecCCtrl = ResManager.GetPrefab("Prefabs/EnhanceCommunity/EnhanceC").GetComponent<SentenceCtrlC>();
    //    ecCCtrl.transform.SetParent(transform);
    //    ecCCtrl.evtFinished += OnecCCtrlFinished;
    //    ecCCtrl.evtRedo += OnecCCtrlRedo;
    //}
    //private void OnecDCtrlFinished()
    //{
    //    ecCCtrl.evtFinished -= OnecCCtrlFinished;
    //    ecCCtrl.evtRedo -= OnecCCtrlRedo;
    //    ecCCtrl.Dispose();
    //    tpv = ResManager.GetPrefab("Prefabs/UI/TestPaperView").GetComponent<TestPaperView>();
    //    tpv.evtFinished += OnTestPaperFinished;
    //    tpv.evtRedo += OnTestPaperRedo;
    //}
    //private void OnecDCtrlRedo()
    //{
    //    ecCCtrl.evtFinished -= OnecCCtrlFinished;
    //    ecCCtrl.evtRedo -= OnecCCtrlRedo;
    //    ecCCtrl.Dispose();
    //    ecCCtrl = ResManager.GetPrefab("Prefabs/EnhanceCommunity/EnhanceC").GetComponent<SentenceCtrlD>();
    //    ecCCtrl.transform.SetParent(transform);
    //    ecCCtrl.evtFinished += OnecCCtrlFinished;
    //    ecCCtrl.evtRedo += OnecCCtrlRedo;
    //}
    //void OnTestPaperRedo()
    //{
    //    tpv.evtFinished -= OnTestPaperFinished;
    //    tpv.evtRedo -= OnTestPaperRedo;
    //    tpv.Dispose();
    //    tpv = ResManager.GetPrefab("Prefabs/UI/TestPaperView").GetComponent<TestPaperView>();
    //    tpv.evtFinished += OnTestPaperFinished;
    //    tpv.evtRedo += OnTestPaperRedo;
    //}
    //void OnTestPaperFinished()
    //{
    //    tpv.evtFinished -= OnTestPaperFinished;
    //    tpv.evtRedo -= OnTestPaperRedo;
    //    tpv.Dispose();
    //    //通知当前阶段完成
    //    GlobalEntity.GetInstance().Dispatch<ModelTasks>(FlowModel.mEvent.FlowStepFinished, ModelTasks.EnhanceCommunity);
    //}
    //void RemoveListens()
    //{
    //    //com.redoClickEvent -= SwapModel.GetInstance().Redo;
    //    //com.nextClickEvent -= SwapModel.GetInstance().NextLevel;
    //}
    //public void Dispose()
    //{
    //    //销毁、资源释放、监听移除            
    //    RemoveListens();
    //    Destroy(gameObject);
    //}
}
