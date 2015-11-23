using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//英雄属性配置
public class HeroAttrConfig
{
    protected float attackStart = 20;
    protected float defenseStart = 10;
    protected float mattackStart = 20;
    protected float mdefenseStart = 10;
    protected float magicStart = 30;
    protected float healthStart = 30;
    protected float attackspeedStart = 1;// 次/s
    protected float runspeedStart = 5;//奔跑速度 5
    protected float dropexpStart = 100;
    protected float needexpStart = 100;

    //下面的值取值范围0-5
    protected float attackOff = 4;
    protected float defenseOff = 3;
    protected float mattackOff = 1;
    protected float mdefenseOff = 1;
    protected float magicOff = 2;
    protected float healthOff = 5;
    protected float attackspeedOff = 0;//单位 次/s
    protected float runspeedOff = 0;
    protected float dropexpOff = 10;
    protected float needexpOff = 10;
    protected float attrValueFactor = 1;
    //人物属性包括不同等级，不同
    protected Dictionary<int, Dictionary<string, float>> mConfig;
    public Dictionary<int, Dictionary<string, float>> config
    {
        get { return mConfig; }
        set { throw new Exception("setConfig is error"); }
    }

    public float get(int level, string attrname)
    {
        var levelDict = get(level);
        if (levelDict.ContainsKey(attrname))
        {
            return levelDict[attrname];
        }
        else
        {
            return 0;
        }
    }

    Dictionary<string, float> get(int level)
    {
        return mConfig[level];
    }

    private int calcValueGrowInt(int x, float offset, float factor)
    {
        //y = factor * x ^ 3 + start
        return (int)(Math.Pow((int)(x * offset / 2.0), 2) * factor);
    }
    private float calcValueGrowFloat(int x, float offset, float factor)
    {
        //y = factor * x ^ 3 + start
        return (float)Math.Pow((int)(x * offset / 2.0), 2) * factor;
    }

    public HeroAttrConfig()
    {
        mConfig = new Dictionary<int, Dictionary<string, float>>();
        initAttrs();

        //最大等级100级
        for (int i = 0; i < BaseConfig.HERO_MAX_LEVEL; i++)
        {
            int level = i + 1;
            Dictionary<string, float> levelDict = new Dictionary<string, float>();
            levelDict[Constants.HERO_ATTR_ATTACK] = attackStart + calcValueGrowInt(level, attackOff, attrValueFactor);
            levelDict[Constants.HERO_ATTR_DEFENSE] = defenseStart + calcValueGrowInt(level, defenseOff, attrValueFactor);
            levelDict[Constants.HERO_ATTR_MATTACK] = mattackStart + calcValueGrowInt(level, mattackOff, attrValueFactor);
            levelDict[Constants.HERO_ATTR_MDEFENSE] = mdefenseStart + calcValueGrowInt(level, mdefenseOff, attrValueFactor);
            levelDict[Constants.HERO_ATTR_HEALTH_MAX] = healthStart + calcValueGrowInt(level, healthOff, attrValueFactor);
            levelDict[Constants.HERO_ATTR_MAGIC_MAX] = magicStart + calcValueGrowInt(level, magicOff, attrValueFactor);
            levelDict[Constants.HERO_ATTR_DROP_EXP] = dropexpStart + calcValueGrowInt(level, dropexpOff, attrValueFactor);
            levelDict[Constants.HERO_ATTR_NEED_EXP] = needexpStart + calcValueGrowInt(level, needexpOff, attrValueFactor);
            levelDict[Constants.HERO_ATTR_ATTACKSPEED] = attackspeedStart + calcValueGrowFloat(level, attackspeedOff, attrValueFactor) / 1000;
            levelDict[Constants.HERO_ATTR_RUNSPEED] = runspeedStart + calcValueGrowFloat(level, runspeedOff, attrValueFactor) / 1000;
            mConfig[level] = levelDict;
        }
    }
    
