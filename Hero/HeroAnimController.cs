using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroAnimController : BaseObject {
    private Animator mAnim;
    private Vector3 mHeroTargetPosition = Vector3.zero;
    private NavMeshAgent mNavAgent;
    private Rigidbody mRigidBody;
    private bool isRunning;
    private bool isJumping;
    private string mCurrRunningAnim;

    protected override void Start() {
        mAnim = GetComponent<Animator>();
        mNavAgent = GetComponent<NavMeshAgent>();
        mRigidBody = GetComponent<Rigidbody>();

        mAnim.applyRootMotion = false;

        mCurrRunningAnim = "updateIdleAnim";
        StartCoroutine("updateAnim");
    }

    IEnumerator updateAnim()
    {
        while (this.enabled)
        {
            yield return StartCoroutine(mCurrRunningAnim);
        }
    }

    public void doRun(Vector3 target)
    {
        StartCoroutine(run(target));
    }

    IEnumerator run(Vector3 target)
    {
        if (mNavAgent.isActiveAndEnabled)
        {
            //如果正在running，那么先转向
            NavMeshPath calcPath = new NavMeshPath();
            bool calcRet = mNavAgent.CalculatePath(target, calcPath);//注意路点不包括起始位置和最终位置
            Vector3 lookTarget = target;
            if (calcRet) {
                if (calcPath.corners.Length >= 2)
                {
                    lookTarget = calcPath.corners[1];
                }
            }
            transform.rotation = Quaternion.LookRotation(lookTarget - transform.position);

            mHeroTargetPosition = target;
            mNavAgent.SetDestination(mHeroTargetPosition);
            isRunning = true;
        }
        else
        {
            Debug.Log("Call HeroAnimController.run when mNavAgent is not active");
        }

        yield return 0;
    }

    public void doJump()
    {
        if (!isJumping)
        {
            isJumping = true;
        }
    }

    IEnumerator updateRunAnim()
    {
        setAnimValue("speed", 1);
        do
        {
            yield return 0;
        } while (!isJumping && (mNavAgent.remainingDistance > mNavAgent.stoppingDistance || mNavAgent.pathPending) && isRunning);

        setAnimValue("speed", 0);

        isRunning = false;

        if (isJumping)
        {
            yield return new WaitForSeconds(0.1f);
            mCurrRunningAnim = "updateJumpAnim";
        }
        else
        {
            mCurrRunningAnim = "updateIdleAnim";
        }
    }

    IEnumerator updateIdleAnim()
    {
        setAnimValue("speed", 0);
        setAnimValue("newJump", 0);
        do
        {
            yield return 0;
        } while (!isRunning && !isJumping);

        if (isRunning)
        {
            mCurrRunningAnim = "updateRunAnim";
        }
        else if (isJumping)
        {
            mCurrRunningAnim = "updateJumpAnim";
        }
    }

    IEnumerator updateJumpAnim()
    {
        setAnimValue("newJump", 0.11f);
        float currJumpValue = 0.11f;//跳跃起始时间
        float jumpValueTotal = 1.79f;//跳跃时间
        float jumpHeight = 2;//跳跃高度
        float startUpSpeed = 2 * jumpHeight / (jumpValueTotal / 2);// v = at/2 up方向起始速度
        float upA = -startUpSpeed / (jumpValueTotal / 2);//跳跃up方向加速度
        float currUpSpeed = startUpSpeed;//当前跳跃up方向速度
        float costTime = 1.0f;//整个跳跃动画花费时间
        float jumpValueGrowSpeed = jumpValueTotal / costTime; //每次 jumpValue 增长多少

        mNavAgent.enabled = false;//跳跃时取消navAgent，否则根本跳不起来
        mRigidBody.useGravity = false;//跳跃时取消重力，否则重力会影响跳跃高度的计算

        do
        {
            float offJumpValue = jumpValueGrowSpeed * Time.deltaTime;
            currJumpValue = currJumpValue + offJumpValue;
            setAnimValue("newJump", currJumpValue);
            //跳跃时前进
            float forwardDis = offJumpValue * mNavAgent.speed;

            //跳跃时上升
            float startUpSpeedInner = currUpSpeed;
            currUpSpeed = startUpSpeedInner + upA * offJumpValue; // vt = v0 + at
            float upDis = (Mathf.Pow(currUpSpeed, 2) - Mathf.Pow(startUpSpeedInner, 2)) / (2 * upA);
               
            transform.position += transform.forward * forwardDis + transform.up * upDis;
            yield return 0;
        } while (currJumpValue <= 1.9f);

        mNavAgent.enabled = true;
        mRigidBody.useGravity = true;

        mRigidBody.velocity = Vector3.zero;//跳跃完成之后，速度设为0，防止刚体撞击反弹。

        setAnimValue("newJump", 0);
        isJumping = false;

        yield return 0;

        setAnimValue("speed", 0);
        mCurrRunningAnim = "updateIdleAnim";
    }

    void setAnimValue(string name, float value)
    {
        mAnim.SetFloat(name, value);
    }

    float getAnimValue(string name)
    {
        return mAnim.GetFloat(name);
    }
}
