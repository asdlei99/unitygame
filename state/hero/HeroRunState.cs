using System;
using System.Collections.Generic;
using UnityEngine;
class HeroRunState : HeroState
{
    Vector3 mTarget = Vector3.zero;
    public override void onEnter()
    {
        resetTarget();
    }

    public override void onUpdate()
    {
        resetTarget();
    }

    void resetTarget()
    {
        if (hasExtraData("runtotarget"))
        {
            Vector3 runToTarget = (Vector3)getExtraData("runtotarget");
            if (!runToTarget.Equals(mTarget)) {
                mTarget = runToTarget;
                AnimCtl.setNewDestination(mTarget);
            }
        }
    }

    public override string switchToNextState()
    {
        if (AnimCtl.IsJumping)
        {
            return "HeroJumpState";
        }
        else if (AnimCtl.isArrivedTarget())
        {
            AnimCtl.checkEnemy();
            if (AnimCtl.IsChasing)//TODO: 可能遇到敌人了。
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
        AnimCtl.stopRun();
    }
}
