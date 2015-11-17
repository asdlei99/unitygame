using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
class HeroSync: BaseObject
{
    public static void init() {}
    static HeroSync()
    {
        Debug.Log("HeroSync");
        GlobalObject.addNewGlobalComponent<HeroSync>();
    }

    Dictionary<string, GameObject> SyncDict = new Dictionary<string, GameObject>();
    int mTimesPerSec = 30;

    protected override void Start()
    {
        StartCoroutine("checkSyncCoroutine");
    }

    IEnumerator checkSyncCoroutine()
    {
        while (this.enabled)
        {
            checkSync();
            yield return new WaitForSeconds(1.0f / mTimesPerSec);
        }
    }

    //检查我自己的动作
    void checkSync()
    {
        foreach(var pair in SyncDict)
        {
            string name = pair.Key;
            GameObject gameObject = pair.Value;
            //TODO:
            //检查位置
            //检查身上绑定的效果
            //检查当前动画
        }
    }

    public void registerSync(string name, GameObject gameObject)
    {
        Debug.Log(" in registerSync gameObject = " + gameObject + ", name= "+ name);
        SyncDict[name] = gameObject;
    }

    public void removeSync(string name)
    {
        SyncDict.Remove(name);
    }
    
    public void setCheckInterval(int timesPerSec)
    {
        mTimesPerSec = timesPerSec;
    }
}
