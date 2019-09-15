using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace POP.Framework
{

    /// <summary>
    /// Base class of all menus that will be generated and used in the application
    /// </summary>
    [RequireComponent(typeof(UnityEngine.RectTransform))]
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

        /// <summary>
        /// flips an internal field that tells the menu if it must accept input
        /// </summary>
        /// <param name="enable"></param>
        public void EnableInput(bool enable = true)
        {
            _inputEnabled = enable;
        }

        protected virtual void InputIndependantUpdateRoutine() { }


        /// <summary>
        /// updates the menu FSM and calls InputIndependantUpdateRoutine and InputDependantUpdateRoutine internally
        /// </summary>
        public void UpdateMenu()
        {
            _baseMenuFSM.UpdateStateMachine();

            InputIndependantUpdateRoutine();

            if (!_inputEnabled)
                return;

            InputDependantUpdateRoutine();
        }

        protected virtual void InputDependantUpdateRoutine() { }

        protected virtual void InitTransitionIn()
        {
            _transitionerToUse.PerformTransition(()=> {
                _baseMenuFSM.SetState(BaseMenuStates.Main);
                TemporaryVariableManager.GetTemporaryVariable<UnityAction<BaseMenu>>(this, _onTransitionInCBIndex)?.Invoke(this);
            });
        }

        protected virtual void InitMain() { }
        protected virtual void UpdateMain() { }
        protected virtual void TerminateMain() { }

        protected virtual void UpdateTransitionIn()
        {
            _transitionerToUse.UpdateTransitioner();   
        }

        protected virtual void TerminateTransitionIn()
        {
            TemporaryVariableManager.SetTemporaryVariable<UnityAction<BaseMenu>>(this, _onTransitionInCBIndex, null);
        }


        protected virtual void InitTransitionOut()
        {
            _transitionerToUse.PerformTransition(() => {
                _baseMenuFSM.SetState(BaseMenuStates.Inactive);
                TemporaryVariableManager.GetTemporaryVariable<UnityAction<BaseMenu>>(this, _onTransitionOutCBIndex)?.Invoke(this);
            },-1);
        }

        protected virtual void UpdateTransitionOut()
        {
            _transitionerToUse.UpdateTransitioner();
        }

        protected virtual void TerminateTransitionOut()
        {
            TemporaryVariableManager.SetTemporaryVariable<UnityAction<BaseMenu>>(this, _onTransitionOutCBIndex, null);
        }


        /// <summary>
        /// Construction routine. 
        /// </summary>
        //dont like the fact that this has to be public, unfortunately c# has no chill for friends :/ :p
        public void ConstructionRoutine()
        {
            _baseMenuFSM = new FSM<BaseMenuStates>();
            _baseMenuFSM.Initialize(this);

            if (_transitionerToUse == null)
                _transitionerToUse = GetComponent<BaseTransitioner>();
            
            ConstructionRoutineInternal();
        }

        protected virtual void ConstructionRoutineInternal() { }


        protected virtual void DestructionRoutineInternal() { }
        public virtual void DestructionRoutine()
        {
            DestructionRoutineInternal();
        }


        /// <summary>
        /// Sets menu state to transitioning in
        /// </summary>
        /// <param name="onComplete"></param>
        public virtual void TransitionIn(UnityAction<BaseMenu> onComplete = null)
        {
            TemporaryVariableManager.SetTemporaryVariable<UnityAction<BaseMenu>>(this, _onTransitionInCBIndex, onComplete,true);
            _baseMenuFSM.SetState(BaseMenuStates.TransitionIn);
        }


        /// <summary>
        /// sets menu state to transitioning out
        /// </summary>
        /// <param name="onComplete"></param>
        public virtual void TransitionOut(UnityAction<BaseMenu> onComplete = null)
        {
            TemporaryVariableManager.SetTemporaryVariable<UnityAction<BaseMenu>>(this, _onTransitionOutCBIndex, onComplete, true);
            _baseMenuFSM.SetState(BaseMenuStates.TransitionOut);
        }


    }
}