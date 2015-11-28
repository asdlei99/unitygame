using UnityEngine;
using System.Collections;

class GlobalObject: BaseObject
{
    public static EventDispatcher EventDispatcher;
    public static InputManager InputManager;
    public static Util Util;
    private static GameObject GlobalComponentsParent;

    //测试函数
    private static void test()
    {
        HeroAttrConfig heroconfig = Singleton<HeroAttrConfig>.getInstance();
        Debug.Log(heroconfig.ToString());
    }

    public static void initGlobalObject()
    {
        getGlobalObject();
        //test();
    }

    public static GameObject getGlobalObject()
    {
        if (GlobalComponentsParent == null)
        {
            GlobalComponentsParent = GameObject.Find(Constants.GLOBAL_EMPTY_OBJECT);
            if (GlobalComponentsParent == null)
            {
                GlobalComponentsParent = GameObject.Instantiate(ResourceManager<GameObject>.load("prefab/globalRes/EmptyObject"));
                GlobalComponentsParent.name = Constants.GLOBAL_EMPTY_OBJECT;

                //EventDispatcher
                EventDispatcher = GlobalComponentsParent.AddComponent<EventDispatcher>();
                //InputManager
                InputManager = GlobalComponentsParent.AddComponent<InputManager>();
                //Util
                Util = GlobalComponentsParent.AddComponent<Util>();

                //sound
                SoundManager.getInstance().playBackground();
            }
        }
        return GlobalComponentsParent;
    }

    public static T getComponent<T>() where T : Component
    {
        initGlobalObject();
        return GlobalComponentsParent.GetComponent<T>();
    }

    public static T addNewGlobalComponent<T>() where T : Component
    {
        initGlobalObject();
        T t = GlobalComponentsParent.GetComponent<T>();
        if (null == t)
        {
           return GlobalComponentsParent.AddComponent<T>();
        }
        return t;
    }

    public static void removeGlobalComponent<T>() where T : Component
    {
        Component c = GlobalComponentsParent.GetComponent<T>();
        if (c != null)
        {
            GameObject.Destroy(c);
        }
    }
}
