﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using POP.Modules;
using UnityEngine.Events;

namespace POP.Framework
{
    //Overaching app state management
    public class AppManager : SingletonBehaviour<AppManager>
    {

        [SerializeField]
        private Canvas _UIRoot;

        public Canvas UIRoot
        {
            get
            {
                return _UIRoot;
            }
        }

        // ideally i would have done the generation and configuration of modules at runtime, and initialize them through a config file, 
        // but i definitely have a deadline to adhere to, and i dont have the time
        [SerializeField]
        private SplashScreenManager _splashScreenPrefab;

        [SerializeField]
        private GameManager _gameManagerPrefab;

        UnityAction _maintainenceDelegate = TemporaryVariableManager.GetMaintainenceDelegate();


        public enum AppStates
        {
            Initialization,
            SplashScreen,
            MainRoutine,
            Destruction
        }

        FSM<AppStates> _appManagerFSM;

        protected override void InitializeSingleton()
        {
            _appManagerFSM = new FSM<AppStates>();
            _appManagerFSM.Initialize(Instance);


            //create universal singletons
            
        }

        private void InitInitialization()
        {
            _appManagerFSM.SetState(AppStates.SplashScreen);
        }


        private void InitSplashScreen()
        {
           SplashScreenManager ssm = Instantiate(_splashScreenPrefab);
            ssm.transform.SetParent(_UIRoot.transform,false);
        }

        private void UpdateSplashScreen()
        {
            SplashScreenManager.Instance?.UpdateSplashScreenManager();
        }

        private void TerminateSplashScreen()
        {
            if (SplashScreenManager.Instance != null)
                Destroy(SplashScreenManager.Instance.gameObject);
        }


        private void InitMainRoutine()
        {
            GameManager go = Instantiate(_gameManagerPrefab);
            go.transform.SetParent(transform, false);
        }

        private void UpdateMainRoutine()
        {
            GameManager.Instance.UpdateGameManager();
        }

        private void TerminateMainRoutine()
        {
            Destroy(GameManager.Instance.gameObject);
        }


        public void EndCurrentState()
        {
            Debug.Log("EndCurrentState called during " + _appManagerFSM.GetState());

            // so this function is supposed to switch the current state and handle its termination,
            // but given the size of the program, we can safely just increment the state and be done.
            _appManagerFSM.SetState((AppStates)(_appManagerFSM.GetState() + 1));
        }

        protected override void UpdateSingleton()
        {
            _appManagerFSM.UpdateStateMachine();
            _maintainenceDelegate.Invoke();
            
        }

        protected override void OnDestroySingleton()
        {
            //release code will be called from within the fsm class
            _appManagerFSM = null;
        }

      
    }

}