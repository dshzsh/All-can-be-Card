using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardManager;
using static MsgMagicUse;
using static SystemManager;

public class Cmagic_14 : CardBase
{
    public MsgMagicUse nowUse;

    public int holdMax = 0; // 0 ~ holdMax-1 为有效的魔法
    public List<CardBase> magics = new();
    public List<float> cdBear = new(); // 正在承受的冷却
    public List<float> cdBearMax = new(); // 正在承受的冷却

    // 标记技能是否能使用，统一无法施法来做显示的效果
    public int magicCanUseMeta = 1;
    public List<int> magicCanUse = new();

    public CardBase tempMagic = new();

    [JsonIgnore]
    public GameObject magicPanel;
    [JsonIgnore]
    public List<GameObject> magicPanelKeys = new();
    [JsonIgnore]
    public GameObject magicBarUI;
    [JsonIgnore]
    public GameObject useBar;
    [JsonIgnore]
    public BarValue useWind = new();
    public List<CardBase> GetMagics()
    {
        List<CardBase> ans = new();
        for (int i = 0; i < holdMax; i++)
            ans.Add(magics[i]);
        return ans;
    }
}
public class Dmagic_14:DataBase
{
    public string magicPanelName;
    public string magicKeyName;
    public string magicBarName;
    public string useBarName;

    [JsonIgnore]
    public GameObject magicPanel;
    [JsonIgnore]
    public GameObject magicKey;
    [JsonIgnore]
    public GameObject magicBarUI;
    [JsonIgnore]
    public GameObject useBar;
    public override void Init(int id)
    {
        magicPanel = DataManager.LoadResource<GameObject>(id, magicPanelName);
        magicKey = DataManager.LoadResource<GameObject>(id, magicKeyName);
        magicBarUI = DataManager.LoadResource<GameObject>(id, magicBarName);
        useBar = DataManager.LoadResource<GameObject>(id, useBarName);
    }
}
public class MsgMagicCon : MsgBase
{
    public CObj_2 cobj;
    public int key;
    public Vector3 pos;

    public MsgMagicCon() { }
    public MsgMagicCon(int key, Vector3 pos)
    {
        this.key = key;
        this.pos = pos;
    }
}
public class MsgMagicUse : MsgBaseWithTag
{
    public enum AddCardType
    {
        none,
        bullet,
        ysw
    }

    public CObj_2 live;
    public CardBase magic;
    public bool doWindDown = false;
    public bool doWindUp = false;
    public int key;
    public Vector3 pos;
    public float pow = 1;
    public float instantAtkSpeed = 1f;
    public bool isConUse = false;//是否通过magicCon来进行的使用（主动直接使用和通过效果使用的区别）

    public int bdUse = 0;// 使用被动效果触发的技能，和主动使用的效果不一样，用在会产生魔法物体的被动效果上，0为普通的主动技能

    public int costKey;
    public CardBase costLive;
    public bool isNoCost = false;

