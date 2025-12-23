using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBar : MonoBehaviour
{
    public bool valid = true;
    public GameObject barCur;

    protected Cbuffbase_36 buff;
    public float timeMax = 0;
    public void Set(Cbuffbase_36 buff)
    {
        this.valid = true;
        this.buff = buff;
        timeMax = buff.buffInfo.time;
    }
    private void Update()
    {
        if (valid == false)
        {
            Destroy(gameObject);
        }

        if (buff == null) return;

        if (buff.buffInfo.time > timeMax) timeMax = buff.buffInfo.time;

        barCur.transform.localScale = new Vector3(Mathf.Clamp(buff.buffInfo.time, 0, timeMax) / Mathf.Max(timeMax, MyMath.SmallFloat), 1);
    }
}
