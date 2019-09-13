using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace POP.Framework
{
    public class GameManager : SingletonBehaviour<GameManager>
    {

        public enum GameStates
        {
            MainMenu,
            InGame,
            GameOver
        }

        FSM<GameStates> _gmFSM;


        protected override void InitializeSingleton()
        {
            _gmFSM = new FSM<GameStates>();
            _gmFSM.Initialize(Instance);
        }

        private void InitMainMenu()
        {

        }

        private void UpdateMainMenu()
        {

        }

        private void TerminateMainMenu()
        {

        }


        protected override void OnDestroySingleton()
        {

        }
    }
}
