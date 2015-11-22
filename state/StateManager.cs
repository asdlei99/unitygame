using System;
using System.Collections.Generic;
using UnityEngine;
class StateManager: BaseObject
{
    Dictionary<string, BaseState> mStatesDict = new Dictionary<string, BaseState>();
    Dictionary<string, object> mExtraData = new Dictionary<string, object>();//用于不同状态之间传递数据
    BaseState mCurrState;
    public BaseState CurrState
    {
        get { return mCurrState; }
        set { mCurrState = value; }
    }
    protected override void Start()
    {
    }

    public void addState(BaseState state)
    {
        string name = state.GetType().Name;
        checkStateName(name, false);
        mStatesDict[name] = state;
        state.StateManager = this;
    }

    public void removeState(BaseState state)
    {
        string name = state.GetType().Name;
        checkStateName(name);
        mStatesDict.Remove(name);
    }

    public void setDefaultState(string defaultState)
    {
        checkStateName(defaultState);
        switchState(defaultState);
    }

    void checkStateName(string stateName, bool checkNotExist = true)
    {
        bool containsKey = mStatesDict.ContainsKey(stateName);
        if (checkNotExist ? !containsKey : containsKey)
        {
            throw new ArgumentException("stateName " + stateName + (checkNotExist ? " is not exists" : "is already exists"));
        }
    }

    void switchState(string newStateName)
    {
        checkStateName(newStateName);
        if (mCurrState != null)
        {
            mCurrState.onExit();
        }
        mCurrState = mStatesDict[newStateName];
        mCurrState.onEnter();
        cleanExtraData();
    }

    void trySwitchState()
    {
        string nextStateName = mCurrState.switchToNextState();
        if (nextStateName != null)
        {
            switchState(nextStateName);
        }
    }

    protected override void Update()
    {
        if (mCurrState != null)
        {
            mCurrState.onUpdate();
            trySwitchState();
        }
    }

    public void setExtraData(string key, object value)
    {
        mExtraData[key] = value;
    }

    public object getExtraData(string key)
    {
        return mExtraData[key];
    }

    public bool hasExtraData(string key)
    {
        return mExtraData.ContainsKey(key);
    }

    public void cleanExtraData()
    {
        mExtraData.Clear();
    }
}