    public Dmagic mdata;
    public Dictionary<AddCardType, List<CardBase>> mks = new Dictionary<AddCardType, List<CardBase>>();
    public MsgMagicUse() { }
    public MsgMagicUse(MsgMagicUse other)
    {
        live = other.live;
        magic = other.magic;
        key = other.key;
        pos = other.pos;
        pow = other.pow;
        mdata = new Dmagic(other.mdata);
        mks = other.mks;
        instantAtkSpeed = other.instantAtkSpeed;
        triList = other.triList;
        triPos = other.triPos;
        costKey = other.costKey;
        costLive = other.costLive;
        isNoCost = other.isNoCost;
        bdUse = other.bdUse;
    }
    public MsgMagicUse(CObj_2 live, CardBase magic, Vector3 pos, Dictionary<AddCardType, List<CardBase>> mks = null, int bdUse = 0, bool noCost = false)
    {
        this.live = live; costLive = live;
        this.magic = magic;
        if (magic is Cmagicbase_17 mmagic)
        {
            mdata = new Dmagic(mmagic.mdata);
            pow = mmagic.pow;
        }
        else mdata = new Dmagic();
        this.key = Smagic_14.TempKey;
        costKey = Smagic_14.TempKey;
        this.pos = pos;
        if (mks != null)
            this.mks = mks;
        instantAtkSpeed = Shealth_4.GetAttf(live, BasicAttID.atkSpeed);
        this.bdUse = bdUse;
        if (noCost || bdUse != 0)
            ToNoCost();
    }
    public void AddMk(AddCardType type, CardBase card)
    {
        List<CardBase> addcards = mks.GetValueOrDefault(type, new List<CardBase>());
        addcards.Add(card);
        mks[type] = addcards;
    }
    public bool TryGetMK<T>(AddCardType type,out T card) where T : CardBase
    {
        List<CardBase> addcards = mks.GetValueOrDefault(type, null);
        if (addcards == null)
        {
            card = null;
            return false;
        }
        foreach(CardBase com in addcards)
        {
            if(com is T)
            {
                card = com as T;
                return true;
            }
        }
        card = null;
        return false;
    }
    public void ToNoCost()
    {
        mdata.cost = 0;
        mdata.cd = 0;
        isNoCost = true;
        costKey = Smagic_14.TempKey;
    }
}
public class MsgMagicOn : MsgBase
{
    public int key;
    public CardBase magic;
    public int op;
    public bool ok = false;

    public MsgMagicOn() { }
    public MsgMagicOn(CardBase magic, int op, int key = -1)
    {
        this.magic = magic;
        this.op = op;
        this.key = key;
    }
}
public class MsgOnItem : MsgBase
{
    public CardBase item;
    public int op;
    public MsgOnItem() { }
    public MsgOnItem(CardBase item, int op)
    {
        this.item = item;
        this.op = op;
    }
}
public class MsgRestoreMana:MsgBase
{
    public float value;
    public MsgRestoreMana(float value)
    {
        this.value = value;
    }
}
public class MsgRestoreCd:MsgBase
{
    public float value = 0;
    public int key = -1;
    public bool isPercent = false;
    public float resTime = 0;// 剩下来溢出的冷却时间
    public CardBase magic;

    public bool IsMyMagic(CardBase magic)
    {
        return (magic == this.magic);
    }
}
public class MsgMagicInterrupt : MsgBase
{
    public CardBase magic;
    public bool interruptNow = false;
}
public class MsgCostMana:MsgBase
{
    public float cost = 0f;
    public bool ok = false;
    public MsgCostMana(float cost)
    {
        this.cost = cost;
    }
    public MsgCostMana() { }
}
public class Smagic_14 : SystemBase
{
    public static int TempKey = 1000;
    public static List<CardBase> GetAllMagics(Cmagic_14 myMagic)
    {
        return myMagic.magics;
    }
    public static void InterruptNowMagic(CardBase magic)
    {
        MsgMagicInterrupt msg = new MsgMagicInterrupt() { magic = magic };
        SendMsg(GetTop(magic), MsgType.MagicInterrupt, msg);
    }
    public static void InterruptLiveNowMagic(CardBase card)
    {
        MsgMagicInterrupt msg = new MsgMagicInterrupt() { interruptNow = true };
        SendMsg(GetTop(card), MsgType.MagicInterrupt, msg);
    }
    public static void RecoverMagicCd(CardBase magic, float time = 1f, bool isPercent = true)
    {
        SendMsg(GetTop(magic), MsgType.RestoreCd, new MsgRestoreCd() { magic = magic, value = time, isPercent = isPercent });
    }
    public static void RecoverMagicCd(int key, CardBase live ,float time = 1f, bool isPercent = true)
    {
        SendMsg(GetTop(live), MsgType.RestoreCd, new MsgRestoreCd() { key = key, value = time, isPercent = isPercent });
    }
    public static void RecoverAllMagicCd(CardBase live, float time = 1f, bool isPercent = true)
    {
        if (!TryGetCobj(live, out var cobj)) return;
        if (cobj.myMagic == null) return;
        for (int i = 0; i < cobj.myMagic.holdMax; i++)
            RecoverMagicCd(i, live, time, isPercent);
    }
    public static void ChangeHoldMax(Cmagic_14 mymagic, int op)
    {
        mymagic.holdMax += op;
        if (op==1)
        {
            mymagic.magics.Add(ContainedCnull(mymagic));
            mymagic.cdBear.Add(0f);
            mymagic.cdBearMax.Add(1f);
            mymagic.magicCanUse.Add(1);
        }
        else
        {
            CardBase magic = mymagic.magics[mymagic.holdMax];
            mymagic.magics.RemoveAt(mymagic.holdMax);
            mymagic.cdBear.RemoveAt(mymagic.holdMax);
            mymagic.cdBearMax.RemoveAt(mymagic.holdMax);
            mymagic.magicCanUse.RemoveAt(mymagic.holdMax);
            InactiveComponent(mymagic, magic);
            if (magic.id != 0)
                Sbag_40.LiveGetItem(mymagic, magic);
        }
    }
    // 主要用于显示这个键是否能用的透明的效果
    public static bool CanUseMagic(Cmagic_14 card, int key)
    {
        if (card.magicCanUseMeta != 1) return false;
        if (key == TempKey) return true;
        return card.magicCanUse[key] == 1;
    }

