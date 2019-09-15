using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using POP.Framework;
using POP.Modules.Gameplay;

public static class GameDataContainer
{
    public static PopPeepTypeIntDict MatchCountByType = new PopPeepTypeIntDict();
    public static PopPeepTypeIntDict PossibleMatchesByType = new PopPeepTypeIntDict();
    public static int GridSize = 0;

    public static float GetMatchRatio(PopPeep.PopPeepTypes type)
    {
        return (float)((float)MatchCountByType[type]/ PossibleMatchesByType[type]);
    }

    public static bool AllMatchesMade()
    {
        foreach (var pair in MatchCountByType)
        {
            if (pair.Value != PossibleMatchesByType[pair.Key])
                return false;
        }
        return true;
    }

    public static void Refresh()
    {
        MatchCountByType.Clear();
        PossibleMatchesByType.Clear();
        GridSize = 0;
        System.Array ppt = System.Enum.GetValues(typeof(PopPeep.PopPeepTypes));
        foreach (var t in ppt)
        {
            MatchCountByType.Add((PopPeep.PopPeepTypes)t, 0);
            PossibleMatchesByType.Add((PopPeep.PopPeepTypes)t, 0);
        }
    }
}
