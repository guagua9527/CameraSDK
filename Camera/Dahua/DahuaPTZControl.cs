using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CameraSDK.Camera.Dahua.SDK;
using CameraSDK.Enum;

namespace CameraSDK.Camera.Dahua
{
    class DahuaPTZControl : PTZControlBase
    {
        protected DahuaCamera camera;

        public DahuaPTZControl(DahuaCamera camera):base(camera)
        {
            this.camera = camera;
        }

        public override PTZ_INFO_BASE GetPTZ_Info()
        {
            object info = new NET_PTZ_LOCATION_INFO();
            NETClient.QueryDevState(camera.m_LoginID, EM_DEVICE_STATE.PTZ_LOCATION, ref info, typeof(NET_PTZ_LOCATION_INFO), 1000);

            Console.WriteLine(((NET_PTZ_LOCATION_INFO)info).ToString());

            NET_PTZ_LOCATION_INFO _info = (NET_PTZ_LOCATION_INFO)info;
            DAHUA_PTZ_INFO PTZ_Info = new DAHUA_PTZ_INFO(_info.nPTZPan, _info.nPTZTilt, _info.nPTZZoom, camera.MAX_ZOOM);

            return PTZ_Info;
        }
        
        public override int GetSDKCommand(PTZCommand command)
        {
            switch (command)
            {
                case PTZCommand.PTZ_LEFT:
                    return (int)EM_EXTPTZ_ControlType.LEFT_CONTROL;
                case PTZCommand.PTZ_RIGHT:
                    return (int)EM_EXTPTZ_ControlType.RIGHT_CONTROL;
                case PTZCommand.PTZ_UP:
                    return (int)EM_EXTPTZ_ControlType.UP_CONTROL;
                case PTZCommand.PTZ_DOWN:
                    return (int)EM_EXTPTZ_ControlType.DOWN_CONTROL;
                case PTZCommand.PTZ_UP_LEFT:
                    return (int)EM_EXTPTZ_ControlType.LEFTTOP;
                case PTZCommand.PTZ_UP_RIGHT:
                    return (int)EM_EXTPTZ_ControlType.RIGHTTOP;
                case PTZCommand.PTZ_DOWN_LEFT:
                    return (int)EM_EXTPTZ_ControlType.LEFTDOWN;
                case PTZCommand.PTZ_DOWN_RIGHT:
                    return (int)EM_EXTPTZ_ControlType.RIGHTDOWN;
                case PTZCommand.PTZ_MOVE_STOP:
                    return (int)EM_EXTPTZ_ControlType.TOTAL;
                case PTZCommand.PTZ_ZOOM_IN:
                    return (int)EM_EXTPTZ_ControlType.ZOOM_ADD_CONTROL;
                case PTZCommand.PTZ_ZOOM_IN_STOP:
                    return (int)EM_EXTPTZ_ControlType.ZOOM_ADD_CONTROL;
                case PTZCommand.PTZ_ZOOM_OUT:
                    return (int)EM_EXTPTZ_ControlType.ZOOM_DEC_CONTROL;
                case PTZCommand.PTZ_ZOOM_OUT_STOP:
                    return (int)EM_EXTPTZ_ControlType.ZOOM_DEC_CONTROL;
                case PTZCommand.PTZ_ZOOM_ALL_STOP:
                    return (int)EM_EXTPTZ_ControlType.TOTAL;
                case PTZCommand.PTZ_FOCUS_NEAR:
                    return (int)EM_EXTPTZ_ControlType.FOCUS_ADD_CONTROL;
                case PTZCommand.PTZ_FOCUS_NEAR_STOP:
                    return (int)EM_EXTPTZ_ControlType.FOCUS_ADD_CONTROL;
                case PTZCommand.PTZ_FOCUS_FAR:
                    return (int)EM_EXTPTZ_ControlType.FOCUS_DEC_CONTROL;
                case PTZCommand.PTZ_FOCUS_ALL_STOP:
                    return (int)EM_EXTPTZ_ControlType.FOCUS_DEC_CONTROL;
                case PTZCommand.PTZ_ALL_STOP:
                    return (int)EM_EXTPTZ_ControlType.TOTAL;
                default:
                    throw new Exception($"不支持的枚举:{command}");
            }
        }

        public override bool PTZ_Control(PTZCommand command, bool isStop)
        {
            return PTZ_Control(command, Default_Speed, isStop);
        }

        public override bool PTZ_Control(PTZCommand command, int speed, bool isStop)
        {
            EM_EXTPTZ_ControlType type = (EM_EXTPTZ_ControlType)GetSDKCommand(command);

            return PTZControl(type, speed, speed, isStop);
        }

        private bool PTZControl(EM_EXTPTZ_ControlType type, int param1, int param2, bool isStop)
        {
            return NETClient.PTZControl(camera.m_LoginID, 0, type, param1, param2, 0, isStop, IntPtr.Zero);
        }

        public override bool PTZ_Down()
        {
            return PTZ_Down(Default_Speed);
        }

        public override bool PTZ_Down(int speed)
        {
            return NETClient.PTZControl(camera.m_LoginID, 0, EM_EXTPTZ_ControlType.DOWN_CONTROL, 0, speed, 0, false, IntPtr.Zero);
        }

