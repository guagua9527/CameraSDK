using CameraSDK.Camera.Uniview.SDK;
using CameraSDK.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraSDK.Camera.Uniview
{
    class UniviewPTZControl : PTZControlBase
    {
        protected UniviewCamera camera;

        public UniviewPTZControl(UniviewCamera camera) : base(camera)
        {
            this.camera = camera;
        }

        public override bool PTZ_Control(PTZCommand command)
        {
            int com = GetSDKCommand(command);
            return PTZ_Control(com);
        }

        public override bool PTZ_Control(PTZCommand command, int speed)
        {
            int com = GetSDKCommand(command);
            return PTZ_Control(com, speed);
        }

        protected override bool PTZ_Control(int command)
        {
            return PTZControl(command, Default_Speed);
        }

        protected override bool PTZ_Control(int command, int speed)
        {
            return PTZControl(command, speed);
        }

        public override bool PTZ_Down()
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_TILTDOWN, Default_Speed);
        }

        public override bool PTZ_Down(int speed)
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_TILTDOWN, speed);
        }

        public override bool PTZ_Down_Stop()
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_ALLSTOP, Default_Speed);
        }

        public override bool PTZ_Down_Left()
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_LEFTDOWN, Default_Speed);
        }

        public override bool PTZ_Down_Left(int speed)
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_LEFTDOWN, speed);
        }

        public override bool PTZ_Down_Left_Stop()
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_ALLSTOP, Default_Speed);
        }

        public override bool PTZ_Down_Right()
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_RIGHTDOWN, Default_Speed);
        }

        public override bool PTZ_Down_Right(int speed)
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_RIGHTDOWN, speed);
        }

        public override bool PTZ_Down_Right_Stop()
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_ALLSTOP, Default_Speed);
        }

        public override bool PTZ_Left()
        {
            Console.WriteLine("left");
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_PANLEFT, Default_Speed);
        }

        public override bool PTZ_Left(int speed)
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_PANLEFT, speed);
        }

        public override bool PTZ_Left_Stop()
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_ALLSTOP, Default_Speed);
        }

        public override bool PTZ_Right()
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_PANRIGHT, Default_Speed);
        }

        public override bool PTZ_Right(int speed)
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_PANRIGHT, speed);
        }

        public override bool PTZ_Right_Stop()
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_ALLSTOP, Default_Speed);
        }

        public override bool PTZ_Up()
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_TILTUP, Default_Speed);
        }

        public override bool PTZ_Up(int speed)
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_TILTUP, speed);
        }

        public override bool PTZ_Up_Stop()
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_ALLSTOP, Default_Speed);
        }

        public override bool PTZ_Up_Left()
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_LEFTUP, Default_Speed);
        }

        public override bool PTZ_Up_Left(int speed)
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_LEFTUP, speed);
        }

        public override bool PTZ_Up_Left_Stop()
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_ALLSTOP, Default_Speed);
        }

        public override bool PTZ_Up_Right()
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_RIGHTUP, Default_Speed);
        }

        public override bool PTZ_Up_Right(int speed)
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_RIGHTUP, speed);
        }

        public override bool PTZ_Up_Right_Stop()
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_ALLSTOP, Default_Speed);
        }

        public override bool PTZ_Zoom_In()
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_ZOOMTELE, Default_Speed);
        }

        public override bool PTZ_Zoom_In(int speed)
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_ZOOMTELE, speed);
        }

        public override bool PTZ_Zoom_In_Stop()
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_ZOOMTELE_STOP, Default_Speed);
        }

        public override bool PTZ_Zoom_Out()
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_ZOOMWIDE, Default_Speed);
        }

        public override bool PTZ_Zoom_Out(int speed)
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_ZOOMWIDE, speed);
        }

        public override bool PTZ_Zoom_Out_Stop()
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_ZOOMWIDE_STOP, Default_Speed);
        }

        public override bool PTZ_Focus_Near()
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_FOCUSNEAR, Default_Speed);
        }

        public override bool PTZ_Focus_Near(int speed)
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_FOCUSNEAR, speed);
        }

        public override bool PTZ_Focus_Near_Stop()
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_FOCUSNEAR_STOP, Default_Speed);
        }

        public override bool PTZ_Focus_Far()
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_FOCUSFAR, Default_Speed);
        }

        public override bool PTZ_Focus_Far(int speed)
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_FOCUSFAR, speed);
        }

        public override bool PTZ_Focus_Far_Stop()
        {
            return PTZControl((int)NETDEV_PTZ_E.NETDEV_PTZ_FOCUSFAR_STOP, Default_Speed);
        }

        /// <summary>
        /// 云台控制命令
        /// </summary>
        /// <param name="PTZCommand">命令类型</param>
        /// <param name="speed">云台速度</param>
        /// <param name="ChannelID">通道号</param>
        /// <returns></returns>
		public bool PTZControl(int PTZCommand, int speed, int ChannelID = 1)
        {
            if (camera.PlayHandle != IntPtr.Zero)
            {
                Console.WriteLine("PlayHandle");
                return NETDEVSDK.TRUE == NETDEVSDK.NETDEV_PTZControl(camera.PlayHandle, PTZCommand, speed); //启用的预览的云台控制

            }
            else if (camera.deviceInfo.m_lpDevHandle != IntPtr.Zero)
            {
                Console.WriteLine(camera.deviceInfo.m_lpDevHandle);
                Console.WriteLine("m_lpDevHandle");
                return NETDEVSDK.TRUE == NETDEVSDK.NETDEV_PTZControl_Other(camera.deviceInfo.m_lpDevHandle, ChannelID, PTZCommand, speed); //没有启动预览的云台控制
            }
            Console.WriteLine("false");
            return false;
        }

        public override bool SetPTZ(float Pan, float Tilt, float Zoom, int ChannelId = 1)
        {
            return SetPTZ(new Uniview_PTZ_INFO(Pan, Tilt, Zoom), ChannelId);
        }

        public override bool SetPTZ(PTZ_INFO_BASE PTZ, int ChannelId = 1)
        {
            Uniview_PTZ_INFO INFO = PTZ as Uniview_PTZ_INFO;
            Console.WriteLine("INFO--->"+INFO);
            NETDEV_PTZ_ABSOLUTE_MOVE_S PTZ_MOVE_S = new NETDEV_PTZ_ABSOLUTE_MOVE_S
            {
                fPanTiltX = INFO.P_Data,
                fPanTiltY = INFO.T_Data,
                fZoomX = INFO.Z_Data
            };

            return NETDEVSDK.TRUE == NETDEVSDK.NETDEV_PTZAbsoluteMove(camera.deviceInfo.m_lpDevHandle, ChannelId, PTZ_MOVE_S);
        }

        public override PTZ_INFO_BASE GetPTZ_Info(int ChannelId = 1)
        {
            NETDEV_PTZ_STATUS_S _PTZ_STATUS_S = new NETDEV_PTZ_STATUS_S();

            int iRet = NETDEVSDK.NETDEV_PTZGetStatus(camera.deviceInfo.m_lpDevHandle, ChannelId, ref _PTZ_STATUS_S);
            if (iRet == NETDEVSDK.FALSE)
            {
                return null;
            }
            return Uniview_PTZ_INFO.GetNewBySDKData(_PTZ_STATUS_S.fPanTiltX
                , _PTZ_STATUS_S.fPanTiltY
                , _PTZ_STATUS_S.fZoomX
                , _PTZ_STATUS_S.enPanTiltStatus
                , _PTZ_STATUS_S.enZoomStatus);
        }

        public override int GetSDKCommand(PTZCommand command)
        {
            switch (command)
            {
                case PTZCommand.PTZ_LEFT:
                    return (int)NETDEV_PTZ_E.NETDEV_PTZ_PANLEFT;
                case PTZCommand.PTZ_RIGHT:
                    return (int)NETDEV_PTZ_E.NETDEV_PTZ_PANRIGHT;
                case PTZCommand.PTZ_UP:
                    return (int)NETDEV_PTZ_E.NETDEV_PTZ_TILTUP;
                case PTZCommand.PTZ_DOWN:
                    return (int)NETDEV_PTZ_E.NETDEV_PTZ_TILTDOWN;
                case PTZCommand.PTZ_UP_LEFT:
                    return (int)NETDEV_PTZ_E.NETDEV_PTZ_LEFTUP;
                case PTZCommand.PTZ_UP_RIGHT:
                    return (int)NETDEV_PTZ_E.NETDEV_PTZ_RIGHTUP;
                case PTZCommand.PTZ_DOWN_LEFT:
                    return (int)NETDEV_PTZ_E.NETDEV_PTZ_LEFTDOWN;
                case PTZCommand.PTZ_DOWN_RIGHT:
                    return (int)NETDEV_PTZ_E.NETDEV_PTZ_RIGHTDOWN;
                case PTZCommand.PTZ_MOVE_STOP:
                    return (int)NETDEV_PTZ_E.NETDEV_PTZ_MOVE_STOP;
                case PTZCommand.PTZ_ZOOM_IN:
                    return (int)NETDEV_PTZ_E.NETDEV_PTZ_ZOOMTELE;
                case PTZCommand.PTZ_ZOOM_IN_STOP:
                    return (int)NETDEV_PTZ_E.NETDEV_PTZ_ZOOMTELE_STOP;
                case PTZCommand.PTZ_ZOOM_OUT:
                    return (int)NETDEV_PTZ_E.NETDEV_PTZ_ZOOMWIDE;
                case PTZCommand.PTZ_ZOOM_OUT_STOP:
                    return (int)NETDEV_PTZ_E.NETDEV_PTZ_ZOOMWIDE_STOP;
                case PTZCommand.PTZ_ZOOM_ALL_STOP:
                    return (int)NETDEV_PTZ_E.NETDEV_PTZ_ZOOM_STOP;
                case PTZCommand.PTZ_FOCUS_NEAR:
                    return (int)NETDEV_PTZ_E.NETDEV_PTZ_FOCUSNEAR;
                case PTZCommand.PTZ_FOCUS_NEAR_STOP:
                    return (int)NETDEV_PTZ_E.NETDEV_PTZ_FOCUSNEAR_STOP;
                case PTZCommand.PTZ_FOCUS_FAR:
                    return (int)NETDEV_PTZ_E.NETDEV_PTZ_FOCUSFAR_STOP;
                case PTZCommand.PTZ_FOCUS_ALL_STOP:
                    return (int)NETDEV_PTZ_E.NETDEV_PTZ_FOCUS_AND_IRIS_STOP;
                case PTZCommand.PTZ_ALL_STOP:
                    return (int)NETDEV_PTZ_E.NETDEV_PTZ_ALLSTOP;
                default:
                    return (int)NETDEV_PTZ_E.NETDEV_PTZ_ALLSTOP;
            }
        }
    }

    public class Uniview_PTZ_INFO : PTZ_INFO_BASE
    {
        public int PanTiltStatus;       //云台状态枚举 0 空闲  1 正在执行  0xff 非法值
        public int ZoomStatus;          //聚焦状态枚举 同上

        public float P_Data
        {
            get
            {
                if (Pan == 0 || Pan == 360)
                {
                    return 0;
                }
                else if (Pan == 180)
                {
                    return 1;
                }
                else if(Pan > 180)
                {
                    return Pan / -180 + 1;
                }
                return Pan / 180;
            }
        }
        public float T_Data { get => Tilt / 90; }
        public float Z_Data { get => Zoom / UniviewCamera.MAX_ZOOM; }

        public Uniview_PTZ_INFO(float pan, float tilt, float zoom) : base(pan, tilt, zoom)
        {
            if (this.Zoom > UniviewCamera.MAX_ZOOM)
            {
                this.Zoom = UniviewCamera.MAX_ZOOM;
            }
        }

        public static Uniview_PTZ_INFO GetNewBySDKData(float pData, float tData, float zData, int panTiltStatus, int zoomStatus)
        {
            float Pan, Tilt, Zoom;

            if (pData == 0 || pData == -1)
            {
                Pan = 0;
            }
            else if (pData == 1)
            {
                Pan = 180;
            }
            else
            {
                Pan = pData * 180;
                if (pData < 0)
                {
                    Pan += 360;
                }
            }

            Tilt = tData * 90;
            Zoom = zData * UniviewCamera.MAX_ZOOM;

            return new Uniview_PTZ_INFO(Pan, Tilt, Zoom)
            {
                PanTiltStatus = panTiltStatus,
                ZoomStatus = zoomStatus
            };
        }

        public override string ToString()
        {
            return $"Pan:{Pan}, Tilt:{Tilt}, Zoom:{Zoom}, PanTiltStatus:{PanTiltStatus}, ZoomStatus:{ZoomStatus}\n" +
                $"P:{P_Data}, T:{T_Data}, Z:{Z_Data}";
        }
    }
}
