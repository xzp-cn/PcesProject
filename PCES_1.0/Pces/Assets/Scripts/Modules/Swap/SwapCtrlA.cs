﻿using System.Collections;
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
    GameObject xhTk;
    GameObject lsTk;
    GameObject qhw;
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
            selectUI.closeEvent += CloseSelectUICallback;
        }
        UIManager.Instance.GetUI<Dialog>("Dialog").SetPos();
        UIManager.Instance.SetUIDepthTop("selectionUI");

        PeopleManager.Instance.Reset();

        LS = PeopleManager.Instance.GetPeople(PeopleTag.LS_BD).GetAnimatorOper();
        XH = PeopleManager.Instance.GetPeople(PeopleTag.XH_BD).GetAnimatorOper();
        FDLS = PeopleManager.Instance.GetPeople(PeopleTag.FDLS_BD).GetAnimatorOper();
        LS.PlayForward("idle");
        XH.PlayForward("idle");
        FDLS.PlayForward("idle");
        HighLightCtrl.GetInstance().OffAllObjs();
        GetTukaObject();
    }
    /// <summary>
    /// 初始化桌子上的涂卡
    /// </summary>
    void GetTukaObject()
    {
        Reinforcement rfc = SwapModel.GetInstance().CurReinforcement;
        //rfc = new Reinforcement(new PropsData("chips", 2, PropsType.reinforcement, "薯片"));//测试代码 
        if (rfc != null)
        {
            Debug.Log("GetTukaObject");

            int objId = rfc.pData.id;
            qhw = ObjectsManager.instanse.GetQHW();
            qhw.transform.SetParent(transform);
            string qhwName = ((PropsTag)objId).ToString();
            qhw.name = qhwName;
            QHWCtrl qhwCtrl = qhw.GetComponent<QHWCtrl>();
            qhwCtrl.ShowObj(qhwName);

            string _tuka = "tuka_" + ((PropsTag)objId).ToString();
            Material matSource = SwapModel.GetInstance().GetTuKa(_tuka).GetComponent<MeshRenderer>().materials[1];//图卡材质
            GameObject deskTuka = ObjectsManager.instanse.GetdeskTuka();//桌面图卡
            deskTuka.transform.SetParent(transform);
            deskTuka.name = _tuka;

            TukaCtrl tukaCtrl = deskTuka.GetComponent<TukaCtrl>();
            xhTk = tukaCtrl.ShowObj("XH_ZS_TUKA");
            Material matTar = xhTk.GetComponent<MeshRenderer>().materials[1];
            matTar.CopyPropertiesFromMaterial(matSource);

            lsTk = tukaCtrl.GetObj("LS_JZS_TUKA");
            matTar = lsTk.GetComponent<MeshRenderer>().materials[1];
            matTar.CopyPropertiesFromMaterial(matSource);

            Invoke("SnatchXh", 1);
        }
        else
        {
            Tip();
        }
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
        //设置当前选择的强化物     
        PropsData pData = SwapModel.GetInstance().GetObj(selectObj).GetComponent<PropsObject>().pData;
        SwapModel.GetInstance().CurReinforcement = new Reinforcement(pData);
        ResetAll();
    }
    void ResetAll()
    {
        //CancelInvoke();
        this.enabled = false;
        ReDo();
    }
    void CloseSelectUICallback()
    {
        Debug.Log("close");
        //ReDo();
    }
    void SnatchXh()
    {
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
        if (cobj.objname == "fdls_shou")
        {
            ClickDispatcher.Inst.EnableClick = false;
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
        ClickFdlsHandTip();
    }
    void ClickFdlsHandTip()
    {
        ChooseDo.Instance.DoWhat(5, RedoClickFdlsHand, FdlsDragHandTakeCard);
        ClickDispatcher.Inst.EnableClick = true;
        if (fdlshand == null)
        {
            fdlshand = FDLS.transform.Find("FDLS/fdls_shou").gameObject;
        }
        HighLightCtrl.GetInstance().FlashOn(fdlshand);
    }
    void RedoClickFdlsHand()
    {
        //ClickDispatcher.Inst.EnableClick = false;
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
        ClickDispatcher.Inst.EnableClick = false;
        HighLightCtrl.GetInstance().FlashOff(fdlshand);
        FDLS.PlayForward("FDLS_A_1ST_ZN");
        XH.Complete += FdlsDragHandTakeCardCallback;
        XH.PlayForward("XH_A_1ST_BZNK");
    }
    /// <summary>
    /// 辅导老师抓手拿卡回调
    /// </summary>
    void FdlsDragHandTakeCardCallback()
    {
        //FDLS.PlayForward("idle");
        XHCtrl ctrl = XH.GetComponent<XHCtrl>();
        string name = SwapModel.GetInstance().CurReinforcement.pData.name;
        Material matSource = SwapModel.GetInstance().GetTuKa("tuka_" + name).GetComponent<MeshRenderer>().materials[1];
        Material matTar = ctrl.r_tuka2.transform.Find("tuka2 1").GetComponent<MeshRenderer>().materials[1];
        matTar.CopyPropertiesFromMaterial(matSource);
        ctrl.r_tuka2.gameObject.SetActive(true);
        xhTk.gameObject.SetActive(false);

        ClickFdlsDikaHandTip();
    }
    #endregion
    #region 辅导老师抓手递卡
    void ClickFdlsDikaHandTip()
    {
        ChooseDo.Instance.DoWhat(5, RedoFdlsDika, FdlsDika);//
        HighLightCtrl.GetInstance().FlashOn(fdlshand);
        ClickDispatcher.Inst.EnableClick = true;
    }
    void RedoFdlsDika()
    {
        //ClickDispatcher.Inst.EnableClick = false;
        HighLightCtrl.GetInstance().FlashOff(fdlshand);
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要辅导老师协助");
        CancelInvoke("ClickFdlsDikaHandTip");
        Invoke("ClickFdlsDikaHandTip", 2);
    }
    /// <summary>
    /// 辅导老师抓小华手递卡
    /// </summary>
    void FdlsDika()
    {
        CancelInvoke("ClickFdlsDikaHandTip");
        HighLightCtrl.GetInstance().FlashOff(fdlshand);
        ClickDispatcher.Inst.EnableClick = false;
        FDLS.Complete += FdlsDikaCallBack;
        FDLS.PlayForward("FDLS_A_1ST_ZD");
        XH.timePointEvent = (a) =>
        {
            if (a >= 33 && a <= 35)
            {
                XH.timePointEvent = null;
                XH.OnPause();
            }
        };
        XH.PlayForward("XH_A_1ST_BZDK");
    }
    //辅导老师抓手递卡回调
    void FdlsDikaCallBack()
    {
        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickFdlsCallBack);
        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickLsCallBack);
        ClickDispatcher.Inst.EnableClick = true;
        ClickLsHandTip();
    }
    /// <summary>
    /// 教师点中
    /// </summary>
    /// <param name="cobj"></param>
    void ClickLsCallBack(ClickedObj cobj)
    {
        if (cobj.objname == "shou")
        {
            ClickDispatcher.Inst.EnableClick = false;
            Debug.Log("点中 " + cobj.objname);
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
        ChooseDo.Instance.DoWhat(5, RedoLsJieka, LsJieka);
    }
    void RedoLsJieka()
    {
        //ClickDispatcher.Inst.EnableClick = false;
        HighLightCtrl.GetInstance().FlashOn(jshand);
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师接卡");
        CancelInvoke("ClickLsHandTip");
        Invoke("ClickLsHandTip", 2);
    }
    void LsJieka()
    {
        CancelInvoke("ClickLsHandTip");
        ClickDispatcher.Inst.EnableClick = false;
        HighLightCtrl.GetInstance().FlashOff(jshand);
        LS.Complete += LsGiveObjCallback;

        bool pause = true;
        bool pass = true;
        bool pass1 = true;
        bool pass2 = true;
        bool pass3 = true;
        bool pass4 = true;
        LS.timePointEvent = (a) =>
        {
            if (a >= 51 && a <= 53 && pass)//老师接卡
            {
                pass = false;
                LSCtrl ctrl = LS.GetComponent<LSCtrl>();
                string name = SwapModel.GetInstance().CurReinforcement.pData.name;
                Material matSource = SwapModel.GetInstance().GetTuKa("tuka_" + name).GetComponent<MeshRenderer>().materials[1];
                Material matTar = ctrl.ls_tuka2.transform.Find("LS_tuka2 1").GetComponent<MeshRenderer>().materials[1];
                matTar.CopyPropertiesFromMaterial(matSource);
                ctrl.ls_tuka2.gameObject.SetActive(true);

                XHCtrl xctrl = XH.GetComponent<XHCtrl>();
                xctrl.r_tuka2.gameObject.SetActive(false);
                XH.OnContinue();
                FDLS.PlayForward("idle");
            }

            if (a >= 81 && a <= 83 && pass1)//老师桌子放卡片
            {
                pass1 = false;
                LSCtrl ctrl = LS.GetComponent<LSCtrl>();//手上卡隐藏，桌子上的卡显示
                ctrl.ls_tuka2.gameObject.SetActive(false);

                lsTk.gameObject.SetActive(true);
            }

            if (a >= 94 && a <= 96 && pause)
            {
                pause = false;
                LS.OnPause();//在某一帧停止时，下一次还会从该帧执行

                LsJiekaCallback();//提示
            }

            if (a >= 122 && a <= 124 && pass4)//强化物挂到老师手上
            {
                pass4 = false;
                LSCtrl ctrl = LS.GetComponent<LSCtrl>();
                ctrl.SetJoint(qhw);
            }

            if (a >= 145 && a <= 147 && pass2)//小华接受物体时间点
            {
                //               
                LS.timePointEvent = null;
                pass2 = false;
                XH.timePointEvent = (b) =>//小华接过物品 挂载强化物
                {
                    if (b >= 42 && b <= 44 && pass3)
                    {
                        pass3 = false;
                        XHCtrl xhCtrl = XH.GetComponent<XHCtrl>();
                        xhCtrl.SetJoint(qhw);
                        //Debug.LogError("xh");
                    }
                };
                XH.PlayForward("TY_XH_JG");
            }
        };
        LS.PlayForward("TY_LS_JKDW");//LS_tuka/LS_tuka 1  //tuka2
    }
    /// <summary>
    /// 教师接收图卡回调
    /// </summary>
    void LsJiekaCallback()
    {
        swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, true);
        ClickDispatcher.Inst.EnableClick = false;
        ClickmicroPhoneTip();
    }
    #endregion
    #region 点击话筒提示
    void ClickmicroPhoneTip()
    {
        ChooseDo.Instance.DoWhat(5, RedoLsSpeak, ShowSpeakContent);
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StartFlash();
    }
    void RedoLsSpeak()
    {
        //ClickDispatcher.Inst.EnableClick = false;
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StopFlash();
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师说话");
        CancelInvoke("ClickmicroPhoneTip");
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
        CancelInvoke("ClickmicroPhoneTip");
        Dialog dlog = UIManager.Instance.GetUI<Dialog>("Dialog");
        UIManager.Instance.SetUIDepthTop("Dialog");
        string curObjName = SwapModel.GetInstance().CurReinforcement.pData.name_cn;
        dlog.SetDialogMessage("小华要" + curObjName + "呀");
        CancelInvoke("LsGiveInit");
        Invoke("LsGiveInit", 2);
    }
    #endregion
    #region 老师给物品
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
        //ClickDispatcher.Inst.EnableClick = false;
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StopFlash();
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师给相应物品");
        CancelInvoke("ClickLsGiveObjTip");
        Invoke("ClickLsGiveObjTip", 2);
    }
    void LsGiveObj()
    {
        CancelInvoke("ClickLsGiveObjTip");
        Debug.Log("教师给物品");
        HighLightCtrl.GetInstance().FlashOff(jshand);
        ClickDispatcher.Inst.EnableClick = false;
        swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, false);

        LS.OnContinue();//老师给物品。

        //Transform qhw = transform.Find(SwapModel.GetInstance().CurReinforcement.pData.name);
        //LS.timePointEvent = (a) =>//老师递给物品
        //{
        //    if (a == 32)//挂载到老师手上强化物时间点
        //    {
        //        LS.timePointEvent = null;
        //        LSCtrl lsctrl = LS.GetComponent<LSCtrl>();//将当前强化物挂在老师手上    
        //        lsctrl.SetJoint(qhw.gameObject);
        //        //Debug.LogError("ls");
        //    }

        //    if (a == 20)//小华接卡动画播放延迟
        //    {
        //        XH.Complete += XHJiewuCallback;
        //        XH.PlayForward("TY_XH_JG");
        //    }
        //};

        //LS.Complete += LsGiveObjCallback;
        //LS.PlayForward("TY_LS_DW");

        //XH.timePointEvent = (a) =>//小华接过物品 挂载强化物
        //{
        //    if (a == 42)
        //    {
        //        XHCtrl xhCtrl = XH.GetComponent<XHCtrl>();
        //        xhCtrl.SetJoint(qhw.gameObject);
        //        //Debug.LogError("xh");
        //    }
        //};
    }
    void LsGiveObjCallback()
    {
        //selectUI.gameObject.SetActive(false);
        //swapUI.gameObject.SetActive(false);
        //Debug.LogError("LsGiveObjCallback");
        ShowFinalUI();
    }
    void XHJiewuCallback()
    {
        Debug.Log("xh给物品回调用");
    }
    #endregion
    /// <summary>
    /// 显示结算界面
    /// </summary>
    void ShowFinalUI()
    {
        CommonUI com = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        com.nextClickEvent += NextDo;
        com.redoClickEvent += ReDo;
        com.ShowFinalUI();
    }
    void Finish()
    {
        ChooseDo.Instance.ResetAll();
        UIManager.Instance.GetUI<CommonUI>("CommonUI").HideFinalUI();

        XHCtrl xhctrl = XH.GetComponent<XHCtrl>();
        xhctrl.DestroyGuadian();

        LSCtrl lsctrl = LS.GetComponent<LSCtrl>();
        lsctrl.DestroyGuadian();

        RemoveAllListeners();
    }
    void ReDo()
    {
        Finish();
        if (evtRedo != null)
        {
            evtRedo();
        }
    }
    void NextDo()
    {
        Finish();
        if (evtFinished != null)
        {
            evtFinished();
        }
    }
    void RemoveAllListeners()
    {
        CommonUI com = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        com.redoClickEvent -= NextDo;
        com.redoClickEvent -= ReDo;
        com = null;

        swapUI.chooseEvent -= ChooseBtnClickCallback;
        swapUI.speakEvent -= SpeakBtnClickCallback;
        selectUI.okEvent -= SelectUIOkBtnCallback;

        LS.Complete -= LsGiveObjCallback;
        XH.Complete -= XHJiewuCallback;

        XH.timePointEvent = null;
        LS.timePointEvent = null;

        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickFdlsCallBack);
        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickLsCallBack);
    }
    public void Dispose()
    {
        RemoveAllListeners();
        evtFinished = null;
        evtRedo = null;
        Destroy(gameObject);
    }
    private void OnDestroy()
    {

    }
}

