using System;
using System.Collections.Generic;
class HeroModelFactory
{
    public static HeroBaseModel getHeroModel(string type)
    {
        HeroBaseModel model = null;

        switch (type)
        {
            case Constants.HERO_NAME_BOSS:
                model = Singleton<HeroBossModel>.getInstance();
                break;
            case Constants.HERO_NAME_CHANGBIZI:
                model = Singleton<HeroChangbiziModel>.getInstance();
                break;
            case Constants.HERO_NAME_JINGCHA:
                model = Singleton<HeroJingchaModel>.getInstance();
                break;
            case Constants.HERO_NAME_HOME:
                model = Singleton<HeroHomeModel>.getInstance();
                break;
            default:
                break;
        }

        if(model != null)
        {
            model.Type = type;
        }

        return model;
    }
}