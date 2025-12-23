using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBar : MonoBehaviour
{
    public GameObject healthcur;
    private Chealth_4 myAtt;

    public void Set(Chealth_4 health)
    {
        this.myAtt = health;
    }
    private void Update()
    {
        if (myAtt == null) return;

        
        float healthMax = myAtt.GetAttf(BasicAttID.manaMax);
        if (healthMax == 0) healthMax = 1;
        healthcur.transform.localScale = new Vector3(myAtt.nowMana / healthMax, 1);
    }
}
