﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 第一关 -- 区辨喜欢和不喜欢物品的图卡
/// </summary>
public class DistinguishPictureCtrlA : MonoBehaviour
{

    public event System.Action evtFinished;
    public event System.Action evtRedo;
    private CommonUI comUI;
    private GameObject emptyRoot;
    private GameObject RndReinforcementA;
    private GameObject RndNegReinforcementB;
    private GameObject tukaA;
    private GameObject _tukaA;
    private GameObject tukaB;
    private AnimationOper teacherAnim;
    private AnimationOper xiaohuaAnim;
    private LSCtrl lsCtrl;
    private XHCtrl xhctrl;
    private QHWCtrl qhwCtrl;
    private QHWCtrl qhwCtrlB;
    private GameObject goodA;
    private GameObject goodB;


    private void Awake()
    {
        emptyRoot = new GameObject("Root");
    }

    void Start()
    {
        InitGoodsState();
        teacherAnim = PeopleManager.Instance.GetPeople("LS_BD").GetAnimatorOper();
        teacherAnim.PlayForward("idle");


        //1. 进入界面后1秒，触发小华拿B卡递卡的动画。
        Invoke("OnXiaoHuaBring", 1f);
    }

    /// <summary>
    /// 初始化物品状态
    /// </summary>
    void InitGoodsState()
    {
        GameObject qhw = ObjectsManager.instanse.GetQHW();
        qhw.transform.SetParent(emptyRoot.transform);
        qhw.transform.localPosition = new Vector3(-0.12f, 0, -0.18f);
        qhwCtrl = qhw.GetComponent<QHWCtrl>();

        GameObject qhwB = GameObject.Instantiate(qhw);
        qhwB.transform.SetParent(emptyRoot.transform);
        qhwB.name = "qhwB";
        qhwCtrlB = qhwB.GetComponent<QHWCtrl>();

        //随机一个强化物A
        goodA = DistinguishPictureModel.GetInstance().GetRndReinforcement();
        RndReinforcementA = qhwCtrl.GetObj(goodA.name);
        //RndReinforcementA = GameObject.Instantiate(goodA);
        //RndReinforcementA.GetComponent<PropsObject>().pData = goodA.GetComponent<PropsObject>().pData;
        //GameObject qhwA = new GameObject("ReinforcementA");
        //qhwA.transform.SetParent(emptyRoot.transform, false);
        //RndReinforcementA.transform.SetParent(qhwA.transform, false);
        //RndReinforcementA.transform.localPosition = Vector3.zero;
        //qhwA.transform.localPosition = new Vector3(2.5328F, 0.5698F, -0.118F);

        //强化物图卡A
        string tukaNameA = "tuka_" + goodA.name;
        tukaA = GameObject.Instantiate(DistinguishPictureModel.GetInstance().GetTuKa(tukaNameA));
        _tukaA = new GameObject("tukaA");
        _tukaA.transform.SetParent(emptyRoot.transform, false);
        _tukaA.transform.localPosition = new Vector3(2.297f, 0.5466f, 0.415f);
        tukaA.transform.SetParent(_tukaA.transform, false);
        tukaA.transform.localPosition = Vector3.zero;


        //随机一个负强化物B
        goodB = DistinguishPictureModel.GetInstance().GetRndNegReinforcement();
        RndNegReinforcementB = qhwCtrlB.GetObj(goodB.name);

        //RndNegReinforcementB = GameObject.Instantiate(goodB);
        //RndNegReinforcementB.GetComponent<PropsObject>().pData = goodB.GetComponent<PropsObject>().pData;
        //qhwB = new GameObject("NegReinforcementB");
        //qhwB.transform.SetParent(emptyRoot.transform, false);
        //float offY = 0;
        //if (goodB.name == "apple")
        //{
        //    offY = 0.04f;
        //}
        //qhwB.transform.localPosition = new Vector3(2.5328f, 0.5698f + offY, 0.0913f);

        //负强化物图卡B
        string tukaNameB = "tuka_" + goodB.name;
        tukaB = GameObject.Instantiate(DistinguishPictureModel.GetInstance().GetTuKa(tukaNameB));
        GameObject _tukaB = new GameObject("tukaB");
        _tukaB.transform.SetParent(emptyRoot.transform, false);

        _tukaB.transform.localPosition = new Vector3(2.223f, 0.5466f, 0.388f);
        tukaB.transform.SetParent(_tukaB.transform, false);
        tukaB.transform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// 播放小华拿B卡递卡动画
    /// </summary>
    void OnXiaoHuaBring()
    {
        GameObject xiaohuaGo = PeopleManager.Instance.GetPeople("XH_BD");
        xiaohuaAnim = xiaohuaGo.GetAnimatorOper();
        xhctrl = xiaohuaGo.GetComponent<XHCtrl>();

        xhctrl.r_tuka2.GetComponentInChildren<MeshRenderer>().materials[1].CopyPropertiesFromMaterial(tukaB.GetComponentInChildren<MeshRenderer>().materials[1]);
        lsCtrl = teacherAnim.GetComponent<LSCtrl>();
        lsCtrl.ls_tuka2.GetComponentInChildren<MeshRenderer>().materials[1].CopyPropertiesFromMaterial(tukaB.GetComponentInChildren<MeshRenderer>().materials[1]);
        int start = 24;
        int end = 26;
        int start0 = 44;
        int end0 = 46;
        xiaohuaAnim.timePointEvent = (t) =>
        {
            if (t >= start && t <= end)
            {
                xiaohuaAnim.timePointEvent = null;
                xhctrl.r_tuka2.SetActive(true);
                tukaB.SetActive(false);

                xiaohuaAnim.timePointEvent = (tt) =>
                {
                    if (tt >= start0 && tt <= end0)
                    {
                        xiaohuaAnim.timePointEvent = null;
                        xiaohuaAnim.OnPause();
                        GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
                        Debug.Log("DistinguishPictureCtrlA.OnXiaoHuaBring(): 2. 播放结束，提醒操作者点击教师的手，点击后触发接图卡的动作");
                        HighLightCtrl.GetInstance().FlashOn(shou);
                        shou.GetBoxCollider().size = new Vector3(1, 0.2f, 0.5f);
                        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFirst);
                        ClickDispatcher.Inst.EnableClick = true;
                        ChooseDo.Instance.DoWhat(5, RedoClickTeachersHandFirst, null);
                    }
                };

            }
        };
        xiaohuaAnim.PlayForward("TY_XH_NKDK");
    }

