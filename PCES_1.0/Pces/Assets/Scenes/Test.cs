using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    HighlightableObject ho;
    // Use this for initialization
    void Start()
    {
        HighLightCtrl.GetInstance().FlashOn(gameObject);
        Invoke("MUpdate", 2);
    }

    // Update is called once per frame
    void MUpdate()
    {
        HighLightCtrl.GetInstance().FlashOff(gameObject);
    }
}
