using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CNtreasure_1 : CObj_2
{
    public List<int> tags = new List<int>();
    public int rare = -1;
    public List<CardBase> cards = new List<CardBase>();
}
public class DNtreasure_1: DataBase
{
    public string treasurePanel;
    public string treasurePanelItem;

    public static GameObject treasurePanelPrefab;
    public override void Init(int id)
    {
        treasurePanelPrefab = DataManager.LoadResource<GameObject>(id, treasurePanel);
        MNtreasure_1_Panel.treasurePanelItemPrefab = DataManager.LoadResource<GameObject>(id, treasurePanelItem);
    }
}
public class SNtreasure_1 : SObj_2
{
    public class MsgGetTreasure : MsgBase
    {
        public CardBase live;
        public int select;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="live"></param>
        /// <param name="select">-1的话就是取消选择</param>
        public MsgGetTreasure(CardBase live, int select)
        {
            this.live = live;
            this.select = select;
        }
    }
    public class MsgSummonTreasure: MsgBase
    {
        public CNtreasure_1 trea;
        public int choose = 3;
        public MsgSummonTreasure(CNtreasure_1 trea)
        {
            this.trea = trea;
        }
    }
    public static int mTGetTreasure = MsgType.ParseMsgType(GetTypeId(typeof(CNtreasure_1)), 0);
    public static int mTSummonTreasure = MsgType.ParseMsgType(GetTypeId(typeof(CNtreasure_1)), 1);
    public static int mTSummonTreasureBefore = MsgType.ParseMsgType(GetTypeId(typeof(CNtreasure_1)), 2);

    public static int SummonRare(int mapCnt = -1)
    {
        float luck = Shealth_4.GetAttf(GetMainPlayer(), BasicAttID.luck);
        if (mapCnt == -1)
            mapCnt = SCmap_45.GetMapCnt(); // 标记当前的地图层数，影响生成的稀有度
        int rare = 1;
        for(int i=0;i<5;i++)
        {
            if (MyRandom.RandPer(Mathf.Clamp(10 + mapCnt / 2 + luck * 100, 10 + mapCnt / 2, 70) / 100f))
                rare++;
        }
        if (rare > 5) rare = 5;
        return rare;
    }
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.BeInteract, BeInteract);
        AddHandle(mTGetTreasure, GetTreasure);
        AddHandle(MsgType.OnItem, OnItem);
    }
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        CNtreasure_1 card = _card as CNtreasure_1;
        card.tags.Add(MyTag.CardTag.good);
        card.tags.Add(MyTag.CardTag.Pnormal);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CNtreasure_1 card = _card as CNtreasure_1;
        MsgOnItem msg = _msg as MsgOnItem;

        if(msg.op==1)
        {
            // 根据自身的rare产生对应颜色的粒子效果
            if(card.rare == -1)
            {
                card.rare = SummonRare();
            }
            if(card.cards.Count==0)
            {
                MsgSummonTreasure smsg = new MsgSummonTreasure(card);
                SendMsgToPlayer(mTSummonTreasureBefore, smsg);
                // 生成id，并且去重
                List<int> cardToSums = Sitem_33.GetNoRepeatRandomItems(smsg.choose, card.rare, card.tags.ToArray());

                foreach(int i in cardToSums)
                {
                    card.cards.Add(CreateCard(i));
                }
                SendMsg(card, mTSummonTreasure, smsg);
                SendMsgToPlayer(mTSummonTreasure, smsg);
            }
            card.obj.gameObject.GetComponent<MNtreasure_1>().SetParColor(Sitem_33.RareColor.GetRareColor(card.rare));
        }
        else
        {
            
        }
    }
    void GetTreasure(CardBase _card, MsgBase _msg)
    {
        CNtreasure_1 card = _card as CNtreasure_1;
        MsgGetTreasure msg = _msg as MsgGetTreasure;

        if(msg.select==-1)
        {
            card.obj.gameObject.GetComponent<MNtreasure_1>().Close();
            return;
        }
        Sbag_40.LiveGetItem(msg.live, card.cards[msg.select]);
        Sdiedes_27.GiveDiePar(card.obj.gameObject, 1, card.obj.gameObject.GetComponent<MNtreasure_1>().dieColor);
        DestroyCard(card);
    }
    void BeInteract(CardBase _card, MsgBase _msg)
    {
        CNtreasure_1 card = _card as CNtreasure_1;
        MsgBeInteract msg = _msg as MsgBeInteract;

        UIBasic.GiveUI(DNtreasure_1.treasurePanelPrefab).GetComponent<MNtreasure_1_Panel>()
            .Set(msg.live, card, "选择一项宝藏", card.cards);
        card.obj.gameObject.GetComponent<MNtreasure_1>().Open();
    }
}