using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGcyqh_7 : CGqhsbase_11
{

}
public class DGcyqh_7 : DataBase
{
    public float fadeTime = 0.5f;
}
public class SGcyqh_7 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyMagicBegin, MagicBegin, HandlerPriority.Before);
    }
    void MagicBegin(CardBase _card, MsgBase _msg)
    {
        CGcyqh_7 card = _card as CGcyqh_7;
        DGcyqh_7 config = basicConfig as DGcyqh_7;
        MsgMagicUse msg = _msg as MsgMagicUse;

        if (msg.magic != Sqhc_38.GetQhMagic(card)) return;
        if (!TryGetCobj(card, out var obj)) return;
        if (obj.id == GetTypeId(typeof(CLcy_3))) return;

        // 提前结算冷却和耗魔
        Smagic_14.MagicUseCost(msg.live, msg);
        if (msg.valid == false)
            return;

        MsgMagicUse nmsg = new MsgMagicUse(msg); nmsg.ToNoCost(); 
        nmsg.key = Smagic_14.TempKey; // 临时使用，会临时加上去
        nmsg.magic = NewCopy(msg.magic); // 复制魔法


        float timeMax = msg.mdata.windDown + config.fadeTime;
        CLcy_3 cy = SLcy_3.GiveCy(obj, timeMax, config.fadeTime);
        
        nmsg.live = cy;


        Smagic_14.UseMagicEntirely(nmsg);

        msg.valid = false;
    }
}