using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

delegate bool delegateCond();
class StateCondition
{
    public string key;
    public string state;
    public int priority;
    public delegateCond cond;
};

class StateManager: BaseObject
{
    Dictionary<string, BaseState> mStatesDict = new Dictionary<string, BaseState>();
    Dictionary<string, object> mExtraData = new Dictionary<string, object>();//用于不同状态之间传递数据
    ArrayList mCondArr = new ArrayList();

    BaseState mCurrState;
    string mDefaultStateName;
    public BaseState CurrState
    {
        get { return mCurrState; }
        set { mCurrState = value; }
    }
    protected override void Start()
    {
        addState(new DelayState());
    }

    //为某些状态设置条件，如果条件达到必定跳入
    public void setCondForState(string state, int priority, delegateCond cond)
    {
        checkStateName(state);
        mCondArr.Add(new StateCondition() { state = state, priority = priority, cond = cond });
    }

    bool findAndExecuteCondState()
    {
        StateCondition maxPriorityCond = null;
        int currPriority = -1;
        foreach(StateCondition cond in mCondArr)
        {
            if(cond.cond() && cond.priority > currPriority && !mCurrState.GetType().Name.Equals(cond.state))
            {
                currPriority = cond.priority;
                maxPriorityCond = cond;
            }
        }
        if(maxPriorityCond != null)
        {
            switchState(maxPriorityCond.state);
            return true;
        }
        return false;
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
        mDefaultStateName = defaultState;
        switchToDefaultState();
    }

    public void switchToDefaultState()
    {
        switchState(mDefaultStateName);
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
            mCurrState.afterExit();
        }
        mCurrState = mStatesDict[newStateName];
        mCurrState.onEnter();
        cleanExtraData();
    }

    void trySwitchState()
    {
        if (!findAndExecuteCondState())
        {
            string nextStateName = mCurrState.switchToNextState();
            if (nextStateName != null)
            {
                switchState(nextStateName);
            }
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