    public override void Init()
    {
        AddHandle(MsgType.Update, Update);
        AddHandle(MsgType.MagicCon, MagicConGet);
        AddHandle(MsgType.SelfContainerGet, SelfContainerGet);
        AddHandle(MsgType.SelfContainerAllItem, SelfContainerAllItem);
        AddHandle(MsgType.MagicBegin, MagicBegin); // 判断耗魔、冷却决定能不能启用技能
        AddHandle(MsgType.BeginUseMagic, BeginUseMagic);
        AddHandle(MsgType.RestoreCd, RestoreCd);
        AddHandle(MsgType.UseMagicBefore, UseMagicBeforeCost);
        AddHandle(Sbag_40.mTOpenBag, OpenBag, HandlerPriority.After);
        AddHandle(MsgType.MagicPress, MagicPress, HandlerPriority.Highest);
        AddHandle(MsgType.MagicInterrupt, MagicInterrupt);
        AddHandle(MsgType.OnItem, OnItem);
        AddHandle(MsgType.SelfContainerJudge, MyContainerJudge);

        AddHandle(MsgType.OnShowUI, ShowMagicUI);
        AddHandle(MsgType.ParseDescribe, ParseDescribe);
        Dmagic_14 config = basicConfig as Dmagic_14;
        MagicPanel.magicKeyObj = config.magicKey;
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        Cmagic_14 card = _card as Cmagic_14;
        MsgOnItem msg = _msg as MsgOnItem;

        if(msg.op==1)
        {
            if (TryGetCobj(card, out var cobj))
                cobj.myMagic = card;
        }
        else
        {
            if (card.useBar != null)
                GameObject.Destroy(card.useBar);
        }
    }
    
    void MagicInterrupt(CardBase _card, MsgBase _msg)
    {
        Cmagic_14 mymagic = _card as Cmagic_14;
        MsgMagicInterrupt msg = _msg as MsgMagicInterrupt;

        if (mymagic.nowUse == null) return;

        if (msg.magic != mymagic.nowUse.magic && msg.interruptNow == false) return;

        msg.magic = mymagic.nowUse.magic;
        mymagic.nowUse.doWindUp = true;//视为已经完成前摇
        mymagic.nowUse.mdata.windDown = 0f;//立刻结束后摇，能不能进入后摇判定看有没有衔接魔法
        SendMsg(mymagic.nowUse.magic, MsgType.MyMagicInterrupt, msg);
    }
    void MagicPress(CardBase _card, MsgBase _msg)
    {
        Cmagic_14 mymagic = _card as Cmagic_14;
        MsgMagicPress msg = _msg as MsgMagicPress;

        if (msg.key >= mymagic.holdMax || msg.key >= mymagic.magics.Count) return;

        msg.magic = mymagic.magics[msg.key];
        if(mymagic.nowUse!=null && msg.magic== mymagic.nowUse.magic && !mymagic.nowUse.doWindDown)
        {
            msg.isNowUse = true;
            msg.nowUse = mymagic.nowUse;
        }
    }
    void RestoreCd(CardBase _card, MsgBase _msg)
    {
        Cmagic_14 mymagic = _card as Cmagic_14;
        MsgRestoreCd msg = _msg as MsgRestoreCd;

        //优先根据magic匹配key值
        if (msg.key == -1)
        {
            for (int i = 0; i < mymagic.magics.Count; i++)
                if (msg.magic == mymagic.magics[i]) msg.key = i;
            if (msg.key == -1) return;
        }
        
        msg.magic = mymagic.magics[msg.key];//自动适应卸下的magic
        float cdMax = 1;
        if (msg.magic is Cmagicbase_17 cmagic)
            cdMax = cmagic.mdata.cd;
        float time = msg.value * (msg.isPercent ? cdMax : 1);
        mymagic.cdBear[msg.key] -= time;
        if (mymagic.cdBear[msg.key] < 0)
        {
            msg.resTime = -mymagic.cdBear[msg.key];
            mymagic.cdBearMax[msg.key] = MyMath.SmallFloat;
            mymagic.cdBear[msg.key] = 0;
        }
    }
    
