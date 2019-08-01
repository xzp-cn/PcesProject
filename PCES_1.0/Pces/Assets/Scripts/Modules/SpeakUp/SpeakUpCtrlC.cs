using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 第三关 主动要求镜头跟随小华旋转
/// </summary>
public class SpeakUpCtrlC : MonoBehaviour
{
    public System.Action evtFinished;
    public System.Action evtRedo;
    private CommonUI comUI;
    private GameObject emptyRoot;
    private GameObject judaiGobj; //句带
    private GameObject RndReinforcementA; //强化物
    private GameObject tukaA;  //图卡
    private GameObject FBNKT_KA_Anim;
    private AnimationOper xiaohuaAnim;
    private LegacyAnimationOper FBNKT_KA_AnimOper;
    private AnimationOper LS;
    private QHWCtrl qhwCtrl;
    XHCtrl xhCtrl;
    private GameObject goodA;
    Vector3 zhuozi2Pos;
    Vector3 camPos;
    Vector3 lsOldPos;
    void Start()
    {
        GameObject xiaohuaGo = PeopleManager.Instance.GetPeople("XH_BD");

        //camPos = Camera.main.transform.parent.localPosition;
        //Camera.main.transform.parent.localPosition = new Vector3(4.032f, 1.071f, 0.43f);
        GlobalDataManager.GetInstance().SetPcesCamera(Vector3.zero);

        Transform zhuozi2 = EnhanceCommunityModel.GetInstance().Jiaoshi().transform.Find("shinei/zhuozi2");
        zhuozi2Pos = zhuozi2.transform.localPosition;
        zhuozi2.localPosition = new Vector3(0.0608f, 0, 0.036536f);

        if (xiaohuaGo.GetComponent<XHCtrl>() == null)
        {
            xiaohuaGo.AddComponent<XHCtrl>().InitComplete = () =>
            {
                xiaohuaGo.GetComponent<XHCtrl>().r_tuka.SetActive(false);
                xiaohuaGo.GetComponent<XHCtrl>().r_tuka2.SetActive(false);
                xiaohuaGo.GetComponent<XHCtrl>().r_judai.SetActive(false);
                xiaohuaGo.GetComponent<XHCtrl>().r_judai2.SetActive(false);
            };
        }


        //初始化老师位置和相机位置
        LS = PeopleManager.Instance.GetPeople("LS_BD").GetAnimatorOper();
        lsOldPos = LS.transform.localPosition;
        LS.transform.localPosition = new Vector3(1.516f, 0.02f, 0.022f);
        //Camera.main.transform.parent.localPosition = new Vector3(3.835f, 1.071f, 0.05f);
        emptyRoot = new GameObject("Root");

        GameObject qhwm = ObjectsManager.instanse.GetQHW();
        qhwm.transform.SetParent(emptyRoot.transform);
        qhwCtrl = qhwm.GetComponent<QHWCtrl>();
        qhwCtrl.transform.localPosition = new Vector3(1.508f, 0, 0);

        //生成我要句带
        GameObject judaiParent = new GameObject("judaiParent");
        judaiParent.transform.SetParent(emptyRoot.transform, false);
        judaiParent.transform.localPosition = new Vector3(2.281f, 0.549f, -0.334f);
        judaiGobj = GameObject.Instantiate(ObjectsManager.instanse.propList[(int)PropsTag.judai_woyao].gameObject);
        judaiGobj.GetComponent<PropsObject>().pData = ObjectsManager.instanse.propList[(int)PropsTag.judai_woyao].pData;
        judaiGobj.transform.SetParent(judaiParent.transform, false);

        //随机一个强化物A
        goodA = SpeakUpModel.GetInstance().GetRndReinforcement();

        RndReinforcementA = qhwCtrl.GetObj(goodA.name);
        //RndReinforcementA.GetComponent<PropsObject>().pData = goodA.GetComponent<PropsObject>().pData;
        //GameObject qhwA = new GameObject("ReinforcementA");
        //qhwA.transform.SetParent(emptyRoot.transform, false);
        //RndReinforcementA.transform.SetParent(qhwA.transform, false);
        //RndReinforcementA.transform.localPosition = Vector3.zero;
        //qhwA.transform.localPosition = new Vector3(2.5328F, 0.5698F, -0.118F);
        //强化物图卡A
        string tukaNameA = "tuka_" + goodA.name;
        tukaA = Instantiate(DistinguishPictureModel.GetInstance().GetTuKa(tukaNameA));
        GameObject _tukaA = new GameObject("tukaA");
        _tukaA.transform.SetParent(emptyRoot.transform, false);
        _tukaA.transform.localPosition = new Vector3(999f, 999f, 999f);
        tukaA.transform.SetParent(_tukaA.transform, false);
        tukaA.transform.localPosition = Vector3.zero;

        FBNKT_KA_Anim = ResManager.GetPrefab("Prefabs/AnimationKa/XH_D_2ND_FYFT_KA");
        FBNKT_KA_Anim.name = "XH_D_2ND_FYFT_KA";
        FBNKT_KA_Anim.transform.SetParent(emptyRoot.transform, false);
        //我要图卡
        FBNKT_KA_Anim.transform.Find("XH_judaiA/XH_judaiA 1/tukaA/tukaA 1").GetComponent<MeshRenderer>().materials[1].CopyPropertiesFromMaterial(judaiGobj.GetComponent<MeshRenderer>().materials[1]);
        //强化物图卡
        FBNKT_KA_Anim.transform.Find("XH_judaiA/XH_judaiA 1/tukaB/tukaB 1").GetComponent<MeshRenderer>().materials[1].CopyPropertiesFromMaterial(tukaA.GetComponent<MeshRenderer>().materials[1]);

        PeopleManager.Instance.GetPeople("FDLS_BD").SetActive(false);

        //xiaohuaGo.transform.localScale = Vector3.zero;
        xiaohuaAnim = xiaohuaGo.GetAnimatorOper();
        xhCtrl = xiaohuaAnim.GetComponent<XHCtrl>();
        //进入界面1秒后，触动小华翻开沟通本，并把字卡和图卡都粘在句带，并走到老师的面前的动画。
        Invoke("OnXiaoHuaBring", 1f);
    }

