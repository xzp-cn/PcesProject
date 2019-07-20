using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFlah : MonoBehaviour
{
    Outline outline;
    Color c = new Color(0, 50, 255, 255);
    bool isFlash = false;
    private void Awake()
    {
        outline = GetComponent<Outline>();
        if (outline == null)
        {
            outline = gameObject.AddComponent<Outline>();
        }
    }
    public void InitOutline(Vector2 effectDistanse)
    {
        outline.effectDistance = effectDistanse;
        outline.effectColor = c;
    }
    public void StartFlash()
    {
        isFlash = true;
        outline.enabled = true;
        InitOutline(new Vector2(4, -4));
        StartCoroutine(Flash());
    }
    IEnumerator Flash()
    {
        WaitForSeconds wf = new WaitForSeconds(0.12f);
        while (isFlash)
        {
            outline.enabled = !outline.enabled;
            yield return wf;
        }
    }
    public void StopFlash()
    {
        isFlash = false;
        outline.enabled = false;
        StopAllCoroutines();
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
