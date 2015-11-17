using UnityEngine;
using System.Collections;
using System;

public class BuildSelector: Selectable 
{
    public override void onSelect()
    {
        Debug.Log("build is onSelect");
    }
}
