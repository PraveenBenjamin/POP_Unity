﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using POP.UI.Menus;
using POP.Modules.Gameplay;

namespace POP.Framework
{
    public class GameManager : SingletonBehaviour<GameManager>
    {

        public enum GameStates
        {
            Init,
            Pregame,
            InGame,
            GameOver
        }

        FSM<GameStates> _gmFSM;


        protected override void InitializeSingleton()
        {
            _gmFSM = new FSM<GameStates>();
            _gmFSM.Initialize(Instance);


            if (MenuManager.Instance == null)
            {
                GameObject mmGo = new GameObject("MenuManager");
                mmGo.AddComponent<MenuManager>();
                mmGo.transform.SetParent(AppManager.Instance.transform, false);
            }


            if (ActorManager.Instance == null)
            {
                GameObject amGo = new GameObject("ActorManager");
                amGo.AddComponent<ActorManager>();
                amGo.transform.SetParent(AppManager.Instance.transform, false);
            }

            //idle until the menu manager triggers the ingame setstate call
            SetGameState(GameStates.Pregame);


            AudioManager.Instance.PlayLooping(AudioManager.AudioClipType.BackgroundMusic);
        }

        private void InitPregame()
        {
            CameraTransitioner.Instance.TransitionTo(CameraTransitioner.CameraPositions.MainMenu, () =>
             {
                 MenuManager.Instance.PushMenu<MainMenu>();
             }, BaseTransitioner.LerpType.Cubic, Constants.globalAnimationSpeed);
        }

        public void SetGameState(GameStates toSet)
        {
            _gmFSM.SetState(toSet);
        }

        private void InitInGame()
        {
            MenuManager.Instance.PushMenu<InGameMenu>(()=>
            {
                GameObject gpGo = new GameObject("GameplayHandler");
                gpGo.AddComponent<GameplayScript>();
                gpGo.transform.SetParent(AppManager.Instance.transform, false);
            });
        }

        private void UpdateInGame()
        {
            if(GameplayScript.Instance != null)
                GameplayScript.Instance.UpdateGameplayScript();
        }

        private void TerminateInGame()
        {
            DestroyImmediate(GameplayScript.Instance.gameObject);
            MenuManager.Instance.PopMenu(() =>
            {
                CameraTransitioner.Instance.TransitionTo(CameraTransitioner.CameraPositions.GameOver, () =>
                 {
                     MenuManager.Instance.PushMenu<GameOverMenu>();
                 }, BaseTransitioner.LerpType.Cubic, Constants.globalAnimationSpeed);
                
            });
            
        }


        public void UpdateGameManager()
        {

            MenuManager.Instance.UpdateMenuManager();

            ActorManager.Instance.UpdateActorManager();

            _gmFSM.UpdateStateMachine();
            
        }


        protected override void OnDestroySingleton()
        {

        }
    }
}
