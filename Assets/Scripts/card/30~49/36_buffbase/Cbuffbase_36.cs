using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardManager;
using static SystemManager;

public class Cbuffbase_36 : Citem_33
{
    public Sbuff_35.BuffInfo buffInfo;
}

public class Sbuffbase_36: Sitem_33
{
    public static void SetBuffBar(Cbuffbase_36 card, MsgOnItem msg, ref BuffBar buffBar, GameObject buffBarPrefab)
    {
        if (!TryGetCobj(card, out var cobj)) return;

        if (msg.op == 1)
        {
            if (buffBar == null)
            {
                buffBar = UIBasic.GiveLiveUpUI(buffBarPrefab, cobj).GetComponent<BuffBar>();
            }
            else if (buffBar.valid == false)
            {
                buffBar.valid = true;
            }

            buffBar.Set(card);
        }
        else
        {
            if (buffBar != null)
            {
                buffBar.valid = false;
            }
        }
    }

    public override void Init()
    {
        base.Init();
    }
    public override Color GetColor(CardBase _card)
    {
        return GoodUIColor.Buff;
    }
}