using RotaryHeart.Lib.SerializableDictionary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using POP.Modules.Gameplay;

namespace POP.Framework
{
    [System.Serializable]
    public class DifficultyLevelImageDict : SerializableDictionaryBase<DifficultyLevel, Image> { }

    [System.Serializable]
    public class DifficultyLevelIntDict : SerializableDictionaryBase<DifficultyLevel, int> { }

    [System.Serializable]
    public class PopPeepTypeColorDict : SerializableDictionaryBase<PopPeep.PopPeepTypes, Color> { }

    [System.Serializable]
    public class PopPeepTypeIntDict : SerializableDictionaryBase<PopPeep.PopPeepTypes, int> { }

    [System.Serializable]
    public class PopPeepTypeSliderDict : SerializableDictionaryBase<PopPeep.PopPeepTypes, Slider> { }

}