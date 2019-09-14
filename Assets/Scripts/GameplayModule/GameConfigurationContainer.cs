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


        public static DifficultyLevel Difficulty = DifficultyLevel.Easy;

        public const float GameCountdownDuration = 4;

        public const float SplashScreenTransitionTime = 0.5f;

        public const float SplashScreenHangTime = 0.5f;


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

        protected override void InitializeSingleton()
        {
            //throw new System.NotImplementedException();
        }

        protected override void OnDestroySingleton()
        {
            //throw new System.NotImplementedException();
        }
    }
}
