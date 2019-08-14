using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HighLightCtrl : SingleTemplate<HighLightCtrl>
{
    List<HighlightableObject> hoList = new List<HighlightableObject>();
    public Camera[] cameras;
    //Color32 c1 = new Color(255, 132, 0, 0), c2 = new Color(206, 215, 21, 255);
    Color c1 = new Color(1, 1, 0, 0), c2 = new Color(1, 1, 1, 1);
    //float freq = 2f;
    /// <summary>
    /// 初始化
    /// </summary>
    void Init()
    {
        if (cameras == null)
        {
            cameras = UnityEngine.Object.FindObjectsOfType<Camera>();
        }
        if (cameras != null)
        {
            for (int i = 0; i < cameras.Length; i++)
            {
                HighlightingEffect hef = cameras[i].GetComponent<HighlightingEffect>();
                if (hef == null)
                {
                    hef = cameras[i].gameObject.AddComponent<HighlightingEffect>();
                    //hef._downsampleFactor = 1;
                    //hef.iterations = 2;
                }
            }
        }
        else
        {
            Debug.LogError("高亮插件需要Camera!");
        }
    }
    /// <summary>
    /// 初始高亮物体
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    HighlightableObject InitHighlightObj(GameObject obj)
    {
        HighlightableObject ho = obj.GetComponent<HighlightableObject>();
        if (ho == null)
        {
            ho = obj.AddComponent<HighlightableObject>();
        }
        Init();
        if (!hoList.Contains(ho))
        {
            hoList.Add(ho);
        }
        return ho;
    }
    /// <summary>
    /// 让物体常亮
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="c"></param>
    public void ConstantOn(GameObject obj, Color32 c)
    {
        InitHighlightObj(obj).ConstantOn(c);
    }
    /// <summary>
    /// 常亮关闭
    /// </summary>
    /// <param name="go"></param>
    public void ConstantOff(GameObject go)
    {
        HighlightableObject ho = go.GetComponent<HighlightableObject>();
        if (ho != null)
        {
            ho.ConstantOff();
        }
    }
    /// <summary>
    /// 开启闪烁
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="c1"></param>
    /// <param name="c2"></param>
    /// <param name="freq"></param>
    public void FlashOn(GameObject obj, Color32 c1, Color32 c2, float freq)
    {
        InitHighlightObj(obj).FlashingOn(c1, c2, freq);
    }
    public void FlashOn(GameObject obj)
    {
        //Debug.LogError(c1 + "   " + c2);
        InitHighlightObj(obj).FlashingOn(c1, c2);
    }
    /// <summary>
    /// 关闭闪烁
    /// </summary>
    /// <param name="go"></param>
    public void FlashOff(GameObject go)
    {
        HighlightableObject ho = go.GetComponent<HighlightableObject>();
        if (ho != null)
        {
            //Debug.Log("flash off  " + ho.name);
            ho.FlashingOff();
        }
    }
    /// <summary>
    /// 关闭所有高亮显示
    /// </summary>
    public void OffAllObjs()
    {
        for (int i = 0; i < hoList.Count; i++)
        {
            hoList[i].Off();
        }
    }
}
