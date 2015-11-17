using System;
using System.Collections.Generic;
using UnityEngine;
class MouseUI: BaseObject
{
    Renderer mRenderer;
    protected override void Start()
    {
        mRenderer = GetComponent<Renderer>();
        this.getEventDispatcher().mapEvent(Events.EVENT_INPUT_SCREEN_CLICK, this, onClickScreen);
    }

    private void onClickScreen(string evt, object data)
    {
        Vector3 target = (Vector3)data;
        transform.position = target;
        mRenderer.material.color = new Color(1, 0, 0, 1);
    }
    void updateMouseUIColor()
    {
        Color currColor = mRenderer.material.color;
        Color targetColor = new Color(currColor.r, currColor.g, currColor.b, 0);
        Color setColor = Color.Lerp(currColor, targetColor, Time.deltaTime * 5);
        mRenderer.material.color = setColor;
    }

    protected override void Update()
    {
        updateMouseUIColor();
    }
}