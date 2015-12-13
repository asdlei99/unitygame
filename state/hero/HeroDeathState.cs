using System;
using System.Collections.Generic;
using UnityEngine;
class HeroDeathState : HeroState
{
    bool fadeout = false;
    bool waitRebirth = false;
    float rebirthCount = 0;
    bool rebirth = false;
    Renderer renderer = null;
    Color targetColor;
    public override void onEnter()
    {
        Debug.Log("wanghy -- dead state enter");
        renderer = GameObject.transform.FindChild("model").GetComponent<Renderer>();
        AnimCtl.setAnimValue("isDead", true);
    }

    public override void onUpdate()
    {
        if (waitRebirth)//第三步等待重生
        {
            Debug.Log("wanghy -- dead state update 1");
            //移动到出生点
        }
        else if (fadeout)//第二步淡出
        {
            Debug.Log("wanghy -- dead state update 2");
            Color currColor = renderer.material.color;
            Color targetColor = new Color(currColor.r, currColor.g, currColor.b, 0);
            Color setColor = Color.Lerp(currColor, targetColor, Time.deltaTime);
            renderer.material.color = setColor;
            if(setColor.a <= 0.05)
            {
                waitRebirth = true;
            }
        }
        else//第一步 dead 动画
        {
            //检查dead动画
            AnimatorStateInfo info = AnimCtl.Anim.GetCurrentAnimatorStateInfo(0);
            Debug.Log("wanghy -- dead state update 3 normalizedTime=" + info.normalizedTime + ", length=" + info.length);
            if (info.normalizedTime >= info.length)//动画结束
            {
                AnimCtl.setAnimValue("isDead", false);
                fadeout = true;
            }
        }
    }

    public override string switchToNextState()
    {
        if (rebirth)
        {
            return "HeroIdleState";
        }
        return null;
    }


    public override void onExit()
    {
        rebirth = false;
        fadeout = false;
        waitRebirth = false;
        rebirthCount = 0;
        rebirth = false;
        AnimCtl.setAnimValue("isDead", false);
    }
}
