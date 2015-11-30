using System;
using System.Collections.Generic;

class HeroChangbiziModel:HeroBaseModel
{
    protected override void initConfig()
    {
        mConfig = new HeroChangbiziConfig();
        mCamp = "Enemy";
    }

    protected override HeroBaseModel newInstance()
    {
        return new HeroChangbiziModel();
    }
}