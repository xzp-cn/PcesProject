using UnityEngine;
using System.Collections;

namespace future3d.unityLibs
{
    /// <summary>
    /// 第三人称视角相机跟随
    /// </summary>
    public class MouseFollow : MonoBehaviour
    {

        private Vector3 offset; //相对距离
        public Transform target; //注视目标
        public float v; //每秒移动速度
        public float w; //每秒旋转速度

        void Start()
        {
            //计算保持相对距离
            offset = target.position - transform.position;
        }

        void Update()
        {
            transform.position = Vector3.Lerp(transform.position, target.position - offset, Time.deltaTime * v);
            Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * w);

        }
    }
}
