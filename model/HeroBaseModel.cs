using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttrChange
{
    public string which;
    public string what;
    public float from;
    public float to;
}

public class HeroStatus
{
    public bool isActive;
    public float time;
    public float startTime;
    public object value;
}

public class HeroBaseModel: BaseModel
{
    protected float mAttackDistance = 4f;//攻击距离
    protected float mAlertDistance = 10;//可发现敌人距离
    protected string mCamp = "";//阵营，属于魏蜀吴哪一国家
    protected GameObject mGameObject = null;
    protected string mType = "";//英雄的名字作为此model唯一标记
    protected int mLevel = 1;//英雄等级
    protected int mCurrExp = 0;//英雄当前经验

    protected Dictionary<string, HeroStatus> mStatus;//英雄当前的状态，中毒，冰冻

    //位置
    protected Vector3 mHeroInitPosition = new Vector3(82.4f, 20.00751f, 97.5f);

    //技能
    //装备
    protected HeroAttrConfig mConfig = null;//配置文件
    protected Dictionary<string, float> mChangedDatas = null;//

    protected HeroBaseModel()
    {
        initConfig();
        mChangedDatas = new Dictionary<string, float>();
        mChangedDatas[Constants.HERO_ATTR_HEALTH] = get(Constants.HERO_ATTR_HEALTH_MAX);//初始化血和蓝
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

    public Vector3 InitPosition
    {
        get {
            return mHeroInitPosition;
        }
        set {
            mHeroInitPosition = value;
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
        }
    }

    public void enableStatus(string key, HeroStatus status)
    {
        if (!mStatus.ContainsKey(key))
        {
            mStatus[key] = status;
        }
        else
        {
            mStatus[key].time += status.time;
        }
    }

    public void closeStatus(string key)
    {
        mStatus.Remove(key);
    }
    public HeroStatus getStatus(string key)
    {
        if (mStatus.ContainsKey(key))
        {
            return mStatus[key];
        }
        return null;
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
    public virtual void add(string type, float value)
    {
        if (!mChangedDatas.ContainsKey(type))
        {
            mChangedDatas[type] = 0;
        }
        mChangedDatas[type] += value;
        if (value != 0)
        {
            GlobalObject.EventDispatcher.dispatchEvent(Events.EVT_HERO_ATTR_CHANGED, new HeroAttrChange() {which = mType, what = type, from = value, to = get(type) });
        }
    }

    //普通攻击
    public void attack(HeroBaseModel model)
    {
        bool isCrit = false;
        if(!BaseConfig.isHit(get(Constants.HERO_ATTR_HIT), get(Constants.HERO_ATTR_DODGE)))
        {
            Debug.Log("attack error! is not hit!!");
            return;
        }
        else
        {
            if(BaseConfig.isCrit(get(Constants.HERO_ATTR_CRIT), get(Constants.HERO_ATTR_DECRIT)))//暴击
            {
                Debug.Log("attack status is crit!!");
                GlobalObject.EventDispatcher.dispatchEvent(Events.EVT_HERO_CRIT);
                isCrit = true;
            }
            else if (BaseConfig.isFreeze(get(Constants.HERO_ATTR_FREEZE), get(Constants.HERO_ATTR_IMMUNITY)))//冰冻
            {
                Debug.Log("attack status is freeze!!");
                model.enableStatus(Constants.HERO_STATUS_FREEZE, new HeroStatus() {startTime=Time.time, isActive=true, time = 3 });
            }
            else if(BaseConfig.isVertigo(get(Constants.HERO_ATTR_VERTIGO), get(Constants.HERO_ATTR_IMMUNITY)))//眩晕
            {
                Debug.Log("attack status is vertigo!!");
                model.enableStatus(Constants.HERO_STATUS_FREEZE, new HeroStatus() { startTime = Time.time, isActive = true, time = 3 });
            }
            else if (BaseConfig.isPoison(get(Constants.HERO_ATTR_POISON), get(Constants.HERO_ATTR_IMMUNITY)))//中毒
            {
                Debug.Log("attack status is poison!!");
                model.enableStatus(Constants.HERO_STATUS_FREEZE, new HeroStatus() { startTime = Time.time, isActive = true, time = 3, value=300 });
            }
        }

        float attackValue = get(Constants.HERO_ATTR_ATTACK);
        float mattackValue = get(Constants.HERO_ATTR_MATTACK);
        float defenseValue = model.get(Constants.HERO_ATTR_DEFENSE);
        float mdefenseValue = model.get(Constants.HERO_ATTR_MDEFENSE);
        float damage = 0;
        if (attackValue < mattackValue)
        {
            damage = BaseConfig.damage(attackValue, mattackValue, defenseValue, mdefenseValue);
        }
        else
        {
            damage = BaseConfig.mdamage(attackValue, mattackValue, defenseValue, mdefenseValue);
        }

        if (isCrit)
        {
            damage *= 2;
        }

        //掉血
        model.add(Constants.HERO_ATTR_HEALTH, -damage);
    }
    //群攻
    public void attackGroup(HeroBaseModel model)
    {

    }

    public bool isDead()
    {
        return get(Constants.HERO_ATTR_HEALTH) <= 0;
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
