using CameraSDK.Camera.Dahua.SDK;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CameraSDK.Camera.Dahua
{
    public class DahuaCamera : CameraBase
    {
        internal IntPtr m_LoginID = IntPtr.Zero; //登录句柄
        internal IntPtr m_PlayID = IntPtr.Zero; //播放句柄
        internal fSnapRevCallBack _SnapRevCallBack; //抓图回调
        internal NET_DEVICEINFO_Ex _DeviceInfo = new NET_DEVICEINFO_Ex();

        fDrawCallBack drawCallBack;
        fRealDataCallBackEx2 fRealDataCallBack;
        /// <summary>
        /// SDK是否初始化
        /// </summary>
        public static bool IS_SDK_INIT { get; private set; }

        ushort m_SnapSerialNum = 0;

        public DahuaCamera(string ip, ushort port, string userName, string password, double height):base(ip,port,userName, password,height)
        {
            Init();

            MAX_ZOOM = 40;

            this.PTZControl = new DahuaPTZControl(this);
        }

        public DahuaCamera(CAMERA_CONFIG config) : this(config.Ip, config.Port, config.UserName, config.Password, config.Height)
        {
            
            if (config.HaveNullOrEmpty())
            {
                throw new Exception("Config 字段不能为null或空!");
            }
        }

        public override void Dispose()
        {
            m_SnapSerialNum = 0;
            SDK_Dispose();
        }

        public override string GetLastError()
        {
            return NETClient.GetLastError();
        }

        public override int GetLastErrorCode()
        {
            return OriginalSDK.CLIENT_GetLastError();
        }

        public override void GetResolution(ref int width, ref int height)
        {
            object obj = new NET_ENCODE_VIDEO_INFO()
            {
                dwSize = (uint)Marshal.SizeOf(typeof(NET_ENCODE_VIDEO_INFO)),
                emFormatType = EM_FORMAT_TYPE.NORMAL,
            };

            bool ret = NETClient.GetEncodeConfig(m_LoginID, EM_CFG_ENCODE_TYPE.VIDEO, 0, ref obj, 1000);

            NET_ENCODE_VIDEO_INFO info = (NET_ENCODE_VIDEO_INFO)obj;

            width = info.nWidth;
            height = info.nHeight;
        }

        public override bool Init()
        {
            SDK_Init();

            _SnapRevCallBack = new fSnapRevCallBack(SnapRevCallBack);
            NETClient.SetSnapRevCallBack(_SnapRevCallBack, IntPtr.Zero);

            Camera_Login(IpAddress, Port, UserName, Password);


            return true;
        }

        /// <summary>
        /// 注册实时数据回调
        /// 这里是设置YUV数据的
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public override bool RegistVideoDataCallBack(VideoDataCallBackHanlder callback)
        {
            //TODO
            fRealDataCallBack = (lRealHandle, dwDataType, pBuffer, dwBufSize, param, dwUser) =>
            {
                if (!DecodeVideoData) return;

                byte[] data = new byte[dwBufSize];

                Marshal.Copy(pBuffer, data, 0, (int)dwBufSize);

                //创建YUV数据的原始图像
                Mat mat = new Mat(1080 * 3 / 2, 1920, Emgu.CV.CvEnum.DepthType.Cv8U, 1); //宽高的获取

                Marshal.Copy(data, 0, mat.DataPointer, data.Length);

                //YUV图像转BGR图像
                Mat res = new Mat(1080, 1920, Emgu.CV.CvEnum.DepthType.Cv8U, 3);
                CvInvoke.CvtColor(mat, res, Emgu.CV.CvEnum.ColorConversion.Yuv2BgrIyuv);

                Bitmap bmp = new Bitmap(res.Bitmap);
                Task.Run(() => callback?.Invoke(bmp));

                res.Dispose();
                mat.Dispose();

                GC.Collect();
            };


            bool ret = NETClient.SetRealDataCallBack(RealPlayHandle, fRealDataCallBack, IntPtr.Zero, EM_REALDATA_FLAG.YUV_DATA);

            return ret;
        }

        public override bool SaveRealData(string folderPath)
        {
            string fullpath = folderPath + @"\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".avi";
            if (!m_IsInSave)
            {
                m_IsInSave = NETClient.SaveRealData(RealPlayHandle, fullpath);
            }
            else
            {
                LastCustomError = "已经在录像";
                return false;
            }
            return m_IsInSave;
        }

        public override bool StartRealPlay(IntPtr hwnd)
        {
            return StartRealPlay(hwnd, 0);
        }

        public override bool StartRealPlay(IntPtr hwnd, int Channel)
        {
            PlayHwnd = hwnd;

            RealPlayHandle = NETClient.RealPlay(m_LoginID, Channel, hwnd, EM_RealPlayType.Realplay);

            IsPlay = true;

            return RealPlayHandle != IntPtr.Zero;
        }

        public override bool StopRealPlay(IntPtr hwnd)
        {
            bool ret = NETClient.StopRealPlay(hwnd);
            if (ret)
            {
                RealPlayHandle = IntPtr.Zero;
                IsPlay = false;
            }

            return ret;
        }

        public override bool StopSaveRealData()
        {
            if (m_IsInSave)
            {
                bool ret = NETClient.StopSaveRealData(RealPlayHandle);
                m_IsInSave = false;
                return ret;
            }
            return false;
        }

        protected override bool Camera_Login(string ipAddress, ushort port, string userName, string password)
        {
            m_LoginID = NETClient.Login(ipAddress, (ushort)port, userName, password, EM_LOGIN_SPAC_CAP_TYPE.TCP, IntPtr.Zero, ref _DeviceInfo);

            return m_LoginID != IntPtr.Zero;
        }

        protected override bool Camera_Logout()
        {
            if (NETClient.Logout(m_LoginID))
            {
                m_LoginID = IntPtr.Zero;
                return true;
            }
            return false;
        }

        protected static bool SDK_Dispose()
        {
            if (IS_SDK_INIT)
            {
                NETClient.Cleanup();
                IS_SDK_INIT = false;
            }
            return true;
        }

        protected static bool SDK_Init()
        {
            if (!IS_SDK_INIT)
            {
                IS_SDK_INIT = NETClient.Init(null, IntPtr.Zero, null);
            }

            return IS_SDK_INIT;
        }

        /// <summary>
        /// 本地抓图
        /// </summary>
        /// <param name="fullPath">绝对路径</param>
        /// <returns></returns>
        public override bool Snap(string fullPath)
        {
            if(RealPlayHandle == IntPtr.Zero)
            {
                return false;
            }

            return NETClient.CapturePicture(RealPlayHandle, fullPath, EM_NET_CAPTURE_FORMATS.JPEG);
        }

        /// <summary>
        /// 远程抓图
        /// </summary>
        /// <param name="fullPath">绝对路径</param>
        /// <returns></returns>
        public override bool SnapEx(string fullPath)
        {
            if(m_LoginID == IntPtr.Zero)
            {
                return false;
            }

            NET_SNAP_PARAMS asyncSnap = new NET_SNAP_PARAMS
            {
                Channel = 0,
                Quality = 6,
                ImageSize = 2,
                mode = 0,
                InterSnap = 0,
                CmdSerial = m_SnapSerialNum
            };

            m_SnapSerialNum++;
            return NETClient.SnapPictureEx(m_LoginID, asyncSnap, IntPtr.Zero);
        }

        private void SnapRevCallBack(IntPtr lLoginID, IntPtr pBuf, uint RevLen, uint EncodeType, uint CmdSerial, IntPtr dwUser)
        {
            if (EncodeType == 10) //.jpg
            {
                DateTime now = DateTime.Now;
                string fileName = string.Format("{0}-{1}-{2}-{3}-{4}-{5}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second) + ".jpg";
                string filePath = ImageSavePath + "\\" + fileName;
                byte[] data = new byte[RevLen];
                Marshal.Copy(pBuf, data, 0, (int)RevLen);
                try
                {
                    //when the file is operate by local capture will throw expection.
                    using (FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate))
                    {
                        stream.Write(data, 0, (int)RevLen);
                        stream.Flush();
                        stream.Dispose();
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        /// <summary>
        /// 设置远程抓图回调,需要在RealPlay之前调用
        /// </summary>
        /// <param name="snapRevCallBack"></param>
        public void RegisterSnapCallBack(fSnapRevCallBack m_SnapRevCallBack)
        {
            NETClient.SetSnapRevCallBack(m_SnapRevCallBack, IntPtr.Zero);
        }

        public override void RegisterDrawFuncCB(DrawFuncCallBackHandle drawFunc)
        {
            drawCallBack = (lLoginId, lPlayHandle, Hdc, dwUser) =>
            {
                drawFunc?.Invoke(Hdc);
            };

            NETClient.NetRigisterDrawFun(drawCallBack, IntPtr.Zero);
        }
    }
}