    private void ClickTeachersPromptFirst()
    {
        ChooseDo.Instance.DoWhat(5, RedoClickTeachersHandFirst, null);
    }

    private void RedoClickTeachersHandFirst()
    {
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师接图卡");
        CancelInvoke("ClickTeachersPromptFirst");
        Invoke("ClickTeachersPromptFirst", 2);
    }

    /// <summary>
    /// 点击教师的手回调
    /// </summary>
    /// <param name="cobj"></param>
    private void OnClickTeacherHandFirst(ClickedObj cobj)
    {
        Debug.Log("DistinguishPictureCtrlA.OnClickTeacherHandFirst(): " + cobj.objname);
        if (cobj.objname == "shou")
        {
            ChooseDo.Instance.Clicked();
            CancelInvoke("ClickTeachersPromptFirst");
            GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFirst);
            ClickDispatcher.Inst.EnableClick = false;
            HighLightCtrl.GetInstance().OffAllObjs();


            //播放老师接图卡动画
            int start = 47;
            int end = 48;
            teacherAnim.timePointEvent = (t) =>
            {
                if (t >= start && t <= end)
                {

                    xhctrl.r_tuka2.SetActive(false);

                    lsCtrl.ls_tuka2.SetActive(true);

                    xiaohuaAnim.OnContinue();
                }

                if(t >= 81 && t <= 83)
                {
                    //老师放下图卡
                    lsCtrl.ls_tuka2.SetActive(false);
                    tukaB.SetActive(true);
                    tukaB.transform.parent.localPosition = new Vector3(2.5f, 0.5466f, 0.388f);
                    tukaB.transform.localPosition = Vector3.zero;
                }

                if (t >= 94 && t <= 96)
                {
                    //老师接图卡动画结束
                    teacherAnim.timePointEvent = null;
                    teacherAnim.OnPause();
                    OnReceiveTuKa();
                }
            };

            teacherAnim.PlayForward("TY_LS_JKDW");
        }
    }

    private void OnReceiveTuKa()
    {

        Debug.Log("DistinguishPictureCtrlA.OnReceiveTuKa(): 3. 播放结束，提醒操作者点击教师的手，点击后触发教师给小华B的动画。");
        //3. 播放结束，提醒操作者点击教师的手，点击后触发教师给小华B的动画。
        GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
        HighLightCtrl.GetInstance().FlashOn(shou);

        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandSecond);
        ClickDispatcher.Inst.EnableClick = true;

        ChooseDo.Instance.DoWhat(5, RedoClickTeachersHandSecond, null);
    }

    private void RedoClickTeachersHandSecond()
    {
        CancelInvoke("ClickTeachersPromptSecond");
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        //PropsObject pb = goodA.GetComponentInChildren<PropsObject>();
        //string cn_name = pb.pData.name_cn;
        tip.SetTipMessage("需要教师给相应物品");
        Invoke("ClickTeachersPromptSecond", 2);
    }

    private void ClickTeachersPromptSecond()
    {
        ChooseDo.Instance.DoWhat(5, RedoClickTeachersHandSecond, null);
    }

    private void OnClickTeacherHandSecond(ClickedObj cobj)
    {
        if (cobj.objname == "shou")
        {
            ChooseDo.Instance.Clicked();
            CancelInvoke("ClickTeachersPromptSecond");
            GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandSecond);
            ClickDispatcher.Inst.EnableClick = false;
            GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
            HighLightCtrl.GetInstance().FlashOff(shou);

            //播放教师给小华B的动画--(做在接图卡里)

            int st = 121;
            int et = 124;
            int start = 129;
            int end = 132;
            bool passed = false;
            bool passed2 = false;
            Vector3 oldPos = Vector3.zero;
            Vector3 oldParentPos = Vector3.zero;
            lsCtrl.l_guadian.transform.localPosition = Vector3.zero;
            teacherAnim.timePointEvent = (t) =>
            {
                if (t >= st && t <= et && !passed)
                {
                    if (lsCtrl != null)
                    {
                        //老师拿负强化物
                        oldParentPos = RndNegReinforcementB.transform.parent.localPosition;
                        oldPos = RndNegReinforcementB.transform.localPosition;
                        lsCtrl.SetJoint(RndNegReinforcementB.transform.parent.gameObject);
                        RndNegReinforcementB.transform.parent.localPosition = Vector3.zero;
                        RndNegReinforcementB.transform.localPosition = Vector3.zero;

                    }
                    passed = true;
                }

                if (t >= start && t <= end && !passed2)
                {
                    passed2 = true;
                    Debug.Log("DistinguishPictureCtrlA.OnClickTeacherHandSecond(): 4. 播放结束，触发小华用手推开B的动画。");

                    xiaohuaAnim.PlayForward("XH_C_1ST_JJ");
                }

                if (t >= 197 && t <= 199)
                {
                    teacherAnim.timePointEvent = null;
                    UnityEngine.Debug.Log("DistinguishPictureCtrlA::teacherAnim.Complete(): ");
                    //老师放下负强化物
                    RndNegReinforcementB.transform.parent.SetParent(emptyRoot.transform);
                    RndNegReinforcementB.transform.localPosition = oldPos;
                    RndNegReinforcementB.transform.parent.localPosition = oldParentPos;
                    RndNegReinforcementB.transform.parent.localRotation = Quaternion.Euler(Vector3.zero);
                }
            };

            //4. 播放结束，触发小华用手推开B的动画。
            xiaohuaAnim.Complete += () =>
            {
                OnXiaoHuaPushB();
            };
            teacherAnim.OnContinue();
            //teacherAnim.PlayForward("TY_LS_DW");
        }
    }

    void OnXiaoHuaPushB()
    {
        //5. 播放结束，提醒操作者点击教师的手，点击后触发教师指A卡的动画。
        ChooseDo.Instance.DoWhat(5, RedoClickTeachersHandThird, null);
        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandThird);
        ClickDispatcher.Inst.EnableClick = true;
        GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
        HighLightCtrl.GetInstance().FlashOn(shou);
    }

    private void RedoClickTeachersHandThird()
    {
        CancelInvoke("ClickTeachersPromptThird");
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师指图卡");
        Invoke("ClickTeachersPromptThird", 2);
    }

    private void ClickTeachersPromptThird()
    {
        ChooseDo.Instance.DoWhat(5, RedoClickTeachersHandThird, null);
    }


    void OnClickTeacherHandThird(ClickedObj cobj)
    {
        if (cobj.objname == "shou")
        {
            ChooseDo.Instance.Clicked();
            CancelInvoke("ClickTeachersPromptThird");
            GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
            HighLightCtrl.GetInstance().FlashOff(shou);
            GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandThird);
            ClickDispatcher.Inst.EnableClick = false;

            teacherAnim.Complete += () =>
            {
                //6. 播放结束，触发小华拿起A卡、递卡的动画。
                Debug.Log("DistinguishPictureCtrlA.OnClickTeacherHandThird(): 6. 播放结束，触发小华拿起A卡、递卡的动画。");
                xhctrl.r_tuka2.GetComponentInChildren<MeshRenderer>().materials[1].CopyPropertiesFromMaterial(tukaA.GetComponentInChildren<MeshRenderer>().materials[1]);
                xhctrl.r_tuka2.SetActive(true);

                int start = 24;
                int end = 26;
                int start0 = 44;
                int end0 = 46;
                xiaohuaAnim.timePointEvent = (t) =>
                {
                    if (t >= start && t <= end)
                    {
                        tukaA.SetActive(false);

                        xiaohuaAnim.timePointEvent = (tt) =>
                        {
                            if (tt >= start0 && tt <= end0)
                            {
                                xiaohuaAnim.timePointEvent = null;
                                xiaohuaAnim.OnPause();

                                OnXiaoHuaBringAToTeacher();
                            }
                        };

                    }
                };
                xiaohuaAnim.PlayForward("TY_XH_NKDK");

            };
            teacherAnim.PlayForward("LS_C_1ST_ZZ");
        }
    }

    private bool isEnter = false;

    void OnXiaoHuaBringAToTeacher()
    {
        if (isEnter)
        {
            //防止帧事件多次进入
            return;
        }
        isEnter = true;
        //7. 播放结束，提醒操作者点击教师的手，点击后触发教师接卡的动画。
        Debug.Log("DistinguishPictureCtrlA.OnXiaoHuaBringAToTeacher(): 7. 播放结束，提醒操作者点击教师的手，点击后触发教师接卡的动画。");
        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFourth);
        ClickDispatcher.Inst.EnableClick = true;
        GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
        HighLightCtrl.GetInstance().FlashOn(shou);
        ChooseDo.Instance.DoWhat(5, RedoClickTeachersHandFourth, null);
    }

    private void RedoClickTeachersHandFourth()
    {
        CancelInvoke("ClickTeachersPromptFourth");
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师接图卡");
        Invoke("ClickTeachersPromptFourth", 2);
    }

    private void ClickTeachersPromptFourth()
    {
        ChooseDo.Instance.DoWhat(5, RedoClickTeachersHandFourth, null);
    }

    void OnClickTeacherHandFourth(ClickedObj cobj)
    {
        if (cobj.objname == "shou")
        {
            ChooseDo.Instance.Clicked();
            CancelInvoke("ClickTeachersPromptFourth");
            GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFourth);
            ClickDispatcher.Inst.EnableClick = false;
            GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
            HighLightCtrl.GetInstance().FlashOff(shou);

            xhctrl.r_tuka2.GetComponentInChildren<MeshRenderer>().materials[1].CopyPropertiesFromMaterial(tukaA.GetComponentInChildren<MeshRenderer>().materials[1]);
            xhctrl.r_tuka2.SetActive(true);

            //播放老师接图卡动画
            int start = 47;
            int end = 48;
            teacherAnim.timePointEvent = (t) =>
            {
                if (t >= start && t <= end)
                {
                    UnityEngine.Debug.Log("DistinguishPictureCtrlA::OnClickTeacherHandFourth() : teacherAnim.timePointEvent");
                    xhctrl.r_tuka2.SetActive(false);
                    teacherAnim.timePointEvent = null;
                    lsCtrl.ls_tuka2.GetComponentInChildren<MeshRenderer>().materials[1].CopyPropertiesFromMaterial(tukaA.GetComponentInChildren<MeshRenderer>().materials[1]);
                    lsCtrl.ls_tuka2.SetActive(true);
                    xiaohuaAnim.OnContinue();
                }
            };

            teacherAnim.Complete += ShowMicoUI;

            teacherAnim.PlayForward("TY_LS_JK");
        }
    }

    private void ShowMicoUI()
    {
        //老师第二次接下图卡
        lsCtrl.ls_tuka2.SetActive(false);
        tukaA.SetActive(true);
        tukaA.transform.parent.localPosition = new Vector3(2.5f, 0.5466f, 0.388f);

        //8. 播放结束，提醒操作者点击话筒，点击后话筒旁边显示“你要吃XXX呀”
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
            dialog.SetDialogMessage("小华要" + gift + "呀");

            //9. 显示2秒，结束后，提醒操作者点击教师的手，点击后触发教师给小华的动画。
            Invoke("ClickTeachersHandFinal", 2f);
        };
    }

    private void ClickTeachersHandFinal()
    {
        teacherAnim.Complete -= ShowMicoUI;
        Dialog dialog = UIManager.Instance.GetUI<Dialog>("Dialog");
        dialog.Show(false);
        GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
        HighLightCtrl.GetInstance().FlashOn(shou);

        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFinal);
        ClickDispatcher.Inst.EnableClick = true;
        ChooseDo.Instance.DoWhat(5, RedoClickTeachersHandFinal, null);
    }

    private void RedoClickTeachersHandFinal()
    {
        CancelInvoke("ClickTeachersPromptFinal");
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师给相应物品");
        Invoke("ClickTeachersPromptFinal", 2);
    }

    private void ClickTeachersPromptFinal()
    {
        ChooseDo.Instance.DoWhat(5, RedoClickTeachersHandFinal, null);
    }

    private void OnClickTeacherHandFinal(ClickedObj cobj)
    {
        if (cobj.objname == "shou")
        {
            ChooseDo.Instance.Clicked();
            CancelInvoke("ClickTeachersPromptFinal");
            GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFinal);
            ClickDispatcher.Inst.EnableClick = false;

            HighLightCtrl.GetInstance().FlashOff(cobj.go);

            xiaohuaAnim.Complete += () =>
            {
                //11. 播放结束，出现下一关和重做的按钮。
                Debug.Log("DistinguishPictureCtrlA.OnClickTeacherHandFinal(): 11. 播放结束，出现下一关和重做的按钮。");
                comUI = UIManager.Instance.GetUI<CommonUI>("CommonUI");
                comUI.redoClickEvent += OnReDo;
                comUI.nextClickEvent += OnNextDo;
                comUI.ShowFinalUI();
            };

            int st = 37;
            int et = 39;
            int stm = 45;
            int etm = 47;
            int xhst = 24;
            int xhet = 26;
            teacherAnim.timePointEvent = (a) =>//老师递给物品
            {
                if (a > st && a < et)//挂载到老师手上强化物时间点
                {
                    //将当前强化物挂在老师手上
                    xhctrl.SetJoint(RndReinforcementA.transform.parent.gameObject);
                    RndReinforcementA.transform.parent.localPosition = Vector3.zero;
                    RndReinforcementA.transform.localPosition = Vector3.zero;
                }

                if (a > stm && a < etm)//小华接卡动画播放延迟一边挂载强化物
                {
                    teacherAnim.timePointEvent = null;


                    xiaohuaAnim.timePointEvent = (aa) =>//小华接过物品
                    {
                        if (aa > xhst && aa < xhet)
                        {
                            xiaohuaAnim.timePointEvent = null;


                            xiaohuaAnim.OnPause();
                        }
                    };
                    xiaohuaAnim.PlayForward("TY_XH_JG");
                }
            };

            teacherAnim.PlayForward("TY_LS_DW");
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
        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFirst);
        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandSecond);
        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandThird);
        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFourth);
        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFinal);
        if (comUI == null)
        {
            comUI = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        }
        comUI.redoClickEvent -= OnReDo;
        comUI.nextClickEvent -= OnNextDo;
        comUI = null;
        xiaohuaAnim.OnContinue();
        xiaohuaAnim.timePointEvent = null;
        teacherAnim.timePointEvent = null;
        xhctrl.DestroyGuadian();
        lsCtrl.DestroyGuadian();


        evtFinished = null;
        evtRedo = null;
        if (emptyRoot != null)
        {
            Destroy(emptyRoot);
            emptyRoot = null;
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {

    }
}
