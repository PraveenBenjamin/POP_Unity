using POP.Framework;
using POP.Modules.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace POP.UI.Menus
{

    //i would have added the option to restart the game from here, but im afraid il run out of time, so im gonna let him go through the main menu again
    public class GameOverMenu : BaseMenu
    {

        public enum GameOverStates
        {
            AnimatingResults,
            //dormant essentially
            ShowingOptions
        }

        public FSM<GameOverStates> _goFSM;

        private int _commonTimerIndex = 0;

        [SerializeField]
        private PopPeepTypeSliderDict _ppSliderDic;

        [SerializeField]
        private TextMeshPro _titleText;

        [SerializeField]
        private TextMeshPro _messageText;


        public void OnBackToMainMenu()
        {
            MenuManager.Instance.PopMenu(() =>
            {
                CameraTransitioner.Instance.TransitionTo(CameraTransitioner.CameraPositions.MainMenu, () =>
                {
                    GameManager.Instance.SetGameState(GameManager.GameStates.Pregame);
                    MenuManager.Instance.PushMenu<MainMenu>();
                },BaseTransitioner.LerpType.Cubic,Constants.globalAnimationSpeed);
            });
        }

        private void InitAnimatingResults()
        {

            //set all sliders to 0
            foreach (KeyValuePair<PopPeep.PopPeepTypes, Slider> pair in _ppSliderDic)
            {
                pair.Value.value = 0;
            }


        }

        private void UpdateAnimatingResults()
        {

            float timer = TemporaryVariableManager.GetTemporaryVariable<float>(this, _commonTimerIndex);
            timer += Time.deltaTime;

            if (timer > Constants.globalAnimationSpeed)
                timer = Constants.globalAnimationSpeed;

            TemporaryVariableManager.SetTemporaryVariable<float>(this, _commonTimerIndex, timer, true);

            float nVal = timer / Constants.globalAnimationSpeed;

            foreach (KeyValuePair<PopPeep.PopPeepTypes, Slider> pair in _ppSliderDic)
            {
                pair.Value.value = Mathfx.Clerp(0, GameDataContainer.MatchCountByType[pair.Key], nVal);
            }

            if (nVal == 1)
            {
                _goFSM.SetState(GameOverStates.ShowingOptions);
            }

        }

        private void TerminateAnimatingResults()
        {
            TemporaryVariableManager.SetTemporaryVariable<float>(this, _commonTimerIndex, 0, true);
        }


        private void TerminateShowingOptions()
        {
            foreach (KeyValuePair<PopPeep.PopPeepTypes, Slider> pair in _ppSliderDic)
            {
                pair.Value.value = 0;
            }
        }


        protected override void ConstructionRoutineInternal()
        {
            _goFSM = new FSM<GameOverStates>();
            _goFSM.Initialize(this);
        }

        protected override void DestructionRoutineInternal()
        {
            _goFSM = null;
        }

        protected override void InitMain()
        {
            //throw new System.NotImplementedException();
        }

        protected override void InputDependantUpdateRoutine()
        {
            //throw new System.NotImplementedException();
        }

        protected override void TerminateMain()
        {
            //throw new System.NotImplementedException();
        }

        protected override void UpdateMain()
        {
            _goFSM.UpdateStateMachine();
            //throw new System.NotImplementedException();
        }

    }
}