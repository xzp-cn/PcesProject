using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 第三关 -- 区辨多张图片
/// </summary>
public class DistinguishPictureCtrlC : MonoBehaviour {

    public event System.Action evtFinished;
    public event System.Action evtRedo;
    private PromptHelper prp;
    private CommonUI comUI;

    private void Awake()
    {

    }

    void Start () {
        //1. 进入界面后1秒，触发小华翻开沟通本并拿出图卡，递给老师的动画。
        prp = UIManager.Instance.GetUI<PromptHelper>("Prompt");
        prp.SetText("1. 进入界面后1秒，触发小华拿A卡递卡的动画。");
        Invoke("OnXiaoHuaPassGouTongBenToTeacher", 1f);

    }

    void OnXiaoHuaPassGouTongBenToTeacher()
    {
        //2. 播放结束，提醒操作者点击教师的手，点击后触发接图卡的动作。播放结束，提醒操作者点击话筒，点击后话筒旁边显示“你要XXX呀”
        GameObject xiaohuaGo = PeopleManager.Instance.GetPeople("XH_BD");
        AnimationOper xiaohuaAnim = xiaohuaGo.GetAnimatorOper();

        xiaohuaAnim.Complete += () =>
        {
            GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
            prp.SetText("2. 播放结束，提醒操作者点击教师的手，点击后触发接图卡的动作");
            HighLightCtrl.GetInstance().FlashOn(shou);
            shou.GetBoxCollider().size = new Vector3(1, 0.2f, 0.5f);
            GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFirst);
            ClickDispatcher.Inst.EnableClick = true;

            ChooseDo.Instance.DoWhat(5, RedoClickTeachersHandFirst, null);
        };
        xiaohuaAnim.PlayForward("TY_XH_NKDK");
    }

    private void ClickTeachersPromptFirst()
    {
        ClickDispatcher.Inst.EnableClick = true;
        ChooseDo.Instance.DoWhat(5, RedoClickTeachersHandFirst, null);
    }

    private void RedoClickTeachersHandFirst()
    {
        ClickDispatcher.Inst.EnableClick = false;
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("请点击老师的手");
        CancelInvoke("ClickTeachersPromptFirst");
        Invoke("ClickTeachersPromptFirst", 2);
    }

    private void OnClickTeacherHandFirst(ClickedObj cobj)
    {
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
                prp.SetText("3. 播放结束，提醒操作者点击话筒，显示“你要XXX呀”");
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
            string gift = "XXX";
            dialog.SetDialogMessage("你要"+gift);

            //4. 播放结束，触发小华拿起B的动画。
            prp.SetText("4. 播放结束，触发小华拿起B的动画。");
            GameObject xiaohuaGo = PeopleManager.Instance.GetPeople("XH_BD");
            AnimationOper xiaohuaAnim = xiaohuaGo.GetAnimatorOper();
            xiaohuaAnim.Complete += () =>
            {
                //3. 显示2秒，结束后，提醒操作者点击教师的手，点击后触发教师给小华的动画。
                OnTeacherTakeXiaoHuaB();
            };
            xiaohuaAnim.PlayForward("TY_XH_NKDK");
        };


    }

    private void OnTeacherTakeXiaoHuaB()
    {
        AnimationOper teacherAnim = PeopleManager.Instance.GetPeople("LS_BD").GetAnimatorOper();
        teacherAnim.Complete += () =>
        {
            //4. 播放结束，触发小华接过XXX。
            prp.SetText("4. 播放结束，触发小华接过XXX。");
            OnXiaoHuaAccept();
        };
        teacherAnim.PlayForward("TY_LS_JK");
    }

    private void OnXiaoHuaAccept()
    {
        GameObject xiaohuaGo = PeopleManager.Instance.GetPeople("XH_BD");
        AnimationOper xiaohuaAnim = xiaohuaGo.GetAnimatorOper();
        xiaohuaAnim.Complete += () =>
        {
            //5. 播放结束，出现下一关和重做的按钮。
            prp.SetText("5. 播放结束，出现测试题界面。");
            comUI = UIManager.Instance.GetUI<CommonUI>("CommonUI");
            comUI.redoClickEvent += OnReDo;
            comUI.nextClickEvent += OnNextDo;
            comUI.ShowFinalUI();
        };
        xiaohuaAnim.PlayForward("TY_XH_NKDK");
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
        prp = null;
        comUI = null;
        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFirst);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {

    }
}
