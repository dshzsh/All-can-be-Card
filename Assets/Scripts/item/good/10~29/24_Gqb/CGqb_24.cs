using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;
using Newtonsoft.Json;

public class CGqb_24 : Citem_33
{
    public float coin = 0f;

    [JsonIgnore]
    public GameObject leftUpUI;
}
public class DGqb_24 : DataBase
{
    public string leftUpUI;

    public GameObject leftUpUIPrefab;
    public override void Init(int id)
    {
        leftUpUIPrefab = DataManager.LoadResource<GameObject>(id, leftUpUI);
    }
}
public class SGqb_24 : Sitem_33
{
    public static float GetCoinCnt(CardBase card)
    {
        if (!TryGetCobj(card, out var live)) return 0f;
        MsgGetCoinCnt msg = new();
        SendMsg(live, mTGetCoinCnt, msg);
        return msg.coin;
    }
    public static bool CostCoin(CardBase card, float coinCost)
    {
        if (!TryGetCobj(card, out var live)) return false;
        MsgCostCoin msg = new(coinCost);
        SendMsg(live, mTCostCoin, msg);
        return msg.ok;
    }
    public static void GetCoin(CardBase card, float coinAdd)
    {
        if (!TryGetCobj(card, out var live)) return;
        MsgGetCoin msg = new(coinAdd);
        SendMsg(live, mTGetCoin, msg);
        if (msg.ok == false)
        {
            CGqb_24 qb = CreateCard<CGqb_24>();
            AddComponent(qb, CreateCard<CGQwfxx_15>());
            Sbag_40.LiveGetItem(live, qb);
            msg = new(coinAdd);
            SendMsg(live, mTGetCoin, msg);
        }
    }
    static readonly float[] rareValue = { 0.0f, 0.2f, 0.3f, 0.4f, 0.6f, 1.0f };
    public static float ItemValue(CardBase card)
    {
        if (card is not Citem_33 item) return 0f;

        float ans = 100f;

        int rare = 0;
        Ditem ditem = DataManager.GetConfig<Ditem>(card.id);
        if (ditem != null) rare = ditem.rare;
        rare = System.Math.Clamp(rare, 1, 5);
        ans *= rareValue[rare];

        if (card is Cmagicbase_17 || card is CGqhsbase_11)
            ans /= 2;

        return ans * item.pow;
    }
    public static int mTGetCoinCnt = MsgType.ParseMsgType(CardField.good, 24, 0);
    public class MsgGetCoinCnt : MsgBase
    {
        public float coin = 0;
    }
    public static int mTCostCoin = MsgType.ParseMsgType(CardField.good, 24, 1);
    public class MsgCostCoin: MsgBase
    {
        public float coin = 0;
        public bool ok = false;
        public MsgCostCoin(float coin)
        {
            this.coin = coin;
        }
    }
    public static int mTGetCoin = MsgType.ParseMsgType(CardField.good, 24, 2);
    public class MsgGetCoin : MsgBase
    {
        public float coinAdd = 0;
        public bool ok = false;
        public MsgGetCoin(float coinAdd)
        {
            this.coinAdd = coinAdd;
        }
    }

    public override void Init()
    {
        base.Init();
        AddHandle(mTGetCoinCnt, GetCoinCnt);
        AddHandle(mTCostCoin, CostCoin);
        AddHandle(mTGetCoin, GetCoin);
        AddHandle(MsgType.OnShowUI, OnShowUI);
    }
    void GetCoinCnt(CardBase _card, MsgBase _msg)
    {
        CGqb_24 card = _card as CGqb_24;
        MsgGetCoinCnt msg = _msg as MsgGetCoinCnt;

        msg.coin = card.coin;
    }
    void CostCoin(CardBase _card, MsgBase _msg)
    {
        CGqb_24 card = _card as CGqb_24;
        MsgCostCoin msg = _msg as MsgCostCoin;

        if(card.coin < msg.coin)
        {
            msg.ok = false;
            return;
        }
        card.coin -= msg.coin;
        msg.ok = true;
    }
    void GetCoin(CardBase _card, MsgBase _msg)
    {
        CGqb_24 card = _card as CGqb_24;
        MsgGetCoin msg = _msg as MsgGetCoin;

        card.coin += msg.coinAdd;
        msg.ok = true;
    }
    void OnShowUI(CardBase _card, MsgBase _msg)
    {
        CGqb_24 card = _card as CGqb_24;
        MsgOnShowUI msg = _msg as MsgOnShowUI;
        DGqb_24 config = basicConfig as DGqb_24;

        if (msg.op == 1)
        {
            card.leftUpUI = UIBasic.GiveLeftUpUI(config.leftUpUIPrefab);
            card.leftUpUI.GetComponent<MGqb_24_UI>().Set(card);
        }
        else
        {
            if(card.leftUpUI!=null)
            {
                GameObject.Destroy(card.leftUpUI);
            }
        }
    }
}