    public override string ToString()
    {
        string s = "";
        s = s + "基础属性：\n";
        s = s + "\t attackStart=" + attackStart + "\n";
        s = s + "\t defenseStart=" + defenseStart + "\n";
        s = s + "\t mattackStart=" + mattackStart + "\n";
        s = s + "\t mdefenseStart=" + mdefenseStart + "\n";
        s = s + "\t healthStart=" + healthStart + "\n";
        s = s + "\t magicStart=" + magicStart + "\n";
        s = s + "\t attackspeedStart=" + attackspeedStart + "\n";
        s = s + "\t runspeedStart=" + runspeedStart + "\n";
        s = s + "\t dropexpStart=" + dropexpStart + "\n";
        s = s + "\t needexpStart=" + needexpStart + "\n";
        s = s + "-------\n";
        s = s + "\t attackOffset=" + attackOff + "\n";
        s = s + "\t defenseOffset=" + defenseOff + "\n";
        s = s + "\t mattackOffset=" + mattackOff + "\n";
        s = s + "\t mdefenseOffset=" + mdefenseOff + "\n";
        s = s + "\t healthOffset=" + healthOff + "\n";
        s = s + "\t magicOffset=" + magicOff + "\n";
        s = s + "\t attackspeedOffset=" + attackspeedOff + "\n";
        s = s + "\t runspeedOffset=" + runspeedOff + "\n";
        s = s + "\t dropexpOff=" + dropexpOff + "\n";
        s = s + "\t needexpOff=" + needexpOff + "\n";
        s = s + "\t attrValueFactor=" + attrValueFactor + "\n";
        s = s + "分级属性：";
        s = s + "\t等级\t属性名\t值\n";
        for(int i = 0; i < BaseConfig.HERO_MAX_LEVEL; i++)
        {
            int level = i + 1;
            Dictionary<string, float> levelDict = mConfig[level];
            s = s + "\t" + level + "\t" + Constants.HERO_ATTR_ATTACK + "\t" + levelDict[Constants.HERO_ATTR_ATTACK] + "\n";
            s = s + "\t" + level + "\t" + Constants.HERO_ATTR_DEFENSE + "\t" + levelDict[Constants.HERO_ATTR_DEFENSE] + "\n";
            s = s + "\t" + level + "\t" + Constants.HERO_ATTR_MATTACK + "\t" + levelDict[Constants.HERO_ATTR_MATTACK] + "\n";
            s = s + "\t" + level + "\t" + Constants.HERO_ATTR_MDEFENSE + "\t" + levelDict[Constants.HERO_ATTR_MDEFENSE] + "\n";
            s = s + "\t" + level + "\t" + Constants.HERO_ATTR_HEALTH_MAX + "\t" + levelDict[Constants.HERO_ATTR_HEALTH_MAX] + "\n";
            s = s + "\t" + level + "\t" + Constants.HERO_ATTR_MAGIC_MAX + "\t" + levelDict[Constants.HERO_ATTR_MAGIC_MAX] + "\n";
            s = s + "\t" + level + "\t" + Constants.HERO_ATTR_ATTACKSPEED + "" + levelDict[Constants.HERO_ATTR_ATTACKSPEED] + "\n";
            s = s + "\t" + level + "\t" + Constants.HERO_ATTR_RUNSPEED + "\t" + levelDict[Constants.HERO_ATTR_RUNSPEED] + "\n";
            s = s + "\t" + level + "\t" + Constants.HERO_ATTR_DROP_EXP + "\t" + levelDict[Constants.HERO_ATTR_DROP_EXP] + "\n";
            s = s + "\t" + level + "\t" + Constants.HERO_ATTR_NEED_EXP + "\t" + levelDict[Constants.HERO_ATTR_NEED_EXP] + "\n";
        }
        return s;
    }

    //物理伤害公式 减法
    public int damage(int attack, int mattack, int defense, int mdefense)
    {
        return (int)((attack * 1.5 + mattack * 0.5) - (defense * 1.5 + mdefense * 0.5));
    }

    //魔法伤害公式 减法
    public int mdamage(int attack, int mattack, int defense, int mdefense)
    {
        return (int)((mattack * 1.5 + attack * 0.5) - (mdefense * 1.5 + defense * 0.5));
    }

    //物理群攻：假设群攻一般会同时攻击到5个人。那么每个人承受的攻击为普通攻击的0.2倍
    public int damageGroups(int attack, int mattack, int defense, int mdefense, int distance, int maxDistance)
    {
        return damage(attack, mattack, defense, mdefense) / 5 * (int)(1.0 * distance / maxDistance);
    }

    //魔法群攻
    public int mdamageGroups(int attack, int mattack, int defense, int mdefense, int distance, int maxDistance)
    {
        return mdamage(attack, mattack, defense, mdefense) / 5 * (int)(1.0 * distance / maxDistance);
    }

    //是否命中
    public bool isHit(int hit, int dodage)
    {
        return isTrigger(hit, dodage);
    }

    //是否暴击
    public bool isCrit(int crit, int decrit)
    {
        return isTrigger(crit, decrit);
    }

    //是否眩晕
    public bool isVertigo(int vertigo, int immunity)
    {
        return isTrigger(vertigo, immunity);
    }

    //是否冰冻
    public bool isFreeze(int freeze, int immunity)
    {
        return isTrigger(freeze, immunity);
    }

    //是否中毒
    public bool isPoison(int poison, int immunity)
    {
        return isTrigger(poison, immunity);
    }

    //正反两方触发事件的概率，均为0-100的值
    public bool isTrigger(int value, int devalue)
    {
        return new System.Random((int)Time.time).Next() < value * (100 - devalue);
    }
    
    public float getDropExp(int level)
    {
        return get(level)[Constants.HERO_ATTR_NEED_EXP];
    }

    protected virtual void initAttrs()
    {
        attackStart = 20;
        defenseStart = 10;
        mattackStart = 20;
        mdefenseStart = 10;
        magicStart = 30;
        healthStart = 30;
        attackspeedStart = 1;//1s 1次
        runspeedStart = 5;//奔跑速度 5
        dropexpStart = 100;
        needexpStart = 100;

        //下面的值取值范围0-5
        attackOff = 4;
        defenseOff = 3;
        mattackOff = 1;
        mdefenseOff = 1;
        magicOff = 2;
        healthOff = 6;
        attackspeedOff = 0.5f;//0-1取值
        runspeedOff = 0.8f;//0-1取值
        dropexpOff = 10;
        needexpOff = 10;
        attrValueFactor = 1;
    }
}
