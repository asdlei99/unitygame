using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class HeroCamera: BaseObject
{
    private Vector3 mCameraDirect = new Vector3(-5.5f, 8.3f, 0.1f);

    protected override void Update()
    {
        Vector3 currCameraPos = Camera.main.transform.position;
        Vector3 currHeroPos = this.transform.position;
        Vector3 exceptCameraPos = currHeroPos + mCameraDirect;
        Camera.main.transform.position = Vector3.Lerp(currCameraPos, exceptCameraPos, Time.deltaTime);
    }
}
