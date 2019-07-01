using UnityEngine;
using System.Collections;

namespace future3d.unityLibs
{
    /// <summary>
    /// 实现对物体透明
    /// </summary>
    public class TranspaneryCtrl : MonoBehaviour
    {

        public Renderer obj; //要透明的物体
        public Material oldMat; //要透明的物体旧材质
        private Material transparentMat; //透明材质
        public float alpha; //调整透明度
        void Awake()
        {
            obj = GetComponent<Renderer>();
            transparentMat = new Material(Shader.Find("Legacy Shaders/Transparent/Diffuse"));

        }

        /// <summary>
        /// 应用透明
        /// </summary>
        public void ApplyTransparent(float alpha = 0.5f)
        {
            if (obj != null)
            {
                if (oldMat == null)
                {
                    oldMat = obj.material;
                }

                transparentMat.SetColor("_Color", new Color(1, 1, 1, alpha));
                obj.material = transparentMat;
            }
        }

        /// <summary>
        /// 取消透明
        /// </summary>
        public void CancelTransparent()
        {
            if (obj != null)
            {
                if (oldMat == null)
                {
                    oldMat = obj.material;
                }

                obj.material = oldMat;
            }
        }

        void OnDestroy()
        {
            obj = null;
            transparentMat = null;
            oldMat = null;
        }
    }
}
