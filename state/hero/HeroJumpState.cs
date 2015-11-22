using System;
using System.Collections.Generic;
using UnityEngine;
class HeroJumpState : HeroState
{
    public override void onEnter()
    {
    }
    public override void onUpdate()
    {
    }
    public override string switchToNextState()
    {
        if (!AnimCtl.IsJumping)
        {
            if (AnimCtl.IsChasing)
            {
                return "HeroChaseState";
            }
            else
            {
                return "HeroIdleState";
            }
        }
        return null;
    }
    public override void onExit()
    {
        AnimCtl.stopJump();
    }
}
