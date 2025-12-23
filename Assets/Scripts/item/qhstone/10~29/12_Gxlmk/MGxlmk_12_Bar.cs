using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MGxlmk_12_Bar : BasicBar
{
    public Image image;
    public override void SpUpdate()
    {
        base.SpUpdate();
        float x = GetProcess();
        if (x >= 0.9999f)
            image.color = Color.green;
        else image.color = Color.yellow;
    }
}
