using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using POP.Framework;
using POP.Modules.Gameplay;
using UnityEngine.UI;

namespace POP.UI.Menus
{
    public class LevelSelectMenu : BaseMenu
    {

        private Button _leftButton;
        private Button _rightButton;

        public void OnDifficultySelected(int difficulty)
        {
            MenuManager.Instance.PopMenu(() =>
            {
                GameManager.Instance.SetGameState(GameManager.GameStates.InGame);

                //set the appropriate value in the container
                GameConfigurationContainer.Difficulty = (DifficultyLevel)difficulty;
            });
        }

        public void OnDifficultyChanged(int delta)
        {
            int currentDifficulty = (int)GameConfigurationContainer.Difficulty;
            int maxDiffLevel = (int)DifficultyLevel.Hard;
            int requiredDifficulty = Mathf.Clamp(currentDifficulty + delta,0, maxDiffLevel);

            _leftButton.interactable = requiredDifficulty == 0;
            _rightButton.interactable = requiredDifficulty == maxDiffLevel;

            GameConfigurationContainer.Difficulty = (DifficultyLevel) requiredDifficulty;
        }


        protected override void ConstructionRoutineInternal()
        {
            throw new System.NotImplementedException();
        }

        protected override void DestructionRoutineInternal()
        {
            throw new System.NotImplementedException();
        }

        protected override void InitMain()
        {
            OnDifficultyChanged(0);
        }

        protected override void InputDependantUpdateRoutine()
        {
            throw new System.NotImplementedException();
        }

        protected override void TerminateMain()
        {
            throw new System.NotImplementedException();
        }

        protected override void UpdateMain()
        {
            throw new System.NotImplementedException();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
