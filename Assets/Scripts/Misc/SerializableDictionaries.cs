using RotaryHeart.Lib.SerializableDictionary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using POP.Modules.Gameplay;
using POP.Framework;
using POP.UI.Menus;

namespace POP.Framework
{
    [System.Serializable]
    public class DifficultyLevelConfigurationDict : SerializableDictionaryBase<DifficultyLevel, GameConfigurationContainer.DifficultyLevelConfiguration> { }

    [System.Serializable]
    public class DifficultyLevelIntDict : SerializableDictionaryBase<DifficultyLevel, int> { }

    [System.Serializable]
    public class PopPeepTypeColorDict : SerializableDictionaryBase<PopPeep.PopPeepTypes, Color> { }

    [System.Serializable]
    public class PopPeepTypeGODict : SerializableDictionaryBase<PopPeep.PopPeepTypes, GameObject> { }

    [System.Serializable]
    public class PopPeepTypeGameOverDatumDict : SerializableDictionaryBase<PopPeep.PopPeepTypes, GameOverMenu.GameOverTextDatum> { }

    [System.Serializable]
    public class PopPeepTypeIntDict : SerializableDictionaryBase<PopPeep.PopPeepTypes, int> { }

    [System.Serializable]
    public class PopPeepTypeSliderDict : SerializableDictionaryBase<PopPeep.PopPeepTypes, Slider> { }

    [System.Serializable]
    public class RectTransformArrayOfTransitionDatumDict : SerializableDictionaryBase<RectTransform, BaseTransitioner.TransitionDatum[]> { }


    [System.Serializable]
    public class StringTransitionDataDict : SerializableDictionaryBase<string, BaseTransitioner.TransitionData> { }


    [System.Serializable]
    public class StringGODict : SerializableDictionaryBase<string, GameObject> { }


    [System.Serializable]
    public class AudioClipTypeAudioClipDic : SerializableDictionaryBase<AudioManager.AudioClipType, AudioClip> { }


}