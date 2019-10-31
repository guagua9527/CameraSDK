using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraSDK.Enum
{
    public enum PTZCommand
    {
        PTZ_LEFT = 0x01,
        PTZ_RIGHT = 0x02,
        PTZ_UP = 0x03,
        PTZ_DOWN = 0x04,
        PTZ_UP_LEFT = 0x05,
        PTZ_UP_RIGHT = 0x06,
        PTZ_DOWN_LEFT = 0x07,
        PTZ_DOWN_RIGHT = 0x08,
        PTZ_MOVE_STOP = 0x10,

        PTZ_ZOOM_IN = 0x20,
        PTZ_ZOOM_IN_STOP = 0x21,
        PTZ_ZOOM_OUT = 0x22,
        PTZ_ZOOM_OUT_STOP = 0x23,

        PTZ_ZOOM_ALL_STOP = 0x25,

        PTZ_FOCUS_NEAR = 0x30,
        PTZ_FOCUS_NEAR_STOP = 0x31,
        PTZ_FOCUS_FAR = 0x32,
        PTZ_FOCUS_FAR_STOP = 0x33,

        PTZ_FOCUS_ALL_STOP = 0x35,

        PTZ_ALL_STOP = 0xFF
    }
}
