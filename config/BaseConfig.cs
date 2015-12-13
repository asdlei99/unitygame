using System;
using System.Collections.Generic;
using UnityEngine;
class BaseConfig
{
    //一些基础值
    public const int HERO_MAX_LEVEL = 100;//最大等级
    public const int HERO_ATTACK_SPEED_MAX = 10;//10次/s
    public const int HERO_RUN_SPEED_MAX = 10;//10m/s
    public const int HERO_GROUP_ATTACK_ASSUME_COUNT = 5;//假定群攻平均攻击5个单位。

    //是否命中
    public static bool isHit(float hit, float dodage)
    {
        return isTrigger(hit, dodage);
    }

    //是否暴击
    public static bool isCrit(float crit, float decrit)
    {
        return isTrigger(crit, decrit);
    }

    //是否眩晕
    public static bool isVertigo(float vertigo, float immunity)
    {
        return isTrigger(vertigo, immunity);
    }

    //是否冰冻
    public static bool isFreeze(float freeze, float immunity)
    {
        return isTrigger(freeze, immunity);
    }

    //是否中毒
    public static bool isPoison(float poison, float immunity)
    {
        return isTrigger(poison, immunity);
    }

    //正反两方触发事件的概率，均为0-100的值
    public static bool isTrigger(float value, float devalue)
    {
        return new System.Random((int)Time.time).Next() % 100 <= (value - devalue) / value * 100;
    }


    //物理伤害公式 减法
    public static int damage(float attack, float mattack, float defense, float mdefense)
    {
        return (int)((attack * 1.5 + mattack * 0.5) - (defense * 1.5 + mdefense * 0.5));
    }

    //魔法伤害公式 减法
    public static int mdamage(float attack, float mattack, float defense, float mdefense)
    {
        return (int)((mattack * 1.5 + attack * 0.5) - (mdefense * 1.5 + defense * 0.5));
    }

    //物理群攻：假设群攻一般会同时攻击到5个人。那么每个人承受的攻击为普通攻击的0.2倍
    public static int damageGroups(float attack, float mattack, float defense, float mdefense, float distance, float maxDistance)
    {
        return damage(attack, mattack, defense, mdefense) / 5 * (int)(1.0 * distance / maxDistance);
    }

    //魔法群攻
    public static int mdamageGroups(float attack, float mattack, float defense, float mdefense, float distance, float maxDistance)
    {
        return mdamage(attack, mattack, defense, mdefense) / 5 * (int)(1.0 * distance / maxDistance);
    }
}
