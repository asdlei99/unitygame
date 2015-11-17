using System;
using System.Collections.Generic;
using UnityEngine;
class HeroBaseModel: BaseModel
{
    protected GameObject mGameObject;
    protected string mType;//类型
    protected int mLevel;//英雄等级
    protected int mCurrExp;//英雄当前经验
    //技能
    //装备
    protected HeroAttrConfig mConfig;//配置文件
    protected Dictionary<string, float> mChangedDatas;//
    public GameObject GameObject
    {
        get
        {
            return mGameObject;
        }
        set
        {
            mGameObject = value;
        }
    } 
    public string Type
    {
        get
        {
            return mType;
        }
        set
        {
            mType = value;
        }
    }

    public HeroAttrConfig Config
    {
        get
        {
            return mConfig;
        }
        set
        {
            mConfig = value;
        }
    }

    public int CurrExp
    {
        get
        {
            return mCurrExp;
        }
        set
        {
            mCurrExp = value;
        }
    }

    public int Level
    {
        get
        {
            return mLevel;
        }
        set
        {
            mLevel = value;
        }
    }

    public void growExp(int exp)
    {
        this.mCurrExp += exp;
        int needExp = (int)this.mConfig.get(mLevel, Constants.HERO_ATTR_NEED_EXP);
        if (this.mCurrExp > needExp) //升级
        {
            this.mCurrExp -= needExp;
            mLevel++;
            GlobalObject.EventDispatcher.dispatchEvent(Events.EVT_HERO_UPGRADE_LEVEL, mType);
            GlobalObject.EventDispatcher.dispatchEvent(Events.EVT_HERO_ATTR_CHANGED, mType);
        }
    }

    public float get(string type)
    {
        return this.mConfig.get(mLevel, type) + mChangedDatas[type];
    }

    //增加/减少数值
    public void add(string type, float value)
    {
        if (!mChangedDatas.ContainsKey(type))
        {
            mChangedDatas[type] = 0;
        }
        mChangedDatas[type] += value;
        GlobalObject.EventDispatcher.dispatchEvent(Events.EVT_HERO_ATTR_CHANGED, mType);
    }

    protected virtual void initConfig()
    {
    }

    protected HeroBaseModel()
    {
        initConfig();
        mChangedDatas = new Dictionary<string, float>();
    }

    protected virtual HeroBaseModel newInstance()
    {
        return null;
    }

    public HeroBaseModel clone()
    {
        HeroBaseModel model = newInstance();
        model.Config = new HeroBossConfig();
        model.Type = mType;
        model.Level = mLevel;
        model.CurrExp = mCurrExp;
        return model;
    }
}
