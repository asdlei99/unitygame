using UnityEngine;
using System.Collections;

public class ResourceManager<T> where T : Object {

    public static T load(string path)
    {
        T resource = loadFromResource(path);
        if(resource == null)
        {
            resource = loadFromAB(path);
        }
        return resource;
    }

    public static T loadFromResource(string path)
    {
        return Resources.Load<T>(path);
    }

    //TODO: 方法未实现
    //先不用assetBundle，在项目最后再用也可以哦。
    //实现一个约定，令path同最后打出来的assetBundle有一个对应关系。
    public static T loadFromAB(string path)
    {
        return null;
    }
}
