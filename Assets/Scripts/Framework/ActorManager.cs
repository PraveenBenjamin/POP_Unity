using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace POP.Framework
{
    public class ActorManager : SingletonBehaviour<ActorManager>
    {

        private Dictionary<System.Type,Dictionary<string,BaseActor>> _actorMap;


        protected override void InitializeSingleton()
        {
            _actorMap = new Dictionary<Type, Dictionary<string, BaseActor>>();
        }

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


            GameObject newActorGO = new GameObject(uid);
            T newActor = newActorGO.AddComponent<T>();
            newActor.Initialize();
            dicToUse.Add(uid, newActor);

            return newActor;
        }


        private Dictionary<string, BaseActor> GetActorDictionary(System.Type type)
        {
            if (_actorMap.ContainsKey(type))
                return _actorMap[type];

            return null;
        }

        public BaseActor GetActor<T>(string uid)
        {
            Dictionary<string, BaseActor> dic = GetActorDictionary(typeof(T));
            if (dic.ContainsKey(uid))
                return dic[uid];

            return null;
        }

        public int GetActorCount<T>()
        {
            Dictionary<string, BaseActor> dic = GetActorDictionary(typeof(T));
            if(dic != null)
                return dic.Count;

            return -1;
        }


        public void DestroyActor(ref BaseActor toRelease)
        {

            System.Type type = toRelease.GetType();

            Dictionary<string, BaseActor> dic = GetActorDictionary(type);
            if (dic.ContainsKey(toRelease.ActorUID))
                dic.Remove(toRelease.ActorUID);

            toRelease.DestructionRoutine();
        }

        public void UpdateActorManager()
        {
            foreach (KeyValuePair<System.Type, Dictionary<string, BaseActor>> pairOuter in _actorMap)
            {
                foreach (KeyValuePair<string, BaseActor> pairInner in pairOuter.Value)
                {
                    pairInner.Value.UpdateActor();
                }
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
