using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ContentFit : UnityEngine.EventSystems.UIBehaviour
{
    public RectTransform content;
    public RectTransform fit;
    public float leftMargin;
    public float rightMargin;
    protected override void Awake()
    {
        content = GetComponentInChildren<RectTransform>();
        fit = transform.parent.GetComponent<RectTransform>();
    }
    protected override void OnRectTransformDimensionsChange()
    {
        //base.OnBeforeTransformParentChanged();
        //UnityEngine.Debug.Log("TipUI::OnRectTransformDimensionsChange():" + content.sizeDelta.x);
        float x = leftMargin + content.sizeDelta.x + rightMargin;
        float y = content.sizeDelta.y;
        fit.sizeDelta = new Vector2(x, y);
    }
}
