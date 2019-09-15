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
            Initializing,
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

        private float _gridInterval;

        private PopPeep _currentlySelectedPeep;


        protected override void InitializeSingleton()
        {
            _currentlySelectedPeep = null;
            _gpFSM = new FSM<GameStates>();
            _gpFSM.Initialize(this);
        }

        public void Resume()
        {
            //countdown again
            _gpFSM.SetState(GameStates.Countdown);
        }

        public void Pause()
        {
            //stay dormant essentially
            _gpFSM.SetState(GameStates.Paused);
        }

        public void IsSelected(PopPeep pp)
        {
            if (_gpFSM.GetState() != GameStates.Gameplay)
                return;

            if (_currentlySelectedPeep == null)
                _currentlySelectedPeep = pp;
            else
            {
                if (pp != _currentlySelectedPeep)
                {
                    float dist = Vector2.Distance(_currentlySelectedPeep.ArrayPos, pp.ArrayPos);
                    if (_currentlySelectedPeep.Type == pp.Type && dist < _gridInterval * 1.1f)
                    {
                        OnMatch(_currentlySelectedPeep, pp);
                    }
                    else
                    {
                        OnMismatch(_currentlySelectedPeep, pp);
                    }
                }
                _currentlySelectedPeep = null;
            }
        }

        private void OnMatch(PopPeep pp1, PopPeep pp2)
        {
            //MATCH!
            ActorManager.Instance.DestroyActor(pp1);
            ActorManager.Instance.DestroyActor(pp2);
            int score = TemporaryVariableManager.GetTemporaryVariable<int>(this, _scoreIndex);
            score += 10;
            MenuManager.Instance.PeekMenu<InGameMenu>().ScoreText = score.ToString();
            TemporaryVariableManager.SetTemporaryVariable<int>(this, _scoreIndex, score, true);

            if (!GameDataContainer.MatchCountByType.ContainsKey(pp1.Type))
                GameDataContainer.MatchCountByType.Add(pp1.Type, 0);

            GameDataContainer.MatchCountByType[pp1.Type] += 1;
        }


        private IEnumerator SwapPopPeeps(PopPeep pp1, PopPeep pp2,float transitionTime = 2,BaseTransitioner.LerpType lType = BaseTransitioner.LerpType.Cubic)
        {
            float timer = 0;

            if (pp1 == null || pp2 == null)
                yield return 0;

            Vector3 startPos = pp1.transform.position;
            Vector3 endPos = pp2.transform.position;
            float nVal = 0;

            pp1.Enable(false);
            pp2.Enable(false);

            while (timer < transitionTime)
            {
                timer += Time.deltaTime;
                nVal = timer / transitionTime;

                switch (lType)
                {
                    default:
                        nVal = Mathfx.Hermite(0, 1, nVal);
                    break;
                }
                pp1.transform.position = Vector3.Lerp(startPos, endPos, nVal);
                pp2.transform.position = Vector3.Lerp(startPos, endPos, 1- nVal);

                yield return null;
            }

            Vector2 arrayPos = pp1.ArrayPos;
            pp1.SetArrayPos(pp2.ArrayPos);
            pp2.SetArrayPos(arrayPos);

            pp1.Enable(true);
            pp2.Enable(true);

            yield return 0;
        }


        private void OnMismatch(PopPeep pp1, PopPeep pp2)
        {
            //SWAP!
            //i normally dont use coroutines, but i guess it would help to show that i do know how
            // and i dont really have too much time left....
            StartCoroutine(SwapPopPeeps(pp1, pp2,Constants.globalAnimationSpeed));

        }


        private void InitializePeeps(List<PopPeep> allPeeps)
        {
            int count = allPeeps.Count;
            int availableTypes = System.Enum.GetNames(typeof(PopPeep.PopPeepTypes)).Length - 1;
            for (int i = count; i > 0; i -= 2)
            {
                int randColorIndex = UnityEngine.Random.Range(0, availableTypes);

                GameDataContainer.PossibleMatchesByType[(PopPeep.PopPeepTypes)randColorIndex] += 1;

                int rand = UnityEngine.Random.Range(0, allPeeps.Count-1);
                PopPeep ppRef = allPeeps[rand];
                ppRef.SetType((PopPeep.PopPeepTypes)randColorIndex);
                allPeeps.RemoveAt(rand);

                rand = UnityEngine.Random.Range(0, allPeeps.Count - 1);
                ppRef = allPeeps[rand];
                ppRef.SetType((PopPeep.PopPeepTypes)randColorIndex);
                allPeeps.RemoveAt(rand);

            }
        }

        private void InitInitializing()
        {
            if (ActorManager.Instance == null)
            {
                GameObject amGo = new GameObject("ActorManager");
                amGo.AddComponent<ActorManager>();
            }

            _currentlySelectedPeep = null;
            GameDataContainer.Refresh();

            InGameMenu temp = MenuManager.Instance.PeekMenu<InGameMenu>();
            temp.CountdownTextScale = Vector2.zero;

            //we can finally start creating actors now
            _drawArea = GameConfigurationContainer.Instance.GetDrawArea(GameConfigurationContainer.Difficulty);

            int gridSize = GameConfigurationContainer.Instance.GetGridSize(GameConfigurationContainer.Difficulty);
            GameDataContainer.GridSize = gridSize;

            float widthInterval = _drawArea.rectTransform.rect.width / (float)gridSize;
            float heightInterval = _drawArea.rectTransform.rect.height / (float)gridSize;

            _gridInterval = widthInterval;

            Vector3 placementPos =Vector3.one;
            placementPos.x = ((_drawArea.rectTransform.rect.width + widthInterval) / 2) * -1;
            placementPos.y = (_drawArea.rectTransform.rect.height - heightInterval) / 2;
            placementPos.z = 0;

            List<PopPeep> allGeneratedPeeps = new List<PopPeep>();

            for (int row = 0; row < gridSize; ++row)
            {
                for (int col = 0; col < gridSize; ++col)
                {
                    //create our little dudes
                    PopPeep pp = ActorManager.Instance.CreateActor<PopPeep>(row.ToString()+"_"+col.ToString());

                    //place our little dudes
                    pp.transform.SetParent(_drawArea.transform.parent,false);
                    pp.transform.SetAsFirstSibling();

                    RectTransform pprt = pp.GetComponent<RectTransform>();
                    pprt.anchorMin = new Vector2(0, 1);
                    pprt.anchorMax = new Vector2(0, 1);
                    pprt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1);// = new Rect(Vector2.zero, 1);
                    pprt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1);
                    //pprt.rect.height = 1;

                    placementPos.x += widthInterval;

                    pp.transform.localPosition = placementPos;

                    //give it a little padding
                    pp.transform.localScale = Vector3.one * (widthInterval * 0.8f);

                    pp.SetArrayPos(row, col);

                    allGeneratedPeeps.Add(pp);
                }

                placementPos.x = ((_drawArea.rectTransform.rect.width + widthInterval) / 2) * -1;
                placementPos.y -= heightInterval;
            }


            InitializePeeps(allGeneratedPeeps);

            Color drawAreaCol = _drawArea.material.color;
            drawAreaCol.a = 1;
            _drawArea.material.color = drawAreaCol;

            TemporaryVariableManager.SetTemporaryVariable<float>(this, _commonTimerIndex, 0, true);

            MenuManager.Instance.PeekMenu<InGameMenu>().TimerText = "0";
            MenuManager.Instance.PeekMenu<InGameMenu>().ScoreText = "0";

            TemporaryVariableManager.SetTemporaryVariable<float>(this, _gameTimerIndex, (float)(GameConfigurationContainer.Instance.GetMaxGameTime(GameConfigurationContainer.Difficulty)), true);
            TemporaryVariableManager.SetTemporaryVariable<int>(this, _scoreIndex, 0, true);

        }

        private void UpdateInitializing()
        {
            float timer = TemporaryVariableManager.GetTemporaryVariable<float>(this, _commonTimerIndex);
            timer += Time.deltaTime;

            if (timer > Constants.globalAnimationSpeed)
                timer = Constants.globalAnimationSpeed;

            TemporaryVariableManager.SetTemporaryVariable<float>(this, _commonTimerIndex, timer, true);

            float nVal = timer / Constants.globalAnimationSpeed;

            Color drawAreaCol = _drawArea.material.color;
            drawAreaCol.a = Mathf.Lerp(1, 0, nVal);
            _drawArea.material.color = drawAreaCol;

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
            TemporaryVariableManager.SetTemporaryVariable<float>(this, _commonTimerIndex, GameConfigurationContainer.GameCountdownDuration, true);
        }

        private void UpdateCountdown()
        {
            float timer = TemporaryVariableManager.GetTemporaryVariable<float>(this, _commonTimerIndex);
            timer -= Time.deltaTime;

            InGameMenu temp = MenuManager.Instance.PeekMenu<InGameMenu>();
            temp.CountdownText = timer > 1 ? ((int)timer).ToString() : Constants.countdownGoText;
            float mod = timer % 1;
            mod = Mathfx.Hermite(0, 1, mod);
            temp.CountdownTextScale = Vector2.one * Mathfx.Sinerp(0.0f, 1, mod);
            Color col = temp.CountdownTextColor;
            col.a = mod;
            temp.CountdownTextColor = col;
            

            if (timer <= 0)
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

            MenuManager.Instance.PeekMenu<InGameMenu>().TimerText = Mathf.Round((timeAvailable * 100.0f)/100.0f).ToString();

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

            Color drawAreaCol = _drawArea.material.color;
            drawAreaCol.a = Mathf.Lerp(0, 1, nVal);
            _drawArea.material.color = drawAreaCol;

            if (nVal == 1)
            {
                _gpFSM.SetState(GameStates.Inactive);
            }
        }

        private void TerminateComplete()
        {
            GameManager.Instance.SetGameState(GameManager.GameStates.GameOver);
            ActorManager.Instance.DestroyAllActorOfType<PopPeep>();
        }


        public void UpdateGameplayScript()
        {
            _gpFSM.UpdateStateMachine();
        }

        protected override void OnDestroySingleton()
        {
            _currentlySelectedPeep = null;
            StopAllCoroutines();
        }
    }
}
