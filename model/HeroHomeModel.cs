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

    public override void add(string type, float value)
    {
        base.add(type, value);
        if (isDead())
        {
            GlobalObject.EventDispatcher.dispatchEvent(Events.EVT_GAME_OVER);
        }
    }
}
