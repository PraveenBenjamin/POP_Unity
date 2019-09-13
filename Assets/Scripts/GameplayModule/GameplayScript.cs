using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using POP.Framework;
using UnityEngine.UI;

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
            Complete
        }

        private FSM<GameStates> _gpFSM;

        protected override void InitializeSingleton()
        {
            _gpFSM = new FSM<GameStates>();
        }

        private void InitInitializing()
        {
            //do all the required initial calculations here

            //we can finally start creating actors now
            Image drawArea = GameConfigurationContainer.Instance.GetDrawArea(GameConfigurationContainer.Difficulty);

            int gridSize = GameConfigurationContainer.Instance.GetGridSize(GameConfigurationContainer.Difficulty);

            float widthInterval = drawArea.rectTransform.rect.width / (float)gridSize;
            float heightInterval = drawArea.rectTransform.rect.height / (float)gridSize;

            Vector2 placementPos = new Vector2(widthInterval/2,-heightInterval/2);

            for (int row = 0; row < gridSize; ++row)
            {
                for (int col = 0; col < gridSize; ++col)
                {
                    //create our little dudes
                    PopPeep pp = ActorManager.Instance.CreateActor<PopPeep>(row.ToString()+"_"+col.ToString());

                    //place our little dudes
                    pp.transform.SetParent(drawArea.transform,false);

                    placementPos.x += widthInterval;

                    pp.transform.localPosition = placementPos;
                }

                placementPos.x = widthInterval / 2;
                placementPos.y -= heightInterval;
            }
            
        }

        private void UpdateInitializing()
        {
            //do actor generation here
        }

        private void TerminateInitializing()
        {
            // do i need this?
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
