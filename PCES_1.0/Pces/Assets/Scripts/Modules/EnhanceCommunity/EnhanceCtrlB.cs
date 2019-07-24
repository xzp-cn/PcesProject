using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhanceCtrlB : MonoBehaviour
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
    AnimationOper GTB;//沟通本
    Vector3 camPos;
    GameObject deskTuka;
    private void Awake()
    {
        this.name = "EnhanceCtrlB";
        camPos = Camera.main.transform.parent.localPosition;
        Camera.main.transform.parent.localPosition = new Vector3(3.895f, 1.071f, 0.43f);
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
        UIManager.Instance.SetUIDepthTop("selectionUI");

        if (LS == null)
        {
            LS = Instantiate(PeopleManager.Instance.GetPeople(PeopleTag.LS_BD)).GetAnimatorOper();
            LS.name = "LS";
            LS.transform.SetParent(transform);
            LS.transform.localPosition = new Vector3(1.4f, 0, 0.022f);

            XH = Instantiate(PeopleManager.Instance.GetPeople(PeopleTag.XH_BD)).GetAnimatorOper();
            XH.name = "XH";
            XH.transform.SetParent(transform);

            PeopleManager.Instance.gameObject.SetActive(false);

            LS.PlayForward("idle");
            XH.gameObject.SetActive(false);
        }

        HighLightCtrl.GetInstance().OffAllObjs();
        GetTukaObject();
    }
    /// <summary>
    /// 初始化桌子上的涂卡
    /// </summary>
    void GetTukaObject()
    {
        if (GTB == null)
        {
            GameObject gtb = ResManager.GetPrefab("Prefabs/Objects/TY_GTB");
            gtb.name = "TY_GTB";
            gtb.transform.SetParent(transform);
            gtb.transform.localPosition = new Vector3(2.27f, 0.5672f, 0.376f);
            GTB = gtb.AddComponent<AnimationOper>();
        }
        //Reinforcement rfc = EnhanceCommunityModel.GetInstance().CurReinforcement;
        //rfc = new Reinforcement(new PropsData("chips", 2, PropsType.reinforcement, "薯片"));//测试代码 
        Reinforcement rfc = GlobalDataManager.GetInstance().CurReinforcement;
        EnhanceCommunityModel.GetInstance().CurReinforcement = rfc;
        if (rfc != null)
        {
            Debug.Log("GetTukaObject");

            int objId = rfc.pData.id;
            GameObject obj = Instantiate(SwapModel.GetInstance().GetObj(objId));
            obj.name = ((PropsTag)objId).ToString();
            obj.transform.SetParent(transform);
            PropsObject po = obj.GetComponent<PropsObject>();
            po.setPos(new Vector3(3.88f, 0.57f, 0.17f));//TODO:每个物体的位置有待调整     

            string _tuka = "tuka_" + ((PropsTag)objId).ToString();
            deskTuka = Instantiate(SwapModel.GetInstance().GetTuKa(_tuka));
            deskTuka.transform.SetParent(transform);
            deskTuka.name = _tuka;
            PropsObject pot = deskTuka.GetComponent<PropsObject>();
            pot.setPos(new Vector3(2.2974f, 0.57f, 0.376f));
            deskTuka.gameObject.SetActive(false);

            Invoke("XhTakeCard", 1);
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
    #region 小华走动拿卡并给卡
    void XhTakeCard()
    {
        XH.gameObject.SetActive(true);
        //XH.Complete += XhTakeCardCallback;
        XH.timePointEvent = (a) =>
        {
            if (a == 110)
            {
                GTB.timePointEvent = (b) =>
                {
                    if (b == 28)
                    {
                        GTB.timePointEvent = null;
                        deskTuka.gameObject.SetActive(true);
                    }
                };
                GTB.PlayForward("onePaper");
            }

            if (a == 187)
            {
                XHCtrl ctrl = XH.GetComponent<XHCtrl>();
                string name = EnhanceCommunityModel.GetInstance().CurReinforcement.pData.name;
                Material matSource = EnhanceCommunityModel.GetInstance().GetTuKa("tuka_" + name).GetComponent<MeshRenderer>().materials[1];
                Material matTar = ctrl.r_tuka2.transform.Find("tuka2 1").GetComponent<MeshRenderer>().materials[1];
                matTar.CopyPropertiesFromMaterial(matSource);
                transform.Find("tuka_" + name).gameObject.SetActive(false);
                ctrl.r_tuka2.gameObject.SetActive(true);
            }
            if (a == 404)
            {
                XH.timePointEvent = null;
                XH.OnPause();
                XhTakeCardCallback();
            }
        };

        XH.PlayForward("XH_B_2ND_ZFNZD");

    }
    /// <summary>
    /// 小华拿卡递卡回调
    /// </summary>
    void XhTakeCardCallback()
    {
        //GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickFdlsCallBack);
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
        LS.timePointEvent = (a) =>
        {
            if (a == 55)
            {
                LS.timePointEvent = null;
                LSCtrl ctrl = LS.GetComponent<LSCtrl>();
                string name = EnhanceCommunityModel.GetInstance().CurReinforcement.pData.name;
                Material matSource = EnhanceCommunityModel.GetInstance().GetTuKa("tuka_" + name).GetComponent<MeshRenderer>().materials[1];
                Material matTar = ctrl.ls_tuka2.transform.Find("LS_tuka2 1").GetComponent<MeshRenderer>().materials[1];
                matTar.CopyPropertiesFromMaterial(matSource);
                ctrl.ls_tuka2.gameObject.SetActive(true);

                XHCtrl xctrl = XH.GetComponent<XHCtrl>();
                xctrl.r_tuka2.gameObject.SetActive(false);
                XH.OnContinue();
                //FDLS.PlayForward("idle");
            }
        };
        LS.PlayForward("TY_LS_JK");//LS_tuka/LS_tuka 1  //tuka2        
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
        ClickDispatcher.Inst.EnableClick = false;
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
        Dialog dlog = UIManager.Instance.GetUI<Dialog>("Dialog");
        UIManager.Instance.SetUIDepthTop("Dialog");
        string curObjName = EnhanceCommunityModel.GetInstance().CurReinforcement.pData.name_cn;
        dlog.SetDialogMessage("小华要吃" + curObjName);
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
        ClickDispatcher.Inst.EnableClick = false;
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StopFlash();
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师给相应物品");
        ClickLsGiveObjTip();
    }
    void LsGiveObj()
    {
        //XH.PlayForward("empty");    
        XH.transitionTime = 0f;
        XH.transform.localPosition = new Vector3(1.4f, 0, 0);
        Debug.Log("教师给物品");
        HighLightCtrl.GetInstance().FlashOff(jshand);
        ClickDispatcher.Inst.EnableClick = false;
        swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, false);

        Transform qhw = transform.Find(EnhanceCommunityModel.GetInstance().CurReinforcement.pData.name);
        LS.timePointEvent = (a) =>//老师递给物品
        {
            if (a == 32)//挂载到老师手上强化物时间点
            {
                LS.timePointEvent = null;
                LSCtrl lsctrl = LS.GetComponent<LSCtrl>();//将当前强化物挂在老师手上    
                lsctrl.SetJoint(qhw.gameObject);
                //Debug.LogError("ls");
            }

            if (a == 20)//小华接卡动画播放延迟
            {
                XH.Complete += XHJiewuCallback;
                //XH.timePointEvent = (b) => {
                //    if (b<20)
                //    {
                //        XH.transform.localPosition = new Vector3(1.4f, 0, 0);
                //    }
                //};
                XH.PlayForward("TY_XH_JG");
            }
        };

        LS.Complete += LsGiveObjCallback;
        LS.PlayForward("TY_LS_DW");

        XH.timePointEvent = (a) =>//小华接过物品 挂载强化物
        {
            if (a == 42)
            {
                XHCtrl xhCtrl = XH.GetComponent<XHCtrl>();
                xhCtrl.SetJoint(qhw.gameObject);
                //Debug.LogError("xh");
            }
        };
    }
    void LsGiveObjCallback()
    {
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
        UIManager.Instance.SetUIDepthTop("CommonUI");
        com.ShowFinalUI();
    }
    void Finish()
    {
        ChooseDo.Instance.ResetAll();
        UIManager.Instance.GetUI<CommonUI>("CommonUI").HideFinalUI();
        RemoveAllListeners();
    }
    void ReDo()
    {
        Debug.Log("redo");
        Finish();
        evtRedo();
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
        swapUI.chooseEvent -= ChooseBtnClickCallback;
        swapUI.speakEvent -= SpeakBtnClickCallback;
        selectUI.okEvent -= SelectUIOkBtnCallback;

        com = null;
        evtFinished = null;
        evtRedo = null;
    }
    public void Dispose()
    {
        PeopleManager.Instance.gameObject.SetActive(true);
        RemoveAllListeners();
        Camera.main.transform.parent.localPosition = camPos;
        Destroy(gameObject);
    }
    private void OnDestroy()
    {

    }
}
