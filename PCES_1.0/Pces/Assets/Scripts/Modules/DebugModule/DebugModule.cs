using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugModule : SingleTemplate<DebugModule> {

    public DebugModule(){

    }

    public enum GEvt
    {
        Show,
        Hide,
    }

    /// <summary>
    /// Choose QiangHuaWu Index
    /// </summary>
    public int ChoiceIndex;

    /// <summary>
    /// Choose NegQiangHuaWu Index
    /// </summary>
    public int ChoiceNegIndex;

    /// <summary>
    /// Choose Mid Index
    /// </summary>
    public int ChoiceMidIndex;

    public List<int> SelRnd { get; set; }
    public List<int> SelNegRnd { get; set; }
}