    void MyContainerJudge(CardBase _card, MsgBase _msg)
    {
        Cmagic_14 card = _card as Cmagic_14;
        MsgContainerJudge msg = _msg as MsgContainerJudge;

        if (card != msg.gmsg.container) return;

        if (msg.gmsg.pos < TempKey && (msg.gmsg.pos >= card.holdMax || msg.gmsg.pos < 0))
        {
            msg.ok = false;
            msg.AddMsg("超过上限");
        }

        if (msg.gmsg.op == 1)
        {
            if (Smagicbase_17.IsMagic(msg.gmsg.item))
            {
                msg.ok = true;
                return;
            }
            msg.ok = false;
            msg.AddMsg("只能装备魔法");
        }
        else
        {
            msg.gmsg.pos = card.magics.IndexOf(msg.gmsg.item);
            msg.ok = msg.gmsg.pos != -1;
        }

    }
    // 默认先off再on，不会出现on的时候还没有off的情况
    void SelfContainerGet(CardBase _card, MsgBase _msg)
    {
        Cmagic_14 mymagic = _card as Cmagic_14;
        MsgGetItem msg = _msg as MsgGetItem;        

        //Debug.Log(msg.key + " " + msg.op);

        if (msg.op == 1) // on
        {
            if (msg.pos == TempKey)
            {
                mymagic.tempMagic = msg.item;
                msg.item.container = mymagic;
                ActiveComponent(mymagic, msg.item);
                return;
            }

            if (msg.pos >= mymagic.holdMax || msg.pos < 0) return;

            mymagic.magics[msg.pos] = msg.item;
            msg.item.container = mymagic;
            ActiveComponent(mymagic, msg.item, -msg.pos);
        }
        else // off
        {
            //优先根据magic匹配key值
            if (msg.item != null)
            {
                bool ok = false;
                if (msg.item == mymagic.tempMagic)
                {
                    msg.pos = TempKey;
                    ok = true;
                }
                for (int i = 0; i < mymagic.magics.Count; i++)//自动适应卸下的key值并返回key值
                    if (msg.item == mymagic.magics[i])
                    {
                        msg.pos = i;
                        ok = true;
                        break;
                    }
                if (!ok) return;
            }

            if (msg.pos == TempKey)
            {
                msg.item = mymagic.tempMagic;//自动适应卸下的magic
                msg.item.container = null;

                mymagic.tempMagic = ContainedCnull(mymagic);
                InactiveComponent(mymagic, msg.item);
                return;
            }

            if (msg.pos >= mymagic.magics.Count) return;

            msg.item = mymagic.magics[msg.pos];//自动适应卸下的magic
            msg.item.container = null;


            CardBase Cnull = new CNull_0();
            mymagic.magics[msg.pos] = Cnull;
            Cnull.container = mymagic;
            InactiveComponent(mymagic, msg.item);
        }
    }
    void SelfContainerAllItem(CardBase _card, MsgBase _msg)
    {
        Cmagic_14 card = _card as Cmagic_14;
        MsgContainerAllItem msg = _msg as MsgContainerAllItem;

        msg.items = card.GetMagics();
    }

