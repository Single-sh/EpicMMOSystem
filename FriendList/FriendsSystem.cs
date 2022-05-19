using UnityEngine;
using UnityEngine.Rendering;

namespace EpicMMOSystem;

public static partial class FriendsSystem
{
    private static bool isServer => SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null;

    public static void Init()
    {
        if (isServer)
        {
            InitServer();
        }
        else
        {
            InitClient();
        }
    }
}