    void OnXiaoHuaBring()
    {
        int start0 = 160;
        int end0 = 161;
        xiaohuaAnim.timePointEvent = (t) =>
        {
            if (t >= start0 && t <= end0)
            {
                xiaohuaAnim.timePointEvent = null;


                int start = 287;
                int end = 288;
                xiaohuaAnim.timePointEvent = (tt) =>
                {
                    if (tt >= start && tt <= end)
                    {
                        xiaohuaAnim.timePointEvent = null;
                        XHCtrl xhctrl = xiaohuaAnim.GetComponent<XHCtrl>();
                        xhctrl.r_judai2.SetActive(true);
                        xhctrl.r_judai2.transform.Find("XH_judai_2 1/jd_tuka_1").GetComponent<MeshRenderer>().enabled = false;
                        xhctrl.r_judai2.transform.Find("XH_judai_2 1/jd_tuka_2").GetComponent<MeshRenderer>().materials[1].CopyPropertiesFromMaterial(judaiGobj.GetComponent<MeshRenderer>().materials[1]);
                        xhctrl.r_judai2.transform.Find("XH_judai_2 1/jd_tuka_3").GetComponent<MeshRenderer>().materials[1].CopyPropertiesFromMaterial(tukaA.GetComponent<MeshRenderer>().materials[1]);

                        FBNKT_KA_Anim.transform.Find("XH_judaiA").gameObject.SetActive(false);

                        int start1 = 465;
                        int end1 = 467;
                        xiaohuaAnim.timePointEvent = (ts) =>
                        {
                            if (ts >= start1 && ts <= end1)
                            {
                                xiaohuaAnim.timePointEvent = null;
                                xiaohuaAnim.OnPause();

                                //2. 播放结束，提示操作者点击教师的手，播放教师接卡的动画。
                                GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
                                Debug.Log("SpeakUpCtrlC.OnXiaoHuaBring(): 2. 播放结束，提示操作者点击教师的手，播放教师接卡的动画。");
                                HighLightCtrl.GetInstance().FlashOn(shou);
                                shou.GetBoxCollider().size = new Vector3(1, 0.2f, 0.5f);
                                GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFirst);
                                ClickDispatcher.Inst.EnableClick = true;
                                ChooseDo.Instance.DoWhat(5, RedoClickTeachersHandFirst, null);
                            }

                        };
                    }
                };
            }
        };


