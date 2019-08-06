using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XhQHW : MonoBehaviour
{
    Transform par;
    private void Start()
    {
        par = transform.Find("Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R/ShoulderPart2_R/Elbow_R/Wrist_R/XH_R2");
        InitObjs();
    }

    public void ShowObj(string _name)
    {
        InitObjs();
        GameObject go = par.Find(_name).gameObject;
        go.SetActive(true);
        //return go;
    }

    public void InitObjs()
    {
        //Debug.LogError(par.name);
        for (int i = 0; i < par.childCount; i++)
        {
            par.GetChild(i).gameObject.SetActive(false);
        }
    }
}
