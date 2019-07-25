using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TukaCtrl : MonoBehaviour
{
    GameObject go;
    private void Awake()
    {
        InitObjs();
    }
    /// <summary>
    /// 显示通用强化物下的某一个物体
    /// </summary>
    /// <param name="_name"></param>
    public GameObject ShowObj(string _name)
    {
        InitObjs();
        go = transform.Find(_name).gameObject;
        go.SetActive(true);
        return go;
    }

    public GameObject GetObj(string _name)
    {
        go = transform.Find(_name).gameObject;
        return go;
    }

    public void SetPos()
    {
        go.transform.localPosition = Vector3.zero;
    }
    void InitObjs()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
