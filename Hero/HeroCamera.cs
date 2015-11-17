using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class HeroCamera: BaseObject
{
    private Vector3 mCameraDirect;
    protected override void Start()
    {
        base.Start();
        mCameraDirect = Camera.main.transform.position - this.transform.position;
    }

    protected override void Update()
    {
        Vector3 currCameraPos = Camera.main.transform.position;
        Vector3 currHeroPos = this.transform.position;
        Vector3 exceptCameraPos = currHeroPos + mCameraDirect;
        Camera.main.transform.position = Vector3.Lerp(currCameraPos, exceptCameraPos, Time.deltaTime);
    }
}
