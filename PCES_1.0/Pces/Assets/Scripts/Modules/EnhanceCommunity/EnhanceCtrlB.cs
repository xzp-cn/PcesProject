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
    GameObject xhTk;
    GameObject lsTk;
    GameObject qhw;
    GameObject xhqhw;
    Vector3 zhuozi2Pos;
    private void Awake()
    {
        this.name = "EnhanceCtrlB";
        //camPos = Camera.main.transform.parent.localPosition;
        //Camera.main.transform.parent.localPosition = new Vector3(4.032f, 1.071f, 0.43f);
        GlobalDataManager.GetInstance().SetPcesCamera(Vector3.zero);
    }
    //public bool Finished;
    private void Start()
    {
# if UNITY_EDITOR
        F3DDebug.Log("start", new System.Diagnostics.StackTrace(true));
#else
        Debug.Log("EnhanceCtrlB::Start(): 第二阶段第一关");
#endif
        GlobalEntity.GetInstance().Dispatch<string>(CommonUI.pEvent.LevelChange, "第二关");
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

        if (LS == null)
        {
            LS = PeopleManager.Instance.GetPeople(PeopleTag.LS_BD).GetAnimatorOper();
            LS.transform.localPosition = new Vector3(1.516f, 0, 0.022f);
            //LS.PlayForward("idle");

            FDLS = PeopleManager.Instance.GetPeople(PeopleTag.FDLS_BD).GetAnimatorOper();
            FDLS.transform.localPosition = new Vector3(0, 0, 10000);

            XH = PeopleManager.Instance.GetPeople(PeopleTag.XH_BD).GetAnimatorOper();
            XH.transform.localPosition = new Vector3(0, 0, 10000);
        }

        Transform zhuozi2 = EnhanceCommunityModel.GetInstance().Jiaoshi().transform.Find("shinei/zhuozi2");
        zhuozi2Pos = zhuozi2.transform.localPosition;
        zhuozi2.localPosition = new Vector3(0.0608f, 0, 0.036536f);

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
            qhw = ObjectsManager.instanse.GetQHW();
            qhw.transform.SetParent(transform);
            string qhwName = ((PropsTag)objId).ToString();
            qhw.name = qhwName;
            QHWCtrl qhwCtrl = qhw.GetComponent<QHWCtrl>();
            qhw.transform.localPosition = new Vector3(1.508f, 0, 0);
            qhwCtrl.ShowObj(qhwName);


            string _tuka = "tuka_" + ((PropsTag)objId).ToString();
            Material matSource = EnhanceCommunityModel.GetInstance().GetTuKa(_tuka).GetComponent<MeshRenderer>().materials[1];//图卡材质
            deskTuka = ObjectsManager.instanse.GetdeskTuka();//桌面图卡
            deskTuka.transform.SetParent(transform);
            deskTuka.name = _tuka;

            TukaCtrl tukaCtrl = deskTuka.GetComponent<TukaCtrl>();
            xhTk = tukaCtrl.ShowObj("XH_GTB_TUKA");
            Material matTar = xhTk.GetComponent<MeshRenderer>().materials[1];
            matTar.CopyPropertiesFromMaterial(matSource);

            lsTk = tukaCtrl.GetObj("LS_JZS_TUKA");
            matTar = lsTk.GetComponent<MeshRenderer>().materials[1];
            matTar.CopyPropertiesFromMaterial(matSource);

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
        PropsData pData = EnhanceCommunityModel.GetInstance().GetObj(selectObj).GetComponent<PropsObject>().pData;
        EnhanceCommunityModel.GetInstance().CurReinforcement = new Reinforcement(pData);
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
        XH.transform.localPosition = Vector3.zero;
        //XH.Complete += XhTakeCardCallback;        
        bool pass1 = true;
        bool pass2 = true;
        bool pass3 = true;
        bool gtb = true;
        XH.timePointEvent = (a) =>
        {
            if (a >= 108 && a <= 110 && pass1)
            {
                pass1 = false;
                GTB.timePointEvent = (b) =>
                {
                    if (b >= 26 && b <= 28 && gtb)
                    {
                        gtb = false;
                        GTB.timePointEvent = null;
                        deskTuka.gameObject.SetActive(true);
                    }
                };
                GTB.PlayForward("onePaper");
            }

            if (a >= 185 && a <= 187 && pass2)
            {
                pass2 = false;
                XHCtrl ctrl = XH.GetComponent<XHCtrl>();
                string name = EnhanceCommunityModel.GetInstance().CurReinforcement.pData.name;
                Material matSource = EnhanceCommunityModel.GetInstance().GetTuKa("tuka_" + name).GetComponent<MeshRenderer>().materials[1];
                ctrl.r_tuka.transform.Find("tuka 1").GetComponent<MeshRenderer>().enabled = true;
                Material matTar = ctrl.r_tuka.transform.Find("tuka 1").GetComponent<MeshRenderer>().materials[1];
                matTar.CopyPropertiesFromMaterial(matSource);
                ctrl.r_tuka.gameObject.SetActive(true);
                Invoke("HideTK", 0.4f);
            }
            if (a >= 402 && a <= 404 && pass3)
            {
                pass3 = false;
                XH.timePointEvent = null;
                XH.OnPause();
                XhTakeCardCallback();
            }
        };
        XH.PlayForward("XH_B_2ND_ZFNZD");
    }
    void HideTK()
    {
        xhTk.SetActive(false);
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
        HighLightCtrl.GetInstance().FlashOff(jshand);
        ClickDispatcher.Inst.EnableClick = false;

        LS.Complete += LsGiveObjCallback;

        bool pause = true;
        bool pass1 = true;
        bool pass2 = true;
        bool pass3 = true;
        bool pass4 = true;
        bool pass5 = true;
        LS.timePointEvent = (a) =>
        {
            if (a >= 51 && a <= 53 && pass1)//老师接卡
            {
                pass1 = false;
                LSCtrl ctrl = LS.GetComponent<LSCtrl>();
                string name = EnhanceCommunityModel.GetInstance().CurReinforcement.pData.name;
                Material matSource = EnhanceCommunityModel.GetInstance().GetTuKa("tuka_" + name).GetComponent<MeshRenderer>().materials[1];
                Material matTar = ctrl.ls_tuka2.transform.Find("LS_tuka2 1").GetComponent<MeshRenderer>().materials[1];
                matTar.CopyPropertiesFromMaterial(matSource);
                ctrl.ls_tuka2.gameObject.SetActive(true);

                XHCtrl xctrl = XH.GetComponent<XHCtrl>();
                xctrl.r_tuka.gameObject.SetActive(false);
                XH.OnContinue();
            }

            if (a >= 81 && a <= 83 && pass2)//老师桌子放卡片
            {
                pass2 = false;
                LSCtrl ctrl = LS.GetComponent<LSCtrl>();//手上卡隐藏，桌子上的卡显示
                ctrl.ls_tuka2.gameObject.SetActive(false);

                deskTuka.transform.localPosition = new Vector3(1.513f, 0.0014f, 0.051f);
                lsTk.gameObject.SetActive(true);
            }

            if (a >= 94 && a <= 96 && pause)
            {
                pause = false;
                LS.OnPause();//在某一帧停止时，下一次还会从该帧执行

                LsJiekaCallback();//提示
            }

            if (a >= 122 && a <= 124 && pass3)//强化物挂到老师手上
            {
                pass3 = false;
                LSCtrl ctrl = LS.GetComponent<LSCtrl>();
                //qhw.transform.localPosition = Vector3.zero;
                ctrl.SetJoint(qhw);
            }

            if (a >= 150 && a <= 153 && pass4)//小华接受物体时间点
            {
                //               
                pass4 = false;
                LS.timePointEvent = null;

                LegacyAnimationOper go = ResManager.GetPrefab("Prefabs/AnimationKa/XH_B_2ND_JG_KA").GetLegacyAnimationOper();
                go.name = "XH_B_2ND_JG_KA";
                go.transform.SetParent(transform);

                XH.timePointEvent = (b) =>//小华接过物品 挂载强化物
                {
                    if (b >= 40 && b <= 42 && pass5)
                    {
                        pass5 = false;
                        XH.timePointEvent = null;
                        //XHCtrl xhCtrl = XH.GetComponent<XHCtrl>();
                        //xhCtrl.SetJoint(qhw);
                        //qhw.GetComponent<QHWCtrl>().ResetPos();          
                        XhQHW xhqhw = go.GetComponent<XhQHW>();
                        xhqhw.ShowObj(qhw.name);
                        qhw.SetActive(false);
                    }
                };
                go.PlayForward("XH_B_2ND_JG_KA");
                XH.PlayForward("TY_XH_JG_B2-3");
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
        string curObjName = EnhanceCommunityModel.GetInstance().CurReinforcement.pData.name_cn;
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
        XH.transitionTime = 0f;
        Debug.Log("教师给物品");
        HighLightCtrl.GetInstance().FlashOff(jshand);
        ClickDispatcher.Inst.EnableClick = false;
        swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, false);

        LS.OnContinue();//老师给物品。
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
        HighLightCtrl.GetInstance().OffAllObjs();
        ChooseDo.Instance.ResetAll();
        UIManager.Instance.GetUI<CommonUI>("CommonUI").HideFinalUI();

        if (XH != null)
        {
            XHCtrl xhctrl = XH.GetComponent<XHCtrl>();
            xhctrl.DestroyGuadian();
        }

        if (LS != null)
        {
            LSCtrl lsctrl = LS.GetComponent<LSCtrl>();
            lsctrl.DestroyGuadian();
        }


        Transform zhuozi2 = EnhanceCommunityModel.GetInstance().Jiaoshi().transform.Find("shinei/zhuozi2");
        zhuozi2.transform.localPosition = zhuozi2Pos;

        RemoveAllListeners();
    }
    void ReDo()
    {
        Debug.Log("redo");
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
        if (swapUI != null)
        {
            swapUI.chooseEvent -= ChooseBtnClickCallback;
            swapUI.speakEvent -= SpeakBtnClickCallback;
        }
        if (selectUI != null)
        {
            selectUI.okEvent -= SelectUIOkBtnCallback;
        }

        com = null;

        if (LS != null)
        {
            LS.Complete -= LsGiveObjCallback;
            LS.timePointEvent = null;
            LS.ClearCompleteEvent();
        }

        if (XH != null)
        {
            XH.Complete -= XHJiewuCallback;
            XH.timePointEvent = null;
            XH.ClearCompleteEvent();
        }

        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickLsCallBack);
    }
    public void Dispose()
    {
        //RemoveAllListeners();
        Finish();
        evtFinished = null;
        evtRedo = null;
        //PeopleManager.Instance.Reset();
        if (XH != null)
        {
            XH.gameObject.SetActive(false);
            XH.transform.Find("Group/Main").localPosition = new Vector3(1.952808f, 0, 0.3788859f);
            //XH.transform.localPosition = new Vector3(0, 0, 10000);
            XH.gameObject.SetActive(true);
            XH.OnContinue();
            //XH.PlayForward("idle");
            XH.ClearCompleteEvent();
            DestroyImmediate(XH.gameObject);
            PeopleManager.Instance.GetNewXH();
        }
        GlobalDataManager.GetInstance().SetPcesCamera();
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        CancelInvoke();
    }
}
