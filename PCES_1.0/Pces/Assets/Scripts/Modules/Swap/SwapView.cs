using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 交换东西
/// </summary>
public class SwapView : MonoBehaviour
{
    public SwapCtrlA spACtrl;
    public SwapCtrlB spBCtrl;
    public SwapCtrlC spCCtrl;
    public TestPaperView tpv;
    CommonUI com;
    private void Awake()
    {
        ClickDispatcher.Inst.SetCurrentCamera(Camera.main);
        UnityEngine.Debug.Log("SwapView::Awake(): 第一阶段 以物换物");
        this.name = "SwapView";
    }
    private void Start()
    {
        GlobalDataManager.GetInstance().GetCamera();
        GlobalDataManager.GetInstance().SetPcesCamera();
        Introduct();
    }
    void Introduct()
    {
        com = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        com.SetComUITitle("第一阶段 以物换物");
        Canvas canvas = FindObjectOfType<Canvas>();
        com.transform.SetParent(canvas.transform);

        if (SwapModel.GetInstance().hpUI == null)
        {
            HomePageUI homePageUI = UIManager.Instance.GetUI<HomePageUI>("HomePageUI");
            SwapModel.GetInstance().hpUI = homePageUI;
            homePageUI.transform.SetParent(canvas.transform, false);
            Button startButton = homePageUI.transform.Find("Button").GetComponent<Button>();//开始按钮
            startButton.onClick.AddListener(StartPro);
            startButton.gameObject.GetUIFlash().StartFlash();

            SwapModel.GetInstance().CurReinforcement = null;
        }
        else
        {
            StartPro();
        }
    }
    private void StartPro()
    {

        SwapModel.GetInstance().hpUI.gameObject.SetActive(false);
        spACtrl = ResManager.GetPrefab("Prefabs/Swap/SwapA").GetComponent<SwapCtrlA>();
        spACtrl.transform.SetParent(transform);
        spACtrl.evtFinished += OnSpACtrlFinished;
        spACtrl.evtRedo += OnSpACtrlRedo;
        //Debug.Log("fsfds");
        //spBCtrl = ResManager.GetPrefab("Prefabs/Swap/SwapB").GetComponent<SwapCtrlB>();
        //spBCtrl.transform.SetParent(transform);
        //spBCtrl.evtFinished += OnSpBCtrlFinished;
        //spBCtrl.evtRedo += OnSpBCtrlRedo;
        //spCCtrl = ResManager.GetPrefab("Prefabs/Swap/SwapC").GetComponent<SwapCtrlC>();
        //spCCtrl.transform.SetParent(transform);
        //spCCtrl.evtFinished += OnSpCCtrlFinished;
        //spCCtrl.evtRedo += OnSpCCtrlRedo;

    }
    void OnSpACtrlFinished()
    {
        spACtrl.evtFinished -= OnSpACtrlFinished;
        spACtrl.evtRedo -= OnSpACtrlRedo;
        spACtrl.Dispose();
        spBCtrl = ResManager.GetPrefab("Prefabs/Swap/SwapB").GetComponent<SwapCtrlB>();
        spBCtrl.transform.SetParent(transform);
        spBCtrl.evtFinished += OnSpBCtrlFinished;
        spBCtrl.evtRedo += OnSpBCtrlRedo;
    }
    void OnSpACtrlRedo()
    {
        Debug.Log("OnSpACtrlRedo");
        spACtrl.evtFinished -= OnSpACtrlFinished;
        spACtrl.evtRedo -= OnSpACtrlRedo;
        spACtrl.Dispose();
        spACtrl = ResManager.GetPrefab("Prefabs/Swap/SwapA").GetComponent<SwapCtrlA>();
        spACtrl.transform.SetParent(transform);
        spACtrl.evtFinished += OnSpACtrlFinished;
        spACtrl.evtRedo += OnSpACtrlRedo;
    }
    private void OnSpBCtrlFinished()
    {
        spBCtrl.evtFinished -= OnSpBCtrlFinished;
        spBCtrl.evtRedo -= OnSpBCtrlRedo;
        spBCtrl.Dispose();
        spCCtrl = ResManager.GetPrefab("Prefabs/Swap/SwapC").GetComponent<SwapCtrlC>();
        spCCtrl.transform.SetParent(transform);
        spCCtrl.evtFinished += OnSpCCtrlFinished;
        spCCtrl.evtRedo += OnSpCCtrlRedo;
    }
    void OnSpBCtrlRedo()
    {
        spBCtrl.evtFinished -= OnSpBCtrlFinished;
        spBCtrl.evtRedo -= OnSpBCtrlRedo;
        spBCtrl.Dispose();
        spBCtrl = ResManager.GetPrefab("Prefabs/Swap/SwapB").GetComponent<SwapCtrlB>();
        spBCtrl.transform.SetParent(transform);
        spBCtrl.evtFinished += OnSpBCtrlFinished;
        spBCtrl.evtRedo += OnSpBCtrlRedo;
    }
    private void OnSpCCtrlFinished()
    {
        spCCtrl.evtFinished -= OnSpCCtrlFinished;
        spCCtrl.evtRedo -= OnSpCCtrlRedo;
        spCCtrl.Dispose();
        tpv = ResManager.GetPrefab("Prefabs/UI/TestPaperView").GetComponent<TestPaperView>();
        tpv.evtFinished += OnTestPaperFinished;
        tpv.evtRedo += OnTestPaperRedo;
    }
    private void OnSpCCtrlRedo()
    {
        spCCtrl.evtFinished -= OnSpCCtrlFinished;
        spCCtrl.evtRedo -= OnSpCCtrlRedo;
        spCCtrl.Dispose();
        spCCtrl = ResManager.GetPrefab("Prefabs/Swap/SwapC").GetComponent<SwapCtrlC>();
        spCCtrl.transform.SetParent(transform);
        spCCtrl.evtFinished += OnSpCCtrlFinished;
        spCCtrl.evtRedo += OnSpCCtrlRedo;
    }
    void OnTestPaperRedo()
    {
        tpv.evtFinished -= OnTestPaperFinished;
        tpv.evtRedo -= OnTestPaperRedo;
        tpv.Dispose();
        tpv = ResManager.GetPrefab("Prefabs/UI/TestPaperView").GetComponent<TestPaperView>(); ;
        tpv.evtFinished += OnTestPaperFinished;
        tpv.evtRedo += OnTestPaperRedo;
    }
    void OnTestPaperFinished()
    {
        tpv.evtFinished -= OnTestPaperFinished;
        tpv.evtRedo -= OnTestPaperRedo;
        tpv.Dispose();
        //通知当前阶段完成
        GlobalEntity.GetInstance().Dispatch<ModelTasks>(FlowModel.mEvent.FlowStepFinished, ModelTasks.Swap);
    }
    void RemoveListens()
    {
        //com.redoClickEvent -= SwapModel.GetInstance().Redo;
        //com.nextClickEvent -= SwapModel.GetInstance().NextLevel;
    }
    public void Dispose()
    {
        //Debug.LogError("SwapView");
        if (spACtrl != null)
        {
            spACtrl.Dispose();
        }
        if (spBCtrl != null)
        {
            spBCtrl.Dispose();
        }
        if (spCCtrl != null)
        {
            spCCtrl.Dispose();
        }
        if (tpv != null)
        {
            tpv.Dispose();
        }
        UIManager.Instance.GetUI<SwapUI>("SwapUI").ResetUI();
        PeopleManager.Instance.Reset();
        //销毁、资源释放、监听移除            
        RemoveListens();
        Destroy(gameObject);
    }
}
