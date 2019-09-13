using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using POP.Framework;
using UnityEngine.UI;
using POP.UI.Menus;
using System;

namespace POP.Modules.Gameplay
{
    public class GameplayScript : SingletonBehaviour<GameplayScript>
    {

        public enum GameStates
        {
            Initilizing,
            Countdown,
            Gameplay,
            Paused,
            Complete,
            Inactive
        }

     
        private FSM<GameStates> _gpFSM;

        private Image _drawArea;

        private int _commonTimerIndex = 0;
        private int _gameTimerIndex = 1;
        private int _scoreIndex = 2;


        protected override void InitializeSingleton()
        {
            _gpFSM = new FSM<GameStates>();
        }

        internal void Resume()
        {
            //countdown again
            _gpFSM.SetState(GameStates.Countdown);
        }

        internal void Pause()
        {
            //stay dormant essentially
            _gpFSM.SetState(GameStates.Paused);
        }


        private void InitInitializing()
        {
            if (ActorManager.Instance == null)
            {
                GameObject amGo = new GameObject("ActorManager");
                amGo.AddComponent<ActorManager>();
            }

            //we can finally start creating actors now
            _drawArea = GameConfigurationContainer.Instance.GetDrawArea(GameConfigurationContainer.Difficulty);

            int gridSize = GameConfigurationContainer.Instance.GetGridSize(GameConfigurationContainer.Difficulty);

            float widthInterval = _drawArea.rectTransform.rect.width / (float)gridSize;
            float heightInterval = _drawArea.rectTransform.rect.height / (float)gridSize;

            Vector2 placementPos = new Vector2(widthInterval/2,-heightInterval/2);

            for (int row = 0; row < gridSize; ++row)
            {
                for (int col = 0; col < gridSize; ++col)
                {
                    //create our little dudes
                    PopPeep pp = ActorManager.Instance.CreateActor<PopPeep>(row.ToString()+"_"+col.ToString());

                    //place our little dudes
                    pp.transform.SetParent(_drawArea.transform,false);

                    placementPos.x += widthInterval;

                    pp.transform.localPosition = placementPos;
                }

                placementPos.x = widthInterval / 2;
                placementPos.y -= heightInterval;
            }


            Color drawAreaCol = _drawArea.color;
            drawAreaCol.a = 1;
            _drawArea.color = drawAreaCol;

            TemporaryVariableManager.SetTemporaryVariable<float>(this, _commonTimerIndex, 0, true);

            MenuManager.Instance.PeekMenu<InGameMenu>().TimerText = "0";
            MenuManager.Instance.PeekMenu<InGameMenu>().ScoreText = "0";

            TemporaryVariableManager.SetTemporaryVariable<float>(this, _gameTimerIndex, (float)(GameConfigurationContainer.Instance.GetMaxGameTime(GameConfigurationContainer.Difficulty)), true);
            TemporaryVariableManager.SetTemporaryVariable<float>(this, _scoreIndex, 0, true);

        }

        private void UpdateInitializing()
        {
            float timer = TemporaryVariableManager.GetTemporaryVariable<float>(this, _commonTimerIndex);
            timer += Time.deltaTime;

            if (timer > Constants.globalAnimationSpeed)
                timer = Constants.globalAnimationSpeed;

            TemporaryVariableManager.SetTemporaryVariable<float>(this, _commonTimerIndex, timer, true);

            float nVal = timer / Constants.globalAnimationSpeed;

            Color drawAreaCol = _drawArea.color;
            drawAreaCol.a = Mathf.Lerp(1, 0, nVal);
            _drawArea.color = drawAreaCol;

            if (nVal == 1)
            {
                _gpFSM.SetState(GameStates.Countdown);
            }
        }

        private void TerminateInitializing()
        {
            // do i need this?
            TemporaryVariableManager.SetTemporaryVariable<float>(this, _commonTimerIndex, 0, true);
        }

        private void InitCountdown()
        {
            TemporaryVariableManager.SetTemporaryVariable<float>(this, _commonTimerIndex, 0, true);
        }

        private void UpdateCountdown()
        {
            float timer = TemporaryVariableManager.GetTemporaryVariable<float>(this, _commonTimerIndex);
            timer += Time.deltaTime;

            if (timer > GameConfigurationContainer.GameCountdownDuration)
            {
                _gpFSM.SetState(GameStates.Gameplay);
            }

            TemporaryVariableManager.SetTemporaryVariable<float>(this, _commonTimerIndex, timer, true);
        }

        private void TerminateCountdown()
        {
            TemporaryVariableManager.SetTemporaryVariable<float>(this, _commonTimerIndex, 0, true);
        }


        private void InitGameplay()
        {
            // do i need this?
        }

        private void UpdateGameplay()
        {
            float timeAvailable = TemporaryVariableManager.GetTemporaryVariable<float>(this, _gameTimerIndex);
            timeAvailable -= Time.deltaTime;
            if (timeAvailable < 0)
                timeAvailable = 0;

            TemporaryVariableManager.SetTemporaryVariable<float>(this, _gameTimerIndex, timeAvailable);


            bool gameOver = timeAvailable == 0
                || ActorManager.Instance.GetActorCount<PopPeep>() == 0;

            if (gameOver)
            {
                _gpFSM.SetState(GameStates.Complete);
            }

        }

        private void TerminateGameplay()
        {
            // do i need this?
        }


        private void InitComplete()
        {
           
        }

        private void UpdateComplete()
        {
            float timer = TemporaryVariableManager.GetTemporaryVariable<float>(this, _commonTimerIndex);
            timer += Time.deltaTime;

            if (timer > Constants.globalAnimationSpeed)
                timer = Constants.globalAnimationSpeed;

            TemporaryVariableManager.SetTemporaryVariable<float>(this, _commonTimerIndex, timer, true);

            float nVal = timer / Constants.globalAnimationSpeed;

            Color drawAreaCol = _drawArea.color;
            drawAreaCol.a = Mathf.Lerp(1, 0, nVal);
            _drawArea.color = drawAreaCol;

            if (nVal == 1)
            {
                _gpFSM.SetState(GameStates.Inactive);
            }
        }

        private void TerminateComplete()
        {
            GameManager.Instance.SetGameState(GameManager.GameStates.GameOver);
        }


        public void UpdateGameplayScript()
        {
            _gpFSM.UpdateStateMachine();
        }

        protected override void OnDestroySingleton()
        {
            throw new System.NotImplementedException();
        }
    }
}
