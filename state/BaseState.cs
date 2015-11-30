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
    }

    public virtual void onUpdate()
    {
    }

    public virtual string switchToNextState()
    {
        return null;
    }

    public virtual void onExit()
    {
    }

    public virtual void afterExit()
    {
        this.unmapAllEvents();
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

    //utils
    protected void mapEvent(string evt, EventCallback callback)
    {
        GlobalObject.EventDispatcher.mapEvent(evt, this, callback);
    }

    protected void unmapAllEvents()
    {
        GlobalObject.EventDispatcher.unmapAllEvents(this);
    }

    protected void dispatch(string evt, object data)
    {
        GlobalObject.EventDispatcher.dispatchEvent(evt, data);
    }
}
