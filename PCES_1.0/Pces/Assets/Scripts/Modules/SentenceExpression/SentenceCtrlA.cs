﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentenceCtrlA : MonoBehaviour
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
    //AnimationOper GTB;//沟通本
    AnimationOper gtb;
    private void Awake()
    {
        this.name = "SentenceCtrlA";
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
            //swapUI.chooseEvent += ChooseBtnClickCallback;
            swapUI.speakEvent += SpeakBtnClickCallback;
            swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, true);
            swapUI.SetButtonVisiable(SwapUI.BtnName.chooseButton, false);
        }
        LS = PeopleManager.Instance.GetPeople(PeopleTag.LS_BD).GetAnimatorOper();
        XH = PeopleManager.Instance.GetPeople(PeopleTag.XH_BD).GetAnimatorOper();
        FDLS = PeopleManager.Instance.GetPeople(PeopleTag.FDLS_BD).GetAnimatorOper();
        LS.PlayForward("idle");
        XH.PlayForward("idle");
        FDLS.PlayForward("idle");
        //FDLS.gameObject.SetActive(false);
        //FDLS.PlayForward("FDLS_A_2ND_D");
        HighLightCtrl.GetInstance().OffAllObjs();
        GetTukaObject();
    }
    /// <summary>
    /// 初始化桌子上的涂卡
    /// </summary>
    void GetTukaObject()
    {
        PropsObject pObj = SentenceExpressionModel.GetInstance().GetObj(PropsType.neutralStimulator);//中性刺激物
        Reinforcement rfc = new Reinforcement(pObj.pData);//测试代码 
        SentenceExpressionModel.GetInstance().CurReinforcement = rfc;//设置强化物
        Debug.Log("GetTukaObject");
        Transform objectsTr = new GameObject("objectsParent").transform;
        objectsTr.localPosition = Vector3.zero;
        objectsTr.localScale = Vector3.one;
        objectsTr.rotation = Quaternion.identity;
        objectsTr.SetParent(transform);

        int objId = rfc.pData.id;
        GameObject obj = Instantiate(pObj.gameObject);
        obj.name = ((PropsTag)objId).ToString();
        obj.transform.SetParent(objectsTr);
        PropsObject curObj = obj.GetComponent<PropsObject>();
        curObj.pData = pObj.pData;
        curObj.setPos(new Vector3(2.55f, 0.57f, -0.27f));//TODO:每个物体的位置缩放,角度,有待调整  
        curObj.transform.localScale = Vector3.one * 0.5f;
        Debug.Log(curObj.pData.name_cn + "    " + curObj.pData.name);

        gtb = ResManager.GetPrefab("Prefabs/AnimationKa/XH_D_1ST_FBNKT_ka").GetAnimatorOper();
        gtb.name = PropsTag.TY_GTB.ToString();
        gtb.transform.SetParent(objectsTr);
        gtb.name = "goutongben";
        gtb.transform.SetParent(objectsTr);
        gtb.transform.localPosition = new Vector3(-0.048f, 0, -0.373f);

        string _tuka = "tuka_" + ((PropsTag)objId).ToString();
        GameObject deskTuka = Instantiate(SentenceExpressionModel.GetInstance().GetTuKa(_tuka));
        deskTuka.transform.SetParent(objectsTr);
        deskTuka.name = _tuka;
        PropsObject pot = deskTuka.GetComponent<PropsObject>();
        pot.setPos(new Vector3(2.18f, 0.57f, -0.001f));
        GameObject judai = Instantiate(SentenceExpressionModel.GetInstance().GetObj((int)PropsTag.judai_wokanjian));
        judai.transform.SetParent(objectsTr);
        judai.transform.localEulerAngles = new Vector3(0, -90, 0);
        judai.transform.localPosition = new Vector3(2.274f, 0.5481f, -0.183f);
        judai.name = PropsTag.judai_wokanjian.ToString();

        pot = judai.GetComponent<PropsObject>();
        pot.setPos(new Vector3(2.27f, 0.6f, -0.209f));
        LS.Complete += ClickmicroPhoneTip;
        LS.PlayForward("LS_E_1ST_ZB");
        //Debug.Log("ls");
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
    /// <summary>
    /// 点击话筒
    /// </summary>
    /// <param name="cobj"></param>
    /// 
    void ClickmicroPhoneTip()
    {
        ChooseDo.Instance.DoWhat(5, RedoLsSpeak, ShowSpeakContent);
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StartFlash();
    }
    void RedoLsSpeak()
    {
        ClickDispatcher.Inst.EnableClick = false;
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StopFlash();
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师说话");
        CancelInvoke("ClickmicroPhoneTip");
        Invoke("ClickmicroPhoneTip", 2);
    }
    void SpeakBtnClickCallback()
    {
        UIFlah uf = swapUI.GetMicroBtn.gameObject.GetUIFlash();
        uf.StopFlash();
        swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, false);
        ChooseDo.Instance.Clicked();
    }
    void ShowSpeakContent()
    {
        Dialog dlog = UIManager.Instance.GetUI<Dialog>("Dialog");
        UIManager.Instance.SetUIDepthTop("Dialog");
        dlog.SetDialogMessage("小华看见什么");
        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickLsCallBack);
        ClickDispatcher.Inst.EnableClick = true;
        ClickLsHandTip();
    }
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
        ChooseDo.Instance.DoWhat(5, RedoLsPointJudai, LsPointJudai);
    }
    void RedoLsPointJudai()
    {
        ClickDispatcher.Inst.EnableClick = false;
        HighLightCtrl.GetInstance().FlashOn(jshand);
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师指句带");
        CancelInvoke("ClickLsHandTip");
        Invoke("ClickLsHandTip", 2);
    }
    void LsPointJudai()
    {
        UIManager.Instance.GetUI<Dialog>("Dialog").Show(false);
        HighLightCtrl.GetInstance().FlashOff(jshand);
        ClickDispatcher.Inst.EnableClick = false;
        LS.Complete += LsPointJudaiCallback;
        LS.PlayForward("LS_E_1ST_ZK");
    }
    /// <summary>
    /// 教师接收图卡回调
    /// </summary>
    void LsPointJudaiCallback()
    {
        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickLsCallBack);
        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickFdlsCallBack);
        ClickDispatcher.Inst.EnableClick = true;
        ClickFdlsHandTip();
    }
    void ClickFdlsHandTip()
    {
        ChooseDo.Instance.DoWhat(5, RedoClickFdlsHand, FdlsClickXhHand);
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
    ///辅导教师抓住小华的手翻开沟通本
    /// </summary>
    void FdlsClickXhHand()
    {
        HighLightCtrl.GetInstance().FlashOff(fdlshand);
        ClickDispatcher.Inst.EnableClick = false;
        //FDLS.Complete += FdlsClickXhHandCalllback;
        //FDLS.PlayForward("FDLS_A_2ND_D");//TODO:教师动画播放时有位移
        FDLS.PlayForward("FDLS_B_1ST_FGTB");
        XH.PlayForward("XH_D_1ST_FBNKT");
        gtb.PlayForward("XH_D_1ST_FBNKT_ka");
        //GTB.PlayForward("onePaper");
        XH.Complete += XhTakeCardCallback;
    }
    /// <summary>
    /// 小华拿卡递卡回调
    /// </summary>
    void XhTakeCardCallback()
    {
        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickFdlsCallBack);
        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickLsCallBack);
        ClickDispatcher.Inst.EnableClick = true;
        ClickLsHandJiekaTip();
    }
    void ClickLsHandJiekaTip()
    {
        if (jshand == null)
        {
            jshand = LS.transform.Find("LSB_BD/shou").gameObject;
        }
        HighLightCtrl.GetInstance().FlashOn(jshand);
        ClickDispatcher.Inst.EnableClick = true;
        ChooseDo.Instance.DoWhat(5, RedoLsJieka, LsJieka);
    }
    void RedoLsJieka()
    {
        ClickDispatcher.Inst.EnableClick = false;
        HighLightCtrl.GetInstance().FlashOn(jshand);
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师接卡");
        CancelInvoke("ClickLsHandTip");
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
        ClickmicroPhoneJiekaTip();
    }
    void ClickmicroPhoneJiekaTip()
    {
        ChooseDo.Instance.DoWhat(5, RedoLsJiekaSpeak, ShowSpeakJiekaContent);
        swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, true);
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StartFlash();
    }
    void RedoLsJiekaSpeak()
    {
        ClickDispatcher.Inst.EnableClick = false;
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StopFlash();
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师说话");
        CancelInvoke("ClickmicroPhoneTip");
        Invoke("ClickmicroPhoneTip", 2);
    }
    void ShowSpeakJiekaContent()
    {
        Dialog dlog = UIManager.Instance.GetUI<Dialog>("Dialog");
        UIManager.Instance.SetUIDepthTop("Dialog");
        string curObjName = SentenceExpressionModel.GetInstance().CurReinforcement.pData.name_cn;
        dlog.SetDialogMessage("是的，你看见了" + curObjName + "，你表现得很好!");
        CancelInvoke("LsGiveInit");
        Invoke("LsGiveInit", 2);
    }
    /// <summary>
    ///  老师给物品
    /// </summary>
    void LsGiveInit()
    {
        Debug.Log("给物品");
        UIManager.Instance.GetUI<Dialog>("Dialog").Show(false);
        ClickLsGiveObjTip();
    }
    void ClickLsGiveObjTip()
    {
        ChooseDo.Instance.DoWhat(5, RedoLsGiveObj, LsGiveObj);
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
        LS.PlayForward("TY_LS_DW");
        XH.Complete += XHJiewuCallback;
        XH.PlayForward("TY_XH_JG");
    }
    void LsGiveObjCallback()
    {
        ShowFinalUI();
    }
    void XHJiewuCallback()
    {
        Debug.Log("xh给物品回调用");
    }
    /// <summary>
    /// 显示结算界面
    /// </summary>
    void ShowFinalUI()
    {
        CommonUI com = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        com.nextClickEvent += NextDo;
        com.redoClickEvent += ReDo;
        UIManager.Instance.SetUIDepthTop("CommonUI");
        com.ShowFinalUI();
    }
    void Finish()
    {
        ChooseDo.Instance.ResetAll();
        UIManager.Instance.GetUI<CommonUI>("CommonUI").HideFinalUI();
        RemoveAllListeners();
    }
    void NextDo()
    {
        Finish();
        evtFinished();
    }
    void RemoveAllListeners()
    {
        CommonUI com = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        com.redoClickEvent -= NextDo;
        com.redoClickEvent -= ReDo;
        swapUI.speakEvent -= SpeakBtnClickCallback;
    }
    void ReDo()
    {
        Debug.Log("redo");
        Finish();
        evtRedo();
    }
    public void Dispose()
    {
        RemoveAllListeners();
        Destroy(gameObject);
    }
    private void OnDestroy()
    {

    }
}