    private void MagicOn(CardBase _card, MsgBase _msg)
    {
        MsgMagicOn msg = _msg as MsgMagicOn;
        MsgGetItem nmsg = new MsgGetItem(msg.magic, msg.op, msg.key);
        SelfContainerGet(_card, nmsg);
    }
    /// <summary>
    /// 一般用在释放技能时的复读技能
    /// </summary>
    public static void UseMagic(MsgMagicUse msg)
    {
        SendMsg(msg.magic, MsgType.MyUseMagic, msg);
    }
    /// <summary>
    /// 用在其他希望较为完整释放技能效果的地方
    /// </summary>
    public static bool UseMagicWithBA(MsgMagicUse useMsg)
    {
        SendMsg(useMsg.magic, MsgType.MyUseMagicBefore, useMsg);
        SendMsg(useMsg.live, MsgType.UseMagicBefore, useMsg);

        if (useMsg.valid == false)
        {
            return false;
        }

        UseMagic(useMsg);

        SendMsg(useMsg.magic, MsgType.MyUseMagicAfter, useMsg);
        SendMsg(useMsg.live, MsgType.UseMagicAfter, useMsg);
        return true;
    }
    public static void UseMagicEntirely(MsgMagicUse msg)
    {
        SendMsg(msg.live, MsgType.BeginUseMagic, msg);
    }
    void MagicConGet(CardBase _card, MsgBase _msg)
    {
        Cmagic_14 card = _card as Cmagic_14;
        MsgMagicCon msg = _msg as MsgMagicCon;

        //Debug.Log(msg.key + " " + cardAbandon.magics[msg.key]);
        if (msg.key == -1) // 随机使用看上去能使用的魔法
        {
            List<int> canUse = new List<int>();
            for (int i = 0; i < card.holdMax && i < card.cdBear.Count; i++)
            {
                if (card.cdBear[i] <= 0) canUse.Add(i);
            }
            msg.key = canUse[Random.Range(0, canUse.Count)];
        }

        if (msg.key >= card.holdMax || msg.key >= card.magics.Count) return;// 超过魔法上限的尝试
        if (card.nowUse != null && card.nowUse.magic.id == card.magics[msg.key].id && !card.nowUse.doWindDown) return;//不能打断同ID技能的前后摇

        CObj_2 live = msg.cobj;
        CardBase magic = card.magics[msg.key];

        MsgMagicUse useMsg = new MsgMagicUse(live, magic, msg.pos)
        {
            key = msg.key,
            costKey = msg.key,
            instantAtkSpeed = Shealth_4.GetAttf(live, BasicAttID.atkSpeed)
        };

        if (magic is Cmagicbase_17 mmagic)
        {
            useMsg.mdata = new Dmagic(mmagic.mdata);
            useMsg.pow = mmagic.pow;
        }
        else useMsg.mdata = new Dmagic();
        useMsg.isConUse = true;

        UseMagicEntirely(useMsg);
    }
    private void RecoverTempMagic(Cmagic_14 mymagic, CardBase oldMagic)//瞬间释放时，不打断原有魔法，也不删除原有临时魔法
    {
        if(oldMagic == null) return;
        MagicOn(mymagic, new MsgMagicOn() { key = TempKey, op = -1 });
        MagicOn(mymagic, new MsgMagicOn() { key = TempKey, op = 1, magic = oldMagic });
    }
    void BeginUseMagic(CardBase _card, MsgBase _msg)
    {
        Cmagic_14 card = _card as Cmagic_14;
        MsgMagicUse useMsg = _msg as MsgMagicUse;

        CardBase oldTempMagic = null;

        if (!CanUseMagic(card, useMsg.key)) return;

        if(useMsg.key == TempKey)//释放魔法不在魔法释放器中，临时加入
        {
            oldTempMagic = card.magics.Count > TempKey ? card.magics[TempKey] : null;
            MagicOn(_card, new MsgMagicOn() { key = TempKey, op = -1 });
            MagicOn(_card, new MsgMagicOn() { key = TempKey, op = 1, magic = useMsg.magic });
        }
        SendMsg(GetTop(card), MsgType.MagicBegin, useMsg);// 发送进入前摇的信息
        SendMsg(useMsg.magic, MsgType.MyMagicBegin, useMsg);// 发送进入前摇的信息

        if (useMsg.valid == false)
        {
            RecoverTempMagic(card, oldTempMagic);
            return;
        }

        if (useMsg.mdata.windUp == 0f&&useMsg.mdata.windDown==0)//前后摇都为0，瞬间释放，不再打断
        {
            useMsg.doWindUp = true;
            MagicUse(card, useMsg);
            SendMsg(useMsg.magic, MsgType.MyMagicEnd, useMsg);// 发送后摇结束的信息
            RecoverTempMagic(card, oldTempMagic);
            return;
        }        

        if (card.nowUse != null && card.nowUse.doWindDown == false)
        {
            //发送中断信息
            InterruptNowMagic(card.nowUse.magic);
            //删除临时加入的魔法
            if (card.nowUse.key == TempKey && useMsg.key != TempKey)
                MagicOn(_card, new MsgMagicOn() { key = TempKey, op = -1 });
        }

        card.nowUse = useMsg;//Debug.Log(cardAbandon.nowUse.mdata.windDown);

        //显示头上的使用技能条
        if (card.useBar == null && TryGetCobj(card, out var cobj))
        {
            Dmagic_14 config = basicConfig as Dmagic_14;
            card.useBar = UIBasic.GiveLiveUpUI(config.useBar, cobj);
        }
        card.useWind.max = useMsg.mdata.windDown;
        card.useWind.cur = 0;
        if (card.useBar != null && useMsg.mdata.windDown > 0f)
            card.useBar.GetComponent<UseBar>().Set(card.useWind, useMsg.mdata.windUp / useMsg.mdata.windDown,
                DataManager.GetName(useMsg.magic.id));

        if (useMsg.mdata.windUp == 0f)//前摇为0，直接进入判定阶段
        {
            useMsg.doWindUp = true;
            MagicUse(card, useMsg);
        }
    }
    public static void MagicUseCost(CardBase card, MsgMagicUse useMsg)
    {
        if (!TryGetCobj(card, out var cobj)) return;
        if (cobj.myMagic == null) return;
        UseMagicBeforeCost(cobj.myMagic, useMsg);
    }
    /// <summary>
    /// 消耗法力、产生冷却
    /// </summary>
    static void UseMagicBeforeCost(CardBase _card, MsgBase _msg)
    {
        Cmagic_14 card = _card as Cmagic_14;
        MsgMagicUse useMsg = _msg as MsgMagicUse;

        MsgCostMana manaMsg = new MsgCostMana() { cost = useMsg.mdata.cost };
        SendMsg(useMsg.costLive, MsgType.CostMana, manaMsg);
        if (manaMsg.ok == false)
        {
            useMsg.valid = false;
            return;
        }

        if (useMsg.costKey != TempKey)
        {
            card.cdBear[useMsg.costKey] += useMsg.mdata.cd;
            card.cdBearMax[useMsg.costKey] = Mathf.Max(card.cdBear[useMsg.costKey], card.cdBearMax[useMsg.costKey]);
        }

    }
    void MagicUse(CardBase _card, MsgBase _msg)
    {
        Cmagic_14 card = _card as Cmagic_14;
        MsgMagicUse useMsg = _msg as MsgMagicUse;

        bool ok = UseMagicWithBA(useMsg);

        if (ok == false)
        {
            // 强制结束魔法的释放
            if (card.nowUse.key == TempKey)
                MagicOn(_card, new MsgMagicOn() { key = TempKey, op = -1 });
            if (card.useBar != null)
            {
                GameObject.Destroy(card.useBar);
            }
            card.nowUse = null;
            return;
        }
    }
    void MagicBegin(CardBase _card, MsgBase _msg) // 判定冷却、耗魔等是否满足施法需要
    {
        Cmagic_14 card = _card as Cmagic_14;
        MsgMagicUse msg = _msg as MsgMagicUse;

        if (msg.key != TempKey && card.cdBear[msg.key] > 0)
        {
            msg.valid = false;
            return;
        }
        if (Shealth_4.GetHealth(card) != null && Shealth_4.GetHealth(card).nowMana < msg.mdata.cost)
        {
            msg.valid = false;
            return;
        }
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        Cmagic_14 card = _card as Cmagic_14;
        MsgUpdate msg = _msg as MsgUpdate;

        RecoverAllMagicCd(GetTop(card), msg.time * Shealth_4.GetAttf(card, BasicAttID.cdSpeed), false);

        if(card.nowUse != null)
        {
            card.nowUse.mdata.windUp -= msg.time * card.nowUse.instantAtkSpeed;
            card.nowUse.mdata.windDown -= msg.time * card.nowUse.instantAtkSpeed;

            card.useWind.cur = card.useWind.max - card.nowUse.mdata.windDown;//更新进度显示

            if (card.nowUse.mdata.windUp < 0 && !card.nowUse.doWindUp)
            {
                card.nowUse.doWindUp = true;
                /**/
                if(card.nowUse.mdata.moveUse)//更正施法位置
                {
                    Vector3 pos = SShoulderCamera_37.GetLookPos(card);
                    card.nowUse.pos = pos;
                }

                MagicUse(card, card.nowUse);
            }
            else if (card.nowUse.mdata.windDown < 0 && !card.nowUse.doWindDown)
            {
                card.nowUse.doWindDown = true;
                CardBase offMagic = card.nowUse.magic;
                bool needOff = card.nowUse.key == TempKey;
                MsgMagicUse nowUseBefore = card.nowUse;

                if (TryGetCobj(card, out var cobj))//结束后摇的强制控制
                {
                    cobj.obj.SetLockMotion(0);
                    cobj.obj.SetUpper(0);
                }                

                SendMsg(card.nowUse.magic, MsgType.MyMagicEnd, card.nowUse);// 发送后摇结束的信息

                // 删除显示的条，当且仅当没有新增使用的魔法
                if (card.useBar != null && nowUseBefore == card.nowUse)
                {
                    GameObject.Destroy(card.useBar);
                }

                if (needOff && nowUseBefore==card.nowUse)// 删除临时加入的魔法，要求当前并未被修改
                {
                    MagicOn(_card, new MsgMagicOn() { magic = offMagic, op = -1 });
                }
                
            }
        }
    }

