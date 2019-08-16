using System.Collections;
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
        UnityEngine.Debug.Log("SentenceExpressionView::Awake(): 第六阶段 主动发表意见");
        this.name = "SentenceExpressionView";
    }
    private void Start()
    {
        SentenceExpressionModel.GetInstance().Jiaoshi();

        com = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        com.SetComUITitle("第六阶段 主动发表意见");
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
        //stCCtrl = ResManager.GetPrefab("Prefabs/SentenceExpression/SentenceCtrlC").GetComponent<SentenceCtrlC>();
        //stCCtrl.transform.SetParent(transform);
        //stCCtrl.evtFinished += OnstCCtrlFinished;
        //stCCtrl.evtRedo += OnstCCtrlRedo;
        stDCtrl = ResManager.GetPrefab("Prefabs/SentenceExpression/SentenceCtrlD").GetComponent<SentenceCtrlD>();
        stDCtrl.transform.SetParent(transform);
        stDCtrl.evtFinished += OnstDCtrlFinished;
        stDCtrl.evtRedo += OnstDCtrlRedo;
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
        stDCtrl.evtFinished += OnstDCtrlFinished;
        stDCtrl.evtRedo += OnstDCtrlRedo;
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
    private void OnstDCtrlFinished()
    {
        stDCtrl.evtFinished -= OnstDCtrlFinished;
        stDCtrl.evtRedo -= OnstDCtrlRedo;
        stDCtrl.Dispose();
        tpv = ResManager.GetPrefab("Prefabs/UI/TestPaperView").GetComponent<TestPaperView>();
        tpv.evtFinished += OnTestPaperFinished;
        tpv.evtRedo += OnTestPaperRedo;
    }
    private void OnstDCtrlRedo()
    {
        stDCtrl.evtFinished -= OnstDCtrlFinished;
        stDCtrl.evtRedo -= OnstDCtrlRedo;
        stDCtrl.RedoDispose();
        stDCtrl = ResManager.GetPrefab("Prefabs/SentenceExpression/SentenceCtrlD").GetComponent<SentenceCtrlD>();
        stDCtrl.transform.SetParent(transform);
        stDCtrl.evtFinished += OnstDCtrlFinished;
        stDCtrl.evtRedo += OnstDCtrlRedo;
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
        if (stDCtrl.gameObject != null)
        {
            Destroy(stDCtrl.gameObject);
        }
        PeopleManager.Instance.Reset();
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
        if (stACtrl != null)
        {
            stACtrl.Dispose();
        }
        if (stBCtrl != null)
        {
            stBCtrl.Dispose();
        }
        if (stCCtrl != null)
        {
            stCCtrl.Dispose();
        }
        if (stDCtrl != null)
        {
            stDCtrl.Dispose();
        }
        if (tpv != null)
        {
            tpv.Dispose();
        }
        RemoveListens();
        Destroy(gameObject);
    }
}
