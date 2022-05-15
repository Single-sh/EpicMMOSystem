using System;

namespace EpicMMOSystem;

[Serializable]
public struct Monster
{
    public string name;
    public int minExp;
    public int maxExp;
    
    public Monster(string name, int minExp, int maxExp)
    {
        this.name = name;
        this.minExp = minExp;
        this.maxExp = maxExp;
    }
}