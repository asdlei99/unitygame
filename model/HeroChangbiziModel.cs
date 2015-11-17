using System;
using System.Collections.Generic;

class HeroChangbiziModel:HeroBaseModel
{
    protected override void initConfig()
    {
        mConfig = new HeroChangbiziConfig();
    }

    protected override HeroBaseModel newInstance()
    {
        return new HeroChangbiziModel();
    }
}