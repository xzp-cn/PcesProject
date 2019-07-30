﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 第二关 -- 区辨两个喜欢物品的图卡
/// </summary>
public class DistinguishPictureCtrlB : MonoBehaviour
{

    public event System.Action evtFinished;
    public event System.Action evtRedo;
    private CommonUI comUI;
    private GameObject emptyRoot;
    //private GameObject RndReinforcementA;
    private GameObject RndNegReinforcementB;
    private GameObject qhwB;
    private GameObject tukaA;
    private GameObject _tukaA;
    private GameObject tukaB;
    private AnimationOper teacherAnim;
    private AnimationOper xiaohuaAnim;
    private LSCtrl lsCtrl;
    private XHCtrl xhctrl;
    private QHWCtrl qhwCtrl;
    private GameObject goodA;
    private GameObject goodB;
    private Vector3 oldPos;

    private void Awake()
    {
        emptyRoot = new GameObject("Root");
    }

    void Start()
    {
        InitGoodsState();
        teacherAnim = PeopleManager.Instance.GetPeople("LS_BD").GetAnimatorOper();
        teacherAnim.PlayForward("idle");

        //1. 进入界面后1秒，触发小华拿A卡递卡的动画。
        Invoke("OnXiaoHuaBringA", 1f);
    }

    /// <summary>
    /// 初始化物品状态
    /// </summary>
    void InitGoodsState()
    {
        GameObject qhw = ObjectsManager.instanse.GetQHW();
        qhw.transform.SetParent(emptyRoot.transform);
        qhwCtrl = qhw.GetComponent<QHWCtrl>();

        //随机一个强化物A
        goodA = DistinguishPictureModel.GetInstance().GetRndReinforcement();
        //RndReinforcementA = qhwCtrl.GetObj(goodA.name);
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
        _tukaA.transform.localPosition = new Vector3(2.288f, 0.5466f, 0.408f);
        tukaA.transform.SetParent(_tukaA.transform, false);
        tukaA.transform.localPosition = Vector3.zero;


        //随机一个负强化物B
        goodB = DistinguishPictureModel.GetInstance().GetRndNegReinforcement();
        RndNegReinforcementB = qhwCtrl.GetObj(goodB.name);
        //RndNegReinforcementB = GameObject.Instantiate(goodB);
        //RndNegReinforcementB.GetComponent<PropsObject>().pData = goodB.GetComponent<PropsObject>().pData;
        //qhwB = new GameObject("NegReinforcementB");
        //qhwB.transform.SetParent(emptyRoot.transform, false);
        //float offY = 0;
        //if (goodB.name == "apple")
        //{
        //    offY = 0.04f;
        //}
        //qhwB.transform.localPosition = new Vector3(2.5328f, 0.5698f + offY, 0.333f);
        //RndNegReinforcementB.transform.SetParent(qhwB.transform, false);
        //RndNegReinforcementB.transform.localPosition = Vector3.zero;

        //负强化物图卡B
        string tukaNameB = "tuka_" + goodB.name;
        tukaB = GameObject.Instantiate(DistinguishPictureModel.GetInstance().GetTuKa(tukaNameB));
        GameObject _tukaB = new GameObject("tukaB");
        _tukaB.transform.SetParent(emptyRoot.transform, false);

        _tukaB.transform.localPosition = new Vector3(2.288f, 0.5466f, 0.408f);
        tukaB.transform.SetParent(_tukaB.transform, false);
        tukaB.transform.localPosition = Vector3.zero;

    }

