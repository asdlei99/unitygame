using System;
using System.Collections.Generic;
class HeroHomeModel: HeroBaseModel
{
    protected override void initConfig()
    {
        mConfig = new HeroHomeConfig();
        mCamp = "Hero";
    }

    protected override HeroBaseModel newInstance()
    {
        return new HeroHomeModel();
    }
}
