using System;
using System.Collections.Generic;
using UnityEngine;
class BaseState
{
    StateManager mStateManager;
    public virtual StateManager StateManager
    {
        get { return mStateManager; }
        set {
            mGameObject = value.gameObject;
            mStateManager = value;
        }
    }

    GameObject mGameObject;
    protected GameObject GameObject
    {
        get { return mGameObject; }
    }

    public virtual void onEnter()
    {
        throw new NotImplementedException("BaseState.onEnter()");
    }

    public virtual void onUpdate()
    {
        throw new NotImplementedException("BaseState.onUpdate()");
    }

    public virtual string switchToNextState()
    {
        throw new NotImplementedException("BaseState.switchToNextState()");
    }

    public virtual void onExit()
    {
        throw new NotImplementedException("BaseState.onExit()");
    }

    protected void setExtraData(string name, object obj)
    {
        mStateManager.setExtraData(name, obj);
    }

    protected object getExtraData(string name)
    {
        return mStateManager.getExtraData(name);
    }

    protected bool hasExtraData(string key)
    {
        return mStateManager.hasExtraData(key);
    }

    protected void cleanExtraData()
    {
        mStateManager.cleanExtraData();
    }
}
