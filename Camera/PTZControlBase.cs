using CameraSDK.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraSDK.Camera
{
    /// <summary>
    /// 云台控制基类
    /// </summary>
    public abstract class PTZControlBase
    {
        public int Default_Speed = 3;

        public bool Locked { get; set; } = false;

        public float P_Correct = 0f; //水平校正值
        public float T_Correct = 0f; //水平校正值
        public float Z_Correct = 0f; //水平校正值

        public PTZControlBase(CameraBase camera)
        {

        }

        public abstract bool PTZ_Control(PTZCommand command, bool isStop);
        public abstract bool PTZ_Control(PTZCommand command, int speed, bool isStop);

        protected abstract bool PTZ_Control(int command, bool isStop);
        protected abstract bool PTZ_Control(int command, int speed, bool isStop);

        public abstract bool PTZ_Left();
        public abstract bool PTZ_Left(int speed);
        public abstract bool PTZ_Left_Stop();

        public abstract bool PTZ_Up();
        public abstract bool PTZ_Up(int speed);
        public abstract bool PTZ_Up_Stop();
                             
        public abstract bool PTZ_Right();
        public abstract bool PTZ_Right(int speed);
        public abstract bool PTZ_Right_Stop();
                             
        public abstract bool PTZ_Down();
        public abstract bool PTZ_Down(int speed);
        public abstract bool PTZ_Down_Stop();
                             
        public abstract bool PTZ_Up_Left();
        public abstract bool PTZ_Up_Left(int speed);
        public abstract bool PTZ_Up_Left_Stop();
                             
        public abstract bool PTZ_Up_Right();
        public abstract bool PTZ_Up_Right(int speed);
        public abstract bool PTZ_Up_Right_Stop();
                             
        public abstract bool PTZ_Down_Left();
        public abstract bool PTZ_Down_Left(int speed);
        public abstract bool PTZ_Down_Left_Stop();
                             
        public abstract bool PTZ_Down_Right();
        public abstract bool PTZ_Down_Right(int speed);
        public abstract bool PTZ_Down_Right_Stop();

        public abstract bool PTZ_Zoom_In();
        public abstract bool PTZ_Zoom_In(int speed);
        public abstract bool PTZ_Zoom_In_Stop();

        public abstract bool PTZ_Zoom_Out();
        public abstract bool PTZ_Zoom_Out(int speed);
        public abstract bool PTZ_Zoom_Out_Stop();

        public abstract bool PTZ_Focus_Near();
        public abstract bool PTZ_Focus_Near(int speed);
        public abstract bool PTZ_Focus_Near_Stop();

        public abstract bool PTZ_Focus_Far();
        public abstract bool PTZ_Focus_Far(int speed);
        public abstract bool PTZ_Focus_Far_Stop();

        /// <summary>
        /// 设置PTZ参数
        /// </summary>
        /// <param name="Pan">P</param>
        /// <param name="Tilt">T</param>
        /// <param name="Zoom">Z</param>
        /// <returns></returns>
        public abstract bool SetPTZ(float Pan, float Tilt, float Zoom);
        public abstract bool SetPTZ(float Pan, float Tilt, float Zoom, int ChannelId);

        public abstract bool SetPTZ(PTZ_INFO_BASE PTZ);
        public abstract bool SetPTZ(PTZ_INFO_BASE PTZ, int ChannelId);

        public abstract PTZ_INFO_BASE GetPTZ_Info();

        /// <summary>
        /// 自定义云台操作枚举 转 相机SDK枚举
        /// </summary>
        /// <param name="command">自定义云台操作枚举</param>
        /// <returns>相机SDK枚举值</returns>
        public abstract int GetSDKCommand(PTZCommand command);
    }

    /// <summary>
    /// PTZ参数
    /// </summary>
    public abstract class PTZ_INFO_BASE
    {
        //角度
        public float Pan;
        public float Tilt;
        public float Zoom;

        public PTZ_INFO_BASE(float pan, float tilt, float zoom)
        {
            while (pan < 0)
            {
                pan += 360;
            }
            if (pan >= 360)
            {
                pan %= 360;
            }

            this.Pan = pan;
            this.Tilt = tilt;
            this.Zoom = zoom;
        }

        public override string ToString()
        {
            return $@"Pan:{Pan}, Tilt:{Tilt}, Zoom:{Zoom}";
        }
    }
}
