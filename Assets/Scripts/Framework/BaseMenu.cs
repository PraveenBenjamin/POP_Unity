using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace POP.Framework
{
    [RequireComponent(typeof(UnityEngine.Canvas))]
    public abstract class BaseMenu : MonoBehaviour
    {

        public enum BaseMenuStates
        {
            TransitionIn,
            Main,
            TransitionOut,
            Inactive
        }

        protected FSM<BaseMenuStates> _baseMenuFSM;

        protected BaseTransitioner _transitionerToUse;
        private int _onTransitionInCBIndex = 0;
        private int _onTransitionOutCBIndex = 1;
        private int _onConstructionCompleteCBIndex = 2;
        private int _onDestructionCompleteCBIndex = 3;


        protected bool _inputEnabled = true;
        public void EnableInput(bool enable = true)
        {
            _inputEnabled = enable;
        }

        protected virtual void InputIndependantUpdateRoutine() { }

        public void UpdateMenu()
        {
            InputIndependantUpdateRoutine();

            if (!_inputEnabled)
                return;

            InputDependantUpdateRoutine();
        }

        protected abstract void InputDependantUpdateRoutine();

        protected virtual void InitializeTransitionIn()
        {
            _transitionerToUse.PerformTransition(()=> {
                _baseMenuFSM.SetState(BaseMenuStates.Main);
                TemporaryVariableManager.GetTemporaryVariable<UnityAction<BaseMenu>>(this, _onTransitionInCBIndex)?.Invoke(this);
            });
        }

        protected abstract void InitMain();
        protected abstract void UpdateMain();
        protected abstract void TerminateMain();

        protected virtual void UpdateTransitionIn()
        {
            _transitionerToUse.UpdateTransitioner();   
        }

        protected virtual void TerminateTransitionIn()
        {
            TemporaryVariableManager.SetTemporaryVariable<UnityAction<BaseMenu>>(this, _onTransitionInCBIndex, null);
        }


        protected virtual void InitializeTransitionOut()
        {
            _transitionerToUse.PerformTransition(() => {
                _baseMenuFSM.SetState(BaseMenuStates.Inactive);
                TemporaryVariableManager.GetTemporaryVariable<UnityAction<BaseMenu>>(this, _onTransitionOutCBIndex)?.Invoke(this);
            });
        }

        protected virtual void UpdateTransitionOut()
        {
            _transitionerToUse.UpdateTransitioner();
        }

        protected virtual void TerminateTransitionOut()
        {
            TemporaryVariableManager.SetTemporaryVariable<UnityAction<BaseMenu>>(this, _onTransitionOutCBIndex, null);
        }


        //dont like the fact that this has to be public, unfortunately c# has no chill for friends :/ :p
        public void ConstructionRoutine()
        {
            _baseMenuFSM = new FSM<BaseMenuStates>();
            _baseMenuFSM.Initialize(this);

            _transitionerToUse.InitializeTransitioner();
            ConstructionRoutineInternal();
        }

        protected abstract void ConstructionRoutineInternal();




        protected abstract void DestructionRoutineInternal();
        public virtual void DestructionRoutine()
        {
            DestructionRoutineInternal();
        }



        public virtual void TransitionIn(UnityAction<BaseMenu> onComplete = null)
        {
            TemporaryVariableManager.SetTemporaryVariable<UnityAction<BaseMenu>>(this, _onTransitionInCBIndex, onComplete);
            _baseMenuFSM.SetState(BaseMenuStates.TransitionIn);
        }


        public virtual void TransitionOut(UnityAction<BaseMenu> onComplete = null)
        {
            TemporaryVariableManager.SetTemporaryVariable<UnityAction<BaseMenu>>(this, _onTransitionOutCBIndex, onComplete);
            _baseMenuFSM.SetState(BaseMenuStates.TransitionOut);
        }


    }
}