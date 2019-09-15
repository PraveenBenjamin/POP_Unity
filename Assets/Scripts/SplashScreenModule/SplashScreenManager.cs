using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using POP.Framework;
using POP.Modules.Gameplay;

namespace POP.Modules
{

    /// <summary>
    /// Welp what can i tell you? fades in and fades out images, then transfers control :D simple yet effective :D
    /// </summary>
    // ideally i would have written an animator script from scratch, but i just dont have the time given my deadline.
    // hard-coded splashscreen animation it is.

    [RequireComponent(typeof(Image))]
    public class SplashScreenManager : SingletonBehaviour<SplashScreenManager>
    {
        public enum SplashScreenStates
        {
            TransitionIn,
            Main,
            TransitionOut,
            Completed
        }


        [SerializeField]
        protected Sprite[] _splashScreens;

        protected float _timePerScreen;
        protected float _timePerTransition;

        protected FSM<SplashScreenStates> _ssFSM;

        protected Image _imageHandle;


        protected int _commonTimerIndex = 0;
        protected int _currSplashIndex = 1;

        protected override void InitializeSingleton()
        {
            _imageHandle = GetComponent<Image>();
            _ssFSM = new FSM<SplashScreenStates>();
            _ssFSM.Initialize(Instance);

            TemporaryVariableManager.SetTemporaryVariable<int>(Instance, _currSplashIndex, 0);
            SetSplashScreen(0);

            _timePerTransition = GameConfigurationContainer.SplashScreenTransitionTime;
            _timePerScreen = GameConfigurationContainer.SplashScreenHangTime;
        }


        protected void SetSplashScreen(int index)
        {
            _imageHandle.sprite = _splashScreens[index];
        }



        protected virtual void ChangeAlpha(float normalizedAlpha)
        {
            if (_imageHandle != null)
            {
                Color col = _imageHandle.color;
                col.a = normalizedAlpha;
                _imageHandle.color = col;
            }
        }


        private void InitTransitionIn()
        {
            TemporaryVariableManager.SetTemporaryVariable<float>(Instance, _commonTimerIndex, 0);
            ChangeAlpha(0);
        }

        private void UpdateTransitionIn()
        {
            float timer = TemporaryVariableManager.GetTemporaryVariable<float>(Instance, _commonTimerIndex);
            timer += Time.deltaTime;

            ChangeAlpha(timer / _timePerTransition);

            if (timer > _timePerTransition) {
                _ssFSM.SetState(SplashScreenStates.Main);
                return;
            }

            TemporaryVariableManager.SetTemporaryVariable<float>(Instance, _commonTimerIndex, timer);

        }

        private void TerminateTransitionIn()
        {
            TemporaryVariableManager.SetTemporaryVariable<float>(Instance, _commonTimerIndex, 0);
            ChangeAlpha(1);
        }


        private void InitMain()
        {
            TemporaryVariableManager.SetTemporaryVariable<float>(Instance, _commonTimerIndex, 0);
        }

        private void UpdateMain()
        {
            float timer = TemporaryVariableManager.GetTemporaryVariable<float>(Instance, 0);
            timer += Time.deltaTime;

            if (timer > _timePerScreen)
            {
                _ssFSM.SetState(SplashScreenStates.TransitionOut);
                return;
            }

            TemporaryVariableManager.SetTemporaryVariable<float>(Instance, _commonTimerIndex, timer);
        }

        private void TerminateMain()
        {
            TemporaryVariableManager.SetTemporaryVariable<float>(Instance, _commonTimerIndex, 0);
        }


        private void InitTransitionOut()
        {
            TemporaryVariableManager.SetTemporaryVariable<float>(Instance, _commonTimerIndex, 0);
            ChangeAlpha(1);
        }

        protected virtual void UpdateTransitionOut()
        {
            float timer = TemporaryVariableManager.GetTemporaryVariable<float>(Instance, _commonTimerIndex);
            timer += Time.deltaTime;

            ChangeAlpha( 1 -(timer / _timePerTransition));

            TemporaryVariableManager.SetTemporaryVariable<float>(Instance, _commonTimerIndex, timer);

            if (timer > _timePerTransition)
            {
                int splashScreenIndex = TemporaryVariableManager.GetTemporaryVariable<int>(Instance, _currSplashIndex);
                if (splashScreenIndex < _splashScreens.Length - 1)
                {
                    ++splashScreenIndex;
                    SetSplashScreen(splashScreenIndex);
                    TemporaryVariableManager.SetTemporaryVariable<int>(Instance, _currSplashIndex, splashScreenIndex);
                    _ssFSM.SetState(SplashScreenStates.TransitionIn);
                }
                else
                {
                    _ssFSM.SetState(SplashScreenStates.Completed);
                    AppManager.Instance.EndCurrentState();
                }
            }


        }

        private void TerminateTransitionOut()
        {
            TemporaryVariableManager.SetTemporaryVariable<float>(Instance, _commonTimerIndex, 0);
            ChangeAlpha(0);
        }


        public void UpdateSplashScreenManager()
        {
            _ssFSM.UpdateStateMachine();
        }

        protected override void OnDestroySingleton()
        {
            // hey if you can manually release your own crap, why not right?
            _ssFSM = null;
            _imageHandle = null;

        }
    }

}