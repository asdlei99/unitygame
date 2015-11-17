using UnityEngine;
using System.Collections;

public class BaseObject : MonoBehaviour {
    void log(string msg)
    {
        //Debug.Log(msg);
    }

    protected EventDispatcher getEventDispatcher()
    {
        return GlobalObject.EventDispatcher;
    }

    protected Util getUtil()
    {
        return GlobalObject.Util;
    }

    protected void mapEvent(string evt, EventCallback callback)
    {
        getEventDispatcher().mapEvent(evt, this, callback);
    }

    protected void dispatch(string evt, object data)
    {
        getEventDispatcher().dispatchEvent(evt, data);
    }
    protected void dispatch(string evt)
    {
        getEventDispatcher().dispatchEvent(evt);
    }

    protected void delayCall(float sec, DelayCallBack cb, object data)
    {
        getUtil().delayCallByTime(sec, cb, data);
    }

    protected virtual void Awake()
    {
        log("Awake");
        GlobalObject.initGlobalObject();
    }

    protected virtual void OnEnable()
    {
        log("OnEnable");
    }

    // Use this for initialization
    protected virtual void Start ()
    {
        log("Start");
    }

    // Update is called once per frame
    protected virtual void Update ()
    {
        log("Update");
    }

    protected virtual void LateUpdate()
    {
        log("LateUpdate");
    }

    protected virtual void FixedUpdate()
    {
        log("FixedUpdate");
    }

    protected virtual void OnGUI()
    {
        log("OnGUI");
    }

    protected virtual void OnDisable()
    {
        log("OnDisable");
        getEventDispatcher().unmapAllEvents(this);
    }
}
