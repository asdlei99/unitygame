using System;
using System.Collections.Generic;
using UnityEngine;
class HeroChaseState : HeroState
{
    public override void onEnter()
    {
    }

    public override void onUpdate()
    {
        AnimCtl.updateChase();
    }

    public override string switchToNextState()
    {
        if (AnimCtl.isArrivedTargetForChase())//追上了，转攻击
        {
            return "HeroAttackState";
        }
        else if (AnimCtl.isOutofRangeForChase())//超范围，停止追逐，返回原地
        {
            return "HeroIdleState";
        }
        else if (AnimCtl.IsRunning)
        {
            return "HeroRunState";
        }
        else if (AnimCtl.IsJumping)
        {
            return "HeroJumpState";
        }
        return base.switchToNextState();
    }

    public override void onExit()
    {
        AnimCtl.stopChase();
    }
}
