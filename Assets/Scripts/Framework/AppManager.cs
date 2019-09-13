using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace POP.Framework
{
    //Overaching app state management
    public class AppManager : SingletonBehaviour<AppManager>
    {

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
            _appManagerFSM.Initialize(Instance);
        }

        private void InitInitialization()
        {
            _appManagerFSM.SetState(AppStates.SplashScreen);
        }


        private void InitSplashScreen()
        {

        }

        private void UpdateSplashScreen()
        {

        }

        private void TerminateSplashScreen()
        {
            _appManagerFSM.SetState(AppStates.MainRoutine);
        }




        private void Update()
        {
            _appManagerFSM.UpdateStateMachine();
        }

        protected override void OnDestroySingleton()
        {
            //release code will be called from within the fsm class
            _appManagerFSM = null;
        }

      
    }

}