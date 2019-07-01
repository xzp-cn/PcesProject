using UnityEngine;
using System.Collections;

namespace future3d.unityLibs{
    /// <summary>
    /// UI状态条跟随
    /// </summary>
    public class UIStatusBar : MonoBehaviour{
        public float xOffset; //水平偏移量
        public float yOffset; //垂直偏移量
        public RectTransform recTransform; //状态条UI RectTransform
        public Transform target; //跟随目标物体
        public Camera worldCamera;  //相机


        private Vector3 currPos;
        void Update()
        {
            //世界坐标转为屏幕坐标
            Vector2 screenPos = worldCamera.WorldToScreenPoint(target.position);
            //通过UGUI内部换算将屏幕坐标转化为世界坐标（3D UI深度）
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(recTransform, screenPos, worldCamera, out currPos))
            {
                //返回的世界坐标重新设置UI的世界位置
                recTransform.position = currPos + new Vector3(xOffset,yOffset,0);
            }
        }
    }

}