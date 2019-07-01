using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SingleTemplate<T>
{
    private static T instance;
    public static T GetInstance()
    {
        if (instance == null)
        {
            instance = Activator.CreateInstance<T>();
        }
        return instance;
    }
}

public class MainUIModel : SingleTemplate<MainUIModel>
{
    public MainUIModel()
    {
    }

    public enum CommandFlag
    {
        /// <summary>
        /// 基本概念Click
        /// </summary>
        ClickJiBenGaiNian,

        /// <summary>
        /// 大洲与大洋Click
        /// </summary>
        ClickDaZhouAndDaYang,

        /// <summary>
        /// 大陆Click
        /// </summary>
        ClickDaLu,

        /// <summary>
        /// 岛屿Click
        /// </summary>
        ClickDaoyu,

        /// <summary>
        /// 半岛Click
        /// </summary>
        ClickBanDao,

        /// <summary>
        /// 海峡Click
        /// </summary>
        ClickHaiXia,

        /// <summary>
        /// 大洲Click
        /// </summary>
        ClickDaZhou,

        /// <summary>
        /// 大洋Click
        /// </summary>
        ClickDaYang,

        /// <summary>
        /// 经纬网Click
        /// </summary>
        ClickJinWeiWang,

        /// <summary>
        /// 大洲分界线Click
        /// </summary>
        ClickDaZhouFenJieXian,
    }
}
