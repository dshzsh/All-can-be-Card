using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorBar : MonoBehaviour
{
    public GameObject barCur;

    private CFhd_9 hd;
    private float armorMax = 1f;
    public void Set(CFhd_9 hd)
    {
        this.hd = hd;
        armorMax = Mathf.Max(0.01f, hd.armor);
    }
    private void Update()
    {
        if (hd == null) return;
        if (hd.armor > armorMax) armorMax = hd.armor;
        barCur.transform.localScale = new Vector3(Mathf.Clamp(hd.armor, 0, armorMax) / armorMax, 1);
    }
}
