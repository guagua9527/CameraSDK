using FOD_AI_SDK;
using System;
using System.Drawing;
using System.IO;

namespace CameraSDK.Camera
{
    /// <summary>
    /// 相机基类
    /// </summary>
    public abstract class CameraBase : IDisposable
    {
        public readonly string IpAddress;       //相机IP地址
        public readonly ushort Port;             //相机端口
        public readonly string UserName;        //用户名
        public readonly string Password;        //密码

        public double Height { get; set; }    //相机位置高度

        public bool ShowCenterCross = false;

        public string ImageSavePath { get; set; } = AppDomain.CurrentDomain.BaseDirectory + "image";
        public string VideoSavePath { get; set; } = AppDomain.CurrentDomain.BaseDirectory + "video";

        protected bool m_IsInSave;

        public string LastCustomError { get; protected set; } = "";

        public bool IsInSave { get => m_IsInSave; } //是否正在录像

        public float MAX_ZOOM;
        /// <summary>
        /// 云台控制成员
        /// </summary>
        public PTZControlBase PTZControl { get; protected set; }

        /// <summary>
        /// 播放控件句柄
        /// </summary>
        public IntPtr PlayHwnd;

        /// <summary>
        /// 播放句柄
        /// </summary>
        public IntPtr RealPlayHandle { get; protected set; }

        public AI_SDK ai_SDK;

        /// <summary>
        /// 是否正在播放
        /// </summary>
        public bool IsPlay { get; protected set; } = false;

        /// <summary>
        /// 画面回调委托
        /// </summary>
        /// <param name="bmp">当前帧图像</param>
        public delegate void VideoDataCallBackHanlder(Bitmap bmp);
        //public event VideoDataCallBackHanlder VideoDataCallBackEvent;

        public delegate void DrawFuncCallBackHandle(IntPtr Hdc);
        //public event DrawFuncCallBackHandle DrawFuncCallBackEvent;

        public CameraBase(string ip, ushort port, string userName, string password, double height)
        {
            if (string.IsNullOrEmpty(ip) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password) || port == 0)
            {
                throw new Exception("参数不能为null或空!");
            }
            this.IpAddress = ip;
            this.Port = port;
            this.UserName = userName;
            this.Password = password;

            this.Height = height;

            if (!Directory.Exists(VideoSavePath))
            {
                Directory.CreateDirectory(VideoSavePath);
            }

            if (!Directory.Exists(ImageSavePath))
            {
                Directory.CreateDirectory(ImageSavePath);
            }
        }

        public CameraBase(CAMERA_CONFIG config):this(config.Ip, config.Port, config.UserName, config.Password, config.Height)
        {
            if (config.HaveNullOrEmpty())
            {
                throw new Exception("Config 字段不能为null或空!");
            }
        }



        /// <summary>
        /// 初始化相机
        /// </summary>
        public abstract bool Init();

        /// <summary>
        /// 注册画面回调事件
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public abstract bool RegistVideoDataCallBack(VideoDataCallBackHanlder callback);

        /// <summary>
        /// 登录相机
        /// </summary>
        protected abstract bool Camera_Login(string ipAddress, ushort port, string userName, string password);

        /// <summary>
        /// 退出登录
        /// </summary>
        protected abstract bool Camera_Logout();

        /// <summary>
        /// 注册画面回调事件
        /// </summary>
        /// <param name="drawFunc"></param>
        public abstract void RegisterDrawFuncCB(DrawFuncCallBackHandle drawFunc);

        /// <summary>
        /// 初始化SDK
        /// </summary>
        //protected abstract bool SDK_Init();

        /// <summary>
        /// 释放SDK
        /// </summary>
        //protected abstract bool SDK_Dispose();

        /// <summary>
        /// 开始本地录像
        /// </summary>
        /// <param name="folderPath">文件夹路径(/结尾)</param>
        /// <returns></returns>
        public abstract bool SaveRealData(string folderPath);
        /// <summary>
        /// 停止本地录像
        /// </summary>
        /// <returns></returns>
        public abstract bool StopSaveRealData();

        /// <summary>
        /// 预览抓图
        /// </summary>
        /// /// <param name="fullPath">图像完整储存</param>
        /// <returns></returns>
        public abstract bool Snap(string fullPath);
        /// <summary>
        /// 非预览抓图
        /// </summary>
        /// /// /// <param name="fullPath">图像完整储存</param>
        /// <returns></returns>
        public abstract bool SnapEx(string fullPath);

        /// <summary>
        /// 启动实时预览
        /// </summary>
        /// <param name="hwnd">播放控件句柄</param>
        public abstract bool StartRealPlay(IntPtr hwnd);

        /// <summary>
        /// 停止实时预览
        /// </summary>
        public abstract bool StopRealPlay(IntPtr hwnd);

        /// <summary>
        /// 获取当前分辨率
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public abstract void GetResolution(ref int width, ref int height);

        /// <summary>
        /// 获取最新的SDK错误信息
        /// </summary>
        /// <returns></returns>
        public abstract string GetLastError();

        /// <summary>
        /// 获取最新的SDK错误码
        /// </summary>
        /// <returns></returns>
        public abstract int GetLastErrorCode();

        /// <summary>
        /// 释放相机资源
        /// </summary>
        public abstract void Dispose();
    }

    public struct CAMERA_CONFIG
    {
        public string Ip;
        public ushort Port;

        public string UserName;
        public string Password;

        public double Height;

        public CAMERA_CONFIG(string ip, ushort port, string username, string password, double height = 0)
        {
            this.Ip = ip;
            this.Port = port;
            this.UserName = username;
            this.Password = password;
            this.Height = height;
        }

        public bool HaveNullOrEmpty()
        {
            return string.IsNullOrEmpty(Ip)
                && string.IsNullOrEmpty(UserName)
                && string.IsNullOrEmpty(Password)
                && Port == 0;
        }

        public override string ToString()
        {
            return $"Ip:{Ip}, Port:{Port}, UserName:{UserName}, Password:{Password}";
        }
    }
}
