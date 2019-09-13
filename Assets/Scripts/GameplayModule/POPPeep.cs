using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using POP.Framework;

namespace POP.Modules.Gameplay
{
    [RequireComponent(typeof(RectTransform))]
    public class PopPeep : BaseActor
    {
        public enum PopPeepTypes
        {
            Red = 0,
            Green,
            Blue,
            Black,
            White
        }

        public enum PopPeepStates
        {
            Init,
            Idle,
            Shifty,
            Worried,
            Talkative,
            Selected,
            Matched,
            NotMatched,
            Destroyed
        }

        protected FSM<PopPeepStates> _ppFSM;


        public override void CreationRoutine()
        {
            _ppFSM = new FSM<PopPeepStates>();
        }

        public override void DestructionRoutine()
        {
            _ppFSM = null;
        }


        public void OnClicked()
        {
            _ppFSM.SetState(PopPeepStates.Selected);
        }

        public override void UpdateActor()
        {
            _ppFSM.UpdateStateMachine();
        }

    }
}
