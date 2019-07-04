using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 第三关 -- 区辨多张图片
/// </summary>
public class DistinguishPictureCtrlC : MonoBehaviour
{
    public event System.Action evtRedo, evtFinished;
    private PromptHelper prp;
    private CommonUI comUI;

    private void Awake()
    {

    }

    void Start()
    {
        //1. 进入界面后1秒，触发小华翻开沟通本并拿出图卡，递给老师的动画。
        prp = UIManager.Instance.GetUI<PromptHelper>("Prompt");
        prp.SetText("1. 进入界面后1秒，触发小华拿A卡递卡的动画。");
        Invoke("OnXiaoHuaPassGouTongBenToTeacher", 1f);

    }

    void OnXiaoHuaPassGouTongBenToTeacher()
    {
        //2. 播放结束，提醒操作者点击教师的手，点击后触发接图卡的动作。播放结束，提醒操作者点击话筒，点击后话筒旁边显示“你要XXX呀”



        //3. 显示2秒，结束后，提醒操作者点击教师的手，点击后触发教师给小华的动画。


        //4. 播放结束，触发小华接过XXX。


        //5. 播放结束，出现下一关和重做的按钮。
        prp.SetText("5. 播放结束，出现下一关和重做的按钮。");
        comUI = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        comUI.redoClickEvent += OnReDo;
        comUI.nextClickEvent += OnNextDo;
        comUI.ShowFinalUI();
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
