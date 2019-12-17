using CameraSDK.Camera;
using System;

namespace CameraSDK
{
    public class CameraSDK<T> : IDisposable where T : CameraBase
    {
        public T Camera { get; private set; }

        public static T GetCameraInstance(CAMERA_CONFIG config)
        {
            return (T)Activator.CreateInstance(typeof(T), config);
        }

        public static T GetCameraInstance(string ip, short port, string userName, string password)
        {
            return (T)Activator.CreateInstance(typeof(T), ip, port, userName, password);
        }

        public CameraSDK(string ip, short port, string userName, string password)
        {
            Camera = (T)Activator.CreateInstance(typeof(T), ip, port, userName, password);
        }

        public CameraSDK(CAMERA_CONFIG config)
        {
            Camera = (T)Activator.CreateInstance(typeof(T), config);
        }

        public void Dispose()
        {
            Camera.Dispose();
        }
    }
}
