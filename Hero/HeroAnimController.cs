﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//控制英雄的动画，移动，还有刚体设置
public class HeroAnimController : BaseObject {
    private Animator mAnim;
    private NavMeshAgent mNavAgent;
    private Rigidbody mRigidBody;
    private bool isRunning;
    private Vector3 mRunToTarget = Vector3.zero;
    private bool isJumping;
    private bool isChasing;
    private GameObject mChaseObj;
    private string mCurrRunningAnim;
    private HeroBaseModel mModel;
    private StateManager mStateManager;
    private HeroBloodAndMagic mBloodBar;

    private bool isAttacking;

    private float mAnimInitSpeed = 1;

    public HeroBaseModel Model
    {
        get{ return mModel; }
    }

    public bool IsJumping
    {
        get { return isJumping; }
    }

    public bool IsChasing
    {
        get { return isChasing; }
    }

    public bool IsRunning
    {
        get { return isRunning; }
    }

    public GameObject ChaseObject
    {
        get { return mChaseObj; }
    }

    public Vector3 RunToTarget
    {
        get { return mRunToTarget; }
    }

    public NavMeshAgent NavAgent
    {
        get { return mNavAgent; }
    }

    protected override void Start() {
        mAnim = GetComponent<Animator>();
        mNavAgent = GetComponent<NavMeshAgent>();
        mRigidBody = GetComponent<Rigidbody>();

        mRigidBody.useGravity = false;
        
        //配置文件
        mModel = HeroModelFactory.getHeroModel(gameObject.name);

        //从配置文件读取移动速度
        mNavAgent.speed = mModel.get(Constants.HERO_ATTR_RUNSPEED);

        //取消动画中的位移
        mAnim.applyRootMotion = false;

        //保存动画初始播放速度，只为run动画根据移动速度调整动画播放速度。
        mAnimInitSpeed = mAnim.speed;

        //动作状态机
        mStateManager = gameObject.AddComponent<StateManager>();
        mStateManager.addState(new HeroRunState());
        mStateManager.addState(new HeroChaseState());
        mStateManager.addState(new HeroIdleState());
        mStateManager.addState(new HeroJumpState());
        mStateManager.setDefaultState("HeroIdleState");

        mBloodBar = gameObject.AddComponent<HeroBloodAndMagic>();
    }

    public void startChase(GameObject chaseObj)
    {
        mChaseObj = chaseObj;
        isChasing = true;
        mAnim.speed = mAnimInitSpeed * mNavAgent.speed / Constants.HERO_RUN_NORMAL_SPEED;
        setAnimValue("speed", 1);
        setNewDestination(chaseObj.transform.position);
    }

    public void updateChase()
    {
        setNewDestination(mChaseObj.transform.position);
    }

    public void stopChase()
    {
        isChasing = false;
        if (mNavAgent.isActiveAndEnabled)
        {
            mNavAgent.ResetPath();
        }
        else
        {
            Debug.Log("wanghy -- mNavAgent isnot active when stopChase()");
        }
    }

    public void startIdle()
    {
        setAnimValue("speed", 0);
        mAnim.speed = mAnimInitSpeed;
    }

    public void stopIdle()
    {
    }

    public void startRun(Vector3 target)
    {
        isRunning = true;
        mAnim.speed = mAnimInitSpeed * mNavAgent.speed / Constants.HERO_RUN_NORMAL_SPEED;
        setAnimValue("speed", 1);
        mRunToTarget = target;
    }

    public void stopRun()
    {
        if (mNavAgent.isActiveAndEnabled)
        {
            mNavAgent.ResetPath();
        }
        isRunning = false;
    }

    public void startJump()
    {
        if (isJumping)
        {
            return;
        }
        isJumping = true;
        setAnimValue("speed", 0);
        setAnimValue("newJump", 0.11f);
        mAnim.speed = mAnimInitSpeed;

        mNavAgent.enabled = false;//跳跃时取消navAgent，否则根本跳不起来
    }

    public void stopJump()
    {
        mNavAgent.enabled = true;

        setAnimValue("newJump", 0);
        setAnimValue("speed", 0);
        isJumping = false;
    }

    public void setNewDestination(Vector3 target)
    {
        if (mNavAgent.isActiveAndEnabled)
        {
            //如果正在running，那么先转向
            NavMeshPath calcPath = new NavMeshPath();
            //计算路点
            bool calcRet = mNavAgent.CalculatePath(target, calcPath);
            Vector3 lookTarget = target;
            if (calcRet)
            {
                //转向第一个转折点
                if (calcPath.corners.Length >= 2)
                {
                    lookTarget = calcPath.corners[1];
                }
                transform.rotation = Quaternion.LookRotation(lookTarget - transform.position);

                mNavAgent.SetDestination(target);
            }
        }
        else
        {
            Debug.Log("Call HeroAnimController.run when mNavAgent is not active");
        }
    }

    public bool isArrivedTarget()
    {
        return mNavAgent.remainingDistance <= mNavAgent.stoppingDistance && !mNavAgent.pathPending;
    }

    public bool isOutofRangeForChase()
    {
        return getChaseDistance() > mModel.AlertDistance;
    }

    public bool isArrivedTargetForChase()
    {
        return getChaseDistance() <= mModel.AttackDistance;
    }

    float getChaseDistance()
    {
        return Vector3.Distance(gameObject.transform.position, mChaseObj.transform.position);
    }

    public void checkEnemy()
    {
        //如果不在追逐状态，并且距离目标为可攻击距离，那么就不需要追逐。
        if(!isChasing && mChaseObj != null && isArrivedTargetForChase())
        {
            return;
        }
        //10m内是否有敌人
        Collider[] colliders = Physics.OverlapSphere(transform.position, mModel.AlertDistance);
        foreach (var c in colliders)
        {
            var cGameObj = c.gameObject;
            var triggerObj = cGameObj.GetComponent<HeroAnimController>();
            if (triggerObj != null)
            {
                HeroBaseModel cmodel = HeroModelFactory.getHeroModel(cGameObj.name);
                if (!cmodel.isInSameCamp(mModel))
                {
                    startChase(cGameObj);
                    break;
                }
            }
        }
    }

    //下面3个函数，防止人物相撞后滑动。
    void OnCollisionStay(Collision c)
    {
        mRigidBody.velocity = Vector3.zero;
    }

    void OnCollisionEnter(Collision c)
    {
        mRigidBody.velocity = Vector3.zero;
    }

    void OnCollisionExit(Collision c)
    {
        mRigidBody.velocity = Vector3.zero;
    }

    //下面的函数设置动画状态机的参数，来控制动画转换。
    public void setAnimValue(string name, float value)
    {
        mAnim.SetFloat(name, value);
    }

    public float getAnimValue(string name)
    {
        return mAnim.GetFloat(name);
    }
}
