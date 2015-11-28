using UnityEngine;
using System.Collections;

public class InputManager : BaseObject {
    protected override void Update()
    {
        //获取屏幕点击事件
        //移动端触摸事件
#if UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 1000))
            {
                //Debug.Log("click screen hit gameObject.name=" + hitInfo.transform.gameObject.name);
                GameObject selectedObj = hitInfo.transform.gameObject;
                if (selectedObj.name.Equals("Terrain"))
                {
                    dispatch(Events.EVENT_INPUT_SCREEN_CLICK, hitInfo.point);
                } else {
                    Selectable selectComponent = selectedObj.GetComponent<Selectable>();
                    if (selectComponent != null)
                    {
                        selectComponent.onSelect();
                    }
                }
            }
        }
        if (Input.GetAxis("Jump") == 1)
        {
            dispatch(Events.EVENT_INPUT_JUMP);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            dispatch(Events.EVENT_SKILL0);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            dispatch(Events.EVENT_SKILL1);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            dispatch(Events.EVENT_SKILL2);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            dispatch(Events.EVENT_SKILL3);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            dispatch(Events.EVENT_SKILL4);
        }

#elif UNITY_IPHONE || UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);
            Debug.Log("touchCount = 1, touchPos = " + t.position);
        }
        else if (Input.touchCount >= 1)
        {
            Debug.Log("touchCount = " + Input.touchCount);
            foreach (Touch t in Input.touches)
            {
                Debug.Log("touchPosition=" + t.position);
            }
        }
#endif
    }
}
