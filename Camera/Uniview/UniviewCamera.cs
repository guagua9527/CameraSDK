using CameraSDK.Camera.Uniview.SDK;
using CameraSDK.Enum;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CameraSDK.Camera.Uniview
{
    public class UniviewCamera : CameraBase
    {
        public new static float MAX_ZOOM = 44;

        public int m_panelIndex = -1;
        public int m_deviceIndex = -1;
        public int m_channelIndex = -1;
        public bool m_playStatus = false;/*播放状态，默认未播放*/
        public bool m_recordStatus = false;/*luxiang*/

        internal DeviceInfo deviceInfo;

        public new VideoDataCallBackHanlder VideoDataCallBackEvent;

        NETDEVSDK.NETDEV_DECODE_VIDEO_DATA_CALLBACK_PF videoDataCB;
        NETDEVSDK.NETDEV_DISPLAY_CALLBACK_PF displayCB;

        static UniviewCamera()
        {
            ThreadPool.SetMaxThreads(2000, 1000);
            ThreadPool.SetMinThreads(500, 500);
        }

        public UniviewCamera(CAMERA_CONFIG config) : this(config.Ip, config.Port, config.UserName, config.Password, config.Height)
        {
            if (config.HaveNullOrEmpty())
            {
                throw new Exception("Config 字段不能为null或空!");
            }
        }

        public UniviewCamera(string ip, short port, string userName, string password, double height) : base(ip, port, userName, password, height)
        {
            Init();

            this.PTZControl = new UniviewPTZControl(this);
        }

        public override bool Init()
        {
            //初始化摄像头SDK
            SDK_Init();

            Camera_Login(IpAddress, Port, UserName, Password);

            int pdwChlCount = 256;

            IntPtr pstVideoChlList = new IntPtr();

            pstVideoChlList = Marshal.AllocHGlobal(256 * Marshal.SizeOf(typeof(NETDEV_VIDEO_CHL_DETAIL_INFO_S)));
            int iRet = NETDEVSDK.NETDEV_QueryVideoChlDetailList(deviceInfo.m_lpDevHandle, ref pdwChlCount, pstVideoChlList);
            if (NETDEVSDK.TRUE == iRet)
            {
                deviceInfo.m_channelNumber = pdwChlCount;
                NETDEV_VIDEO_CHL_DETAIL_INFO_S stCHLItem = new NETDEV_VIDEO_CHL_DETAIL_INFO_S();
                for (int i = 0; i < pdwChlCount; i++)
                {
                    IntPtr ptrTemp = new IntPtr(pstVideoChlList.ToInt64() + Marshal.SizeOf(typeof(NETDEV_VIDEO_CHL_DETAIL_INFO_S)) * i);
                    stCHLItem = (NETDEV_VIDEO_CHL_DETAIL_INFO_S)Marshal.PtrToStructure(ptrTemp, typeof(NETDEV_VIDEO_CHL_DETAIL_INFO_S));

                    ChannelInfo channelInfo = new ChannelInfo
                    {
                        m_devVideoChlInfo = stCHLItem
                    };
                    deviceInfo.m_channelInfoList.Add(channelInfo);
                }
            }
            Marshal.FreeHGlobal(pstVideoChlList);

            return true;
        }

        public override bool StartRealPlay(IntPtr hwnd)
        {
            PlayControlHandle = hwnd;

            NETDEV_PREVIEWINFO_S stPreviewInfo = new NETDEV_PREVIEWINFO_S
            {
                dwChannelID = 1,
                dwLinkMode = (int)NETDEV_PROTOCAL_E.NETDEV_TRANSPROTOCAL_RTPTCP,
                dwStreamType = (int)NETDEV_LIVE_STREAM_INDEX_E.NETDEV_LIVE_STREAM_INDEX_MAIN,
                hPlayWnd = PlayControlHandle
            };

            PlayHandle = NETDEVSDK.NETDEV_RealPlay(deviceInfo.m_lpDevHandle, ref stPreviewInfo, IntPtr.Zero, IntPtr.Zero);
            if (PlayHandle == IntPtr.Zero)
            {
                return false;
            }

            RealPlayInfo objRealPlayInfo = new RealPlayInfo
            {
                m_channel = 1,
                m_panelIndex = 1
            };
            deviceInfo.addRealPlayInfo(objRealPlayInfo);

            NETDEVSDK.NETDEV_SetIVAEnable(PlayHandle, 1);
            NETDEVSDK.NETDEV_SetIVAShowParam(7);

            IsPlay = true;

            //注册视频画面显示后数据回调
            //NETDEV_DISPLAY_CALLBACK_PF displayCB = new NETDEVSDK.NETDEV_DISPLAY_CALLBACK_PF(DisplayCallBack);
            //IntPtr displayPtr = Marshal.GetFunctionPointerForDelegate(displayCB);
            //iRet = NETDEVSDK.NETDEV_SetPlayDisplayCB(PlayHandle, displayPtr, IntPtr.Zero);

            return true;
        }

        public bool RegisteDisplayCB(NETDEVSDK.NETDEV_DISPLAY_CALLBACK_PF DisplayCallBack)
        {
            if (PlayHandle == IntPtr.Zero)
            {
                throw new Exception("未启动预览，无法注册画面回调函数");
            }
            displayCB = DisplayCallBack;

            IntPtr displayPtr = Marshal.GetFunctionPointerForDelegate(displayCB);

            return NETDEVSDK.TRUE == NETDEVSDK.NETDEV_SetPlayDisplayCB(PlayHandle, displayPtr, IntPtr.Zero);
        }

        public override bool StopRealPlay()
        {
            bool res = NETDEVSDK.FALSE == NETDEVSDK.NETDEV_StopRealPlay(PlayHandle);

            IsPlay = !res;

            return res;
        }

        /// <summary>
		/// 播放时的画面回调事件
		/// </summary>
		/// <param name="lpUserID"></param>
		/// <param name="pstPictureData"></param>
		/// <param name="lpUserParam"></param>
		//[HandleProcessCorruptedStateExceptions]
        public void VideoDataCallBack(IntPtr lpUserID, ref NETDEV_PICTURE_DATA_S pstPictureData, IntPtr lpUserParam)
        {
            //ai_SDK?.Logger.Log("DistinguishImage", "in1");
            //YUV画面的数据
            NETDEV_PICTURE_DATA_S temp = pstPictureData;
            //ai_SDK?.Logger?.Log("DistinguishImage", "in2");
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                // YUV数据转一维数组
                byte[] Yarr = new byte[temp.dwLineSize[0] * temp.dwPicHeight];
                byte[] Uarr = new byte[temp.dwLineSize[1] * temp.dwPicHeight / 2];
                byte[] Varr = new byte[temp.dwLineSize[2] * temp.dwPicHeight / 2];

                try
                {
                    Marshal.Copy(temp.pucData[0], Yarr, 0, Yarr.Length);
                    Marshal.Copy(temp.pucData[1], Uarr, 0, Uarr.Length);
                    Marshal.Copy(temp.pucData[2], Varr, 0, Varr.Length);
                }
                catch (Exception)
                {
                    return;
                }

                List<byte> yuvData = new List<byte>();
                yuvData.AddRange(Yarr);
                yuvData.AddRange(Uarr);
                yuvData.AddRange(Varr);

                //创建YUV数据的原始图像
                Mat mat = new Mat(temp.dwPicHeight * 3 / 2, temp.dwPicWidth, Emgu.CV.CvEnum.DepthType.Cv8U, 1);

                Marshal.Copy(yuvData.ToArray(), 0, mat.DataPointer, yuvData.Count);

                //YUV图像转BGR图像
                Mat res = new Mat(temp.dwPicHeight, temp.dwPicWidth, Emgu.CV.CvEnum.DepthType.Cv8U, 3);
                CvInvoke.CvtColor(mat, res, Emgu.CV.CvEnum.ColorConversion.Yuv2BgrIyuv);//Yuv2BgrIyuv

                if (ai_SDK != null)
                {
                        Bitmap bmp = new Bitmap(res.Bitmap);
                        //int index = imagePool.AddImage(bmp);
                        //ai_SDK.DistinguishImage(index, bmp);
                        VideoDataCallBackEvent?.Invoke(bmp);

                        bmp.Dispose();
                }

                res.Dispose();
                mat.Dispose();
            });
        }

        public override bool RegistVideoDataCallBack(VideoDataCallBackHanlder callback)
        {
            VideoDataCallBackEvent += callback;

            //注册视频解码数据回调事件
            videoDataCB = new NETDEVSDK.NETDEV_DECODE_VIDEO_DATA_CALLBACK_PF(VideoDataCallBack);

            IntPtr cbPtr = Marshal.GetFunctionPointerForDelegate(videoDataCB);

            int iRet = NETDEVSDK.NETDEV_SetPlayDecodeVideoCB(PlayHandle, videoDataCB, 1, IntPtr.Zero);
            return iRet == NETDEVSDK.TRUE;
        }

        public override void Dispose()
        {
            if (IsPlay)
            {
                if (NETDEVSDK.FALSE == NETDEVSDK.NETDEV_StopRealPlay(PlayHandle))
                {
                    Console.WriteLine(deviceInfo.m_ip + " chl:" + 1, "stop real play", NETDEVSDK.NETDEV_GetLastError());
                }
                IsPlay = false;
            }
            displayCB = null;
            videoDataCB = null;
            SDK_Dispose();
        }

        public override int GetLastError()
        {
            return NETDEVSDK.NETDEV_GetLastError();
        }

        protected override bool Camera_Login(string ipAddress, int port, string userName, string password)
        {
            deviceInfo = new DeviceInfo
            {
                m_ip = this.IpAddress,
                m_port = this.Port,
                m_userName = this.UserName,
                m_password = this.Password
            };

            NETDEV_DEVICE_INFO_S pstDevInfo = new NETDEV_DEVICE_INFO_S();
            IntPtr lpDevHandle = NETDEVSDK.NETDEV_Login(deviceInfo.m_ip, deviceInfo.m_port, deviceInfo.m_userName, deviceInfo.m_password, ref pstDevInfo);

            if (lpDevHandle == IntPtr.Zero)
            {
                throw new Exception("登录失败，错误码:" + NETDEVSDK.NETDEV_GetLastError());
            }

            deviceInfo.m_stDevInfo = pstDevInfo;
            deviceInfo.m_lpDevHandle = lpDevHandle;

            return true;
        }

        protected override bool Camera_Logout()
        {
            return NETDEVSDK.TRUE == NETDEVSDK.NETDEV_Logout(deviceInfo.m_lpDevHandle);
        }

        protected override bool SDK_Dispose()
        {
            return NETDEVSDK.TRUE == NETDEVSDK.NETDEV_Cleanup();
        }

        protected override bool SDK_Init()
        {
            return NETDEVSDK.TRUE == NETDEVSDK.NETDEV_Init();
        }

        public override bool SaveRealData(string folderPath)
        {
            return NETDEVSDK.TRUE == NETDEVSDK.NETDEV_SaveRealData(PlayHandle, Encoding.Default.GetBytes(folderPath), (int)NETDEV_MEDIA_FILE_FORMAT_E
.NETDEV_MEDIA_FILE_VIDEO_AVI_ADD_RCD_TIME);
        }

        public override bool StopSaveRealData()
        {
            return NETDEVSDK.TRUE == NETDEVSDK.NETDEV_StopSaveRealData(PlayHandle);
        }

        public override void GetResolution(ref int width, ref int height)
        {
            NETDEVSDK.NETDEV_GetResolution(PlayHandle, ref width, ref height);
        }
    }
}
