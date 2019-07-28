using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextSpaceCtrl : MonoBehaviour {

    public float fontSpace = 6;
	// Use this for initialization
	void Start () {
        Text[] txt=transform.GetComponentsInChildren<Text>(true);
        for (int i = 0; i < txt.Length; i++)
        {
            TextSpacing ts=txt[i].GetComponent<TextSpacing>();
            if (ts==null)
            {
               ts=txt[i].gameObject.AddComponent<TextSpacing>();
            }
            ts._textSpacing = fontSpace;
        }
	}		
}
