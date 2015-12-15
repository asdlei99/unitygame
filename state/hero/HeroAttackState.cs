using System;
using System.Collections.Generic;
using UnityEngine;
class HeroAttackState : HeroState
{
    public override void onEnter()
    {
        AnimCtl.setAnimValue("attackState", true);
        mapEvent(Events.EVENT_SKILL0, onEvent);
        mapEvent(Events.EVENT_SKILL1, onEvent);
        mapEvent(Events.EVENT_SKILL2, onEvent);
        mapEvent(Events.EVENT_SKILL3, onEvent);
        mapEvent(Events.EVENT_SKILL4, onEvent);
    }

    void onEvent(string evt, object obj)
    {
        Debug.Log("wanghy -- HeroAttackState evt = " + evt);
        resetAnimValue();
        switch (evt)
        {
            case Events.EVENT_SKILL0:
                AnimCtl.setAnimValue("attack0", true);
                break;
            case Events.EVENT_SKILL1:
                AnimCtl.setAnimValue("attack1", true);
                break;
            case Events.EVENT_SKILL2:
                AnimCtl.setAnimValue("attack2", true);
                break;
            case Events.EVENT_SKILL3:
                AnimCtl.setAnimValue("attack3", true);
                break;
            case Events.EVENT_SKILL4:
                AnimCtl.setAnimValue("attack4", true);
                break;
        }
    }

    public override void onUpdate()
    {
        if (AnimCtl.ChaseObject != null)
        {
            //看着敌人
            GameObject.transform.LookAt(AnimCtl.ChaseObject.transform);
        }
        AnimCtl.checkEnemy();

        checkHit();
    }

    void doAttack()
    {
        GameObject enemy = AnimCtl.ChaseObject;
        if (enemy != null)
        {
            HeroBaseModel enemyModel = HeroModelFactory.getHeroModel(enemy.name);
            //攻击敌人
            AnimCtl.Model.attack(enemyModel);
        }
    }

    bool isAttacked = false;
    void checkHit()
    {
        AnimatorStateInfo info = AnimCtl.Anim.GetCurrentAnimatorStateInfo(0);
        float passTime = info.normalizedTime % info.length;
        if (passTime / info.length < 0.5f && isAttacked) {
            isAttacked = false;
        } else if (passTime / info.length >= 0.5f && !isAttacked)
        {
            doAttack();
            isAttacked = true;
        }
    }

    public override string switchToNextState()
    {
        if (AnimCtl.IsJumping)
        {
            return "HeroJumpState";
        }
        else if (AnimCtl.IsChasing)
        {
            return "HeroChaseState";
        }
        else if (AnimCtl.IsRunning)
        {
            return "HeroRunState";
        }else if(AnimCtl.ChaseObject == null)
        {
            return "HeroIdleState";
        }
        return base.switchToNextState();
    }

    void resetAnimValue()
    {
        AnimCtl.setAnimValue("attack0", false);
        AnimCtl.setAnimValue("attack1", false);
        AnimCtl.setAnimValue("attack2", false);
        AnimCtl.setAnimValue("attack3", false);
        AnimCtl.setAnimValue("attack4", false);
    }

    public override void onExit()
    {
        AnimCtl.setAnimValue("attackState", false);
        resetAnimValue();
    }
}
