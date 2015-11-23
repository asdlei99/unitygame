using System;
using System.Collections.Generic;
using UnityEngine;
class HeroBloodAndMagic: BaseObject
{
    BoxCollider mBc;
    Rect mRectBlood;
    Rect mRectBg;
    float mPercent;
    Texture2D mTexBlood;
    Texture2D mTexBg;
    float mBloodWidth;
    float mBloodHeight;
    float mBgWidth;
    float mBgHeight;
    float mOffX;
    float mOffY;
    float mAdjustHeight;
    float mFactor;
    float mTotalScale = 1;
    HeroBaseModel mModel;

    public float Scale
    {
        get { return mTotalScale; }
        set { mTotalScale = value; }
    }

    protected override void Start()
    {
        mTexBlood = ResourceManager<Texture2D>.load("images/globalUI/pbBlood");
        mTexBg = ResourceManager<Texture2D>.load("images/globalUI/pbBg");
        mBc = GetComponent<BoxCollider>();
        mBloodWidth = 90;
        mBloodHeight = 10;
        mOffX = 5;
        mOffY = 5;

        mFactor = mBc.bounds.extents.magnitude;
        mBloodWidth *= mFactor * mTotalScale;
        mBloodHeight *= /*mFactor * */mTotalScale;
        mOffX *= mFactor * mTotalScale;
        mOffY *= /*mFactor * */ mTotalScale;

        mBgWidth = mBloodWidth + 2 * mOffX;
        mBgHeight = mBloodHeight + 2 * mOffY;

        mAdjustHeight = 50;

        mModel = HeroModelFactory.getHeroModel(gameObject.name);
        if (mModel != null)
        {
            float allBlood = mModel.get(Constants.HERO_ATTR_HEALTH_MAX);
            mPercent = mModel.get(Constants.HERO_ATTR_HEALTH) / allBlood;
            mapEvent(Events.EVT_HERO_ATTR_CHANGED, delegate (string __, object data)
            {
                string type = (string)data;
                if (type.Equals(Constants.HERO_ATTR_HEALTH))
                {
                    float currBlood = mModel.get(Constants.HERO_ATTR_HEALTH);
                    mPercent = currBlood / allBlood;
                }
            });
        }
    }

    protected override void OnGUI()
    {
        Vector3 pos = mBc.bounds.center + Vector3.up * mBc.bounds.extents.y / 2;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
        mRectBlood = new Rect(new Vector2(screenPos.x - mBloodWidth / 2, Screen.height - screenPos.y + mOffY - mAdjustHeight), new Vector2(mBloodWidth, mBloodHeight));
        mRectBg = new Rect(new Vector2(screenPos.x - mBgWidth/ 2, Screen.height - screenPos.y - mAdjustHeight), new Vector2(mBgWidth, mBgHeight));

        GUI.DrawTexture(mRectBg, mTexBg, ScaleMode.ScaleAndCrop);

        Rect clipRect = new Rect(new Vector2(mRectBlood.position.x, mRectBlood.position.y), new Vector2(mRectBlood.size.x *mPercent, mRectBlood.size.y));
        GUI.BeginClip(clipRect);
        GUI.DrawTexture(new Rect(Vector2.zero, mRectBlood.size), mTexBlood, ScaleMode.ScaleAndCrop);
        GUI.EndClip();
    }
}