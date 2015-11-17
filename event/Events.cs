using UnityEngine;
using System.Collections;

public class Events {
    public const string EVENT_TEST = "event_test";

    //鼠标点击
    public const string EVENT_INPUT_SCREEN_CLICK = "event_input_screen_click";
    public const string EVENT_INPUT_JUMP = "event_input_jump";

    //选人
    public const string EVENT_SELECT_HERO = "event_select_hero";

    //升级
    public const string EVT_HERO_UPGRADE_LEVEL = "HeroUpgradeLevel";

    //属性变化
    public const string EVT_HERO_ATTR_CHANGED = "HeroAttrChanged";

    //需要同步的事件
}
