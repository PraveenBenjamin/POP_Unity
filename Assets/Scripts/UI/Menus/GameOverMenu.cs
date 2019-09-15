using POP.Framework;
using POP.Modules.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using POP.Misc;

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
        private TextMeshProUGUI _headingText;

        [SerializeField]
        private TextMeshProUGUI _messageText;

        [System.Serializable]
        public class GameOverTextDatum
        {
            public string _headingText;
            public string[] _messageOptions;
        }

        [SerializeField]
        private PopPeepTypeGameOverDatumDict _gameOverTextOptions;


        public void OnBackToMainMenu()
        {
            MenuManager.Instance.PopMenu(() =>
            {
                CameraTransitioner.Instance.TransitionTo(CameraTransitioner.CameraPositions.MainMenu, () =>
                {
                    GameManager.Instance.SetGameState(GameManager.GameStates.Pregame);
                    
                },BaseTransitioner.LerpType.Cubic,Constants.globalAnimationSpeed);

                BillboardScript.Instance.EnableBillboard(true);
            });
        }



        private void RandomizeResultText(PopPeep.PopPeepTypes winner)
        {
            GameOverTextDatum options = _gameOverTextOptions[winner];
            _headingText.text = options._headingText;
            _messageText.text = options._messageOptions[Random.Range(0,options._messageOptions.Length-1)];
        }

        private void InitAnimatingResults()
        {
            BillboardScript.Instance.EnableBillboard(false);
            //set all sliders to 0

            List<PopPeep.PopPeepTypes> winners = new List<PopPeep.PopPeepTypes>();
            float max = -1;
            foreach (KeyValuePair<PopPeep.PopPeepTypes, Slider> pair in _ppSliderDic)
            {
                if (pair.Value.value >= max)
                {
                    if (pair.Value.value > max)
                        winners.Clear();

                    max = pair.Value.value;
                    winners.Add(pair.Key);
                }
                pair.Value.value = 0;

                //i know i know its terrible :( no time :3
                pair.Value.transform.Find("Fill Area/Fill").GetComponent<Image>().color = GameConfigurationContainer.Instance.GetColorCode(pair.Key);
            }

            if (winners.Count == 1)
                RandomizeResultText(winners[0]);
            else
            {
                _headingText.text = "Victor left undecided!";
                _messageText.text = "Candidates say they will definitely run in the re-election!";

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
                float matchRatio = GameDataContainer.GetMatchRatio(pair.Key);
                pair.Value.value = Mathfx.Hermite(0, GameDataContainer.GetMatchRatio(pair.Key), nVal);
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