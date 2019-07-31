using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhanceCommunityView : MonoBehaviour
{
    public EnhanceCtrlA ecACtrl;
    public EnhanceCtrlB ecBCtrl;
    public EnhanceCtrlC ecCCtrl;
    public TestPaperView tpv;
    CommonUI com;
    private void Awake()
    {
        ClickDispatcher.Inst.SetCurrentCamera(Camera.main);
        UnityEngine.Debug.Log("EnhanceCommunityView::Awake(): 第二阶段 增强自发性沟通");
        this.name = "EnhanceCommunityView";

    }
    private void Start()
    {
        EnhanceCommunityModel.GetInstance().Jiaoshi();

        com = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        com.SetComUITitle("第二阶段 增强自发性沟通");
        Canvas canvas = FindObjectOfType<Canvas>();
        com.transform.SetParent(canvas.transform);
        //ecACtrl = ResManager.GetPrefab("Prefabs/EnhanceCommunity/EnhanceA").GetComponent<EnhanceCtrlA>();
        //ecACtrl.transform.SetParent(transform);
        //ecACtrl.evtFinished += OnecACtrlFinished;
        //ecACtrl.evtRedo += OnecACtrlRedo;
        //ecBCtrl = ResManager.GetPrefab("Prefabs/EnhanceCommunity/EnhanceB").GetComponent<EnhanceCtrlB>();
        //ecBCtrl.transform.SetParent(transform);
        //ecBCtrl.evtFinished += OnecBCtrlFinished;
        //ecBCtrl.evtRedo += OnecBCtrlRedo;
        ecCCtrl = ResManager.GetPrefab("Prefabs/EnhanceCommunity/EnhanceC").GetComponent<EnhanceCtrlC>();
        ecCCtrl.transform.SetParent(transform);
        ecCCtrl.evtFinished += OnecCCtrlFinished;
        ecCCtrl.evtRedo += OnecCCtrlRedo;
        //tpv = ResManager.GetPrefab("Prefabs/UI/TestPaperView").GetComponent<TestPaperView>();
        //tpv.transform.SetParent(transform);
        //tpv.evtFinished += OnTestPaperFinished;
        //tpv.evtRedo += OnTestPaperRedo;
    }
    void OnecACtrlFinished()
    {
        ecACtrl.evtFinished -= OnecACtrlFinished;
        ecACtrl.evtRedo -= OnecACtrlRedo;
        ecACtrl.Dispose();
        ecBCtrl = ResManager.GetPrefab("Prefabs/EnhanceCommunity/EnhanceB").GetComponent<EnhanceCtrlB>();
        ecBCtrl.transform.SetParent(transform);
        ecBCtrl.evtFinished += OnecBCtrlFinished;
        ecBCtrl.evtRedo += OnecBCtrlRedo;
    }
    void OnecACtrlRedo()
    {
        ecACtrl.evtFinished -= OnecACtrlFinished;
        ecACtrl.evtRedo -= OnecACtrlRedo;
        ecACtrl.Dispose();
        ecACtrl = ResManager.GetPrefab("Prefabs/EnhanceCommunity/EnhanceA").GetComponent<EnhanceCtrlA>();
        ecACtrl.transform.SetParent(transform);
        ecACtrl.evtFinished += OnecACtrlFinished;
        ecACtrl.evtRedo += OnecACtrlRedo;
    }
    private void OnecBCtrlFinished()
    {
        ecBCtrl.evtFinished -= OnecBCtrlFinished;
        ecBCtrl.evtRedo -= OnecBCtrlRedo;
        ecBCtrl.Dispose();
        ecCCtrl = ResManager.GetPrefab("Prefabs/EnhanceCommunity/EnhanceC").GetComponent<EnhanceCtrlC>();
        ecCCtrl.transform.SetParent(transform);
        ecCCtrl.evtFinished += OnecCCtrlFinished;
        ecCCtrl.evtRedo += OnecCCtrlRedo;
    }
    void OnecBCtrlRedo()
    {
        ecBCtrl.evtFinished -= OnecBCtrlFinished;
        ecBCtrl.evtRedo -= OnecBCtrlRedo;
        ecBCtrl.Dispose();
        ecBCtrl = ResManager.GetPrefab("Prefabs/EnhanceCommunity/EnhanceB").GetComponent<EnhanceCtrlB>();
        ecBCtrl.transform.SetParent(transform);
        ecBCtrl.evtFinished += OnecBCtrlFinished;
        ecBCtrl.evtRedo += OnecBCtrlRedo;
    }
    private void OnecCCtrlFinished()
    {
        ecCCtrl.evtFinished -= OnecCCtrlFinished;
        ecCCtrl.evtRedo -= OnecCCtrlRedo;
        ecCCtrl.Dispose();
        tpv = ResManager.GetPrefab("Prefabs/UI/TestPaperView").GetComponent<TestPaperView>();
        tpv.evtFinished += OnTestPaperFinished;
        tpv.evtRedo += OnTestPaperRedo;
    }
    private void OnecCCtrlRedo()
    {
        ecCCtrl.evtFinished -= OnecCCtrlFinished;
        ecCCtrl.evtRedo -= OnecCCtrlRedo;
        ecCCtrl.Dispose();
        ecCCtrl = ResManager.GetPrefab("Prefabs/EnhanceCommunity/EnhanceC").GetComponent<EnhanceCtrlC>();
        ecCCtrl.transform.SetParent(transform);
        ecCCtrl.evtFinished += OnecCCtrlFinished;
        ecCCtrl.evtRedo += OnecCCtrlRedo;
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
        if (ecACtrl != null)
        {
            ecACtrl.Dispose();
        }
        if (ecBCtrl != null)
        {
            ecBCtrl.Dispose();
        }
        if (ecCCtrl != null)
        {
            ecCCtrl.Dispose();
        }
        if (tpv != null)
        {
            tpv.Dispose();
        }
        RemoveListens();
        Destroy(gameObject);
    }
}
