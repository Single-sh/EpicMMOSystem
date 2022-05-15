using System;
using System.Reflection;
using EpicMMOSystem;

namespace API;

public static class EpicMMOSystem_API
{
    private static MO_API_State state = MO_API_State.NotReady;
    private static MethodInfo eGetLevel;
    private static MethodInfo eAddExp;


    private enum MO_API_State
    {
        NotReady, NotInstalled, Ready 
    }

    public static int GetLevel()
    {
        int result = 0;
        Init();
        if (eGetLevel != null) result = (int)eGetLevel.Invoke(null, null);
        return result;
    }

    public static void AddExp(int value)
    {
        Init();
        eAddExp?.Invoke(null, new object[] { value });
    }
 
    private static void Init()
    { 
        if (state is MO_API_State.Ready or MO_API_State.NotInstalled) return;
        if (Type.GetType("EpicMMOSystem.EpicMMOSystem, EpicMMOSystem") == null)
        {
            state = MO_API_State.NotInstalled;
            return;
        }

        state = MO_API_State.Ready;

        Type actionsMO = Type.GetType("API.EMMOS_API, EpicMMOSystem");
        eGetLevel = actionsMO.GetMethod("GetLevel", BindingFlags.Public | BindingFlags.Static);
        eAddExp = actionsMO.GetMethod("AddExp", BindingFlags.Public | BindingFlags.Static);
    }
}

public static class EMMOS_API
{
    public static int GetLevel()
    {
        return LevelSystem.Instance.getLevel();
    }

    public static void AddExp(int exp)
    {
        LevelSystem.Instance.AddExp(exp);
    }
}
