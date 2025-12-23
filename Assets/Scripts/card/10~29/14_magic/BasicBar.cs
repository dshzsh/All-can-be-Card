using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBar : MonoBehaviour
{
    public bool valid = true;
    public GameObject barCur;

    protected BarValue barValue;
    public void Set(BarValue barValue)
    {
        this.valid = true;
        this.barValue = barValue;
    }
    public virtual void SpUpdate()
    {

    }
    protected float GetProcess()
    {
        return Mathf.Clamp(barValue.cur, 0, barValue.max) / barValue.max;
    }
    private void Update()
    {
        if (valid == false)
        {
            Destroy(gameObject);
        }
        if (barValue == null) return;

        barCur.transform.localScale = new Vector3(GetProcess(), 1);
        SpUpdate();
    }
}
