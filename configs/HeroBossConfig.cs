using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//英雄属性配置
class HeroBossConfig: HeroAttrConfig
{
    protected override void initAttrs()
    {
        attackStart = 30;//攻击初始值
        defenseStart = 5;//防御初始值
        mattackStart = 0;//法攻初始值
        mdefenseStart = 0;//法防初始值
        magicStart = 50;//蓝
        healthStart = 50;//红
        attackspeedStart = 1;//1s 1次
        runspeedStart = 2;//奔跑速度 5
        dropexpStart = 100;
        needexpStart = 100;

        //下面的值取值范围0-5
        attackOff = 5;
        defenseOff = 2;
        mattackOff = 1;
        mdefenseOff = 1;
        magicOff = 1;
        healthOff = 9;
        attackspeedOff = 0f;//取值 0-1
        runspeedOff = 0.5f;//取值 0-1
        dropexpOff = 10;
        needexpOff = 10;
        attrValueFactor = 1;//取值因子
    }
}
