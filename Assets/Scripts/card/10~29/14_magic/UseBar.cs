using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UseBar : BasicBar
{
    public static float barMaxLength = 0.9f;
    public GameObject line;
    public TextMeshProUGUI text;
    public void Set(BarValue barValue, float linePos, string name)
    {
        this.valid = true;
        this.barValue = barValue;
        line.transform.localPosition = new Vector3(linePos * barMaxLength, 0, 0);
        text.text = name;
    }
}
