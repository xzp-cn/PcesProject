using UnityEngine;
using System.Collections;

/// <summary>
/// Legacy 动画播放控制
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
    float timeLength;
    float currLength;

    /// <summary>
    /// 从头开始播放动画剪辑
    /// </summary>
    /// <param name="clipName"></param>
    public void PlayForward(string clipName)
    {
        if (anim)
        {
            animName = clipName;
            anim.CrossFade(clipName, 0.2f, 0, 0);
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
    }
}

