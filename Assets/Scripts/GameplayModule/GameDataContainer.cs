using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using POP.Framework;
using POP.Modules.Gameplay;

public static class GameDataContainer
{
    public static PopPeepTypeIntDict MatchCountByType = new PopPeepTypeIntDict();

    public static void Refresh()
    {
        MatchCountByType.Clear();
        System.Array ppt = System.Enum.GetValues(typeof(PopPeep.PopPeepTypes));
        foreach (var t in ppt)
        {
            MatchCountByType.Add((PopPeep.PopPeepTypes)t, 0);
        }
    }
}
