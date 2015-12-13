using UnityEngine;
using System.Collections;

public class Events {
    public const string EVENT_TEST = "event_test";

    //鼠标点击
    public const string EVENT_INPUT_SCREEN_CLICK = "event_input_screen_click";
    public const string EVENT_INPUT_JUMP = "event_input_jump";

    //技能
    public const string EVENT_SKILL0 = "event_skill0";
    public const string EVENT_SKILL1 = "event_skill1";
    public const string EVENT_SKILL2 = "event_skill2";
    public const string EVENT_SKILL3 = "event_skill3";
    public const string EVENT_SKILL4 = "event_skill4";

    //选人
    public const string EVENT_SELECT_HERO = "event_select_hero";

    //升级
    public const string EVT_HERO_UPGRADE_LEVEL = "event_hero_upgrade_level";

    //属性变化
    public const string EVT_HERO_ATTR_CHANGED = "event_hero_attr_changed";

    //暴击
    public const string EVT_HERO_CRIT = "event_hero_crit";

    //游戏结束
    public const string EVT_GAME_OVER = "evt_game_over";

    //需要同步的事件
}
