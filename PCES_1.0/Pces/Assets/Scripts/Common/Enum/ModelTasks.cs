using UnityEngine;
using System.Collections;
using System.ComponentModel;

public enum ModelTasks
{

    /// <summary>
    /// 首页
    /// </summary>
    [Description("首页")]
    HomePage,

    /// <summary>
    /// 以物换物
    /// </summary>
    [Description("以物换物")]
    Swap,

    /// <summary>
    /// 增强自发性的沟通
    /// </summary>
    [Description("增强自发性的沟通")]
    EnhanceCommunity,

    /// <summary>
    /// 分辨图片
    /// </summary>
    [Description("分辨图片")]
    DistinguishPicture,

    /// <summary>
    /// 接受性问句
    /// </summary>
    [Description("接受性问句")]
    AcceptQuestion,

    /// <summary>
    /// 主动发表意见
    /// </summary>
    [Description("主动发表意见")]
    SentenceExpression,
}
