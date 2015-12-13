using System;
using System.Collections.Generic;
using UnityEngine;
class HeroState : BaseState
{
    HeroController mAnimCtl;
    public override StateManager StateManager
    {
        get { return base.StateManager; }
        set
        {
            base.StateManager = value;
            mAnimCtl = GameObject.GetComponent<HeroController>();
        }
    }

    protected HeroController AnimCtl
    {
        get { return mAnimCtl; }
    }
}