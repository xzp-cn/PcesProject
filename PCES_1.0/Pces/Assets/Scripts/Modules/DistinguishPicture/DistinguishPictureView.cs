using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 第三阶段--分辨图片
/// </summary>
public class DistinguishPictureView : MonoBehaviour {
    //1-3关控制器
    DistinguishPictureCtrlA dpaCtrl;
    DistinguishPictureCtrlB dpbCtrl;
    DistinguishPictureCtrlC dpcCtrl;

    Animation xiaohuaAnim;
    Animation laoshiAnim;

    TestPaperView tpv;

    private void Awake()
    {
        ClickDispatcher.Inst.SetCurrentCamera(Camera.main);
        UnityEngine.Debug.Log("DistinguishPictureView::Awake(): 第三阶段 分辨图片");

    }


    private void Start () {
        CommonUI com = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        com.SetComUITitle("第三阶段 分辨图片");

        //UnityEngine.Debug.Log("DistinguishPictureView::Start(): 第三阶段 第一关 区辨喜欢和不喜欢物品的图卡");
        //dpaCtrl = ResManager.GetPrefab("Prefabs/DistinguishPicture/DistinguishPictureA").GetComponent<DistinguishPictureCtrlA>();
        //dpaCtrl.evtFinished += OnDpaCtrlFinished;
        //dpaCtrl.evtRedo += OnDpaCtrlRedo;

        dpbCtrl = ResManager.GetPrefab("Prefabs/DistinguishPicture/DistinguishPictureB").GetComponent<DistinguishPictureCtrlB>();
        dpbCtrl.evtFinished += OnDpbCtrlFinished;
        dpbCtrl.evtRedo += OnDpbCtrlRedo;

        InitPersonsState();

    }

    /// <summary>
    /// 初始化人物状态
    /// </summary>
    void InitPersonsState()
    {
        ///小华初始状态
        GameObject xiaohuaGo = PeopleManager.Instance.GetPeople("XH_BD");
        xiaohuaGo.transform.localPosition = Vector3.zero;


        ///隐藏辅导老师
        GameObject fudaolsGo = PeopleManager.Instance.GetPeople("FDLS_BD");
        fudaolsGo.SetActive(false);

        ///初始化老师状态
    }

    void OnXiaoHua()
    {
    }

    void OnDpaCtrlFinished()
    {
        Debug.Log("DistinguishPictureView.OnDpaCtrlRedo(): 第三阶段 第一关 区辨喜欢和不喜欢物品的图卡 下一关!!!");
        dpaCtrl.evtFinished -= OnDpaCtrlFinished;
        dpaCtrl.Dispose();

        dpbCtrl = ResManager.GetPrefab("Prefabs/DistinguishPicture/DistinguishPictureB").GetComponent<DistinguishPictureCtrlB>();
        dpbCtrl.evtFinished += OnDpbCtrlFinished;
        dpbCtrl.evtRedo += OnDpbCtrlRedo;
    }

    void OnDpaCtrlRedo()
    {
        Debug.Log("DistinguishPictureView.OnDpaCtrlRedo(): 第三阶段 第一关 区辨喜欢和不喜欢物品的图卡 重做!!!");
        dpaCtrl.evtFinished -= OnDpaCtrlFinished;
        dpaCtrl.evtRedo -= OnDpaCtrlRedo;
        dpaCtrl.Dispose();
        dpaCtrl = ResManager.GetPrefab("Prefabs/DistinguishPicture/DistinguishPictureA").GetComponent<DistinguishPictureCtrlA>();
        dpaCtrl.evtFinished += OnDpaCtrlFinished;
        dpaCtrl.evtRedo += OnDpaCtrlRedo;
    }

    void OnDpbCtrlRedo()
    {
        Debug.Log("DistinguishPictureView.OnDpbCtrlRedo(): 第三阶段 第二关 区辨喜欢和不喜欢物品的图卡 重做!!!");
        dpbCtrl.evtFinished -= OnDpaCtrlFinished;
        dpbCtrl.evtRedo -= OnDpaCtrlRedo;
        dpbCtrl.Dispose();
        dpbCtrl = ResManager.GetPrefab("Prefabs/DistinguishPicture/DistinguishPictureB").GetComponent<DistinguishPictureCtrlB>();
        dpbCtrl.evtFinished += OnDpbCtrlFinished;
        dpbCtrl.evtRedo += OnDpbCtrlRedo;
    }

    private void OnDpbCtrlFinished()
    {
        Debug.Log("DistinguishPictureView.OnDpaCtrlRedo(): 第三阶段 第二关 区辨喜欢和不喜欢物品的图卡 下一关!!!");
        dpbCtrl.evtFinished -= OnDpbCtrlFinished;
        dpbCtrl.Dispose();

        dpcCtrl = ResManager.GetPrefab("Prefabs/DistinguishPicture/DistinguishPictureC").GetComponent<DistinguishPictureCtrlC>();
        dpcCtrl.evtFinished += OnDpcCtrlFinished;
    }

    private void OnDpcCtrlFinished()
    {
        dpcCtrl.evtFinished -= OnDpcCtrlFinished;
        dpcCtrl.Dispose();

        tpv = ResManager.GetPrefab("Prefabs/UI/TestPaperView").GetComponent<TestPaperView>();
        tpv.evtFinished += OnTestPaperRedo;
        tpv.evtRedo += OnTestPaperRedo;
    }


    void OnTestPaperRedo()
    {
        tpv.evtFinished -= OnTestPaperFinished;
        tpv.evtRedo -= OnTestPaperRedo;
        tpv = UIManager.Instance.GetUI<TestPaperView>("TestPaperView");
        tpv.evtFinished += OnTestPaperFinished;
        tpv.evtRedo += OnTestPaperRedo;
    }
    void OnTestPaperFinished()
    {
        tpv.evtFinished -= OnTestPaperFinished;
        tpv.evtRedo -= OnTestPaperRedo;
        tpv.Dispose();
        //通知当前阶段完成
        OnDistinguishPictureFinished();
    }

    void OnDistinguishPictureFinished()
    {
        //第三阶段完成
        GlobalEntity.GetInstance().Dispatch<ModelTasks>(FlowModel.mEvent.FlowStepFinished, ModelTasks.DistinguishPicture);
    }


    public void Dispose()
    {
        if (dpcCtrl != null)
        {
            dpaCtrl.Dispose();
        }
        if (dpbCtrl != null)
        {
            dpbCtrl.Dispose();
        }
        if (dpcCtrl != null)
        {
            dpcCtrl.Dispose();
        }
        Destroy(gameObject);
    }
}
