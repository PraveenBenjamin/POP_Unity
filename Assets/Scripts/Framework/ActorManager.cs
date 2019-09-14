using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Linq;

namespace POP.Framework
{
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

            T newActor = PrefabDatabase.Instance.GetPrefabInstance(typeof(T).ToString()).GetComponent<T>();
            newActor.gameObject.name = uid;
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


        public void DestroyActor(BaseActor toRelease)
        {

            System.Type type = toRelease.GetType();

            Dictionary<string, BaseActor> dic = GetActorDictionary(type);
            if (dic.ContainsKey(toRelease.ActorUID))
            {
                toDestroy.Add(dic[toRelease.ActorUID]);
                _isDirty = true;
            }
            toRelease.DestructionRoutine();
        }

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
