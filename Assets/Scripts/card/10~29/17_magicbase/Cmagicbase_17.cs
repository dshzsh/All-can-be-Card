using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static CardManager;
using static SystemManager;

public class Cmagicbase_17 : Citem_33
{
    public Dmagic mdata;
}
public class Dmagic : DataBase
{
    public float cd = 1;
    public float cost = 0;
    public float useDis = -1f;
    public float windUp = 0f;
    public float windDown = 0f;//负数表示默认后摇
    public bool moveUse = false;
    [JsonIgnore]
    public int wxTag = 0;

    public string animName = "use";

    [JsonIgnore]
    public AnimationClip animClip = null;
    public Dmagic() { }
    public Dmagic(Dmagic other)
    {
        cd = other.cd;
        cost = other.cost;
        windUp = other.windUp;
        windDown = other.windDown;
        useDis = other.useDis;
        moveUse = other.moveUse;
        animClip = other.animClip;
        wxTag = other.wxTag;
    }
    public override void Init(int id)
    {
        animClip = DataManager.LoadResource<AnimationClip>(id, animName);
        if(windDown<0)
        {
            if (animClip != null)
                windDown = animClip.length - 0.2f;
            else windDown = 0;
        }
    }
}

public class Smagicbase_17 : Sitem_33
{
    public static void PlayMagicUseAnim(MsgMagicUse msg)
    {
        // 播放动作
        if (msg.mdata.windDown > 0)
        {
            if (TryGetCobj(msg.live, out var cobj))
            {
                // MyDebug.DrawLine(cobj.obj.transform.position, msg.pos);
                SendMsg(msg.live, MsgType.RotateControl, new MsgRotateControl { seeTo = msg.pos, interrupt = true });

                SObj_2.PlayAtkAnim(cobj, msg.mdata.animClip, msg.mdata.windDown, msg.mdata.moveUse, msg.instantAtkSpeed);
            }
        }
    }
    public override string ParseDescribe(CardBase _card, string text)
    {
        Cmagicbase_17 card = _card as Cmagicbase_17;
        string ans = base.ParseDescribe(_card, text);
        string mdataText = string.Format("<color={2}>冷却:{0:0.00}</color>   <color={3}>耗魔:{1:0.00}</color>"
            , card.mdata.cd, card.mdata.cost, Sitem_33.HealthShow.healthColor[BasicAttID.cd], Sitem_33.HealthShow.healthColor[BasicAttID.cost]);

        if (card.mdata.useDis > 0)
            mdataText += string.Format("\n<color={1}>施法距离:{0:0.00}</color>"
                , card.mdata.useDis, Sitem_33.HealthShow.healthColor[BasicAttID.useDis]);
        if (card.mdata.moveUse)
            mdataText += string.Format("\n<color={0}>移动施法</color>"
                , Sitem_33.HealthShow.healthColor[BasicAttID.speed]);
        if (card.mdata.windDown == 0)
            mdataText += string.Format("\n<color={0}>瞬间施法</color>"
                , Sitem_33.HealthShow.healthColor[BasicAttID.cd]);

        ans = mdataText + "\n" + ans;
        ans = ans + "\n前摇：" + card.mdata.windUp + "\n后摇：" + card.mdata.windDown;
        return ans;
    }
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        Cmagicbase_17 card = _card as Cmagicbase_17;
        Dmagic config = DataManager.GetConfig<Dmagic>(id);
        if (config == null) card.mdata = new Dmagic();
        else card.mdata = new Dmagic(config);
        
    }
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagic, _MyUseMagic);
        AddHandle(MsgType.MyMagicBegin, _MyMagicBegin);
        Dmagic config = DataManager.GetConfig<Dmagic>(id);
        if (config != null)
            config.wxTag = SGRwx_4.GetItemWxTag(id);

        
    }
    public override void Clone(CardBase _from, CardBase _to)
    {
        base.Clone(_from, _to);
        Cmagicbase_17 ncard = _to as Cmagicbase_17;
        Cmagicbase_17 card = _from as Cmagicbase_17;
        ncard.mdata = new Dmagic(card.mdata);
    }
    public void _MyMagicBegin(CardBase _card, MsgBase _msg)
    {
        MsgMagicUse msg = _msg as MsgMagicUse;

        // 修正施法距离
        if(msg.mdata.useDis > 0&&TryGetCobj(msg.live, out var cobj))
        {
            Vector3 init = cobj.obj.transform.position;
            if(Vector3.Distance(init, msg.pos)> msg.mdata.useDis)
            {
                msg.pos = msg.mdata.useDis * (msg.pos - init).normalized + init;
            }
        }

        PlayMagicUseAnim(msg);
    }
    public void _MyUseMagic(CardBase _card, MsgBase _msg)
    {
        
        MyUseMagic(_card, _msg as MsgMagicUse);
    }
    public virtual void MyUseMagic(CardBase _card, MsgMagicUse msg) { }
    public override Color GetColor(CardBase _card)
    {
        return GoodUIColor.Magic;
    }
}