using UnityEngine;
using System.Collections;

/// <summary>
/// Animator 动画播放控制
/// </summary>
public class AnimationOper : MonoBehaviour
{
    public Animator anim;
    public string animName;
    void Awake()
    {
        anim = GetComponent<Animator>();
        IsStart = false;
        IsComplete = false;
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

    public float transitionTime = 0.2f;//过渡时间
    /// <summary>
    /// 从头开始播放动画剪辑
    /// </summary>
    /// <param name="clipName"></param>
    public void PlayForward(string clipName)
    {
        if (anim)
        {
            animName = clipName;
            //Debug.Log(animName);
            anim.CrossFade(clipName, transitionTime, 0, 0);
            //anim.Play(clipName, 0, 0);
            timeLength = anim.GetCurrentAnimatorStateInfo(0).length;
            IsStart = true;
        }
        else
        {
            Debug.Log("没有找到动画");
        }

    }
    public void Update()
    {
        if (IsStart)
        {
            var asif = anim.GetCurrentAnimatorStateInfo(0);
            if (asif.IsName("Base." + animName))
            {
                timeLength = anim.GetCurrentAnimatorStateInfo(0).length;
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

    /// <summary>
    /// 暂停
    /// </summary>
    public void OnPause()
    {
        IsStart = false;
        if(anim != null)
        {
            anim.speed = 0;
        }
    }

    /// <summary>
    /// 继续
    /// </summary>
    public void OnContinue()
    {
        IsStart = true;
        if (anim != null)
        {
            anim.speed = 1;
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

