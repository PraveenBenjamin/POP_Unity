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

        [SerializeField]
        private Button _leftButton;

        [SerializeField]
        private Button _rightButton;

        public void OnDifficultySelected()
        {
            MenuManager.Instance.PopMenu(() =>
            {
                GameManager.Instance.SetGameState(GameManager.GameStates.InGame);
            });
        }

        public void OnDifficultyChanged(int delta)
        {
            int currentDifficulty = (int)GameConfigurationContainer.Difficulty;
            int maxDiffLevel = (int)DifficultyLevel.Hard;
            int requiredDifficulty = Mathf.Clamp(currentDifficulty + delta,0, maxDiffLevel);

            _leftButton.interactable = requiredDifficulty != 0;
            _rightButton.interactable = requiredDifficulty != maxDiffLevel;

            CameraTransitioner.Instance.TransitionTo((CameraTransitioner.CameraPositions)(requiredDifficulty + 2), null, BaseTransitioner.LerpType.Cubic, Constants.globalAnimationSpeed);

            GameConfigurationContainer.Difficulty = (DifficultyLevel) requiredDifficulty;
        }


        protected override void ConstructionRoutineInternal()
        {
            //throw new System.NotImplementedException();
        }

        protected override void DestructionRoutineInternal()
        {
            //throw new System.NotImplementedException();
        }

        protected override void InitMain()
        {
            OnDifficultyChanged(0);
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
            //throw new System.NotImplementedException();
        }
    }
}