        public override bool PTZ_Down_Left()
        {
            return PTZ_Down_Left(Default_Speed);
        }

        public override bool PTZ_Down_Left(int speed)
        {
            return NETClient.PTZControl(camera.m_LoginID, 0, EM_EXTPTZ_ControlType.LEFTDOWN, speed, speed, 0, false, IntPtr.Zero);
        }

        public override bool PTZ_Down_Left_Stop()
        {
            return NETClient.PTZControl(camera.m_LoginID, 0, EM_EXTPTZ_ControlType.LEFTDOWN, Default_Speed, Default_Speed, 0, true, IntPtr.Zero);
        }

        public override bool PTZ_Down_Right()
        {
            return PTZ_Down_Right(Default_Speed);
        }

        public override bool PTZ_Down_Right(int speed)
        {
            return NETClient.PTZControl(camera.m_LoginID, 0, EM_EXTPTZ_ControlType.RIGHTDOWN, speed, speed, 0, false, IntPtr.Zero);
        }

        public override bool PTZ_Down_Right_Stop()
        {
            return NETClient.PTZControl(camera.m_LoginID, 0, EM_EXTPTZ_ControlType.RIGHTDOWN, Default_Speed, Default_Speed, 0, true, IntPtr.Zero);
        }

        public override bool PTZ_Down_Stop()
        {
            return NETClient.PTZControl(camera.m_LoginID, 0, EM_EXTPTZ_ControlType.DOWN_CONTROL, 0, Default_Speed, 0, true, IntPtr.Zero);
        }

        public override bool PTZ_Focus_Far()
        {
            return PTZ_Focus_Far(Default_Speed);
        }

        public override bool PTZ_Focus_Far(int speed)
        {
            return NETClient.PTZControl(camera.m_LoginID, 0, EM_EXTPTZ_ControlType.FOCUS_DEC_CONTROL, 0, speed, 0, false, IntPtr.Zero);
        }

        public override bool PTZ_Focus_Far_Stop()
        {
            return NETClient.PTZControl(camera.m_LoginID, 0, EM_EXTPTZ_ControlType.FOCUS_DEC_CONTROL, 0, Default_Speed, 0, true, IntPtr.Zero);
        }

        public override bool PTZ_Focus_Near()
        {
            return PTZ_Focus_Near(Default_Speed);
        }

        public override bool PTZ_Focus_Near(int speed)
        {
            return NETClient.PTZControl(camera.m_LoginID, 0, EM_EXTPTZ_ControlType.FOCUS_ADD_CONTROL, 0, speed, 0, false, IntPtr.Zero);
        }

        public override bool PTZ_Focus_Near_Stop()
        {
            return NETClient.PTZControl(camera.m_LoginID, 0, EM_EXTPTZ_ControlType.FOCUS_ADD_CONTROL, 0, Default_Speed, 0, true, IntPtr.Zero);
        }

        public override bool PTZ_Left()
        {
            return PTZ_Left(Default_Speed);
        }

        public override bool PTZ_Left(int speed)
        {
            return NETClient.PTZControl(camera.m_LoginID, 0, EM_EXTPTZ_ControlType.LEFT_CONTROL, 0, speed, 0, false, IntPtr.Zero);
        }

        public override bool PTZ_Left_Stop()
        {
            return NETClient.PTZControl(camera.m_LoginID, 0, EM_EXTPTZ_ControlType.LEFT_CONTROL, 0, Default_Speed, 0, true, IntPtr.Zero);
        }

        public override bool PTZ_Right()
        {
            return PTZ_Down_Right(Default_Speed);
        }

        public override bool PTZ_Right(int speed)
        {
            return NETClient.PTZControl(camera.m_LoginID, 0, EM_EXTPTZ_ControlType.RIGHT_CONTROL, 0, speed, 0, false, IntPtr.Zero);
        }

        public override bool PTZ_Right_Stop()
        {
            return NETClient.PTZControl(camera.m_LoginID, 0, EM_EXTPTZ_ControlType.RIGHT_CONTROL, 0, Default_Speed, 0, true, IntPtr.Zero);
        }

        public override bool PTZ_Up()
        {
            return PTZ_Up(Default_Speed);
        }

        public override bool PTZ_Up(int speed)
        {
            return NETClient.PTZControl(camera.m_LoginID, 0, EM_EXTPTZ_ControlType.UP_CONTROL, 0, speed, 0, false, IntPtr.Zero);
        }

        public override bool PTZ_Up_Left()
        {
            return PTZ_Up_Left(Default_Speed);
        }

        public override bool PTZ_Up_Left(int speed)
        {
            return NETClient.PTZControl(camera.m_LoginID, 0, EM_EXTPTZ_ControlType.LEFTTOP, speed, speed, 0, false, IntPtr.Zero);
        }

        public override bool PTZ_Up_Left_Stop()
        {
            return NETClient.PTZControl(camera.m_LoginID, 0, EM_EXTPTZ_ControlType.LEFTTOP, Default_Speed, Default_Speed, 0, true, IntPtr.Zero);
        }

