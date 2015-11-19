using UnityEngine;
using System.Collections;

public class Constants
{
    //空组件名
    public const string GLOBAL_EMPTY_OBJECT = "GlobalEmptyObject";

    //HeroModel
    public const string HERO_NAME_BOSS = "Boss";
    public const string HERO_NAME_JINGCHA = "Jingcha";
    public const string HERO_NAME_CHANGBIZI = "Changbizi";

    //个人属性
    //下面的属性是个人属性，装备也可以带
    public const string HERO_ATTR_ATTACK = "attack";//攻击
    public const string HERO_ATTR_DEFENSE = "defense";//防御
    public const string HERO_ATTR_MATTACK = "mattack";//魔法攻击
    public const string HERO_ATTR_MDEFENSE = "mdefense";//魔法防御
    public const string HERO_ATTR_ATTACKSPEED = "attackspeed";//攻击速度
    public const string HERO_ATTR_RUNSPEED = "runspeed";//奔跑速度
    public const string HERO_ATTR_HEALTH_MAX = "healthmax";//血量管长度
    public const string HERO_ATTR_MAGIC_MAX = "magicmax";//魔法管长度
    public const string HERO_ATTR_DROP_EXP = "dropexp";
    public const string HERO_ATTR_NEED_EXP = "neepexp";
    //变化的量
    public const string HERO_ATTR_HEALTH = "health";//血量
    public const string HERO_ATTR_MAGIC = "magic";//魔法

    //下面的都是装备和技能带的属性
    public const string HERO_ATTR_CRIT = "crit";//暴击
    public const string HERO_ATTR_DECRIT = "decrit";//防暴 = ~暴击
    public const string HERO_ATTR_HIT = "hit";//命中
    public const string HERO_ATTR_DODGE = "dodge";//闪避 = ~命中

    public const string HERO_ATTR_VERTIGO = "vertigo";//眩晕
    public const string HERO_ATTR_FREEZE = "freeze";//冰冻
    public const string HERO_ATTR_POISON = "poison";//中毒
    public const string HERO_ATTR_IMMUNITY = "immunity";//免疫力 = ~眩晕 | ~冰冻 | ~中毒 | ~反弹

    public const string HERO_ATTR_RALLY = "rally";//反弹：没有相对变量
    public const string HERO_ATTR_BEATTACK_ATTR_ADD = "beattackheroattradd";//被攻击属性增加
    public const string HERO_ATTR_BEATTACK_ENEMY_ATTR_ADD = "beattackenemyattradd";//被攻击敌人属性增加


    //人物移动正常速度
    public const float HERO_RUN_NORMAL_SPEED = 5;
}