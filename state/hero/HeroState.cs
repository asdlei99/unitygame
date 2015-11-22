using System;
using System.Collections.Generic;
using UnityEngine;
class HeroState : BaseState
{
    HeroAnimController mAnimCtl;
    public override StateManager StateManager
    {
        get { return base.StateManager; }
        set
        {
            base.StateManager = value;
            mAnimCtl = GameObject.GetComponent<HeroAnimController>();
        }
    }

    protected HeroAnimController AnimCtl
    {
        get { return mAnimCtl; }
    }
}