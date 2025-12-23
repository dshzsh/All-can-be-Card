using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthCur, healthMax;
    public Image ht;

    private CObj_2 cobj;
    
    public void Set(CObj_2 health)
    {
        this.cobj = health;
        Update();
    }
    private void Update()
    {
        UpdateNowHealth();
        UpdateTeam();


    }


    private float oldNowHealth = 0;
    private const float beginReduceTime = 0.5f;
    private float time = 0f;
    private const float reduceTime = 0.5f;
    private float reduceHealthPerSec = 0f;
    private float htHealth = 0f;
    private void UpdateNowHealth()
    {
        if (cobj == null || cobj.myHealth == null) return;

        float maxHealth = cobj.myHealth.GetAttf(BasicAttID.healthMax);
        float nowHealth = cobj.myHealth.nowHealth;

        if (htHealth < nowHealth) htHealth = nowHealth;

        if(oldNowHealth <= nowHealth) // 血量不变或者增多，衰减计时器流动
        {
            time += Time.deltaTime;
            if(time >= beginReduceTime)
            {
                reduceHealthPerSec = (htHealth - nowHealth) / reduceTime;
                time = -MyMath.BigFloat; // 然后不再进行变化，直到血量再次变少
            }
        }
        else // 血量减少，重置衰减
        {
            time = 0f;
            reduceHealthPerSec = 0f;
        }
        oldNowHealth = nowHealth;

        htHealth -= reduceHealthPerSec * Time.deltaTime;
        if (htHealth < nowHealth) htHealth = nowHealth;

        if (maxHealth == 0) maxHealth = 1;
        healthCur.transform.localScale = new Vector3(nowHealth / maxHealth, 1);
        ht.transform.localScale = new Vector3(htHealth / maxHealth, 1);
    }



    private int oldTeam = -1;
    private Color DeepColor(Color color, float pow)
    {
        Color ans = color * pow;
        ans.a = color.a;
        return ans;
    }
    private void UpdateTeam()
    {
        if (cobj == null) return;

        if (cobj.team == oldTeam) return;
        // 根据阵营更改颜色
        Color baseColor = Color.red;
        switch(cobj.team)
        {
            case Slive_19.Team.friend:
                {
                    baseColor = Color.green;
                    break;
                }
            case Slive_19.Team.enemy:
                {
                    baseColor = Color.red;
                    break;
                }
            default:
                {
                    baseColor = new Color(0.5f, 0, 1f);
                    break;
                }
        }
        
        healthCur.color = DeepColor(baseColor, 0.9f);
        healthMax.color = DeepColor(baseColor, 0.3f);

        oldTeam = cobj.team;
    }
}