    /// <summary>
    /// 播放小华拿A卡递卡动画
    /// </summary>
    void OnXiaoHuaBringA()
    {
        GameObject xiaohuaGo = PeopleManager.Instance.GetPeople("XH_BD");
        xiaohuaAnim = xiaohuaGo.GetAnimatorOper();

        xhctrl = xiaohuaGo.GetComponent<XHCtrl>();

        xhctrl.r_tuka2.GetComponentInChildren<MeshRenderer>().materials[1].CopyPropertiesFromMaterial(tukaA.GetComponentInChildren<MeshRenderer>().materials[1]);
        lsCtrl = teacherAnim.GetComponent<LSCtrl>();
        lsCtrl.ls_tuka2.GetComponentInChildren<MeshRenderer>().materials[1].CopyPropertiesFromMaterial(tukaA.GetComponentInChildren<MeshRenderer>().materials[1]);
        int start = 24;
        int end = 26;
        xiaohuaAnim.timePointEvent = (t) =>
        {
            if (t >= start && t <= end)
            {
                //xiaohuaAnim.timePointEvent = null;
                xhctrl.r_tuka2.SetActive(true);
                tukaA.SetActive(false);

                int start0 = 44;
                int end0 = 46;
                xiaohuaAnim.timePointEvent = (tt) =>
                {
                    if (tt >= start0 && tt <= end0)
                    {
                        xiaohuaAnim.timePointEvent = null;
                        xiaohuaAnim.OnPause();
                        GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
                        Debug.Log("DistinguishPictureCtrlB.OnXiaoHuaBring(): 2. 播放结束，提醒操作者点击教师的手，点击后触发接图卡的动作");
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
        tip.SetTipMessage("请点击老师的手");
        CancelInvoke("ClickTeachersPromptFirst");
        Invoke("ClickTeachersPromptFirst", 2);
    }

    /// <summary>
    /// 点击教师的手回调
    /// </summary>
    /// <param name="cobj"></param>
    private void OnClickTeacherHandFirst(ClickedObj cobj)
    {
        Debug.Log("DistinguishPictureCtrlB.OnClickTeacherHandFirst(): " + cobj.objname);
        if (cobj.objname == "shou")
        {
            ChooseDo.Instance.Clicked();
            CancelInvoke("ClickTeachersPromptFirst");
            GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFirst);
            ClickDispatcher.Inst.EnableClick = false;
            HighLightCtrl.GetInstance().FlashOff(cobj.go);


            //2. 播放结束，提醒操作者点击教师的手，点击后触发接A图卡的动作。
            int start = 47;
            int end = 48;
            teacherAnim.timePointEvent = (t) =>
            {
                if (t >= start && t <= end)
                {
                    teacherAnim.timePointEvent = null;
                    xhctrl.r_tuka2.SetActive(false);
                    lsCtrl.ls_tuka2.SetActive(true);
                    xiaohuaAnim.OnContinue();
                }
            };

            teacherAnim.Complete += () =>
            {
                lsCtrl.ls_tuka2.SetActive(false);
                xhctrl.r_tuka2.GetComponentInChildren<MeshRenderer>().materials[1].CopyPropertiesFromMaterial(tukaA.GetComponentInChildren<MeshRenderer>().materials[1]);
                OnClickHuaTong();
            };

            teacherAnim.PlayForward("TY_LS_JK");
        }
    }

    void OnClickHuaTong()
    {
        //3. 播放结束，提醒操作者点击话筒，显示“自己拿”。
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
            string gift = "自己拿";
            dialog.SetDialogMessage(gift);

            //4. 播放结束，触发小华拿起B的动画。
            /*
             小华伸手拿起了老师面前的强化物B。（小华手还没接触到强物化B，物品被老师用左手移到左边，老师的右手指了指强化物B的图卡）。
             */
            Debug.Log("DistinguishPictureCtrlB::OnClickHuaTong(): 4. 播放结束，触发小华拿起B的动画。");
            int start = 20;
            int end = 21;
            xiaohuaAnim.timePointEvent = (t) =>
            {
                if (t >= start && t <= end)
                {
                    xiaohuaAnim.timePointEvent = null;
                    dialog.Show(false);
                    xiaohuaAnim.OnPause();

                    OnXiaoHuaBringB();
                }
            };

            xiaohuaAnim.PlayForward("XH_C_2ND_NA");
        };
    }

    /*
     小华坐在桌子的一边，老师坐在对面。桌上有一张强化物A图卡和另一张强化物B的图卡和实物。(图卡和实物相对应)
  教师手上有一个强化物A和B，
  小华拿到A图卡后，递给老师。 老师接过图卡后，说“自己拿”。
  小华拿到强化物B，则老师一只手拿走强化物A，另一只手指指B的强化物图卡。
  小华看到后拿起图卡B，递递给老师。
  老师接过图卡并说：“哦，你要B”并递给小华。
    */
    private bool passed;

    void OnXiaoHuaBringB()
    {
        if (passed)
        {
            return;
        }
        passed = true;
        GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
        HighLightCtrl.GetInstance().FlashOn(shou);
        //5. 播放结束，提醒操作者点击教师的手，点击后触发教师拿走强化物B动画。
        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandSecond);
        ClickDispatcher.Inst.EnableClick = true;

        ChooseDo.Instance.DoWhat(5, RedoClickTeachersHandSecond, null);
    }

    private void RedoClickTeachersHandSecond()
    {
        CancelInvoke("ClickTeachersPromptSecond");
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("请点击老师的手");
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

            int start = 29;
            int end = 30;
            teacherAnim.timePointEvent = (t) =>
            {
                if (t >= start && t <= end)
                {
                    teacherAnim.timePointEvent = null;
                    xiaohuaAnim.OnContinue();
                    oldPos = RndNegReinforcementB.transform.localPosition;
                    RndNegReinforcementB.transform.localPosition = new Vector3(2.5328f, 0.6098f, 0.085f);
                }
            };

            teacherAnim.Complete += () =>
            {
                //6. 播放结束，提醒操作者点击教师的手，点击后触发教师指指B卡的动画。
                OnClickTeacherShouThird();
            };
            teacherAnim.PlayForward("LS_C_2ND_YZ");
        }
    }

