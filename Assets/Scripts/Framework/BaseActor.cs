using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseActor : RectTransform
{

    // i know i can create a way to initialize a readonly variable at runtime, but i dont intend to waste time doing the same
    //for now imma just gonna pretend like this is readonly and not change it after setting it
    private string _actorUID;

    public string ActorUID
    {
        get
        {
            return _actorUID;
        }
    }


    public void Initialize()
    {
        _actorUID = this.gameObject.name;
        CreationRoutine();
    }

    public abstract void CreationRoutine();
    public abstract void DestructionRoutine();

}
