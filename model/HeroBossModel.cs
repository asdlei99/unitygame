using System;
using System.Collections.Generic;
class HeroBossModel: HeroBaseModel
{
    protected override void initConfig()
    {
        mConfig = new HeroBossConfig();
        mCamp = "Boss";
    }

    protected override HeroBaseModel newInstance()
    {
        return new HeroBossModel();
    }
}
