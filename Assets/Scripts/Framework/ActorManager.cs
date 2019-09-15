using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Linq;

//part of the POP.Framework namespace because this class is part of the architechture of the game. Irreplacable essentially
namespace POP.Framework
{

    /// <summary>
    /// Creates, Manages and Destroys all actors within the game
    /// </summary>
    public class ActorManager : SingletonBehaviour<ActorManager>
    {

        private Dictionary<System.Type,Dictionary<string,BaseActor>> _actorMap;
        private List<BaseActor> toDestroy;
        private bool _isDirty = false;

        protected override void InitializeSingleton()
        {
            _actorMap = new Dictionary<Type, Dictionary<string, BaseActor>>();
            toDestroy = new List<BaseActor>();
        }

        /// <summary>
        /// Creates an actor of type specified as generic parameter
        /// </summary>
        /// <typeparam name="T"> type of actor to create (will poll the PrefabDatabase with a stringified version of this to retrieve actore)</typeparam>
        /// <param name="uid"> uid to assign to the actor for both internal management and easy retrieval </param>
        /// <returns>instance of actor or null</returns>
        public T CreateActor<T>(string uid) where T:BaseActor
        {
            Type toConsider = typeof(T);
            if (!_actorMap.ContainsKey(toConsider))
                _actorMap.Add(typeof(T), new Dictionary<string, BaseActor>());

            Dictionary<string, BaseActor> dicToUse = _actorMap[toConsider];
            if (dicToUse.ContainsKey(uid))
            {
                Debug.LogError("Unable to create actor since specified UID already exists in map");
                return null;
            }

            T newActor = PrefabDatabase.Instance.GetPrefabInstance(typeof(T).ToString())?.GetComponent<T>();
            if (newActor != null)
            {
                newActor.gameObject.name = uid;
                newActor.Initialize();
                dicToUse.Add(uid, newActor);
            }
            return newActor;
        }


        private Dictionary<string, BaseActor> GetActorDictionary(System.Type type)
        {
            if (_actorMap.ContainsKey(type))
                return _actorMap[type];

            return null;
        }

        /// <summary>
        /// Attempts to find and return the actor specified by uid
        /// </summary>
        /// <typeparam name="T"> type of actor </typeparam>
        /// <param name="uid"> uid of actor</param>
        /// <returns>either actor reference or null</returns>
        public BaseActor GetActor<T>(string uid)
        {
            Dictionary<string, BaseActor> dic = GetActorDictionary(typeof(T));
            if (dic.ContainsKey(uid))
                return dic[uid];

            return null;
        }


        /// <summary>
        /// Gets the number of actors of type T that exist in the application
        /// </summary>
        /// <typeparam name="T">type of actor</typeparam>
        /// <returns>positive integer or -1</returns>
        public int GetActorCount<T>()
        {
            Dictionary<string, BaseActor> dic = GetActorDictionary(typeof(T));
            if(dic != null)
                return dic.Count;

            return -1;
        }


        /// <summary>
        /// Destroys the actor specified. Calls BaseActor.DestructionRoutine internally.
        /// </summary>
        /// <param name="toRelease"> actor to destroy </param>
        public void DestroyActor(BaseActor toRelease)
        {

            System.Type type = toRelease.GetType();

            Dictionary<string, BaseActor> dic = GetActorDictionary(type);
            if (dic.ContainsKey(toRelease.ActorUID))
            {
                toDestroy.Add(dic[toRelease.ActorUID]);
                _isDirty = true;
            }
            else
            {
                Destroy(toRelease.gameObject);
            }
            toRelease.DestructionRoutine();
        }


        /// <summary>
        /// Destroys all actors of type specified. Calls BaseActor.DestructionRoutine internally.
        /// </summary>
        /// <typeparam name="T"> type of actor </typeparam>
        public void DestroyAllActorOfType<T>()
        {

            System.Type type = typeof(T);

            Dictionary<string, BaseActor> dic = GetActorDictionary(type);
            if (dic != null)
            {
                foreach (BaseActor act in dic.Values)
                {
                    toDestroy.Add(act);
                    act.DestructionRoutine();
                }
                _isDirty = true;
            }
        }

        /// <summary>
        /// maintains the internal actor dictionaries. Use with caution, performance intensive
        /// </summary>
        private void PerformMaintainence()
        {
            for (int i = 0; i < toDestroy.Count; ++i)
            {
                DestroyImmediate(toDestroy[i].gameObject);
            }

            toDestroy.Clear();

            foreach (KeyValuePair<System.Type, Dictionary<string, BaseActor>> pairOuter in _actorMap)
            {

                if (pairOuter.Key == null)
                    continue;

                var badKeysInner = pairOuter.Value.Where(pair => pair.Value == null)
                        .Select(pair => pair.Key)
                        .ToList();
                foreach (var badKeyInner in badKeysInner)
                {
                    pairOuter.Value.Remove(badKeyInner);
                }
            }

            var badKeys = _actorMap.Where(pair => pair.Key == null || pair.Value.Count == 0)
                    .Select(pair => pair.Key)
                    .ToList();
            foreach (var badKey in badKeys)
            {
                _actorMap.Remove(badKey);
            }
        }


        public void UpdateActorManager()
        {
            foreach (KeyValuePair<System.Type, Dictionary<string, BaseActor>> pairOuter in _actorMap)
            {
                foreach (KeyValuePair<string, BaseActor> pairInner in pairOuter.Value)
                {
                    if (pairInner.Value != null)
                        pairInner.Value.UpdateActor();
                    else
                        _isDirty = true;
                }
            }

            if (_isDirty)
            {

                PerformMaintainence();

                _isDirty = false;
            }
        }


        protected override void OnDestroySingleton()
        {
            foreach (KeyValuePair<System.Type, Dictionary<string, BaseActor>> pairOuter in _actorMap)
            {
                foreach (KeyValuePair<string, BaseActor> pairInner in pairOuter.Value)
                {
                    pairInner.Value.DestructionRoutine();
                }
                pairOuter.Value.Clear();
            }

            _actorMap.Clear();
        }
    }
}
