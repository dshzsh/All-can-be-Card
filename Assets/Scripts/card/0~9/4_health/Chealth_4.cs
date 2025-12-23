using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;
using Newtonsoft.Json;

public class BarValue
{
    public float cur = 1;
    public float max = 1;
}

public class Chealth_4 : CardBase
{
    public bool killed = false;
    public float nowHealth;
    public float nowMana;

    public BasicAtt[] atts = new BasicAtt[BasicAttID.MAX_ATT_NUM];
    public CGAattbase_1[] attCards = new CGAattbase_1[BasicAttID.MAX_ATT_NUM];

    public BasicAtt GetAtt(int attID)
    {
        if (atts[attID] == null)
        {
            atts[attID] = new BasicAtt();
            if (BasicAttID.NeedCard(attID))
            {
                attCards[attID] = CreateCard(BasicAttID.GetAttPid(attID)) as CGAattbase_1;
                attCards[attID].bvalue = atts[attID];
                ActiveComponent(this, attCards[attID]);
            }
        }
        return atts[attID];
    }
    public float GetAttf(int attID)
    {
        if (atts[attID] == null) return 0;
        return atts[attID].GetValue();
    }
    [JsonIgnore]
    public GameObject healthBarUI;
    [JsonIgnore]
    public GameObject healthBar;
}
public class Dhealth_4 :DataBase
{
    
    public string healthBarUIName;
    public string healthBarName;
    public string liveUpName;
    public string textObjName;
    public string textUIName;
    public string errorTextName;

    public GameObject healthBarUI;
    public GameObject healthBar;
    public GameObject liveUp;
    public GameObject textObj;
    public GameObject textUI;
    public GameObject errorText;
    public override void Init(int id)
    {
        healthBarUI = DataManager.LoadResource<GameObject>(id, healthBarUIName);
        healthBar = DataManager.LoadResource<GameObject>(id, healthBarName);
        liveUp = DataManager.LoadResource<GameObject>(id, liveUpName);
        textObj = DataManager.LoadResource<GameObject>(id, textObjName);
        textUI = DataManager.LoadResource<GameObject>(id, textUIName);
        errorText = DataManager.LoadResource<GameObject>(id, errorTextName);
    }
}
public class MsgBeDamage : MsgBaseWithTag
{
    public CardBase from;
    public CardBase to;
    public float damage;
    public Vector3 damagePos = default;
    public string exString = "";
    public bool isKill = false;
    public MsgBeDamage() { }
    public MsgBeDamage(float damage, int wxTag = -1, Vector3 damagePos = default)
    {
        this.damage = damage;
        this.damagePos = damagePos;
        if (wxTag != -1)
        {
            CTTdamType_2 tag = CreateCard<CTTdamType_2>();
            tag.wxTag = wxTag;
            AddTag(tag);
        }
    }
    public void AddStr(string str)
    {
        this.exString += str;
    }
}
public class MsgBeHeal : MsgBase
{
    public CardBase from;
    public CardBase to;
    public float heal;
    public Vector3 healPos;
    public string exString = "";
    public MsgBeHeal() { }
    public MsgBeHeal(float heal)
    {
        this.heal = heal;
    }
    public void AddStr(string str)
    {
        this.exString += str;
    }
}
public class MsgCostHealth : MsgBase
{
    public float heal;
    public Vector3 healPos;
    public MsgCostHealth() { }
    public MsgCostHealth(float cost)
    {
        this.heal = cost;
    }
}
public class MsgDie : MsgBase
{
    
}
public class Shealth_4 : SystemBase
{
    public static BasicAtt GetAtt(CardBase card, int attId)
    {
        if (!TryGetCobj(card, out var cobj)) return new BasicAtt();
        if (cobj.myHealth == null) return new BasicAtt();
        return cobj.myHealth.GetAtt(attId);
    }
    //并不会创建没有的属性，与上一个函数有本质区别
    public static float GetAttf(CardBase card, int attId)
    {
        if (!TryGetCobj(card, out var cobj)) return 0;
        if (cobj.myHealth == null) return 0;
        return cobj.myHealth.GetAttf(attId);
    }
    public static Chealth_4 GetHealth(CardBase card)
    {
        if (!TryGetCobj(card, out var cobj)) return null;
        return cobj.myHealth;
    }
    public static float GetNowHealth(CardBase card, bool percent = false)
    {
        Chealth_4 chealth_4 = GetHealth(card);
        if (chealth_4 == null) return 0;
        if (percent) return chealth_4.nowHealth / chealth_4.GetAttf(BasicAttID.healthMax);
        return chealth_4.nowHealth;
    }
    public static bool HaveHealth(CardBase card)
    {
        if (card == null) return false;
        if (!TryGetCobj(card, out var cobj)) return false;
        return cobj.myHealth != null;
    }
    public static void GiveDamage(CardBase from, CardBase to, MsgBeDamage msg)
    {
        msg.from = from;msg.to = to;

        if(msg.damagePos==default)
        {
            if (TryGetCobj(to, out var cobj))
                msg.damagePos = cobj.obj.Center;
        }

        SendMsg(from, MsgType.GiveDamageBefore, msg);// 暴击结算
        SendMsg(to, MsgType.BeDamageBefore, msg);// 防御结算
        SendMsg(from, MsgType.GiveDamage, msg);
        SendMsg(to, MsgType.BeDamage, msg);// 受到伤害 before:护盾结算
        SendMsg(from, MsgType.GiveDamageAfter, msg);
        SendMsg(to, MsgType.BeDamageAfter, msg);
    }
    public static void GiveHeal(CardBase from, CardBase to, MsgBeHeal msg)
    {
        msg.from = from; msg.to = to;
        if (msg.healPos == default)
        {
            if (TryGetCobj(to, out var cobj))
                msg.healPos = cobj.obj.Center;
        }
        SendMsg(from, MsgType.GiveHeal, msg);
        SendMsg(to, MsgType.BeHeal, msg);
    }
    
