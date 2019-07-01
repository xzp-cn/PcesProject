using System.Collections;
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
    private GameObject tukaB;

    private void Awake()
    {
        emptyRoot = new GameObject("Root");
    }

    void Start()
    {
        InitGoodsState();

        //1. 进入界面后1秒，触发小华拿B卡递卡的动画。
        Invoke("OnXiaoHuaBring", 1f);
    }

    /// <summary>
    /// 初始化物品状态
    /// </summary>
    void InitGoodsState()
    {
        GameObject goodA = DistinguishPictureModel.GetInstance().GetRndReinforcement();
        RndReinforcementA = GameObject.Instantiate(goodA);
        RndReinforcementA.transform.SetParent(emptyRoot.transform, false);
        string tukaNameA = "tuka_" + goodA.name;
        tukaA = DistinguishPictureModel.GetInstance().GetTuKa(tukaNameA);
        tukaA.transform.SetParent(emptyRoot.transform, false);

        GameObject goodB= DistinguishPictureModel.GetInstance().GetRndNegReinforcement();
        RndNegReinforcementB = GameObject.Instantiate(goodB);
        RndNegReinforcementB.transform.SetParent(emptyRoot.transform, false);
        string tukaNameB = "tuka_" + goodB.name;
        tukaB = DistinguishPictureModel.GetInstance().GetTuKa(tukaNameB);
        tukaB.transform.SetParent(emptyRoot.transform, false);
    }

    /// <summary>
    /// 播放小华拿B卡递卡动画
    /// </summary>
    void OnXiaoHuaBring()
    {
        GameObject xiaohuaGo = PeopleManager.Instance.GetPeople("XH_BD");
        AnimationOper xiaohuaAnim = xiaohuaGo.GetAnimatorOper();
        GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;

        xiaohuaAnim.Complete += () =>
        {
            Debug.Log("DistinguishPictureCtrlA.OnXiaoHuaBring(): 2. 播放结束，提醒操作者点击教师的手，点击后触发接图卡的动作");
            HighLightCtrl.GetInstance().FlashOn(shou);
            shou.GetBoxCollider();

            //2. 播放结束，提醒操作者点击教师的手，点击后触发接图卡的动作。
            GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFirst);
            ClickDispatcher.Inst.EnableClick = true;
        };
        xiaohuaAnim.PlayForward("TY_XH_NKDK");
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
            GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFirst);
            ClickDispatcher.Inst.EnableClick = false;
            HighLightCtrl.GetInstance().FlashOff(cobj.go);

            //播放接图卡动画
            AnimationOper teacherAnim = PeopleManager.Instance.GetPeople("LS_BD").GetAnimatorOper();
            teacherAnim.Complete += () =>
            {
                OnReceiveTuKa();
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
    }

    private void OnClickTeacherHandSecond(ClickedObj cobj)
    {
        if (cobj.objname == "shou") {
            GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandSecond);
            ClickDispatcher.Inst.EnableClick = false;
            GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
            HighLightCtrl.GetInstance().FlashOff(shou);

            //播放教师给小华B的动画--(做在接图卡里)

            Debug.Log("DistinguishPictureCtrlA.OnClickTeacherHandSecond(): 4. 播放结束，触发小华用手推开B的动画。");
            //4. 播放结束，触发小华用手推开B的动画。
            GameObject xiaohuaGo = PeopleManager.Instance.GetPeople("XH_BD");
            AnimationOper xiaohuaAnim = xiaohuaGo.GetAnimatorOper();
            xiaohuaAnim.Complete += () =>
            {
                OnXiaoHuaPushB();
            };
            xiaohuaAnim.PlayForward("XH_C_1ST_JJ");

        }

    }

    void OnXiaoHuaPushB()
    {
        //5. 播放结束，提醒操作者点击教师的手，点击后触发教师指A卡的动画。
        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandThird);
        ClickDispatcher.Inst.EnableClick = true;
        GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
        HighLightCtrl.GetInstance().FlashOn(shou);
    }

    void OnClickTeacherHandThird(ClickedObj cobj)
    {
        if (cobj.objname == "shou")
        {
            GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
            HighLightCtrl.GetInstance().FlashOff(shou);
            GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandThird);
            ClickDispatcher.Inst.EnableClick = false;

            GameObject teacherGo = PeopleManager.Instance.GetPeople("LS_BD");
            AnimationOper teacherAnim = teacherGo.GetAnimatorOper();
            teacherAnim.Complete += () =>
            {
                //6. 播放结束，触发小华拿起A卡、递卡的动画。
                Debug.Log("DistinguishPictureCtrlA.OnClickTeacherHandThird(): 6. 播放结束，触发小华拿起A卡、递卡的动画。");
                OnXiaoHuaBringAToTeacher();
            };
            teacherAnim.PlayForward("LS_C_1ST_ZZ");

        }
    }

    void OnXiaoHuaBringAToTeacher()
    {
        //7. 播放结束，提醒操作者点击教师的手，点击后触发教师接卡的动画。
        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFourth);
        ClickDispatcher.Inst.EnableClick = true;
        GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
        HighLightCtrl.GetInstance().FlashOn(shou);
    }

    void OnClickTeacherHandFourth(ClickedObj cobj)
    {
        if (cobj.objname == "shou")
        {
            GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFourth);
            ClickDispatcher.Inst.EnableClick = false;
            GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
            HighLightCtrl.GetInstance().FlashOff(shou);

            //8. 播放结束，提醒操作者点击话筒，点击后话筒旁边显示“你要吃XXX呀”
            SwapUI swapui = UIManager.Instance.GetUI<SwapUI>("SwapUI");
            swapui.SetButtonVisiable(SwapUI.BtnName.microButton, true);
            swapui.SetButtonVisiable(SwapUI.BtnName.chooseButton, false);
            swapui.speakEvent = () =>
            {
                swapui.speakEvent = null;
                swapui.SetButtonVisiable(SwapUI.BtnName.microButton, false);
                TipUI tipui = UIManager.Instance.GetUI<TipUI>("TipUI");
                string gift = "";
                tipui.SetTipMessage("小华要吃" + gift);

                //9. 显示2秒，结束后，提醒操作者点击教师的手，点击后触发教师给小华的动画。
                Invoke("ClickTeachersHandFinal", 2f);
            };



            //10. 播放结束，触发小华接过XXX。


        }
    }

    private void ClickTeachersHandFinal()
    {
        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFourth);
        GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
        HighLightCtrl.GetInstance().FlashOn(shou);

        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFinal);
        ClickDispatcher.Inst.EnableClick = true;

    }

    private void OnClickTeacherHandFinal(ClickedObj cobj)
    {
        if (cobj.objname == "shou")
        {
            GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFinal);
            ClickDispatcher.Inst.EnableClick = false;

            GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
            HighLightCtrl.GetInstance().FlashOff(shou);


            //11. 播放结束，出现下一关和重做的按钮。
            Debug.Log("DistinguishPictureCtrlA.OnClickTeacherHandFourth(): 11. 播放结束，出现下一关和重做的按钮。");
            comUI = UIManager.Instance.GetUI<CommonUI>("CommonUI");
            comUI.redoClickEvent += OnReDo;
            comUI.nextClickEvent += OnNextDo;
            comUI.ShowFinalUI();
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
        if(comUI == null)
        {
            comUI = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        }
        comUI.redoClickEvent -= OnReDo;
        comUI.nextClickEvent -= OnNextDo;
    }

    void Redo()
    {
        if (evtRedo != null)
        {
            evtRedo();
        }
        if(comUI == null)
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
        comUI.redoClickEvent -= OnReDo;
        comUI.nextClickEvent -= OnNextDo;
        comUI = null;
        evtFinished = null;
        evtRedo = null;
        if(emptyRoot != null)
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