        xiaohuaAnim.PlayForward("XH_D_3RD_ZGJD");
        FBNKT_KA_AnimOper = FBNKT_KA_Anim.GetLegacyAnimationOper();
        FBNKT_KA_AnimOper.PlayForward("XH_D_2ND_FYFT_KA");
    }

    private void RedoClickTeachersHandFirst()
    {
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师接卡");
        CancelInvoke("ClickTeachersPromptFirst");
        Invoke("ClickTeachersPromptFirst", 2);
    }

    private void ClickTeachersPromptFirst()
    {
        ChooseDo.Instance.DoWhat(5, RedoClickTeachersHandFirst, null);
    }



    private void OnClickTeacherHandFirst(ClickedObj cobj)
    {
        Debug.Log("SpeakUpCtrlC.OnClickTeacherHandFirst(): " + cobj.objname);
        if (cobj.objname == "shou")
        {
            ChooseDo.Instance.Clicked();
            CancelInvoke("ClickTeachersPromptFirst");
            GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFirst);
            ClickDispatcher.Inst.EnableClick = false;
            HighLightCtrl.GetInstance().FlashOff(cobj.go);

            LS = PeopleManager.Instance.GetPeople(PeopleTag.LS_BD).GetAnimatorOper();

            LSCtrl lsCtrl = LS.GetComponent<LSCtrl>();
            LS.OnContinue();
            int st = 22;
            int et = 24;
            LS.timePointEvent = (a) =>//老师借卡时间点
            {

                if (a >= st && a < et)
                {
                    st = 9999;
                    et = 9999;
                    UnityEngine.Debug.Log("SpeakUpCtrlA::OnClickTeacherHandFinal(): 隐藏沟通本句带");

                    //transform.Find("XH_D_1ST_FBNKT_KA/XH_judaiA").gameObject.SetActive(false);//沟通本图卡隐藏
                    xiaohuaAnim.OnContinue();
                    /*FBNKT_KA_Anim.transform.Find("XH    _judaiA").gameObject.SetActive(false);*///沟通本图卡隐藏
                    xhCtrl.r_judai2.SetActive(false);                                                               /*                    LS.OnPause();  */                                                                    //xiaohuaAnim.PlayForward("XH_D_1ST_BACK");//小华手收回


                    Transform jd1 = lsCtrl.ls_judai.transform.Find("ls_judai_1");
                    jd1.Find("ls_jd_tuka_1").gameObject.SetActive(false);
                    jd1.Find("ls_jd_tuka_2").gameObject.SetActive(true);
                    jd1.Find("ls_jd_tuka_3").gameObject.SetActive(true);

                    jd1.Find("ls_jd_tuka_2").gameObject.GetComponent<MeshRenderer>().materials[1].CopyPropertiesFromMaterial(judaiGobj.GetComponent<MeshRenderer>().materials[1]);
                    jd1.Find("ls_jd_tuka_3").gameObject.GetComponent<MeshRenderer>().materials[1].CopyPropertiesFromMaterial(tukaA.GetComponent<MeshRenderer>().materials[1]);
                    lsCtrl.ls_judai.gameObject.SetActive(true);



                }

                if (a >= 35 && a <= 37)
                {
                    LS.timePointEvent = null;
                    lsCtrl.ls_judai.gameObject.SetActive(false);

                    //5. 播放结束，提醒操作者点击话筒，点击后话筒旁边显示“你要吃XXX呀”
                    SwapUI swapui = UIManager.Instance.GetUI<SwapUI>("SwapUI");
                    swapui.SetButtonVisiable(SwapUI.BtnName.microButton, true);
                    swapui.SetButtonVisiable(SwapUI.BtnName.chooseButton, false);
                    swapui.GetMicroBtn.gameObject.GetUIFlash().StartFlash();
                    swapui.speakEvent = () =>
                    {
                        swapui.GetMicroBtn.gameObject.GetUIFlash().StopFlash();
                        swapui.speakEvent = null;
                        swapui.SetButtonVisiable(SwapUI.BtnName.microButton, false);
                        Dialog dialog = UIManager.Instance.GetUI<Dialog>("Dialog");
                        string gift = goodA.GetComponent<PropsObject>().pData.name_cn;
                        dialog.SetDialogMessage("小华要" + gift + "呀。");

                        //6. 显示2秒，结束后，提醒操作者点击教师的手，点击后触发教师给小华的动画。
                        Invoke("ClickTeachersHandSecond", 2f);
                    };
                }

            };




            //LegacyAnimationOper ka = ResManager.GetPrefab("Prefabs/AnimationKa/TY_LS_JTKJD_KA").GetLegacyAnimationOper();//跟随老师句带移动卡片
            //ka.name = "TY_LS_JTKJD_KA";
            //ka.transform.SetParent(transform);
            //ka.transform.Find("LS_judai_1/ls_judai_1/ls_jd_tuka_1").gameObject.SetActive(false);//隐藏不需要图卡
            //Material matWy = ka.transform.Find("LS_judai_1/ls_judai_1/ls_jd_tuka_2").GetComponent<MeshRenderer>().materials[1];//老师我要
            //Material matObj = ka.transform.Find("LS_judai_1/ls_judai_1/ls_jd_tuka_3").GetComponent<MeshRenderer>().materials[1];//老师图卡物品
            //Material matSourceWy = emptyRoot.transform.Find("XH_D_2ND_FYFT_KA/XH_judaiA/XH_judaiA 1/tukaA/tukaA 1").GetComponent<MeshRenderer>().materials[1];//小华我要图卡
            //Material matSourceObj = emptyRoot.transform.Find("XH_D_2ND_FYFT_KA/XH_judaiA/XH_judaiA 1/tukaB/tukaB 1").GetComponent<MeshRenderer>().materials[1];//小华递卡物品。
            //matWy.CopyPropertiesFromMaterial(matSourceWy);
            //matObj.CopyPropertiesFromMaterial(matSourceObj);//给物品

            //ka.transform.Find("tuka4").gameObject.SetActive(false);//
            //ka.PlayForward("TY_LS_JTKJD_KA");//播放老师图卡动画        图卡等待一帧隐藏

            LS.PlayForward("TY_LS_JTKJD_JG");



            ////3. 播放结束，触发小华把句带递给教师的动画。
            //int st = 22;
            //int et = 24;
            //LS.timePointEvent = (a) =>//老师接卡时间点
            //{
            //    if (a > st && a < et)
            //    {
            //        LS.timePointEvent = null;
            //        XHCtrl xhctrl = xiaohuaAnim.GetComponent<XHCtrl>();
            //        xhctrl.r_judai2.SetActive(false);
            //        xiaohuaAnim.OnContinue();

            //        //5. 播放结束，提醒操作者点击话筒，点击后话筒旁边显示“你要吃XXX呀”
            //        SwapUI swapui = UIManager.Instance.GetUI<SwapUI>("SwapUI");
            //        swapui.SetButtonVisiable(SwapUI.BtnName.microButton, true);
            //        swapui.SetButtonVisiable(SwapUI.BtnName.chooseButton, false);
            //        swapui.GetMicroBtn.gameObject.GetUIFlash().StartFlash();
            //        swapui.speakEvent = () =>
            //        {
            //            swapui.GetMicroBtn.gameObject.GetUIFlash().StopFlash();
            //            swapui.speakEvent = null;
            //            swapui.SetButtonVisiable(SwapUI.BtnName.microButton, false);
            //            Dialog dialog = UIManager.Instance.GetUI<Dialog>("Dialog");
            //            string gift = RndReinforcementA.GetComponent<PropsObject>().pData.name_cn;
            //            dialog.SetDialogMessage("小华要" + gift + "呀。");

            //            //6. 显示2秒，结束后，提醒操作者点击教师的手，点击后触发教师给小华的动画。
            //            Invoke("ClickTeachersHandSecond", 2f);
            //        };
            //    }
            //};

            //LS.PlayForward("TY_LS_JTKJD_JG");
        }
    }

    private void ClickTeachersHandSecond()
    {
        GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
        HighLightCtrl.GetInstance().FlashOn(shou);
        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickteacherHandSecond);
        ClickDispatcher.Inst.EnableClick = true;
        ChooseDo.Instance.DoWhat(5, RedoClickTeachersHandSecond, null);
    }

    private void RedoClickTeachersHandSecond()
    {
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师给相应物品");
        CancelInvoke("ClickTeachersPromptSecond");
        Invoke("ClickTeachersPromptSecond", 2);
    }

    private void ClickTeachersPromptSecond()
    {
        ChooseDo.Instance.DoWhat(5, RedoClickTeachersHandSecond, null);
    }

    private void OnClickteacherHandSecond(ClickedObj cobj)
    {
        if (cobj.objname == "shou")
        {
            Dialog dialog = UIManager.Instance.GetUI<Dialog>("Dialog");
            dialog.Show(false);

            ChooseDo.Instance.Clicked();
            CancelInvoke("ClickTeachersPromptSecond");
            GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickteacherHandSecond);
            ClickDispatcher.Inst.EnableClick = false;
            HighLightCtrl.GetInstance().FlashOff(cobj.go);

            xiaohuaAnim.Complete += () =>
            {
                //8. 播放结束，出现下一关和重做的按钮。
                Debug.Log("SpeakUpCtrlA.OnClickTeacherHandFinal(): 8. 播放结束，出现下一关和重做的按钮。");
                comUI = UIManager.Instance.GetUI<CommonUI>("CommonUI");
                comUI.redoClickEvent += OnReDo;
                comUI.nextClickEvent += OnNextDo;
                comUI.ShowFinalUI();
            };


            int st = 33;
            int et = 35;
            LS.timePointEvent = (a) =>//老师递给物品
            {
                if (a > st && a < et)//挂载到老师手上强化物时间点
                {
                    LSCtrl lsctrl = LS.GetComponent<LSCtrl>();//将当前强化物挂在老师手上
                    lsctrl.SetJoint(RndReinforcementA.transform.parent.gameObject);
                    //RndReinforcementA.transform.localPosition = Vector3.zero;
                    //Debug.LogError("ls");
                }

                if (a > 40 && a < 43)//小华接卡动画播放延迟一边挂载强化物
                {
                    LS.timePointEvent = null;


                    //播放结束，触发小华接过XXX。
                    int xhst = 40;
                    int xhet = 43;
                    xiaohuaAnim.timePointEvent = (aa) =>//小华接过物品
                    {
                        if (aa > xhst && aa < xhet)
                        {
                            xiaohuaAnim.timePointEvent = null;
                            XHCtrl xhCtrl = xiaohuaAnim.GetComponent<XHCtrl>();
                            xhCtrl.SetJoint(RndReinforcementA.transform.parent.gameObject);
                        }
                    };
                    xiaohuaAnim.PlayForward("TY_XH_JG_B2-3");
                }
            };

            LS.PlayForward("TY_LS_DW");
        }
    }

    private void OnReDo()
    {
        Redo();
    }

    private void OnNextDo()
    {
        Finished();
    }

    /// <summary>
    /// 本关完成
    /// </summary>
    void Finished()
    {
        if (evtFinished != null)
        {
            evtFinished();
        }

    }

    void Redo()
    {
        if (evtRedo != null)
        {
            evtRedo();
        }
        if (comUI == null)
        {
            comUI = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        }
        comUI.redoClickEvent -= OnReDo;
        comUI.nextClickEvent -= OnNextDo;
    }

    public void Dispose()
    {
        if (comUI == null)
        {
            comUI = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        }
        comUI.redoClickEvent -= OnReDo;
        comUI.nextClickEvent -= OnNextDo;
        comUI = null;

        xiaohuaAnim.timePointEvent = null;
        Transform zhuozi2 = EnhanceCommunityModel.GetInstance().Jiaoshi().transform.Find("shinei/zhuozi2");
        zhuozi2.transform.localPosition = zhuozi2Pos;


        XHCtrl xhctrl = xiaohuaAnim.GetComponent<XHCtrl>();
        if (xhctrl != null)
        {
            xhctrl.DestroyGuadian();
        }

        LSCtrl lsctrl = LS.GetComponent<LSCtrl>();
        if (lsctrl != null)
        {
            lsctrl.DestroyGuadian();
        }

        evtFinished = null;
        evtRedo = null;
        if (emptyRoot != null)
        {
            Destroy(emptyRoot);
            emptyRoot = null;
        }
        //恢复老师位置和相机位置
        PeopleManager.Instance.GetPeople("LS_BD").transform.localPosition = lsOldPos;
        GlobalDataManager.GetInstance().SetPcesCamera();

        xhctrl.gameObject.SetActive(false);
        xhctrl.transform.Find("Group/Main").localPosition = new Vector3(1.952808f, 0, 0.3788859f);
        //XH.transform.localPosition = new Vector3(0, 0, 10000);
        xhctrl.gameObject.SetActive(true);
        xiaohuaAnim.OnContinue();
        xiaohuaAnim.PlayForward("idle");

        Destroy(gameObject);
    }

    private void OnDestroy()
    {

    }
}
