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
            }
        }
        return GlobalComponentsParent;
    }

    public static T getComponent<T>() where T : MonoBehaviour
    {
        initGlobalObject();
        return GlobalComponentsParent.GetComponent<T>();
    }

    public static void addNewGlobalComponent<T>() where T : MonoBehaviour
    {
        initGlobalObject();
        if (null == GlobalComponentsParent.GetComponent<T>())
        {
            GlobalComponentsParent.AddComponent<T>();
        }
    }
}
