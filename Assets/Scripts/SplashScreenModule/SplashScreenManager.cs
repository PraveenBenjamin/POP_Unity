using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using POP.Framework;

namespace POP.Modules
{

    // ideally i would have written an animator script from scratch, but i just dont have the time given my deadline.
    // hard-coded animation it is.
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
        private Sprite[] _splashScreens;

        [SerializeField]
        private float _timePerScreen;

        [SerializeField]
        private float _timePerTransition;

        private FSM<SplashScreenStates> _ssFSM;

        private Image _imageHandle;


        private int _commonTimerIndex = 0;
        private int _currSplashIndex = 1;

        protected override void InitializeSingleton()
        {
            _imageHandle = GetComponent<Image>();
            _ssFSM = new FSM<SplashScreenStates>();
            _ssFSM.Initialize(Instance);

            TemporaryVariableManager.SetTemporaryVariable<int>(Instance, _currSplashIndex, 0);
            SetSplashScreen(0);
        }


        private void SetSplashScreen(int index)
        {
            _imageHandle.sprite = _splashScreens[index];
        }



        private void ChangeAlpha(float normalizedAlpha)
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

        private void UpdateTransitionOut()
        {
            float timer = TemporaryVariableManager.GetTemporaryVariable<float>(Instance, _commonTimerIndex);
            timer += Time.deltaTime;

            ChangeAlpha(timer / _timePerTransition);

            if (timer > _timePerTransition)
            {
                int splashScreenIndex = TemporaryVariableManager.GetTemporaryVariable<int>(Instance, _currSplashIndex);
                if (splashScreenIndex < _splashScreens.Length - 1)
                {
                    ++splashScreenIndex;
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
            //dont really need this for now
        }
    }

}