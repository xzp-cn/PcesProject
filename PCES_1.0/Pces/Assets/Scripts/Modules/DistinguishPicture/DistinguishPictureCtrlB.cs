using System.Collections;
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
    PromptHelper prp;

    private void Awake()
    {

    }

    void Start()
    {
        AnimationOper teacherAnim = PeopleManager.Instance.GetPeople("LS_BD").GetAnimatorOper();
        teacherAnim.PlayForward("idle");

        //1. 进入界面后1秒，触发小华拿A卡递卡的动画。
        prp = UIManager.Instance.GetUI<PromptHelper>("Prompt");
        prp.SetText("1. 进入界面后1秒，触发小华拿A卡递卡的动画。");
        Invoke("OnXiaoHuaBringA", 1f);
    }

    /// <summary>
    /// 播放小华拿A卡递卡动画
    /// </summary>
    void OnXiaoHuaBringA()
    {
        GameObject xiaohuaGo = PeopleManager.Instance.GetPeople("XH_BD");
        AnimationOper xiaohuaAnim = xiaohuaGo.GetAnimatorOper();


        xiaohuaAnim.Complete += () =>
        {
            GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
            Debug.Log("DistinguishPictureCtrlB.OnXiaoHuaBringA(): 2. 播放结束，提醒操作者点击教师的手，点击后触发接图卡的动作");
            prp.SetText("2. 播放结束，提醒操作者点击教师的手，点击后触发接图卡的动作");
            HighLightCtrl.GetInstance().FlashOn(shou);
            shou.GetBoxCollider().size = new Vector3(1, 0.2f, 0.5f);
            //2. 播放结束，提醒操作者点击教师的手，点击后触发接图卡的动作。
            GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFirst);
            ClickDispatcher.Inst.EnableClick = true;

            ChooseDo.Instance.DoWhat(5, RedoClickTeachersHandFirst, null);
        };
        xiaohuaAnim.PlayForward("TY_XH_NKDK");
    }

    private void ClickTeachersPromptFirst()
    {
        GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
        Debug.Log("DistinguishPictureCtrlA.OnXiaoHuaBring(): 2. 播放结束，提醒操作者点击教师的手，点击后触发接图卡的动作");
        HighLightCtrl.GetInstance().FlashOn(shou);

        ClickDispatcher.Inst.EnableClick = true;
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

            //播放接图卡动画
            AnimationOper teacherAnim = PeopleManager.Instance.GetPeople("LS_BD").GetAnimatorOper();
            teacherAnim.Complete += () =>
            {
                //3. 播放结束，提醒操作者点击话筒，显示“自己拿”。
                prp.SetText("3. 播放结束，提醒操作者点击话筒，显示“自己拿”");
                OnClickHuaTong();
            };
            teacherAnim.PlayForward("TY_LS_JK");
        }


    }

    void OnClickHuaTong()
    {
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
            prp.SetText("4. 播放结束，触发小华拿起B的动画。");
            GameObject xiaohuaGo = PeopleManager.Instance.GetPeople("XH_BD");
            AnimationOper xiaohuaAnim = xiaohuaGo.GetAnimatorOper();
            xiaohuaAnim.Complete += () =>
            {
                OnXiaoHuaBringB();
            };
            xiaohuaAnim.PlayForward("TY_XH_NKDK");
        };


    }

    void OnXiaoHuaBringB()
    {
        GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
        HighLightCtrl.GetInstance().FlashOn(shou);
        prp.SetText("5. 播放结束，提醒操作者点击教师的手，点击后触发教师拿走B动画。");
        //5. 播放结束，提醒操作者点击教师的手，点击后触发教师拿走B动画。
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
        GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
        HighLightCtrl.GetInstance().FlashOn(shou);

        ClickDispatcher.Inst.EnableClick = true;
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

            GameObject teacherGo = PeopleManager.Instance.GetPeople("LS_BD");
            AnimationOper teacherAnim = teacherGo.GetAnimatorOper();
            teacherAnim.Complete += () => {
                //6. 播放结束，提醒操作者点击教师的手，点击后触发教师指指B的动画。
                OnClickTeacherShouThird();
            };
            teacherAnim.PlayForward("TY_LS_JK");


        }
    }

    void OnClickTeacherShouThird()
    {
        GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
        HighLightCtrl.GetInstance().FlashOn(shou);
        prp.SetText("6. 播放结束，提醒操作者点击教师的手，点击后触发教师指指B的动画。");
        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandThird);
        ClickDispatcher.Inst.EnableClick = true;
        ChooseDo.Instance.DoWhat(5, RedoClickTeachersHandThird, null);
    }

    private void ClickTeachersPromptThird()
    {
        GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
        HighLightCtrl.GetInstance().FlashOn(shou);

        ClickDispatcher.Inst.EnableClick = true;
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
            CancelInvoke("ClickTeachersPromptThird");
            GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandThird);
            ClickDispatcher.Inst.EnableClick = false;

            GameObject teacherGo = PeopleManager.Instance.GetPeople("LS_BD");
            AnimationOper teacherAnim = teacherGo.GetAnimatorOper();
            teacherAnim.Complete += () =>
            {
                prp.SetText("7. 播放结束，触发小华拿起B卡的动画。");
                //7. 播放结束，触发小华拿起B卡的动画。
                OnXiaoHuaBringUpB();
            };
            teacherAnim.PlayForward("LS_C_1ST_ZZ");


        }
    }

    void OnXiaoHuaBringUpB()
    {
        GameObject xiaohuaGo = PeopleManager.Instance.GetPeople("XH_BD");
        AnimationOper xiaohuaAnim = xiaohuaGo.GetAnimatorOper();
        xiaohuaAnim.Complete += () =>
        {

            //8. 播放结束，提醒操作者点击教师的手，点击后触发教师接卡的动画。
            prp.SetText("8. 播放结束，提醒操作者点击教师的手，点击后触发教师接卡的动画。");
            GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFourth);
            ClickDispatcher.Inst.EnableClick = true;
            GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
            HighLightCtrl.GetInstance().FlashOn(shou);
        };
        xiaohuaAnim.PlayForward("TY_XH_NKDK");

    }

    void OnClickTeacherHandFourth(ClickedObj cobj)
    {
        if (cobj.objname == "shou")
        {
            GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFourth);
            ClickDispatcher.Inst.EnableClick = false;
            GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
            HighLightCtrl.GetInstance().FlashOff(shou);

            //9. 播放结束，提醒操作者点击话筒，点击后话筒旁边显示“你要吃XXX呀”
            prp.SetText("9. 播放结束，提醒操作者点击话筒，点击后话筒旁边显示“你要吃XXX呀”");
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
                string gift = "";
                dialog.SetDialogMessage("小华要吃" + gift);

                //10. 显示2秒，结束后，提醒操作者点击教师的手，点击后触发教师给小华的动画。
                prp.SetText("10.显示2秒，结束后，提醒操作者点击教师的手，点击后触发教师给小华的动画。");
                Invoke("ClickTeachersHandFinal", 2f);
            };

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
        GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
        HighLightCtrl.GetInstance().FlashOn(shou);
        ClickDispatcher.Inst.EnableClick = true;
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


            //11. 播放结束，触发小华接过XXX。
            prp.SetText("11. 播放结束，触发小华接过XXX。");
            GameObject xiaohuaGo = PeopleManager.Instance.GetPeople("XH_BD");
            AnimationOper xiaohuaAnim = xiaohuaGo.GetAnimatorOper();
            xiaohuaAnim.Complete += () =>
            {
                //12. 播放结束，出现下一关和重做的按钮。
                prp.SetText("12. 播放结束，出现下一关和重做的按钮。");
                comUI = UIManager.Instance.GetUI<CommonUI>("CommonUI");
                comUI.redoClickEvent += OnReDo;
                comUI.nextClickEvent += OnNextDo;
                comUI.ShowFinalUI();
            };
            xiaohuaAnim.PlayForward("TY_XH_JG");


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
        Destroy(gameObject);
    }

    private void OnDestroy()
    {

    }

}
