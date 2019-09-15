using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace POP.Framework
{
    /// <summary>
    /// Root of all actors that will be generated in the game
    /// </summary>
    public abstract class BaseActor : MonoBehaviour
    {

        // i know i can create a way to initialize a readonly variable at runtime, but i dont intend to waste time doing the same
        //for now imma just gonna pretend like this is readonly and not change it after setting it
        private string _actorUID;

        /// <summary>
        /// uid of actore
        /// </summary>
        public string ActorUID
        {
            get
            {
                return _actorUID;
            }
        }


        /// <summary>
        /// initializes the actor by calling BaseActor.CreationRoutine internally
        /// </summary>
        public void Initialize()
        {
            _actorUID = this.gameObject.name;
            CreationRoutine();
        }

        public abstract void CreationRoutine();
        public abstract void DestructionRoutine();

        public virtual void UpdateActor() { }

    }
}
