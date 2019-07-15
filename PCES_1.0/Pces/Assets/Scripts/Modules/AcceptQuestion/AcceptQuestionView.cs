using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceptQuestionView : MonoBehaviour
{
    public AcceptQuesCtrlA aqACtrl;
    public AcceptQuesCtrlB aqBCtrl;
    public AcceptQuesCtrlC aqCCtrl;
    public TestPaperView tpv;
    CommonUI com;
    private void Awake()
    {
        ClickDispatcher.Inst.SetCurrentCamera(Camera.main);
        UnityEngine.Debug.Log("SentenceExpressionView::Awake(): 第五阶段 接受性问句");
        this.name = "SentenceExpressionView";
    }
    private void Start()
    {
        com = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        com.SetComUITitle("第五阶段 增强自发性沟通");
        Canvas canvas = FindObjectOfType<Canvas>();
        com.transform.SetParent(canvas.transform);
        //aqACtrl = ResManager.GetPrefab("Prefabs/AcceptQuestion/AcceptQuesCtrlA").GetComponent<AcceptQuesCtrlA>();
        //aqACtrl.transform.SetParent(transform);
        //aqACtrl.evtFinished += OnaqACtrlFinished;
        //aqACtrl.evtRedo += OnaqACtrlRedo;
        //aqBCtrl = ResManager.GetPrefab("Prefabs/AcceptQuestion/AcceptQuesCtrlB").GetComponent<AcceptQuesCtrlB>();
        //aqBCtrl.transform.SetParent(transform);
        //aqBCtrl.evtFinished += OnaqBCtrlFinished;
        //aqBCtrl.evtRedo += OnaqBCtrlRedo;
        aqCCtrl = ResManager.GetPrefab("Prefabs/AcceptQuestion/AcceptQuesCtrlC").GetComponent<AcceptQuesCtrlC>();
        aqCCtrl.transform.SetParent(transform);
        aqCCtrl.evtFinished += OnstCCtrlFinished;
        aqCCtrl.evtRedo += OnstCCtrlRedo;
        //tpv = ResManager.GetPrefab("Prefabs/UI/TestPaperView").GetComponent<TestPaperView>();
        //tpv.transform.SetParent(transform);
        //tpv.evtFinished += OnTestPaperFinished;
        //tpv.evtRedo += OnTestPaperRedo;
    }
    void OnaqACtrlFinished()
    {
        aqACtrl.evtFinished -= OnaqACtrlFinished;
        aqACtrl.evtRedo -= OnaqACtrlRedo;
        aqACtrl.Dispose();
        aqBCtrl = ResManager.GetPrefab("Prefabs/AcceptQuestion/AcceptQuesCtrlB").GetComponent<AcceptQuesCtrlB>();
        aqBCtrl.transform.SetParent(transform);
        aqBCtrl.evtFinished += OnaqBCtrlFinished;
        aqBCtrl.evtRedo += OnaqBCtrlRedo;
    }
    void OnaqACtrlRedo()
    {
        aqACtrl.evtFinished -= OnaqACtrlFinished;
        aqACtrl.evtRedo -= OnaqACtrlRedo;
        aqACtrl.Dispose();
        aqACtrl = ResManager.GetPrefab("Prefabs/AcceptQuestion/AcceptQuesCtrlA").GetComponent<AcceptQuesCtrlA>();
        aqACtrl.transform.SetParent(transform);
        aqACtrl.evtFinished += OnaqACtrlFinished;
        aqACtrl.evtRedo += OnaqACtrlRedo;
    }
    void OnaqBCtrlFinished()
    {
        aqBCtrl.evtFinished -= OnaqBCtrlFinished;
        aqBCtrl.evtRedo -= OnaqBCtrlRedo;
        aqBCtrl.Dispose();
        aqCCtrl = ResManager.GetPrefab("Prefabs/AcceptQuestion/AcceptQuesCtrlC").GetComponent<AcceptQuesCtrlC>();
        aqCCtrl.transform.SetParent(transform);
        aqCCtrl.evtFinished += OnstCCtrlFinished;
        aqCCtrl.evtRedo += OnstCCtrlRedo;
    }
    void OnaqBCtrlRedo()
    {
        aqBCtrl.evtFinished -= OnaqBCtrlFinished;
        aqBCtrl.evtRedo -= OnaqBCtrlRedo;
        aqBCtrl.Dispose();
        aqBCtrl = ResManager.GetPrefab("Prefabs/AcceptQuestion/AcceptQuesCtrlB").GetComponent<AcceptQuesCtrlB>();
        aqBCtrl.transform.SetParent(transform);
        aqBCtrl.evtFinished += OnaqBCtrlFinished;
        aqBCtrl.evtRedo += OnaqBCtrlRedo;
    }
    void OnstCCtrlFinished()
    {
        aqCCtrl.evtFinished -= OnstCCtrlFinished;
        aqCCtrl.evtRedo -= OnstCCtrlRedo;
        aqCCtrl.Dispose();
        tpv = ResManager.GetPrefab("Prefabs/UI/TestPaperView").GetComponent<TestPaperView>();
        tpv.evtFinished += OnTestPaperFinished;
        tpv.evtRedo += OnTestPaperRedo;
    }
    void OnstCCtrlRedo()
    {
        aqCCtrl.evtFinished -= OnstCCtrlFinished;
        aqCCtrl.evtRedo -= OnstCCtrlRedo;
        aqCCtrl.Dispose();
        aqCCtrl = ResManager.GetPrefab("Prefabs/AcceptQuestion/AcceptQuesCtrlC").GetComponent<AcceptQuesCtrlC>();
        aqCCtrl.transform.SetParent(transform);
        aqCCtrl.evtFinished += OnstCCtrlFinished;
        aqCCtrl.evtRedo += OnstCCtrlRedo;
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
        GlobalEntity.GetInstance().Dispatch<ModelTasks>(FlowModel.mEvent.FlowStepFinished, ModelTasks.SentenceExpression);
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
