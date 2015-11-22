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

    public float Percent
    {
        set { mPercent = value; }
    }

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
    }

    protected override void OnGUI()
    {
        Vector3 pos = mBc.bounds.center + Vector3.up * mBc.bounds.extents.y / 2;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
        mRectBlood = new Rect(new Vector2(screenPos.x - mBloodWidth / 2, Screen.height - screenPos.y + mOffY - mAdjustHeight), new Vector2(mBloodWidth, mBloodHeight));
        mRectBg = new Rect(new Vector2(screenPos.x - mBgWidth/ 2, Screen.height - screenPos.y - mAdjustHeight), new Vector2(mBgWidth, mBgHeight));
        GUI.DrawTexture(mRectBg, mTexBg, ScaleMode.ScaleAndCrop);
        GUI.DrawTexture(mRectBlood, mTexBlood, ScaleMode.ScaleAndCrop);
    }
}