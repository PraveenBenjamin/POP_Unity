using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using POP.Framework;
using UnityEngine.UI;

namespace POP.Modules.Gameplay
{
    public enum DifficultyLevel
    {
        Easy = 0,
        Med,
        Hard
    }

    /// <summary>
    /// Central repository for game configuration values
    /// </summary>
    public class GameConfigurationContainer : SingletonBehaviour<GameConfigurationContainer>
    {
        [System.Serializable]
        public class DifficultyLevelConfiguration
        {
            public Image DrawArea;
            public int GridSize;
            public int GameTime;
        }

        [SerializeField]
        DifficultyLevelConfigurationDict _diffConfigDictionary;

        [SerializeField]
        PopPeepTypeColorDict _colorCodes;


        public static DifficultyLevel Difficulty = DifficultyLevel.Easy;

        public const float GameCountdownDuration = 4;

        public const float SplashScreenTransitionTime = 1f;

        public const float SplashScreenHangTime = 10f;

        public const float BillboardTransitionTime = 2f;

        public const float BillboardHangTime = 5f;

        public const float MaxSelectedTime = 3;

        public const int ReferenceResolutionWidth = 1600;

        public const int ReferenceResolutionHeight = 1000;

        public const float SupportedAspectRatio = 1.78f;


        public Image GetDrawArea(DifficultyLevel level)
        {
            return _diffConfigDictionary[level].DrawArea;
        }

        public int GetGridSize(DifficultyLevel level)
        {
            return _diffConfigDictionary[level].GridSize;
        }

        public int GetMaxGameTime(DifficultyLevel level)
        {
            return _diffConfigDictionary[level].GameTime;
        }

        public Color GetColorCode(PopPeep.PopPeepTypes type)
        {
            return _colorCodes[type];
        }

    }
}
