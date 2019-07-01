using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tick : MonoBehaviour
{
    bool Stop = false;//停止计时 外部点击方法内控制
    bool timeout = false;
    float timeTotal = 0;   //倒计时几秒
    float timeCount = 0;//计时
    float timeStep = 1;//等待时间间隔-协程
    System.Action<TimeStatus> mCallback;
    /// <summary>
    /// 停止计时
    /// </summary>
    /// <param name="isStop"></param>
    public void StopTiming(bool isStop)
    {
        Stop = isStop;
    }
    public void TimingStart(float time, System.Action<TimeStatus> callback)
    {
        ResetData();
        timeTotal = time;
        mCallback = callback;
        StartCoroutine(Timing());
    }
    IEnumerator Timing()
    {
        WaitForSeconds wf = new WaitForSeconds(timeStep);
        TimeStatus isTimeout = TimeStatus.none;
        while (true)
        {
            if (timeCount >= timeTotal)//超出计时
            {
                isTimeout = TimeStatus.outTime;
                Debug.Log("超时");
                break;
            }
            else
            {
                if (Stop)//在没超时间内点击物体
                {
                    isTimeout = TimeStatus.stop;
                    break;
                }
            }
            yield return wf;
            timeCount += timeStep;
        }
        if (mCallback != null)
        {
            mCallback(isTimeout);
        }
        ResetData();
    }
    /// <summary>
    /// 重置数据
    /// </summary>
    void ResetData()
    {
        timeout = false;
        Stop = false;
        timeCount = 0;
        StopAllCoroutines();
    }
}
public enum TimeStatus
{
    none,
    outTime,//超时
    stop//点中物体停止计时
}
