using System;
using System.Collections.Generic;
using UnityEngine;
class HeroIdleState : HeroState
{
    public override void onEnter()
    {
        AnimCtl.startIdle();
    }

    public override void onUpdate()
    {
        AnimCtl.checkEnemy();
    }

    public override string switchToNextState()
    {
        if (AnimCtl.IsJumping)
        {
            return "HeroJumpState";//转到 jump 状态
        }
        else if (AnimCtl.IsRunning)
        {
            return "HeroRunState";//转到 run 状态
        }
        else if (AnimCtl.IsChasing)
        {
            return "HeroChaseState";//转到 chase 状态
        }
        return base.switchToNextState();
    }

    public override void onExit()
    {
        AnimCtl.stopIdle();
    }
}