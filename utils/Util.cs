using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public delegate void DelayCallBack(System.Object data);
public struct Delay
{
    public DelayCallBack callback;
    public System.Object data;
    public Delay(DelayCallBack cb, System.Object d)
    {
        callback = cb;
        data = d;
    }
}
public class Util : BaseObject
{
    private Dictionary<float, ArrayList> mDelayCallByTimeCallbacks;

    protected override void Awake()
    {
        base.Awake();
        mDelayCallByTimeCallbacks = new Dictionary<float, ArrayList>();
    }
    protected override void Update()
    {
        base.Update();
        //计算delayCallByTime
        ArrayList needDeleteKey = new ArrayList();
        foreach (float t in mDelayCallByTimeCallbacks.Keys)
        {
            if (Time.time >= t)
            {
                ArrayList callList = mDelayCallByTimeCallbacks[t];
                foreach (Delay cb in callList)
                {
                    cb.callback(cb.data);
                }
                needDeleteKey.Add(t);
            }
        }
        //Debug.Log("Before needDeleteKey in Utils update for delayCall");

        foreach (float t in needDeleteKey)
        {
            mDelayCallByTimeCallbacks.Remove(t);
        }
    }

    //延迟调用
    public void delayCallByTime(float sec, DelayCallBack callback, System.Object data)
    {
        float targetTime = Time.time + sec;
        ArrayList cbList = null;
        if (!mDelayCallByTimeCallbacks.ContainsKey(targetTime))
        {
            cbList = new ArrayList();
            mDelayCallByTimeCallbacks[targetTime] = cbList;
        }
        else
        {
            cbList = mDelayCallByTimeCallbacks[targetTime];
        }
        cbList.Add(new Delay(callback, data));
    }

    //斐波那契
    public int fibonaqi(int start, int n)
    {
        if(n < 1)
        {
            throw new ArgumentException("Util.fibonaqi is error n must more than 0");
        }
        else if(n == 1)
        {
            return start;
        }
        else if(n == 2)
        {
            return start * 2;
        }
        else
        {
            return fibonaqi(start, n - 1) + fibonaqi(start, n - 2);
        }
    }

    //当前选择的英雄
    string mSelectedHeroName;
    public string SelectedHeroName
    {
        get
        {
            return mSelectedHeroName;
        }

        set
        {
            mSelectedHeroName = value;
        }
    }

    //设置当前是否是主机
    bool mIsHost;
    public bool IsHost
    {
        get
        {
            return mIsHost;
        }
        set
        {
            mIsHost = value;
        }
    }
}

class Singleton<T> where T: new()
{
    static T st;
    public static T getInstance()
    {
        if(st == null)
        {
            st = new T();
        }
        return st;
    }
}