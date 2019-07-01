using UnityEngine;
using System.Collections;

namespace future3d.unityLibs
{

    /// <summary>
    /// 第一人称相机视角控制
    /// </summary>
    public class MouseLook : MonoBehaviour
    {
        /// <summary>
        /// 旋转轴向
        /// </summary>
        public enum RotationAxes
        {

            RotationXAndY = 0,

            /// <summary>
            /// 绕X轴上下旋转
            /// </summary>
            RotationX = 1,

            /// <summary>
            /// 绕Y轴左右旋转
            /// </summary>
            RotationY = 2
        }

        public RotationAxes axes = RotationAxes.RotationXAndY;
        public float sensitivityHor = 9f; //左右旋转灵敏度
        public float sensitivityVert = 9f; //上下旋转灵敏度

        //上下旋转限制45度
        public float minmumVert = -45f;
        public float maxmumVert = 45f;

        private float _rotationX = 0;

        void Start() { }

        void Update()
        {
            if (axes == RotationAxes.RotationY)
            {
                transform.Rotate(0, Input.GetAxis("Horizontal") * sensitivityHor, 0);
            }
            else if (axes == RotationAxes.RotationX)
            {
                _rotationX = _rotationX - Input.GetAxis("Vertical") * sensitivityVert;
                _rotationX = Mathf.Clamp(_rotationX, minmumVert, maxmumVert);
                float rotationY = transform.localEulerAngles.y;
                transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
            }
            else
            {
                _rotationX -= Input.GetAxis("Vertical") * sensitivityVert;
                _rotationX = Mathf.Clamp(_rotationX, minmumVert, maxmumVert);
                float delta = Input.GetAxis("Horizontal") * sensitivityHor;
                float rotationY = transform.localEulerAngles.y + delta;
                transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
            }
        }

    }
}
