using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroAnimController : BaseObject {
    private Animator mAnim;
    private NavMeshAgent mNavAgent;
    private Rigidbody mRigidBody;
    private bool isRunning;
    private bool isJumping;
    private bool isChasing;
    private bool isIgnoreChase;
    private GameObject mChaseObj;
    private string mCurrRunningAnim;
    private HeroBaseModel mModel;

    private bool isAttacking;

    private float mAnimInitSpeed = 1;
    protected override void Start() {
        mAnim = GetComponent<Animator>();
        mNavAgent = GetComponent<NavMeshAgent>();
        mRigidBody = GetComponent<Rigidbody>();
        
        //配置文件
        mModel = HeroModelFactory.getHeroModel(gameObject.name);

        //从配置文件读取移动速度
        mNavAgent.speed = mModel.get(Constants.HERO_ATTR_RUNSPEED);

        //取消动画中的位移
        mAnim.applyRootMotion = false;

        //保存动画初始播放速度，只为run动画根据移动速度调整动画播放速度。
        mAnimInitSpeed = mAnim.speed;

        //动画状态机
        mCurrRunningAnim = "updateIdleAnim";
        StartCoroutine("updateAnim");
    }

    IEnumerator updateAnim()
    {
        while (this.enabled)
        {
            //每次只运行一个状态，此状态运行完毕后，才会运行下一个状态。
            yield return StartCoroutine(mCurrRunningAnim);
        }
    }

    public void doRun(Vector3 target)
    {
        isIgnoreChase = true;//如果点击屏幕下达移动命令的时候，停止追逐目标
        StartCoroutine(run(target));
    }

    //追逐目标时使用的奔跑函数
    public void doRunForChase(Vector3 target)
    {
        StartCoroutine(run(target));
    }

    void setNewDestinationForRun(Vector3 target)
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
                isRunning = true;
            }
        }
        else
        {
            Debug.Log("Call HeroAnimController.run when mNavAgent is not active");
        }
    }

    IEnumerator run(Vector3 target)
    {
        setNewDestinationForRun(target);
        yield return 0;
    }

    void doChase(GameObject target)
    {
        //开始追逐目标
        isChasing = true;
        mChaseObj = target;
        StartCoroutine("chase");
    }

    IEnumerator chase()
    {
        do
        {
            if (isIgnoreChase)//如果追逐过程中，接到奔跑/跳跃命令，则暂时退出追逐。
            {
                break;
            }
            //重新设置目标位置
            setNewDestinationForRun(mChaseObj.transform.position);
            yield return 0;
            //检查是否追逐完成。
            if (!needChase())
            {
                //不要用mNavAgent.Stop()哦
                mNavAgent.ResetPath();
                isChasing = false;
                isRunning = false;//因为追逐函数使用的是runAnim 所以这个需要置为false
            }
            else if(isIgnoreChase)//如果追逐过程中，接到奔跑/跳跃命令，则暂时退出追逐。
            {
                isChasing = false;
            }
        } while (isChasing);

        Debug.Log("wanghy -- chase over " + gameObject.name);
        yield return 0;
    }

    public bool needChase() {
        if (mChaseObj == null)//目前没有追逐目标，则需要寻找目标
        {
            return true;
        }
        else
        {
            //目前是否已经找到追逐目标
            float distance = Vector3.Distance(gameObject.transform.position, mChaseObj.transform.position);
            return distance > mModel.AttackDistance && distance <= mModel.AlertDistance ;
        }
    }

    public void doJump()
    {
        isJumping = true;
        isIgnoreChase = true;//跳跃时关闭追逐
    }

    IEnumerator updateRunAnim()
    {
        //设置动画速度
        mAnim.speed = mAnimInitSpeed * mNavAgent.speed / Constants.HERO_RUN_NORMAL_SPEED;
        setAnimValue("speed", 1);
        do
        {
            yield return 0;
            if (isChasing && !needChase())
            {
                break;
            }
        } while (!isJumping && (mNavAgent.remainingDistance > mNavAgent.stoppingDistance || mNavAgent.pathPending) && (isRunning || isChasing));

        setAnimValue("speed", 0);

        isRunning = false;
        isIgnoreChase = false;

        mAnim.speed = mAnimInitSpeed;

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
        } while (!isRunning && !isJumping && !isChasing);

        if (isRunning || isChasing)
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

        Rigidbody chaseRigidBody = null;
        if (mChaseObj != null)
        {
            chaseRigidBody = mChaseObj.gameObject.GetComponent<Rigidbody>();
            chaseRigidBody.velocity = Vector3.zero;
        }

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
        mRigidBody.useGravity = false;

        mRigidBody.velocity = Vector3.zero;//跳跃完成之后，速度设为0，防止刚体撞击反弹。
        if (chaseRigidBody != null)
        {
            chaseRigidBody.velocity = Vector3.zero;
        }
        setAnimValue("newJump", 0);
        isJumping = false;
        isIgnoreChase = false;

        setAnimValue("speed", 0);

        mCurrRunningAnim = "updateIdleAnim";
        if (isChasing)
        {
            mCurrRunningAnim = "updateRunAnim";
        }
    }

    void checkEnemy()
    {
        if (isChasing || isIgnoreChase || isJumping)
        {
            return;
        }

        if (!needChase())
        {
            return;
        }
        //10m内是否有敌人
        Collider [] colliders = Physics.OverlapSphere(transform.position, mModel.AlertDistance);
        foreach(var c in colliders)
        {
            var cGameObj = c.gameObject;
            var triggerObj = cGameObj.GetComponent<HeroAnimController>();
            if(triggerObj != null)
            {
                HeroBaseModel cmodel = HeroModelFactory.getHeroModel(cGameObj.name);
                if (!cmodel.isInSameCamp(mModel))
                {
                    doChase(cGameObj);
                    break;
                }
            }
        }
    }

    protected override void FixedUpdate()
    {
        checkEnemy();
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
    void setAnimValue(string name, float value)
    {
        mAnim.SetFloat(name, value);
    }

    float getAnimValue(string name)
    {
        return mAnim.GetFloat(name);
    }
}
