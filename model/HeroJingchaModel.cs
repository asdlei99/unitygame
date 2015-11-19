using System;
using System.Collections.Generic;

class HeroJingchaModel:HeroBaseModel
{
    protected override void initConfig()
    {
        mConfig = new HeroJingchaConfig();
        mCamp = "Jingcha";
    }

    protected override HeroBaseModel newInstance()
    {
        return new HeroJingchaModel();
    }
}