using System;
using System.Collections.Generic;
using UnityEngine;
public class HeroBaseModel: BaseModel
{
    protected float mAttackDistance = 4f;//攻击距离
    protected float mAlertDistance = 10;//可发现敌人距离
    protected string mCamp = "";//阵营，属于魏蜀吴哪一国家
    protected GameObject mGameObject = null;
    protected string mType = "";//类型
    protected int mLevel = 1;//英雄等级
    protected int mCurrExp = 0;//英雄当前经验
    //技能
    //装备
    protected HeroAttrConfig mConfig = null;//配置文件
    protected Dictionary<string, float> mChangedDatas = null;//

    protected HeroBaseModel()
    {
        initConfig();
        mChangedDatas = new Dictionary<string, float>();
        mChangedDatas[Constants.HERO_ATTR_HEALTH] = get(Constants.HERO_ATTR_HEALTH_MAX);
        mChangedDatas[Constants.HERO_ATTR_MAGIC] = get(Constants.HERO_ATTR_MAGIC_MAX);
    }

    public float AttackDistance
    {
        get { return mAttackDistance; }
        set { mAttackDistance = value; }
    }

    public float AlertDistance
    {
        get { return mAlertDistance; }
        set { mAlertDistance = value; }
    }

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

    public String Camp
    {
        get { return mCamp; }
        set { mCamp = value; }
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

    public bool isInSameCamp(HeroBaseModel model)
    {
        return Camp.Equals(model.Camp);
    }

    public float get(string type)
    {
        float changedValue = 0;
        if (mChangedDatas.ContainsKey(type))
        {
            changedValue = mChangedDatas[type];
        }
        return this.mConfig.get(mLevel, type) + changedValue;
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