    void ShowMagicUI(CardBase _card, MsgBase _msg)
    {
        Cmagic_14 card = _card as Cmagic_14;
        MsgOnShowUI msg = _msg as MsgOnShowUI;
        Dmagic_14 config = basicConfig as Dmagic_14;

        if (msg.op==1)
        {
            card.magicPanel = UIBasic.GiveUI(config.magicPanel);
            card.magicPanel.GetComponent<MagicPanel>().SetMagic(card);
            card.magicBarUI = UIBasic.GiveLeftUpUI(config.magicBarUI);
            card.magicBarUI.GetComponent<MagicBar>().Set(Shealth_4.GetHealth(card));
        }
        else
        {
            if (card.magicPanel != null)
                GameObject.Destroy(card.magicPanel);
            if (card.magicBarUI != null)
                GameObject.Destroy(card.magicBarUI);
        }
    }
    void OpenBag(CardBase _card, MsgBase _msg)
    {
        Cmagic_14 card = _card as Cmagic_14;
        MsgOpenBag msg = _msg as MsgOpenBag;

        if(card.magicPanel!=null)
        {
            card.magicPanel.transform.SetAsLastSibling();//显示盖过背包
        }
    }
    void ParseDescribe(CardBase _card, MsgBase _msg)
    {
        Cmagic_14 card = _card as Cmagic_14;
        MsgParseDescribe msg = _msg as MsgParseDescribe;

        if (msg.card != card.parent && msg.card != card) return;

        string qhcs = "魔法槽上限：" + card.magics.Count;//当前有几个槽就是上限
        foreach (CardBase com in card.magics)
        {
            qhcs += Cstr(com, true, true) + "";
        }
        msg.text = Sitem_33.InsertField(msg.text, "技能", Sitem_33.MaskColor.ZD, qhcs);
    }
}