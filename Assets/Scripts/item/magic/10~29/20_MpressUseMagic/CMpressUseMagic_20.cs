using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;
using Newtonsoft.Json;

public class CMpressUseMagic_20 : Cmagicbase_17
{
    [JsonIgnore]
    public GameObject powerDraw;
}
public class DMpressUseMagic_20 : DataBase
{
    public bool rotate = false;
    public string powerDrawName;
    [JsonIgnore]
    public GameObject powerDraw;
    public override void Init(int id)
    {
        powerDraw = DataManager.LoadResource<GameObject>(id, powerDrawName);
    }
}
public class SMpressUseMagic_20 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyMagicInterrupt, MyMagicInterrupt);
        AddHandle(MsgType.MagicPress, MagicPress);
        AddHandle(MsgType.MyMagicBegin, MyMagicBegin);
        AddHandle(MsgType.MyMagicEnd, MyMagicEnd);
    }
    void MyMagicBegin(CardBase _card, MsgBase _msg)
    {
        CMpressUseMagic_20 card = _card as CMpressUseMagic_20;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DMpressUseMagic_20 config = DataManager.GetConfig<DMpressUseMagic_20>(id);

        if (config.powerDraw != null && TryGetCobj(card, out var cobj))
        {
            //给予释法蓄力特效
            GiveOnTx(cobj, config.powerDraw, ref card.powerDraw, 1, true);
        }
    }
    
    void MagicPress(CardBase _card, MsgBase _msg)
    {
        CMpressUseMagic_20 card = _card as CMpressUseMagic_20;
        MsgMagicPress msg = _msg as MsgMagicPress;
        DMpressUseMagic_20 config = DataManager.GetConfig<DMpressUseMagic_20>(id);

        //Debug.Log(msg.magic);
        if (msg.magic != card || !msg.isNowUse) return;

        //持续转向当前施法位置
        if(config.rotate)
        {
            SendMsg(GetTop(card), MsgType.RotateControl, new MsgRotateControl
            {
                seeTo = SShoulderCamera_37.GetLookPos(card),
                interrupt = true
            });
        }

        if (msg.isEnd)
        {
            //打断魔法的释放
            Smagic_14.InterruptNowMagic(card);
        }
    }
    void MyMagicInterrupt(CardBase _card, MsgBase _msg)
    {
        End(_card, _msg);
    }
    void MyMagicEnd(CardBase _card, MsgBase _msg)
    {
        End(_card, _msg);
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMpressUseMagic_20 card = _card as CMpressUseMagic_20;

        //开始激光的释放，清除蓄力特效
        if (TryGetCobj(card, out var cobj))
        {
            //清除蓄力特效
            GiveOnTx(cobj, null, ref card.powerDraw, -1, true);
        }
        BeginPressUse(_card, msg);
    }
    void End(CardBase _card, MsgBase _msg)
    {
        CMpressUseMagic_20 card = _card as CMpressUseMagic_20;
        //Debug.Log("end");
        //打断激光的释放，清除蓄力特效
        if (TryGetCobj(card, out var cobj))
        {
            //清除蓄力特效
            GiveOnTx(cobj, null, ref card.powerDraw, -1, true);
        }

        EndPressUse(_card, _msg);
    }
    public virtual void BeginPressUse(CardBase _card, MsgMagicUse msg) { }
    public virtual void EndPressUse(CardBase _card, MsgBase _msg) { }
}