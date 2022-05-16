using System;

namespace EpicMMOSystem;

[Serializable]
public struct Monster
{
    public string name;
    public int minExp;
    public int maxExp;
    public int level;
    
    public Monster(string name, int minExp, int maxExp, int level)
    {
        this.name = name;
        this.minExp = minExp;
        this.maxExp = maxExp;
        this.level = level;
    }
}