    void OnClickTeacherShouThird()
    {
        GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
        HighLightCtrl.GetInstance().FlashOn(shou);
        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandThird);
        ClickDispatcher.Inst.EnableClick = true;
        ChooseDo.Instance.DoWhat(5, RedoClickTeachersHandThird, null);
    }

    private void ClickTeachersPromptThird()
    {
        ChooseDo.Instance.DoWhat(5, RedoClickTeachersHandThird, null);
    }

    private void RedoClickTeachersHandThird()
    {
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("请点击老师的手");
        CancelInvoke("ClickTeachersPromptThird");
        Invoke("ClickTeachersPromptThird", 2);
    }



    void OnClickTeacherHandThird(ClickedObj cobj)
    {
        if (cobj.objname == "shou")
        {
            ChooseDo.Instance.Clicked();
            CancelInvoke("ClickTeachersPromptThird");
            GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandThird);
            ClickDispatcher.Inst.EnableClick = false;

            //7. 播放结束，触发小华拿起B卡的动画。
            OnXiaoHuaBringUpB();
        }
    }

    void OnXiaoHuaBringUpB()
    {
        int start = 24;
        int end = 26;
        xhctrl.r_tuka2.GetComponentInChildren<MeshRenderer>().materials[1].CopyPropertiesFromMaterial(tukaB.GetComponentInChildren<MeshRenderer>().materials[1]);
        xiaohuaAnim.timePointEvent = (t) =>
        {
            if (t >= start && t <= end)
            {
                xiaohuaAnim.timePointEvent = null;
                xhctrl.r_tuka2.SetActive(true);
                tukaB.SetActive(false);

                int start0 = 44;
                int end0 = 46;
                xiaohuaAnim.timePointEvent = (tt) =>
                {
                    if (tt >= start0 && tt <= end0)
                    {
                        start0 = end0 + 1;
                        end0++;
                        xiaohuaAnim.timePointEvent = null;
                        xiaohuaAnim.OnPause();
                        //8. 播放结束，提醒操作者点击教师的手，点击后触发教师接卡的动画。
                        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFourth);
                        ClickDispatcher.Inst.EnableClick = true;
                        GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
                        HighLightCtrl.GetInstance().FlashOn(shou);
                    }
                };
            }

        };
        xiaohuaAnim.PlayForward("TY_XH_NKDK");

    }

    void OnClickTeacherHandFourth(ClickedObj cobj)
    {
        if (cobj.objname == "shou")
        {
            int start = 47;
            int end = 48;
            teacherAnim.timePointEvent = (t) =>
            {
                if (t >= start && t <= end)
                {
                    teacherAnim.timePointEvent = null;
                    xhctrl.r_tuka2.SetActive(false);
                    lsCtrl.ls_tuka2.GetComponentInChildren<MeshRenderer>().materials[1].CopyPropertiesFromMaterial(tukaB.GetComponentInChildren<MeshRenderer>().materials[1]);
                    lsCtrl.ls_tuka2.SetActive(true);
                    xiaohuaAnim.OnContinue();
                }
            };

            teacherAnim.Complete += () =>
            {
                lsCtrl.ls_tuka2.SetActive(false);
                //9. 播放结束，提醒操作者点击话筒，点击后话筒旁边显示“你要吃XXX呀”
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
                    string gift = goodB.GetComponent<PropsObject>().pData.name_cn;
                    dialog.SetDialogMessage("小华要" + gift + "呀");
                    RndNegReinforcementB.transform.localPosition = oldPos;
                    //10. 显示2秒，结束后，提醒操作者点击教师的手，点击后触发教师给小华的动画。
                    Invoke("ClickTeachersHandFinal", 2f);
                };
            };

            teacherAnim.PlayForward("TY_LS_JK");

            GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFourth);
            ClickDispatcher.Inst.EnableClick = false;
            GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
            HighLightCtrl.GetInstance().FlashOff(shou);



        }
    }

    private void ClickTeachersHandFinal()
    {
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
        tip.SetTipMessage("请点击老师的手给小华");
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

            GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
            HighLightCtrl.GetInstance().FlashOff(shou);



            int st = 37;
            int et = 39;
            teacherAnim.timePointEvent = (a) =>//老师递给物品
            {
                if (a > st && a < et)//挂载到老师手上强化物时间点
                {
                    //将当前强化物挂在老师手上
                    lsCtrl.SetJoint(RndNegReinforcementB);
                    RndNegReinforcementB.transform.localPosition = Vector3.zero;
                }

                if (a > 45 && a < 47)//小华接卡动画播放延迟一边挂载强化物
                {
                    xiaohuaAnim.Complete += () =>
                    {
                        //11. 播放结束，触发小华接过XXX。

                        //12. 播放结束，出现下一关和重做的按钮。
                        Debug.Log("DistinguishPictureCtrlB.OnClickTeacherHandFinal(): 11. 播放结束，出现下一关和重做的按钮。");
                        comUI = UIManager.Instance.GetUI<CommonUI>("CommonUI");
                        comUI.redoClickEvent += OnReDo;
                        comUI.nextClickEvent += OnNextDo;
                        comUI.ShowFinalUI();
                    };

                    int xhst = 24;
                    int xhet = 26;
                    xiaohuaAnim.timePointEvent = (aa) =>//小华接过物品
                    {
                        if (aa > xhst && aa < xhet)
                        {
                            xiaohuaAnim.timePointEvent = null;

                            xhctrl.SetJoint(RndNegReinforcementB);
                            RndNegReinforcementB.transform.localPosition = Vector3.zero;
                        }
                    };
                    xiaohuaAnim.PlayForward("TY_XH_JG");
                }
            };

            teacherAnim.PlayForward("TY_LS_DW");
        }
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
        if (comUI == null)
        {
            comUI = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        }
        comUI.redoClickEvent -= OnReDo;
        comUI.nextClickEvent -= OnNextDo;
        xiaohuaAnim.timePointEvent = null;
        teacherAnim.timePointEvent = null;
        xhctrl.DestroyGuadian();
        lsCtrl.DestroyGuadian();
    }

    private void OnReDo()
    {
        Redo();
    }

    void Redo()
    {
        if (evtRedo != null)
        {
            evtRedo();
        }

    }

    public void Dispose()
    {
        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFirst);
        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandSecond);
        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandThird);
        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFourth);
        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFinal);
        comUI.redoClickEvent -= OnReDo;
        comUI.nextClickEvent -= OnNextDo;
        comUI = null;
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