        public override bool PTZ_Up_Right()
        {
            return PTZ_Up_Right(Default_Speed);
        }

        public override bool PTZ_Up_Right(int speed)
        {
            return NETClient.PTZControl(camera.m_LoginID, 0, EM_EXTPTZ_ControlType.RIGHTTOP, speed, speed, 0, false, IntPtr.Zero);
        }

        public override bool PTZ_Up_Right_Stop()
        {
            return NETClient.PTZControl(camera.m_LoginID, 0, EM_EXTPTZ_ControlType.RIGHTTOP, Default_Speed, Default_Speed, 0, true, IntPtr.Zero);
        }

        public override bool PTZ_Up_Stop()
        {
            return NETClient.PTZControl(camera.m_LoginID, 0, EM_EXTPTZ_ControlType.UP_CONTROL, 0, Default_Speed, 0, true, IntPtr.Zero);
        }

        public override bool PTZ_Zoom_In()
        {
            return PTZ_Zoom_In(Default_Speed);
        }

        public override bool PTZ_Zoom_In(int speed)
        {
            return NETClient.PTZControl(camera.m_LoginID, 0, EM_EXTPTZ_ControlType.ZOOM_ADD_CONTROL, 0, speed, 0, false, IntPtr.Zero);
        }

        public override bool PTZ_Zoom_In_Stop()
        {
            return NETClient.PTZControl(camera.m_LoginID, 0, EM_EXTPTZ_ControlType.ZOOM_ADD_CONTROL, 0, Default_Speed, 0, true, IntPtr.Zero);
        }

        public override bool PTZ_Zoom_Out()
        {
            return PTZ_Zoom_Out(Default_Speed);
        }

        public override bool PTZ_Zoom_Out(int speed)
        {
            return NETClient.PTZControl(camera.m_LoginID, 0, EM_EXTPTZ_ControlType.ZOOM_DEC_CONTROL, 0, speed, 0, false, IntPtr.Zero);
        }

        public override bool PTZ_Zoom_Out_Stop()
        {
            return NETClient.PTZControl(camera.m_LoginID, 0, EM_EXTPTZ_ControlType.ZOOM_DEC_CONTROL, 0, Default_Speed, 0, true, IntPtr.Zero);
        }

        public override bool SetPTZ(float Pan, float Tilt, float Zoom)
        {
            return SetPTZ(Pan, Tilt, Zoom, 0);
        }

        public override bool SetPTZ(float Pan, float Tilt, float Zoom, int ChannelId)
        {
            Tilt = Tilt < -180 ? 180 : Tilt;
            Tilt = Tilt > 180 ? 180 : Tilt;

            Zoom = Zoom < 0 ? 0 : Zoom;
            Zoom = Zoom > 128 ? 128 : Zoom;

            while (Pan < 0)
                Pan += 360;

            Pan %= 360;

            int p = (int)(Pan * 10); //0~3600
            int t = (int)(Tilt * 10); //-1800,1800
            int z = (int)(Zoom / 128 * camera.MAX_ZOOM); //0~128


            NET_PTZ_SPACE_UNIT ptz = new NET_PTZ_SPACE_UNIT
            {
                nPositionX = p,
                nPositionY = t,
                nZoom = z,
            };

            NET_PTZ_SPEED_UNIT speed = new NET_PTZ_SPEED_UNIT
            {
                fPositionX = 1,
                fPositionY = 1,
                fZoom = 1,
            };

            NET_PTZ_CONTROL_ABSOLUTELY config = new NET_PTZ_CONTROL_ABSOLUTELY
            {
                stuPosition = ptz,
                stuSpeed = speed,
            };

            int len = Marshal.SizeOf(config);

            IntPtr intPtr = Marshal.AllocHGlobal(len);
            try
            {
                intPtr = Marshal.AllocHGlobal(len);

                Marshal.StructureToPtr(config, intPtr, true);

                return NETClient.PTZControl(camera.m_LoginID, ChannelId, EM_EXTPTZ_ControlType.MOVE_ABSOLUTELY, 0, 0, 0, false, intPtr);
            }
            finally
            {
                Marshal.FreeHGlobal(intPtr);
            }
        }

        public override bool SetPTZ(PTZ_INFO_BASE PTZ)
        {
            return SetPTZ(PTZ, 0);
        }
        public override bool SetPTZ(PTZ_INFO_BASE PTZ, int ChannelId)
        {
            return SetPTZ(PTZ.Pan, PTZ.Tilt, PTZ.Zoom, ChannelId);
        }

        protected override bool PTZ_Control(int command, bool isStop)
        {
            throw new NotImplementedException();
        }

        protected override bool PTZ_Control(int command, int speed, bool isStop)
        {
            throw new NotImplementedException();
        }
    }

    public class DAHUA_PTZ_INFO : PTZ_INFO_BASE
    {
        public DAHUA_PTZ_INFO(int p, int t, int z, float maxzoom) : this(p / 10f, t / 10f, (z / 128f * (maxzoom - 1)) + 1)
        {
            
        }

        public DAHUA_PTZ_INFO(float p, float t, float z):base(p, t, z)
        {

        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
