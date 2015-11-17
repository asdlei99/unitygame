using System;
using System.Collections.Generic;
class HeroBossModel: HeroBaseModel
{
    protected override void initConfig()
    {
        mConfig = new HeroBossConfig();
    }

    protected override HeroBaseModel newInstance()
    {
        return new HeroBossModel();
    }
}
