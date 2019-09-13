﻿using System.Collections;
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
        [SerializeField]
        private DifficultyLevelImageDict _drawAreaDictionary;

        [SerializeField]
        private DifficultyLevelIntDict _gridSizeDictionary;

        public static DifficultyLevel Difficulty = DifficultyLevel.Easy;

    
        public const float GameCountdownDuration = 4;

        [SerializeField]
        private DifficultyLevelIntDict _gameTimerDictionary;


        public Image GetDrawArea(DifficultyLevel level)
        {
            return _drawAreaDictionary[level];
        }

        public int GetGridSize(DifficultyLevel level)
        {
            return _gridSizeDictionary[level];
        }

        public int GetMaxGameTime(DifficultyLevel level)
        {
            return _gameTimerDictionary[level];
        }

        protected override void InitializeSingleton()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnDestroySingleton()
        {
            throw new System.NotImplementedException();
        }
    }
}
