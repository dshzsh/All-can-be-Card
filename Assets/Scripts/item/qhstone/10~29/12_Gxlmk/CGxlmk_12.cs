using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardManager;
using static SystemManager;
using static UnityEditor.Progress;

public class CGxlmk_12 : Citem_33
{
    public bool canMove = true;
    public AttAndRevise att;
    public bool autoEnd = true;// 时间达到上限自动停止蓄力

    public BarValue charge = new BarValue();

    public AnimationClip chargeAnim;

    public MsgMagicUse msg;

    [JsonIgnore]
    public GameObject chargeBar;
}
public class DGxlmk_12 : DataBase
{
    public BasicAtt chargeSpeedDown = new BasicAtt();
}
public class DMcharge : DataBase
{
    public float chargeTime;
    public string chargeAnimName = "null";

    [JsonIgnore]
    public AnimationClip chargeAnim;
    public override void Init(int id)
    {
        chargeAnim = DataManager.LoadResource<AnimationClip>(id, chargeAnimName);
    }
}
public class SGxlmk_12 : Sitem_33
{
    public static void AddXl(CardBase card, float time)
    {
        CGxlmk_12 charge = CreateCard<CGxlmk_12>();
        charge.charge.max = time;
        AddComponent(card, charge);
    }

    public override void Create(CardBase _card)
    {
        base.Create(_card);
        CGxlmk_12 card = _card as CGxlmk_12;
        DGxlmk_12 config = basicConfig as DGxlmk_12;

        card.att = new AttAndRevise(BasicAttID.speed, config.chargeSpeedDown);
        card.charge.cur = -MyMath.BigFloat;
    }
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MagicPress, MagicPress);
        AddHandle(MsgType.Update, Update);
        AddHandle(MsgType.OnItem, OnItem);
        AddHandle(MsgType.BeginUseMagic, BeginUseMagic, HandlerPriority.Before);
    }
    private bool IsInCharge(CGxlmk_12 card)
    {
        return card.charge.cur >= 0;
    }
    private void BeginCharge(CGxlmk_12 card, MsgMagicUse msg)
    {
        //蓄力动画播放
        if (TryGetCobj(card, out var cobj))
        {
            SendMsg(msg.live, MsgType.RotateControl, new MsgRotateControl { seeTo = msg.pos, interrupt = true });

            SObj_2.PlayAtkAnim(cobj, card.chargeAnim, card.charge.max, card.canMove);
        }

        //开始计时
        card.charge.cur = 0;
        card.msg = msg;
        msg.valid = false;
        card.att.UseOnLive(card, 1);

        if(cobj != null)
        {
            card.chargeBar = UIBasic.GiveLiveUpUI(DataManager.GetConfig<DObj_2>(id).gameObject, cobj);
            card.chargeBar.GetComponent<BasicBar>().Set(card.charge);
        }
        
    }
    private void EndCharge(CGxlmk_12 card)
    {
        if (!IsInCharge(card)) return;

        if (TryGetCobj(card, out var cobj))
        {
            cobj.obj.SetLockMotion(0);
        }
        card.msg = null;
        card.charge.cur = -MyTool.BigFloat;
        card.att.UseOnLive(card, -1);

        if (card.chargeBar != null)
            GameObject.Destroy(card.chargeBar);
    }
    void BeginUseMagic(CardBase _card, MsgBase _msg)
    {
        CGxlmk_12 card = _card as CGxlmk_12;
        MsgMagicUse msg = _msg as MsgMagicUse;

        if (!Sqhc_38.IsQhMagic(card, msg.magic))
        {
            //打断自身的蓄力释放
            EndCharge(card);
            return;
        }

        //如果已经有蓄力的内容，结束内容
        if (msg.TryGetTag<CchargeTime_44>(out var _)) return;

        BeginCharge(card, msg);
    }
    private void EndChargeAndAddTime(CGxlmk_12 card, MsgMagicUse msg, float chargeTime)
    {
        if (msg == null || msg.doWindDown) return;
        if (!Sqhc_38.IsQhMagic(card, msg.magic)) return;
        if (msg.TryGetTag<CchargeTime_44>(out var _)) return;

        if (chargeTime > card.charge.max) chargeTime = card.charge.max;

        CchargeTime_44 cc = CreateCard<CchargeTime_44>();
        cc.percent = chargeTime / card.charge.max;
        cc.time = chargeTime;
        msg.AddTag(cc);
        msg.valid = true;
        msg.pos = SShoulderCamera_37.GetLookPos(card);
        EndCharge(card);
        Smagic_14.UseMagicEntirely(msg);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CGxlmk_12 card = _card as CGxlmk_12;
        MsgOnItem msg = _msg as MsgOnItem;

        if (msg.op == -1)
            EndCharge(card);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CGxlmk_12 card = _card as CGxlmk_12;
        MsgUpdate msg = _msg as MsgUpdate;

        card.charge.cur += msg.time;
        if (card.charge.cur >= card.charge.max)
            card.charge.cur = card.charge.max;
        if (card.charge.cur >= card.charge.max && card.autoEnd)//时间到了结算
        {
            EndChargeAndAddTime(card, card.msg, card.charge.max);
        }
    }
    void MagicPress(CardBase _card, MsgBase _msg)
    {
        CGxlmk_12 card = _card as CGxlmk_12;
        MsgMagicPress msg = _msg as MsgMagicPress;

        if (!Sqhc_38.IsQhMagic(card, msg.magic)) return;

        if (msg.isStart) // 如果没有开始按住的行为，则会自动结束；没有开始按住的行为就是怪物使用蓄力技能
            card.autoEnd = false;

        if (IsInCharge(card))
        {
            //持续转向当前施法位置
            SendMsg(GetTop(card), MsgType.RotateControl, new MsgRotateControl
            {
                seeTo = SShoulderCamera_37.GetLookPos(card),
                interrupt = true
            });
        }

        if (msg.isEnd && card.msg != null)//松开蓄力结算
        {
            EndChargeAndAddTime(card, card.msg, msg.time);
        }
    }
}