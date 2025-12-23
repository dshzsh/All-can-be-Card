using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGkndsj_44 : Citem_33
{
    public int conditionID;
    public int attID;

    public float time;

    public string Condition 
    { 
        get
        {
            var cv = SGkndsj_44.idToDes[conditionID];
            return cv.describe;
        } 
    }
    public AttAndRevise Revise 
    { 
        get 
        {
            var cv = SGkndsj_44.idToDes[conditionID];
            return new DbasicAtt.AttAndValue(attID, cv.value * pow).ToRevise();
        } 
    }
    public string exDes
    {
        get
        {
            var cv = SGkndsj_44.idToDes[conditionID];
            if (cv.time < 0) return "";
            else
                return $"\n *效果持续{DataManager.GetFloatText(cv.time)}s，不可叠加";
        }
    }
}
public class DGkndsj_44 : DataBase
{
    
}
public class SGkndsj_44 : Sitem_33
{
    public class ConditionAndValue
    {
        public static int conditionIDCnt = 0;
        public int conditionID;
        public int handleID;
        public CardMsgHandler handler;
        public string describe;
        public float value;
        public float time;
        public ConditionAndValue(int handleID, CardMsgHandler handler, string describe, float value, float time)
        {
            this.conditionID = conditionIDCnt;
            this.handleID = handleID;
            this.handler = handler;
            this.describe = describe;
            this.value = value;
            this.time = time;
            idToDes[conditionID] = this;
            conditionIDCnt++;
        }
    }
    public static Dictionary<int, ConditionAndValue> idToDes = new Dictionary<int, ConditionAndValue>();
    public static List<ConditionAndValue> handleList = new();
    public static List<int> attList = new();

    public const float interval = 0.5f;

    public override void Init()
    {
        base.Init();
        // 这是一个二级道具，所以一切以达成0.3的标准属性价值为标准
        handleList = new List<ConditionAndValue>()
        {
            new(MsgType.BeDamageAfter, BeDamageAfter, "受到伤害后", 0.4f, 2),
            new(MsgType.GiveDamageAfter, GiveDamageAfter, "造成伤害后", 0.3f, 2),
            new(MsgType.UseMagicAfter, UseMagicAfter, "释放技能后", 0.3f, 2),
            new(MsgType.BeHeal, BeHeal, "受到治疗后", 0.5f, 3),
            new(SLFfightBase_6.mTFightStart, FightStart, "战斗开始后", 0.5f, 10),
            new(MsgType.Update, UpdateLow, "生命低于一半时", 0.5f, -1),
            new(MsgType.Update, UpdateHigh, "生命高于一半时", 0.4f, -1),
        };
        attList = new List<int>(){ BasicAttID.atk, BasicAttID.atkSpeed, BasicAttID.def, BasicAttID.cdSpeed, BasicAttID.speed, BasicAttID.crit, BasicAttID.critDam };

        foreach(var cv in handleList)
        {
            AddHandle(cv.handleID, cv.handler);
        }
    }
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        CGkndsj_44 card = _card as CGkndsj_44;
        card.conditionID = Random.Range(0, ConditionAndValue.conditionIDCnt);
        card.attID = MyRandom.RandomInList(attList);
    }
    
    private void GiveBuff(CGkndsj_44 card)
    {
        CardBase live = GetTop(card);
        CFattChange_10 buff = CreateCard<CFattChange_10>();
        var cv = idToDes[card.conditionID];
        buff.attAndRevise = new DbasicAtt.AttAndValue(card.attID, cv.value * card.pow).ToRevise();
        float time = cv.time;
        if (time < 0) time = interval * 2;
        Sbuff_35.GiveBuff(live, live, new MsgBeBuff(buff, time, card.id, Sbuff_35.BeBuffMode.coverByBig));
    }
    void BeDamageAfter(CardBase _card, MsgBase _msg)
    {
        CGkndsj_44 card = _card as CGkndsj_44;

        if (card.conditionID != 0) return;

        GiveBuff(card);
    }
    void GiveDamageAfter(CardBase _card, MsgBase _msg)
    {
        CGkndsj_44 card = _card as CGkndsj_44;

        if (card.conditionID != 1) return;

        GiveBuff(card);
    }
    void UseMagicAfter(CardBase _card, MsgBase _msg)
    {
        CGkndsj_44 card = _card as CGkndsj_44;

        if (card.conditionID != 2) return;

        GiveBuff(card);
    }
    void BeHeal(CardBase _card, MsgBase _msg)
    {
        CGkndsj_44 card = _card as CGkndsj_44;

        if (card.conditionID != 3) return;

        GiveBuff(card);
    }
    void FightStart(CardBase _card, MsgBase _msg)
    {
        CGkndsj_44 card = _card as CGkndsj_44;

        if (card.conditionID != 4) return;

        GiveBuff(card);
    }
    void UpdateLow(CardBase _card, MsgBase _msg)
    {
        CGkndsj_44 card = _card as CGkndsj_44;

        if (card.conditionID != 5) return;

        MsgUpdate msg = _msg as MsgUpdate;

        if (MyTool.IntervalTime(interval, ref card.time, msg.time))
        {
            float nowH = Shealth_4.GetNowHealth(card);
            float maxH = Shealth_4.GetAttf(card, BasicAttID.healthMax);

            if (nowH <= maxH / 2)
                GiveBuff(card);
        }
        
    }
    void UpdateHigh(CardBase _card, MsgBase _msg)
    {
        CGkndsj_44 card = _card as CGkndsj_44;

        if (card.conditionID !=6) return;

        MsgUpdate msg = _msg as MsgUpdate;

        if (MyTool.IntervalTime(interval, ref card.time, msg.time))
        {
            float nowH = Shealth_4.GetNowHealth(card);
            float maxH = Shealth_4.GetAttf(card, BasicAttID.healthMax);

            if (nowH >= maxH / 2)
                GiveBuff(card);
        }
    }
}