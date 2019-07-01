using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace future3d.unityLibs{

    /// <summary>
    /// 进度条
    /// </summary>
    public class UIProgressBar : MonoBehaviour
    {
        private Image progressBar;

        /// <summary>
        /// 初始化进度条
        /// </summary>
        public void Awake()
        {
            progressBar = transform.GetComponent<Image>();

            //设置Image水平填充
            progressBar.type = Image.Type.Filled;
            progressBar.fillMethod = Image.FillMethod.Horizontal;
            progressBar.fillOrigin = (int)Image.OriginHorizontal.Left;

            //设置锚点左对齐
            GetComponent<RectTransform>().anchorMin = new Vector2(0, 0.5f);
            GetComponent<RectTransform>().anchorMax = new Vector2(0, 0.5f);
        }

        /// <summary>
        /// 设置进度条数值
        /// </summary>
        /// <param name="value"></param>
        public void SetProgressValue(float value)
        {
            progressBar.fillAmount = value;
        }
    }
}
