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

    private void Awake()
    {

    }

    void Start()
    {
        //1. 进入界面后1秒，触发小华拿A卡递卡的动画。
        OnXiaoHuaBringA();
    }

    /// <summary>
    /// 播放小华拿A卡递卡动画
    /// </summary>
    void OnXiaoHuaBringA()
    {
        //2. 播放结束，提醒操作者点击教师的手，点击后触发接图卡的动作。
        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFirst);
        ClickDispatcher.Inst.EnableClick = true;
    }

    /// <summary>
    /// 点击教师的手回调
    /// </summary>
    /// <param name="cobj"></param>
    private void OnClickTeacherHandFirst(ClickedObj cobj)
    {
        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFirst);
        ClickDispatcher.Inst.EnableClick = false;

        //3. 播放结束，提醒操作者点击话筒，显示“自己拿”。
        OnClickHuaTong();
    }

    void OnClickHuaTong()
    {
        //4. 播放结束，触发小华拿起B的动画。
        OnXiaoHuaBringB();
    }

    void OnXiaoHuaBringB()
    {
        //5. 播放结束，提醒操作者点击教师的手，点击后触发教师拿走B动画。
        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandSecond);
        ClickDispatcher.Inst.EnableClick = true;
    }


    private void OnClickTeacherHandSecond(ClickedObj cobj)
    {
        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandSecond);
        ClickDispatcher.Inst.EnableClick = false;
        //6. 播放结束，提醒操作者点击教师的手，点击后触发教师指指B的动画。

        //7. 播放结束，触发小华拿起B卡的动画。
        OnXiaoHuaBringUpB();
    }

    void OnXiaoHuaBringUpB()
    {
        //8. 播放结束，提醒操作者点击教师的手，点击后触发教师接卡的动画。
        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandThird);
        ClickDispatcher.Inst.EnableClick = true;
    }

    void OnClickTeacherHandThird(ClickedObj cobj)
    {
        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandThird);
        ClickDispatcher.Inst.EnableClick = false;

        //9. 播放结束，提醒操作者点击话筒，点击后话筒旁边显示“你要吃XXX呀”

        //10. 显示2秒，结束后，提醒操作者点击教师的手，点击后触发教师给小华的动画。

        //11. 播放结束，触发小华接过XXX。

        //12. 播放结束，出现下一关和重做的按钮。
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
