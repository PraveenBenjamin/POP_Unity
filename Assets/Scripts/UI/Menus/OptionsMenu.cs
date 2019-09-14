using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using POP.Framework;
using UnityEngine.UI;

namespace POP.UI.Menus
{
    public class OptionsMenu : BaseMenu
    {

        [SerializeField]
        private Slider _volumeSliderHandle;
        public void OnVolumeChanged()
        {
            AudioManager.Instance.SetVolume(_volumeSliderHandle.value);
        }

        public void OnBackButton()
        {
            MenuManager.Instance.PopMenu();
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
            //throw new System.NotImplementedException();
        }

        protected override void InputDependantUpdateRoutine()
        {
           // throw new System.NotImplementedException();
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
