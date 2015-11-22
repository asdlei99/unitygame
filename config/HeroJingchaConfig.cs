using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//英雄属性配置
class HeroJingchaConfig: HeroAttrConfig
{
    protected override void initAttrs()
    {
        attackStart = 20;//攻击初始值
        defenseStart = 10;//防御初始值
        mattackStart = 20;//法攻初始值
        mdefenseStart = 10;//法防初始值
        magicStart = 30;//蓝
        healthStart = 30;//红
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
        healthOff = 5;
        attackspeedOff = 0.5f;//取值 0-1
        runspeedOff = 0.8f;//取值 0-1
        attrValueFactor = 1;//取值因子
        dropexpOff = 10;
        needexpOff = 100;
    }
}
