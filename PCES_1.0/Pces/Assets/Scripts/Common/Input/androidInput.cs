using UnityEngine;
using System.Collections;

namespace future3d.unityLibs
{
    /// <summary>
    /// 触摸信息
    /// </summary>
    public struct InnerTouchInfo
    {
        public int fingerId;
        public Vector2 position;
        public Vector2 deltaPosition;
        public float deltaTime;
        public int tapCount; //击次数
        public TouchPhase phase;  //触摸阶段 Began、Move、Stationary、Ended、Canceled
    }

    /// <summary>
    /// 触摸类型
    /// </summary>
    public enum TouchType
    {
        /// <summary>
        /// 无触摸
        /// </summary>
        NoneTouch,

        /// <summary>
        /// 单点触摸
        /// </summary>
        SingleTouch,

        /// <summary>
        /// 双点触摸
        /// </summary>
        DoubleTouch,

        /// <summary>
        /// 多点触摸(大于2)
        /// </summary>
        MultiTouch
    }

    /// <summary>
    /// 安卓触摸控制
    /// </summary>
    public class androidInput : MonoBehaviour
    {
        private static InnerTouchInfo[] touches = new InnerTouchInfo[0];  //记录多点触摸信息
        private static InnerTouchInfo[] touches0 = new InnerTouchInfo[0]; //记录无触摸信息
        private static InnerTouchInfo[] touches1 = new InnerTouchInfo[1]; //记录单点触摸信息
        private static InnerTouchInfo[] touches2 = new InnerTouchInfo[2]; //记录双点触摸信息

        void Start()
        {

        }

        /// <summary>
        /// 控制开启/禁用多点触摸
        /// </summary>
        public static bool MultiTouchEnabled
        {
            get { return Input.multiTouchEnabled; }

            set { Input.multiTouchEnabled = value; }
        }

        //记录触摸信息，分发出去
        void Update()
        {
            if (0 == Input.touches.Length)
            {
                GlobalEntity.GetInstance().Dispatch<InnerTouchInfo[]>(TouchType.NoneTouch, touches0);
                return;
            }
            else if (1 == Input.touches.Length)
            {
                int i = 0;
                foreach (Touch touch in Input.touches)
                {
                    touches1[i].deltaPosition = touch.deltaPosition;
                    touches1[i].deltaTime = touch.deltaTime;
                    touches1[i].fingerId = touch.fingerId;
                    touches1[i].phase = touch.phase;
                    touches1[i].position = touch.position;
                    touches1[i].tapCount = touch.tapCount;
                    ++i;
                }
                GlobalEntity.GetInstance().Dispatch<InnerTouchInfo[]>(TouchType.SingleTouch, touches1);
                return;
            }
            else if (2 == Input.touches.Length)
            {
                int i = 0;
                foreach (Touch touch in Input.touches)
                {
                    touches2[i].deltaPosition = touch.deltaPosition;
                    touches2[i].deltaTime = touch.deltaTime;
                    touches2[i].fingerId = touch.fingerId;
                    touches2[i].phase = touch.phase;
                    touches2[i].position = touch.position;
                    touches2[i].tapCount = touch.tapCount;
                    ++i;
                }
                GlobalEntity.GetInstance().Dispatch<InnerTouchInfo[]>(TouchType.DoubleTouch, touches2);
                return;
            }
            else
            {
                touches = new InnerTouchInfo[Input.touches.Length];
                int i = 0;
                foreach (Touch touch in Input.touches)
                {
                    touches[i].deltaPosition = touch.deltaPosition;
                    touches[i].deltaTime = touch.deltaTime;
                    touches[i].fingerId = touch.fingerId;
                    touches[i].phase = touch.phase;
                    touches[i].position = touch.position;
                    touches[i].tapCount = touch.tapCount;
                    ++i;
                }
                GlobalEntity.GetInstance().Dispatch<InnerTouchInfo[]>(TouchType.MultiTouch, touches);
                return;
            }
        }
    }
}
