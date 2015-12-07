using System;
using System.Collections.Generic;
using UnityEngine;
class HeroDeathState : HeroState
{
    bool fadeout = false;
    bool waitRebirth = false;
    float rebirthCount = 0;
    bool rebirth = false;
    public override void onEnter()
    {
        AnimCtl.setAnimValue("isDead", true);
    }

    public override void onUpdate()
    {
        if (waitRebirth)//第三步等待重生
        {
            //移动到出生点
        }
        else if (fadeout)//第二步淡出
        {
            //TODO
        }
        else//第一步 dead 动画
        {
            //检查dead动画
            AnimatorStateInfo info = AnimCtl.Anim.GetCurrentAnimatorStateInfo(0);
            if (info.normalizedTime >= 1)//动画结束
            {
                //
            }
        }
    }

    public override string switchToNextState()
    {
        if (rebirth)
        {
            return "HeroIdleState";
        }
        return null;
    }


    public override void onExit()
    {
        rebirth = false;
        fadeout = false;
        waitRebirth = false;
        rebirthCount = 0;
        rebirth = false;
        AnimCtl.setAnimValue("isDead", false);
    }
}
