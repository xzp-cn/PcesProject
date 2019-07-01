using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HighLightCtrl : SingleTemplate<HighLightCtrl>
{
    List<HighlightableObject> hoList = new List<HighlightableObject>();
    Camera camera;
    Color32 c1 = new Color(190, 171, 71, 255), c2 = new Color32(210, 27, 255, 255);
    float freq = 2f;
    /// <summary>
    /// 初始化
    /// </summary>
    void Init()
    {
        if (camera == null)
        {
            camera = UnityEngine.Object.FindObjectOfType<Camera>();
            if (camera != null)
            {
                HighlightingEffect hef = camera.GetComponent<HighlightingEffect>();
                if (hef == null)
                {
                    hef = camera.gameObject.AddComponent<HighlightingEffect>();
                }
            }
            else
            {
                Debug.LogError("高亮插件需要Camera!");
            }
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
        hoList.Add(ho);
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
            Debug.Log("flash off  " + ho.name);
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
