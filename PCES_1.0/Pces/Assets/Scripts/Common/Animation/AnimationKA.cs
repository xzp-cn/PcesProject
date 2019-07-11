using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationKA : MonoBehaviour
{
    Animation ani;
    private void Awake()
    {
        if (ani == null)
        {
            ani = GetComponent<Animation>();
            ani.playAutomatically = false;
            ani.wrapMode = WrapMode.Once;
        }
    }
    public void PlayForward(string _ani)
    {
        if (!ani.IsPlaying(_ani))
        {
            ani.Play(_ani);
        }
    }
}
