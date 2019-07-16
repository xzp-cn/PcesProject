using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegacyAnimationOper : MonoBehaviour
{
    public Animation anim;
    public string animName;
    void Awake()
    {
        anim = GetComponent<Animation>();
        IsStart = false;
        IsComplete = false;
        anim.playAutomatically = false;
    }
    public WrapMode SetWrapMode
    {
        get
        {
            return anim.wrapMode;
        }
        set
        {
            anim.wrapMode = value;
        }
    }
    /// <summary>
    /// 当画是否开始
    /// </summary>
    public bool IsStart
    {
        get;
        set;
    }

    /// <summary>
    /// 动画是否完成
    /// </summary>
    public bool IsComplete
    {
        get;
        set;
    }

    public event System.Action Complete;
    public System.Action<float> timePointEvent; //时间点事件,参数为当前时间
    float timeLength;
    float currLength;

    public float transitionTime = 0f;//过渡时间-
    /// <summary>
    /// 从头开始播放动画剪辑
    /// </summary>
    /// <param name="clipName"></param> 
    public void PlayForward(string clipName)
    {
        if (anim)
        {
            animName = clipName;
            if (!anim.IsPlaying(clipName))
            {
                anim.Play(clipName);
            }
            //anim.Play(clipName, 0, 0);
            timeLength = anim[clipName].length;
            IsStart = true;
        }
        else
        {
            Debug.Log("没有找到动画  " + clipName);
        }

    }
    public void Update()
    {
        if (IsStart)
        {
            if (anim.IsPlaying(animName))
            {
                timeLength = anim[animName].length;
                if (currLength <= timeLength)
                {
                    if (timePointEvent != null)
                    {
                        timePointEvent(currLength);
                    }
                    currLength += Time.deltaTime;
                }
                else
                {
                    IsStart = false;
                    IsComplete = true;
                    currLength = 0;
                    if (Complete != null)
                    {
                        Complete();
                        Complete = null;
                        timePointEvent = null;
                    }
                }
            }
        }
    }
    void OnDestroy()
    {
        IsStart = false;
        IsComplete = false;
        Complete = null;
        timePointEvent = null;
    }
}
