using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//英雄属性配置
class HeroChangbiziConfig : HeroAttrConfig
{
    protected override void initAttrs()
    {
        attackStart = 10;//攻击初始值
        defenseStart = 5;//防御初始值
        mattackStart = 5;//法攻初始值
        mdefenseStart = 3;//法防初始值
        magicStart = 10;//蓝
        healthStart = 20;//红
        attackspeedStart = 1;//1s 1次
        runspeedStart = 3;//奔跑速度 5
        dropexpStart = 10;
        needexpStart = 10;

        //下面的值取值范围0-5
        attackOff = 2;
        defenseOff = 1;
        mattackOff = 1;
        mdefenseOff = 1;
        magicOff = 1;
        healthOff = 3;
        attackspeedOff = 0.5f;//取值 0-1
        runspeedOff = 0.8f;//取值 0-1
        attrValueFactor = 1;//取值因子
        dropexpOff = 5;
        needexpOff = 10;
    }
}
