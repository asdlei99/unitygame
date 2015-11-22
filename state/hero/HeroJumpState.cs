using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
class HeroJumpState : HeroState
{
    bool needSwitchAnim = false;
    public override void onEnter()
    {
        AnimCtl.StartCoroutine(updateJumpAnim());
    }
    public override void onUpdate()
    {
    }
    public override string switchToNextState()
    {
        string nextStateName = null;
        if (needSwitchAnim)
        {
            if (AnimCtl.IsChasing)
            {
                nextStateName = "HeroChaseState";
            }
            else
            {
                nextStateName = "HeroIdleState";
            }
        }
        return nextStateName;
    }

    IEnumerator updateJumpAnim()
    {
        float currJumpValue = 0.11f;//跳跃起始时间
        float jumpValueTotal = 1.79f;//跳跃时间
        float jumpHeight = 4;//跳跃高度
        float startUpSpeed = 2 * jumpHeight / (jumpValueTotal / 2);// v = at/2 up方向起始速度
        float upA = -startUpSpeed / (jumpValueTotal / 2);//跳跃up方向加速度
        float currUpSpeed = startUpSpeed;//当前跳跃up方向速度
        float costTime = 1.0f;//整个跳跃动画花费时间
        float jumpValueGrowSpeed = jumpValueTotal / costTime; //每次 jumpValue 增长多少

        double currTime = Time.time;
        int times = 0;
        do
        {
            times++;
            currTime += Time.deltaTime;
            float offJumpValue = jumpValueGrowSpeed * Time.deltaTime;
            currJumpValue = currJumpValue + offJumpValue;
            AnimCtl.setAnimValue("newJump", currJumpValue);
            //跳跃时前进
            float forwardDis = offJumpValue * AnimCtl.NavAgent.speed;

            //跳跃时上升
            float startUpSpeedInner = currUpSpeed;
            currUpSpeed = startUpSpeedInner + upA * offJumpValue; // vt = v0 + at
            float upDis = (Mathf.Pow(currUpSpeed, 2) - Mathf.Pow(startUpSpeedInner, 2)) / (2 * upA);

            AnimCtl.transform.position += AnimCtl.transform.forward * forwardDis + AnimCtl.transform.up * upDis;
            yield return 0;
        } while (currJumpValue <= 1.9f);
        needSwitchAnim = true;
    }

    public override void onExit()
    {
        needSwitchAnim = false;
        AnimCtl.stopJump();
    }
}
