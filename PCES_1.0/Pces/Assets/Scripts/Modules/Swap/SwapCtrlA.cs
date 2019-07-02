using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapCtrlA : MonoBehaviour
{
    public event System.Action evtFinished;
    public event System.Action evtRedo;
    SwapUI swapUI;
    SelectionUI selectUI;
    GameObject fdlshand;
    GameObject jshand;
    GameObject lsObject;
    GameObject xhObject;
    AnimationOper LS;
    AnimationOper XH;
    AnimationOper FDLS;
    private void Awake()
    {
        this.name = "SwapA";
    }
    //public bool Finished;
    private void Start()
    {
        Init();
    }
    public void Init()
    {
        if (swapUI == null)
        {
            swapUI = UIManager.Instance.GetUI<SwapUI>("SwapUI");
            swapUI.chooseEvent += ChooseBtnClickCallback;
            swapUI.speakEvent += SpeakBtnClickCallback;
            swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, false);
            swapUI.SetButtonVisiable(SwapUI.BtnName.chooseButton, true);
        }
        if (selectUI == null)
        {
            selectUI = UIManager.Instance.GetUI<SelectionUI>("selectionUI");
            selectUI.okEvent += SelectUIOkBtnCallback;
        }
        UIManager.Instance.SetUIDepthTop("selectionUI");
        LS = PeopleManager.Instance.GetPeople(PeopleTag.LS_BD).GetAnimatorOper();
        XH = PeopleManager.Instance.GetPeople(PeopleTag.XH_BD).GetAnimatorOper();
        FDLS = PeopleManager.Instance.GetPeople(PeopleTag.FDLS_BD).GetAnimatorOper();
        Tip();
    }
    /// <summary>
    /// 提示点击强化物按钮
    /// </summary>    
    void Tip()
    {
        UIFlah chooeFlash = swapUI.GetChooseBtn.gameObject.GetUIFlash();
        chooeFlash.StartFlash();
    }
    /// <summary>
    /// 选择强化物按钮点击回调
    /// </summary>
    void ChooseBtnClickCallback()
    {
        UIFlah uf = swapUI.GetChooseBtn.gameObject.GetUIFlash();
        uf.StopFlash();
        selectUI.gameObject.SetActive(true);
        //对当前场景做一些处理
    }
    /// <summary>
    /// 选择强化物界面ok按钮回调用
    /// </summary>  
    void SelectUIOkBtnCallback(int selectObj)
    {
        swapUI.SetButtonVisiable(SwapUI.BtnName.chooseButton, false);
        Transform objectsTr = new GameObject("objectsParent").transform;
        objectsTr.localPosition = Vector3.zero;
        objectsTr.localScale = Vector3.one;
        objectsTr.rotation = Quaternion.identity;
        objectsTr.SetParent(transform);
        //老师手上显示物品
        lsObject = Instantiate(SwapModel.GetInstance().GetObj(selectObj));
        lsObject.transform.SetParent(objectsTr);
        PropsObject po = lsObject.GetComponent<PropsObject>();
        po.setPos(new Vector3(2.55f, 0.57f, -0.11f));//TODO:每个物体的位置有待调整        
        lsObject.name = ((PropsTag)selectObj).ToString();
        //Debug.Log(SwapModel.GetInstance().GetObj(selectObj).GetComponent<PropsObject>().pData.name_cn);
        //Debug.Log(po.pData.name_cn);
        //小华桌子上面涂卡
        string tuka = "tuka_" + ((PropsTag)selectObj).ToString();
        GameObject deskTuka = Instantiate(SwapModel.GetInstance().GetTuKa(tuka));
        deskTuka.transform.SetParent(objectsTr);
        deskTuka.name = tuka;
        po = deskTuka.GetComponent<PropsObject>();
        po.setPos(new Vector3(2.18f, 0.57f, -0.001f));
        //设置当前选择的强化物     
        PropsData pData = SwapModel.GetInstance().GetObj(selectObj).GetComponent<PropsObject>().pData;
        SwapModel.GetInstance().CurReinforcement = new Reinforcement(pData);
        //播放小华抢东西动画         
        XH.Complete += SnatchXhCallback;
        XH.PlayForward("TY_XH_QIANG");
    }
    /// <summary>
    /// 辅导老师手被点击
    /// </summary>
    /// <param name="cobj"></param>
    void ClickFdlsCallBack(ClickedObj cobj)
    {
        Debug.Log("点中 " + cobj.objname);
        if (cobj.objname == "fdls_shou")
        {
            ChooseDo.Instance.Clicked();
        }
    }
    #region 小华抓手拿卡
    /// <summary>
    /// 小华抢东西动画回调
    /// </summary>
    void SnatchXhCallback()
    {
        //计时开始     
        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickFdlsCallBack);
        ChooseDo.Instance.DoWhat(5, RedoClickFdlsHand, FdlsDragHandTakeCard);
        ClickFdlsHandTip();
    }
    void ClickFdlsHandTip()
    {
        ClickDispatcher.Inst.EnableClick = true;
        if (fdlshand == null)
        {
            fdlshand = FDLS.transform.Find("FDLS/fdls_shou").gameObject;
        }
        HighLightCtrl.GetInstance().FlashOn(fdlshand);
    }
    void RedoClickFdlsHand()
    {
        ClickDispatcher.Inst.EnableClick = false;
        HighLightCtrl.GetInstance().FlashOff(fdlshand);
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要辅导老师协助");
        CancelInvoke("ClickFdlsHandTip");
        Invoke("ClickFdlsHandTip", 2);
    }
    //点中辅导老师手后的回调
    /// <summary>
    ///辅导老师抓手拿卡
    /// </summary>
    void FdlsDragHandTakeCard()
    {
        CancelInvoke("ClickFdlsHandTip");
        HighLightCtrl.GetInstance().FlashOff(fdlshand);
        ClickDispatcher.Inst.EnableClick = false;
        FDLS.PlayForward("FDLS_A_1ST_ZN");
        XH.Complete += FdlsDragHandTakeCardCallback;
        XH.PlayForward("XH_A_1ST_BZNK");
    }
    /// <summary>
    /// 辅导老师抓手拿卡回调
    /// </summary>
    void FdlsDragHandTakeCardCallback()
    {
        ChooseDo.Instance.DoWhat(5, RedoFdlsDika, FdlsDika);//
        //FDLS.PlayForward("idle");
        ClickFdlsDikaHandTip();
    }
    #endregion
    #region 辅导老师抓手递卡
    void ClickFdlsDikaHandTip()
    {
        HighLightCtrl.GetInstance().FlashOn(fdlshand);
        ClickDispatcher.Inst.EnableClick = true;
    }
    void RedoFdlsDika()
    {
        ClickDispatcher.Inst.EnableClick = false;
        HighLightCtrl.GetInstance().FlashOff(fdlshand);
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要辅导老师协助");
        Invoke("ClickFdlsDikaHandTip", 2);
    }
    /// <summary>
    /// 辅导老师抓小华手递卡
    /// </summary>
    void FdlsDika()
    {
        HighLightCtrl.GetInstance().FlashOff(fdlshand);
        ClickDispatcher.Inst.EnableClick = false;
        FDLS.Complete += FdlsDikaCallBack;
        FDLS.PlayForward("FDLS_A_1ST_ZD");
        XH.PlayForward("XH_A_1ST_BZDK");
    }
    //辅导老师抓手递卡回调
    void FdlsDikaCallBack()
    {
        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickFdlsCallBack);
        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickLsCallBack);
        ChooseDo.Instance.DoWhat(5, RedoLsJieka, LsJieka);
        ClickDispatcher.Inst.EnableClick = true;
        ClickLsHandTip();
    }
    /// <summary>
    /// 教师点中
    /// </summary>
    /// <param name="cobj"></param>
    void ClickLsCallBack(ClickedObj cobj)
    {
        Debug.Log("点中 " + cobj.objname);
        if (cobj.objname == "shou")
        {
            ChooseDo.Instance.Clicked();
        }
    }
    void ClickLsHandTip()
    {
        if (jshand == null)
        {
            jshand = LS.transform.Find("LSB_BD/shou").gameObject;
        }
        HighLightCtrl.GetInstance().FlashOn(jshand);
        ClickDispatcher.Inst.EnableClick = true;
    }
    void RedoLsJieka()
    {
        ClickDispatcher.Inst.EnableClick = false;
        HighLightCtrl.GetInstance().FlashOn(jshand);
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师接卡");
        Invoke("ClickLsHandTip", 2);
    }
    void LsJieka()
    {
        HighLightCtrl.GetInstance().FlashOff(jshand);
        ClickDispatcher.Inst.EnableClick = false;
        LS.Complete += LsJiekaCallback;
        LS.PlayForward("TY_LS_JK");
    }
    /// <summary>
    /// 教师接收图卡回调
    /// </summary>
    void LsJiekaCallback()
    {
        swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, true);
        ClickDispatcher.Inst.EnableClick = false;
        ChooseDo.Instance.DoWhat(5, RedoLsSpeak, ShowSpeakContent);
        ClickmicroPhoneTip();
    }
    #endregion
    #region 点击话筒提示
    void ClickmicroPhoneTip()
    {
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StartFlash();
    }
    void RedoLsSpeak()
    {
        ClickDispatcher.Inst.EnableClick = false;
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StopFlash();
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师说话");
        Invoke("ClickmicroPhoneTip", 2);
    }
    void SpeakBtnClickCallback()
    {
        Debug.Log("按钮点击");
        UIFlah uf = swapUI.GetMicroBtn.gameObject.GetUIFlash();
        uf.StopFlash();
        swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, false);
        ChooseDo.Instance.Clicked();
    }
    void ShowSpeakContent()
    {
        Dialog dlog = UIManager.Instance.GetUI<Dialog>("Dialog");
        UIManager.Instance.SetUIDepthTop("Dialog");
        string curObjName = SwapModel.GetInstance().CurReinforcement.pData.name_cn;
        dlog.SetDialogMessage("小华要吃" + curObjName);
        Invoke("LsGiveInit", 2);
    }
    #endregion
    #region 老师给物品
    void LsGiveInit()
    {
        Debug.Log("给物品");
        UIManager.Instance.GetUI<Dialog>("Dialog").Show(false);
        ChooseDo.Instance.DoWhat(5, RedoLsGiveObj, LsGiveObj);
        ClickLsGiveObjTip();
    }
    void ClickLsGiveObjTip()
    {
        HighLightCtrl.GetInstance().FlashOn(jshand);
        ClickDispatcher.Inst.EnableClick = true;
    }
    void RedoLsGiveObj()
    {
        ClickDispatcher.Inst.EnableClick = false;
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StopFlash();
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师给相应物品");
        ClickLsGiveObjTip();

    }
    void LsGiveObj()
    {
        Debug.Log("教师给物品");
        HighLightCtrl.GetInstance().FlashOff(jshand);
        ClickDispatcher.Inst.EnableClick = false;
        swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, false);
        LS.Complete += LsGiveObjCallback;
        LS.PlayForward("TY_LS_JKDW");
    }
    void LsGiveObjCallback()
    {
        XH.Complete += XHJiewuCallback;
        XH.PlayForward("TY_XH_JG");
    }
    void XHJiewuCallback()
    {
        ShowFinalUI();
    }
    #endregion
    /// <summary>
    /// 显示结算界面
    /// </summary>
    void ShowFinalUI()
    {
        CommonUI com = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        com.redoClickEvent += NextDo;
        com.redoClickEvent += ReDo;
        com.ShowFinalUI();
    }
    void ReDo()
    {
        RemoveAllListeners();
        evtRedo();
    }
    void NextDo()
    {
        RemoveAllListeners();
        evtFinished();
    }
    void RemoveAllListeners()
    {
        CommonUI com = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        com.redoClickEvent -= NextDo;
        com.redoClickEvent -= ReDo;
        swapUI.chooseEvent -= ChooseBtnClickCallback;
        swapUI.speakEvent -= SpeakBtnClickCallback;
        selectUI.okEvent -= SelectUIOkBtnCallback;
    }
    public void Dispose()
    {
        CommonUI com = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        com.HideFinalUI();
        Destroy(gameObject);
    }
}

