using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 提示重做工具类
/// </summary>
public class ChooseDo : SingleTon<ChooseDo>
{
    private System.Action redo, nextdo;
    public bool bNextDo;
    float time = 0;
    float currTime = 0;
    /// <summary>
    /// 注册重做和下一步
    /// </summary>
    /// <param name="_time"></param>
    /// <param name="_redo"></param>
    /// <param name="_nextdo"></param>
    public void DoWhat(float _time, System.Action _redo, System.Action _nextdo)
    {
        StopCoroutine("LoopCalll");
        time = _time;
        bNextDo = false;
        currTime = 0;
        redo = _redo;
        nextdo = _nextdo;
        StartCoroutine("LoopCall");
    }
    IEnumerator LoopCall()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            currTime += Time.deltaTime;
            if (currTime < time)
            {
                if (bNextDo)
                {
                    if (nextdo != null)
                    {
                        nextdo();
                    }
                    break;
                }
            }
            else
            {
                if (redo != null)
                {
                    redo();
                }
                break;
            }

        }
    }
    /// <summary>
    /// 点击调用
    /// </summary>
    public void Clicked()
    {
        bNextDo = true;
    }
    void DelayCall()
    {
        if (bNextDo)
        {
            nextdo();
        }
    }
}