    public override void Clone(CardBase _from, CardBase _to)
    {
        base.Clone(_from, _to);
        Chealth_4 cfrom = _from as Chealth_4;
        Chealth_4 nto = _to as Chealth_4;

        nto.nowHealth = cfrom.nowHealth;
        nto.nowMana = cfrom.nowMana;
        for(int i=0;i<nto.atts.Length;i++)
        {
            if (cfrom.atts[i] == null) continue;
            nto.atts[i] = new BasicAtt(cfrom.atts[i]);
            if (BasicAttID.NeedCard(i))
            {
                nto.attCards[i] = CreateCard(BasicAttID.GetAttPid(i)) as CGAattbase_1;
                nto.attCards[i].bvalue = nto.atts[i];
                ActiveComponent(nto, nto.attCards[i]);
            }
        }
    }
    public override void Init()
    {
        AddHandle(MsgType.BeDamage, BeDamage);
        AddHandle(MsgType.BeHeal, BeHeal);
        AddHandle(MsgType.RestoreMana, RestoreMana);
        AddHandle(MsgType.CostMana, CostMana);
        AddHandle(MsgType.CostHealth, CostHealth);
        AddHandle(MsgType.OnShowUI, OnShowUI);
        AddHandle(MsgType.OnItem, OnItem);
        AddHandle(MsgType.Update, Update);
        AddHandle(MsgType.ParseDescribe, ParseDescribe);

        Dhealth_4 config = basicConfig as Dhealth_4;
        UIBasic.textObj = config.textObj;
        UIBasic.textUI = config.textUI;
        UIBasic.liveUpUIPrefab = config.liveUp;
        UIBasic.errorTextObj = config.errorText;
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        Chealth_4 card = _card as Chealth_4;
        MsgOnItem msg = _msg as MsgOnItem;
        Dhealth_4 config = basicConfig as Dhealth_4;

        if (!TryGetCobj(card, out var cobj)) return;

        if (msg.op == 1)
        {
            card.healthBar = UIBasic.GiveLiveUpUI(config.healthBar, cobj);
            card.healthBar.GetComponent<HealthBar>().Set(cobj);
            cobj.myHealth = card;
        }
        else
        {
            if (card.healthBar != null)
                GameObject.Destroy(card.healthBar);
            cobj.myHealth = null;
        }
    }
    void OnShowUI(CardBase _card, MsgBase _msg)
    {
        Chealth_4 card = _card as Chealth_4;
        MsgOnShowUI msg = _msg as MsgOnShowUI;
        Dhealth_4 config = basicConfig as Dhealth_4;

        if(msg.op==1)
        {
            if(TryGetCobj(_card, out var cobj))
            {
                card.healthBarUI = UIBasic.GiveLeftUpUI(config.healthBarUI);
                card.healthBarUI.GetComponent<HealthBar>().Set(cobj);
            }
            // Debug.Log("aaa");
        }
        else
        {
            if (card.healthBarUI != null)
                GameObject.Destroy(card.healthBarUI);
        }
    }
    private bool TryDie(CardBase cobj, Chealth_4 card)
    {
        if (card.nowHealth <= 0 && !card.killed)
        {
            MsgDie dieMsg = new MsgDie();
            SendMsg(cobj, MsgType.Die, dieMsg);
            if (dieMsg.valid)
            {
                card.killed = true;
                return true;
            }
        }
        return false;
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        Chealth_4 card = _card as Chealth_4;
        MsgUpdate msg = _msg as MsgUpdate;

        TryDie(msg.cobj, card);
    }
    void BeDamage(CardBase _card, MsgBase _msg)
    {
        Chealth_4 card = _card as Chealth_4;
        MsgBeDamage msg = _msg as MsgBeDamage;

        card.nowHealth -= msg.damage;

        GiveDamageText(msg, msg.damage);

        //死亡结算
        msg.isKill = TryDie(msg.to, card);
    }
    public const float NoShowTextHealthRate = 0.001f;
    void BeHeal(CardBase _card, MsgBase _msg)
    {
        Chealth_4 card = _card as Chealth_4;
        MsgBeHeal msg = _msg as MsgBeHeal;

        if (msg.heal + card.nowHealth > card.GetAttf(BasicAttID.healthMax))
            msg.heal = card.GetAttf(BasicAttID.healthMax) - card.nowHealth;

        card.nowHealth += msg.heal;
        if (msg.heal >= card.GetAttf(BasicAttID.healthMax) * NoShowTextHealthRate)
        {
            GiveHealText(card, msg);
        }
    }
    void RestoreMana(CardBase _card, MsgBase _msg)
    {
        Chealth_4 mymagic = _card as Chealth_4;
        MsgRestoreMana msg = _msg as MsgRestoreMana;

        mymagic.nowMana += msg.value;
        if (mymagic.nowMana > mymagic.GetAttf(BasicAttID.manaMax)) mymagic.nowMana = mymagic.GetAttf(BasicAttID.manaMax);
    }
    void CostMana(CardBase _card, MsgBase _msg)
    {
        Chealth_4 mymagic = _card as Chealth_4;
        MsgCostMana msg = _msg as MsgCostMana;

        if (mymagic.nowMana >= msg.cost)
        {
            mymagic.nowMana -= msg.cost;
            msg.ok = true;
        }
    }
    void CostHealth(CardBase _card, MsgBase _msg)
    {
        Chealth_4 card = _card as Chealth_4;
        MsgCostHealth msg = _msg as MsgCostHealth;

        card.nowHealth -= msg.heal;
    }
    public static float GetNumSize(float num)
    {
        float ans = Mathf.Sqrt(num) - 0.1f;
        if (ans >= 1) ans = Mathf.Sqrt(ans);
        return Mathf.Clamp(ans, 0.3f, 3f);
    }
    public static void GiveDamageText(MsgBeDamage msg, float damage)
    {
        Color textColor = Color.red;
        if(msg.TryGetTag<CTTdamType_2>(out var damType))
        {
            textColor = SGRwx_4.GetLightColor(damType.wxTag);
        }
        if (msg.HaveTag(STTzssh_3.tid))
        {
            textColor = Color.white;
        }
        const float randShift = 0.25f;
        UIBasic.GiveText(msg.exString + MyTool.SuperNumText(damage)
            , msg.damagePos + randShift * Random.insideUnitSphere, textColor, GetNumSize(damage));
    }
    private void GiveHealText(Chealth_4 mylive, MsgBeHeal msg)
    {
        UIBasic.GiveText("+" + msg.exString + MyTool.SuperNumText(msg.heal)
            , msg.healPos, Color.green, GetNumSize(msg.heal));
    }
    void ParseDescribe(CardBase _card, MsgBase _msg)
    {
        Chealth_4 card = _card as Chealth_4;
        MsgParseDescribe msg = _msg as MsgParseDescribe;
        Dhealth_4 config = basicConfig as Dhealth_4;

        if (msg.card != card && msg.card != card.parent) return;

        string ans = "";
        for(int i = 0; i < card.atts.Length; i++)
        {
            if (card.atts[i] == null) continue;
            //不显示几乎为0的数据
            if (card.atts[i].GetValue() >= -MyMath.SmallFloat && card.atts[i].GetValue() <= MyMath.SmallFloat)
                continue;
            string attCardStr;
            if (card.attCards[i] != null)
            {
                attCardStr = Cstr(card.attCards[i], noUnderline: true);
            }
            else
            {
                CGAattbase_1 attCard = CreateCard(BasicAttID.GetAttPid(i)) as CGAattbase_1;
                attCard.bvalue = card.atts[i];
                attCardStr = Cstr(attCard, noUnderline: true);
            }
            switch (i)
            {
                case BasicAttID.healthMax:
                    ans = ans + "<color=" + Sitem_33.HealthShow.healthColor[BasicAttID.healthMax] + ">"
                    + attCardStr + "：" + MyTool.SuperNumText(card.nowHealth) + "/" + MyTool.SuperNumText(card.GetAttf(i)) + "</color>\n";
                    break;
                case BasicAttID.manaMax:
                    ans = ans + "<color=" + Sitem_33.HealthShow.healthColor[BasicAttID.cost] + ">"
                    + attCardStr + "：" + MyTool.SuperNumText(card.nowMana) + "/" + MyTool.SuperNumText(card.GetAttf(i)) + "</color>\n";
                    break;
                case BasicAttID.def:
                    ans = ans + "<color=" + Sitem_33.HealthShow.healthColor[i] + ">" +
                         attCardStr + "：" + card.atts[i].AttString() +
                        " (减伤：" + string.Format("{0:0.00}", 100 - 100 * SGAdef_5.DefDamageReduction(card.atts[i].GetValue())) + "%)" + "</color>\n";
                    break;
                default:
                    {
                        string attNumStr;
                        if (BasicAttID.attData[i].percentShow)
                            attNumStr = MyTool.SuperNumText(card.atts[i].GetValue() * 100) + "%";
                        else attNumStr = card.atts[i].AttString();
                        ans = ans + "<color=" + Sitem_33.HealthShow.healthColor[i] + ">" +
                        attCardStr + "：" + attNumStr + "</color>\n";
                        break;
                    }
            }
            
        }
        
        msg.text = Sitem_33.InsertField(msg.text, "基本属性", Sitem_33.MaskColor.MK, ans);
    }
}