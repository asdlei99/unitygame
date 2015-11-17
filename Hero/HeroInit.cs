﻿using System;
using System.Collections.Generic;
using UnityEngine;
class HeroInit: BaseObject
{
    HeroAnimController mAnimCtl;
    HeroCamera mCamera;
    protected override void Start()
    {
        mAnimCtl = this.gameObject.AddComponent<HeroAnimController>();
        mCamera = this.gameObject.AddComponent<HeroCamera>();
        mapEvents();
        bindModel();

        //默认选人
        delayCall(1, delegate (object d)
        {
            dispatch(Events.EVENT_SELECT_HERO, Constants.HERO_NAME_JINGCHA);
        }, null);
    }

    //事件监听
    void mapEvents()
    {
        mapEvent(Events.EVENT_SELECT_HERO, onEvent);
    }

    private void onEvent(string evt, object data)
    {
        switch (evt)
        {
            case Events.EVENT_INPUT_JUMP:
                mAnimCtl.doJump();
                break;
            case Events.EVENT_INPUT_SCREEN_CLICK:
                mAnimCtl.doRun((Vector3)data);
                break;
            case Events.EVENT_SELECT_HERO:
                string selectName = (string)data;
                Debug.Log("select hero selectName = " + selectName);
                this.getUtil().SelectedHeroName = selectName;
                if (selectName.Equals(this.gameObject.name))
                {
                    mapEvent(Events.EVENT_INPUT_JUMP, onEvent);
                    mapEvent(Events.EVENT_INPUT_SCREEN_CLICK, onEvent);
                    //让gameObject检查同步
                    HeroSync.init();
                    GlobalObject.getComponent<HeroSync>().registerSync(gameObject.name, gameObject);
                }
                Debug.Log("before onEvent break");
                break;
            default:
                break;
        }
    }

    //绑定GameObject
    void bindModel()
    {
        HeroModelFactory.getHeroModel(this.gameObject.name).GameObject = this.gameObject;
    }
}