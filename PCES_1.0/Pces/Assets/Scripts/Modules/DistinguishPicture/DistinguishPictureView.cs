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

    private void Awake()
    {
        ClickDispatcher.Inst.SetCurrentCamera(Camera.main);
        UnityEngine.Debug.Log("DistinguishPictureView::Awake(): 第三阶段 分辨图片");


    }


    private void Start () {
        CommonUI com = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        com.SetComUITitle("第三阶段 分辨图片");

        UnityEngine.Debug.Log("DistinguishPictureView::Start(): 第三阶段 第一关 区辨喜欢和不喜欢物品的图卡");
        dpaCtrl = ResManager.GetPrefab("Prefabs/DistinguishPicture/DistinguishPictureA").GetComponent<DistinguishPictureCtrlA>();
        dpaCtrl.evtFinished += OnDpaCtrlFinished;
        dpaCtrl.evtRedo += OnDpaCtrlRedo;

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
        GameObject laoshiGo = PeopleManager.Instance.GetPeople("LS_BD");
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
