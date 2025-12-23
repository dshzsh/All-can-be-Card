using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;
using Newtonsoft.Json;

public class Cbag_40 : CardBase
{
    public List<CardBase> goods = new List<CardBase>();
}
public class Dbag_40 : DataBase
{
    public string bagUIName;
    public string goodUIName;
    public string bagGoodUIName;//在背包打开中显示的小物体
    public string goodPreSeeName;
    public string preSeeGoodUIName;//在预览里的小型goodUI
    public string dragHint;

    [JsonIgnore]
    public GameObject bagUI;
    [JsonIgnore]
    public GameObject goodUI;
    [JsonIgnore]
    public GameObject bagGoodUI;
    [JsonIgnore]
    public GameObject goodPreSee;
    [JsonIgnore]
    public GameObject preSeeGoodUI;
    public GameObject dragHintObj;
    public override void Init(int id)
    {
        bagUI = DataManager.LoadResource<GameObject>(id, bagUIName);
        goodUI = DataManager.LoadResource<GameObject>(id, goodUIName);
        bagGoodUI = DataManager.LoadResource<GameObject>(id, bagGoodUIName);
        goodPreSee = DataManager.LoadResource<GameObject>(id, goodPreSeeName);
        preSeeGoodUI = DataManager.LoadResource<GameObject>(id, preSeeGoodUIName);
        dragHintObj = DataManager.LoadResource<GameObject>(id, dragHint);
    }
}
public class MsgOpenBag :MsgBase
{
    public bool isClose = false;//给esc用的
}
public class MsgGetItem: MsgBase
{
    public CardBase container;
    public CardBase item;
    public int op;
    public int pos;//pos为-1标志着卸下或者装配失败
    //public bool getOk = true;

    public MsgGetItem() { }
    public MsgGetItem(CardBase item, int op, int pos = -1, CardBase container = null)
    {
        this.item = item;
        this.op = op;
        this.pos = pos;
        this.container = container;
    }
}
public class MsgContainerJudge : MsgBase
{
    public MsgGetItem gmsg;
    public bool ok = true;
    public bool giveMsg = false;
    public string errorMsg = "";

