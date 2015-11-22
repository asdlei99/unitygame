using System;
using System.Collections.Generic;
using UnityEngine;

class DelayState: BaseState
{
    string mNextState;
    float mDelay;
    float mTime;
    public override void onEnter()
    {
        mNextState = (string)getExtraData("nextState");
        mDelay = Time.time + (float)getExtraData("delay");
        mTime = Time.time;
        Debug.Log("wanghy 1 -- mTime=" + mTime + ", mDelay = " + mDelay);
    }

    public override void onUpdate()
    {
        mTime += Time.deltaTime;
        Debug.Log("wanghy 2 -- mTime=" + mTime + ", mDelay = " + mDelay);
    }

    public override string switchToNextState()
    {
        if (mTime >= mDelay)
        {
            Debug.Log("wanghy 3 -- mTime=" + mTime + ", mDelay = " + mDelay + ",mNextState=" + mNextState);
            return mNextState;
        }
        return null;
    }

    public override void onExit()
    {
        mNextState = null;
        mDelay = -1;
        mTime = -1;
    }
}