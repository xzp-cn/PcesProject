using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace future3d.unityLibs
{
    /// <summary>
    /// 渐隐渐现
    /// </summary>
    public class UIFade : MonoBehaviour
    {
        public float durations = 1.0f; //渐变过程时间
        private float alpha = 1.0f;  //目标透明度
        private Image image;  //UI Image

        public bool isFade; //控制渐隐/渐现

        void Start()
        {
            image = GetComponent<Image>();
        }

        void Update()
        {
            if (image != null)
            {
                if (isFade)
                {
                    UIHide();
                }
                else
                {
                    UIShow();
                }

            }
        }

        /// <summary>
        /// 显示
        /// </summary>
        public void UIShow()
        {
            alpha = 1.0f;
            image.CrossFadeAlpha(alpha, durations, true); //第3个参数设置表示不受Time.TimeScale影响
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public void UIHide()
        {
            alpha = 0.0f;
            image.CrossFadeAlpha(alpha, durations, true); //第3个参数设置表示不受Time.TimeScale影响
        }
    }
}