    public void AddMsg(string msg)
    {
        if(giveMsg)
        {
            errorMsg += msg + "\n";
        }
    }
}
public class Sbag_40: SystemBase
{
    public static int mTOpenBag = MsgType.ParseMsgType(GetTypeId(typeof(Cbag_40)), 0);
    public static int mTGetItem = MsgType.ParseMsgType(GetTypeId(typeof(Cbag_40)), 1);
    public static int mTFirstGetItem = MsgType.ParseMsgType(GetTypeId(typeof(Cbag_40)), 2);
    public static int mTMyFirstGetItem = -MsgType.ParseMsgType(GetTypeId(typeof(Cbag_40)), 2);
    public static BagUI bagUI;
    public override void Init()
    {
        AddHandle(MsgType.Update, Update);
        AddHandle(mTOpenBag, OpenBagUI);
        AddHandle(MsgType.SelfContainerGet, GetItem);
        AddHandle(MsgType.OnItem, OnItem);
        AddHandle(MsgType.SelfContainerJudge, MyContainerJudge);
        AddHandle(MsgType.SelfContainerAllItem, SelfContainerAllItem);

        Dbag_40 config = basicConfig as Dbag_40;
        bagUI = UIBasic.GiveUI(config.bagUI).GetComponent<BagUI>();
        bagUI.gameObject.SetActive(false);
        GoodPreSee.goodPreSee = config.goodPreSee;
        BagUI.bagGoodUIPrefab = config.bagGoodUI;
        CardDragUI.goodUIPrefab = config.goodUI;
        ExText.preSeeGoodUIPrefab = config.preSeeGoodUI;
        CardDragUI.dragHintPrefab = config.dragHintObj;
    }
    public static bool IsBagOpen(CardBase card)
    {
        if (bagUI.gameObject == null) return false;
        return bagUI.gameObject.activeSelf;
    }
    public static bool IsMyBagOpen(Cbag_40 bag)
    {
        return IsBagOpen(bag) && bagUI.bag == bag;
    }
    public static void LiveGetItem(CardBase card, CardBase item)
    {
        if (!TryGetCobj(card, out var cobj)) return;

        SendMsg(cobj.myBag, MsgType.SelfContainerGet, new MsgGetItem(item, 1));
    }
    public static Cbag_40 LiveGetBag(CardBase card)
    {
        if (!TryGetCobj(card, out var cobj)) return null;
        return cobj.myBag;
    }
    public static void OffItem(Cbag_40 bag, CardBase item)
    {
        SendMsg(bag, MsgType.SelfContainerGet, new MsgGetItem(item, -1));
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        Cbag_40 card = _card as Cbag_40;
        MsgOnItem msg = _msg as MsgOnItem;
        Dbag_40 config = basicConfig as Dbag_40;

        if (msg.op == 1)
        {
            if (TryGetCobj(card, out var cobj))
                cobj.myBag = card;
        }
        else
        {
            if (bagUI!=null && bagUI.bag == card)
            {
                bagUI.gameObject.SetActive(false);
            }
        }
    }
    void GetItem(CardBase _card, MsgBase _msg)
    {
        Cbag_40 card = _card as Cbag_40;
        MsgGetItem msg = _msg as MsgGetItem;
        msg.container = card;

        if(msg.item==null)
        {
            MyDebug.LogWarning("空的物体装载信息");
            return;
        }

        if(msg.op==1)
        {
            if(msg.item is Citem_33 citem)
            {
                if(!citem.getted)
                {
                    SendMsg(citem, mTMyFirstGetItem, msg);
                    SendMsg(GetTop(card), mTFirstGetItem, msg);
                }
                citem.getted = true;

                if (msg.valid == false)
                    return;
            }

            if (msg.pos != -1 && msg.pos < card.goods.Count)
                card.goods.Insert(msg.pos, msg.item);
            else card.goods.Add(msg.item);
            msg.item.container = card;
            //tag包含good的物体会直接被生效
            if (MyTag.HaveTag(msg.item.id, MyTag.CardTag.good))
                ActiveComponent(card, msg.item, -msg.pos);
        }
        else
        {
            msg.pos = card.goods.IndexOf(msg.item);
            //Debug.Log(msg.pos);
            if (msg.pos == -1) return;

            card.goods.Remove(msg.item);
            msg.item.container = null;
            if (MyTag.HaveTag(msg.item.id, MyTag.CardTag.good))
                InactiveComponent(card, msg.item);
        }

        if (IsMyBagOpen(card)) bagUI.SetBag(card, card.goods);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        Cbag_40 card = _card as Cbag_40;
        MsgUpdate msg = _msg as MsgUpdate;
        Dbag_40 config = basicConfig as Dbag_40;

        
    }
    void MyContainerJudge(CardBase _card, MsgBase _msg)
    {
        Cbag_40 card = _card as Cbag_40;
        MsgContainerJudge msg = _msg as MsgContainerJudge;

        if (card != msg.gmsg.container) return;

        if (msg.gmsg.op == 1)
        {
            msg.ok = true;
            return;
        }
        else
        {
            msg.gmsg.pos = card.goods.IndexOf(msg.gmsg.item);
            msg.ok = msg.gmsg.pos != -1;
        }

    }
    void SelfContainerAllItem(CardBase _card, MsgBase _msg)
    {
        Cbag_40 card = _card as Cbag_40;
        MsgContainerAllItem msg = _msg as MsgContainerAllItem;

        msg.items = card.goods;
    }
    private void OpenBagUI(CardBase _card, MsgBase _msg = default)
    {
        Cbag_40 bag = _card as Cbag_40;
        MsgOpenBag msg = _msg as MsgOpenBag;

        if(msg!=default && msg.isClose==true)//ESC时的特殊只关闭
        {
            if (IsBagOpen(bag) == false) return;
        }

        if (!IsBagOpen(bag)) //打开背包界面
        {
            bagUI.SetBag(bag, bag.goods);
            bagUI.OpenBag();
        }
        else //关闭背包界面
        {
            bagUI.Close();
        }
    }
}