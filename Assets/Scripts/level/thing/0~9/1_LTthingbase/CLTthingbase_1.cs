using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;
using Unity.VisualScripting.FullSerializer;
using System.Linq;

public class CLTthingbase_1 : CObj_2
{
    public CfloorBase_2 fromFloor;

    public int nowChat = 0;
    public int toChat = 0;
    public CardBase live;
    public float pow = 1f;
    public MInteractBoxUI ui;
}
public class DLFthing_10 : DataBase
{
    public string thingPanel;

    public static GameObject thingPanelPrefab;
    public override void Init(int id)
    {
        thingPanelPrefab = DataManager.LoadResource<GameObject>(id, thingPanel);
    }
}
public class DthingText : DataBase
{
    // 这里的-1代表任意地方都可以
    public List<int> meetDifficulty = new List<int>();
    public bool canMeet;
    public string title;
    public List<Ttext> texts = new();
    public class Ttext
    {
        public string content;
        public List<string> choice;
    }

    public static Dictionary<int, HashSet<int>> difToThingIDs = new();
    public override void Init(int id)
    {
        if (meetDifficulty == null) return;

        if (canMeet == false) return;

        foreach(int dif in meetDifficulty)
        {
            HashSet<int> list = difToThingIDs.GetValueOrDefault(dif, new HashSet<int>());
            list.Add(id);
            difToThingIDs[dif] = list;
            // Debug.Log($"{dif} {list.Count}");
        }
    }
}
public class SLTthingbase_1 : SObj_2
{
    public static int mTThingChoose = MsgType.ParseMsgType(CardField.thing, 1, 0);
    public class MsgThingChoose : MsgBase
    {
        public CardBase live;
        public int choose;
        public MsgThingChoose(CardBase live, int choose)
        {
            this.live = live;
            this.choose = choose;
        }
    }

    public static int GetRandThing(int dif, bool useMapRand = false)
    {
        HashSet<int> list = new HashSet<int>();
        if(DthingText.difToThingIDs.TryGetValue(dif, out var nlist))
            list.UnionWith(nlist);
        list.UnionWith(DthingText.difToThingIDs[-1]);
        list.ExceptWith(SGRbase_1.GetFilteredItems());
        return MyRandom.RandomInList(list.ToList(), useMapRand);
    }
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.BeInteract, BeInteract);
        AddHandle(MsgType.MyInteractItem, MyInteractItem);
        AddHandle(mTThingChoose, ThingChoose);
    }
    // chooseItem后返回的位置
    void MyInteractItem(CardBase _card, MsgBase _msg)
    {
        CLTthingbase_1 card = _card as CLTthingbase_1;
        MsgInteractItem msg = _msg as MsgInteractItem;

        msg.close = true;
        ChatCallBack(card, card.toChat, 0, card.live);
    }
    void BeInteract(CardBase _card, MsgBase _msg)
    {
        CLTthingbase_1 card = _card as CLTthingbase_1;
        MsgBeInteract msg = _msg as MsgBeInteract;

        card.live = msg.live;
        ToChat(card, card.nowChat);
    }
    void ThingChoose(CardBase _card, MsgBase _msg)
    {
        CLTthingbase_1 card = _card as CLTthingbase_1;
        MsgThingChoose msg = _msg as MsgThingChoose;

        ChatCallBack(card, card.nowChat, msg.choose, msg.live);
    }
    public virtual void ChatCallBack(CLTthingbase_1 card, int nowChat, int choose, CardBase live)
    {
        EndChat(card);
    }
    public void EndChat(CLTthingbase_1 card)
    {
        if (card.fromFloor != null)
            SfloorBase_2.GiveNextCsm(card.fromFloor);

        DestroyCard(card);
    }
    /// <summary>
    /// 用在chatCallBack中，新起一个对话内容选项
    /// </summary>
    public void ToChat(CLTthingbase_1 card, int chat)
    {
        card.nowChat = chat;
        DthingText dthingText = DataManager.GetConfig<DthingText>(id);

        string content = DataManager.ParseVariableWithPow(card, dthingText.texts[card.nowChat].content, card.pow, false);
        List<string> choice = new();
        foreach (string choose in dthingText.texts[card.nowChat].choice)
            choice.Add(DataManager.ParseVariableWithPow(card, choose, card.pow, false));

        UIBasic.GiveUI(DLFthing_10.thingPanelPrefab).GetComponent<MLTthingbase_1_panel>()
            .Set(card, dthingText.title, content, choice);
    }
    /// <summary>
    /// toChat直接到对应的callback的位置，choose没有意义
    /// </summary>
    public MInteractBoxUI ChooseItem(CLTthingbase_1 card, int toChat, string title = "选择物品", string use = "确认")
    {
        card.toChat = toChat;

        var ui = UIBasic.GiveUI(DMOdzt_5.forgeUIPrefab).GetComponent<MInteractBoxUI>();
        ui.SetWithUse(card.live, card, title, false, "已经使用", use);
        card.ui = ui;
        return ui;
    }
    public void ChangeUITitle(CLTthingbase_1 card, string title)
    {
        if (card.ui == null) return;

        card.ui.title.text = title;
    }
}