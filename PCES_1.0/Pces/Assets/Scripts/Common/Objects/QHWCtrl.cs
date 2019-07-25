using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QHWCtrl : MonoBehaviour
{
    GameObject go;
    private void Awake()
    {
        transform.localPosition = Vector3.zero;
        InitObjs();
    }
    /// <summary>
    /// 显示通用强化物下的某一个物体
    /// </summary>
    /// <param name="_name"></param>
    public void ShowObj(string _name)
    {
        InitObjs();
        go = transform.Find(_name).gameObject;
        go.SetActive(true);
    }
    void InitObjs()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
