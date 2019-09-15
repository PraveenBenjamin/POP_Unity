using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using POP.Framework;
using POP.Modules.Gameplay;
using UnityEngine.UI;
using POP.Misc;

namespace POP.UI.Menus
{
    /// <summary>
    /// implementation of the LevelSelectMenu
    /// </summary>
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

        protected override void InitMain()
        {
            OnDifficultyChanged(0);
        }

        public void OnDifficultyChanged(int delta)
        {
            int currentDifficulty = (int)GameConfigurationContainer.Difficulty;
            int maxDiffLevel = (int)DifficultyLevel.Hard;
            int requiredDifficulty = Mathf.Clamp(currentDifficulty + delta,0, maxDiffLevel);

            _leftButton.interactable = requiredDifficulty != 0;
            _rightButton.interactable = requiredDifficulty != maxDiffLevel;

            CameraBehaviour.Instance.TransitionTo((CameraBehaviour.CameraPositions)(requiredDifficulty + 2), null, BaseTransitioner.LerpType.Cubic, Constants.globalAnimationSpeed);

            GameConfigurationContainer.Difficulty = (DifficultyLevel) requiredDifficulty;
        }

        public void OnBackToMainMenu()
        {
            MenuManager.Instance.PopMenu(() =>
            {
                CameraBehaviour.Instance.TransitionTo(CameraBehaviour.CameraPositions.MainMenu, () =>
                 {
                     MenuManager.Instance.PushMenu<MainMenu>();
                 }, BaseTransitioner.LerpType.Cubic, Constants.globalAnimationSpeed);
            });
        }

    }
}
