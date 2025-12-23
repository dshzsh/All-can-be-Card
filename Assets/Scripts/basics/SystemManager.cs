using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using System.Linq;

public static class SystemManager
{
    public static class HandlerPriority 
    {
        public static int Highest = 100000;
        public static int High = 300;
        public static int Before = 200;
        public static int JustBefore = 100;
        public static int Normal = 0;
        public static int JustAfter = -100;
        public static int After = -200;
        public static int Low = -300;
        public static int Lowest = -100000;
    }
    public class HandlerAndPriority
    {
        public int msgType;
        public CardMsgHandler handler;
        public int priority;
        public bool islocal;
    }
    public delegate void CardMsgHandler(CardBase _card, MsgBase _msg);

    static Dictionary<Type, int> cardid = new Dictionary<Type, int>();
    static Dictionary<int, Type> cardType = new Dictionary<int, Type>();
    public static List<SystemBase> cardSystem = new List<SystemBase>();
    static bool isInited = false;
    public static void Init()
    {
        if (isInited) return;
        isInited = true;

        int cnt = DataManager.GetCardCount();
        for (int i = 0; i < cnt; i++)
        {
            int id = DataManager.GetVid(i);
            string name = DataManager.GetRawName(i) + "_" + id;
            cardid.Add(Type.GetType("C" + name), i);
            cardType.Add(i, Type.GetType("C" + name));
        }
        for (int i = 0; i < cnt; i++)
        {
            int id = DataManager.GetVid(i);
            string name = DataManager.GetRawName(i) + "_" + id;
            SystemBase sys = (SystemBase)Activator.CreateInstance(Type.GetType("S" + name));
            sys.id = i;
            Type dataType = Type.GetType("D" + name);
            if(DataManager.GetConfigs(i) != null)
                foreach (DataBase config in DataManager.GetConfigs(i))
                {
                    if(config.GetType().Equals(dataType))
                    {
                        sys.basicConfig = config;
                        break;
                    }
                }
            cardSystem.Add(sys);
            
        }
        for (int i = 0; i < cnt; i++)
        {
            cardSystem[i].Init();
        }

    }
    public static int GetTypeId(Type type)
    {
        return cardid[type];
    }
    public static int GetCardId(CardBase card)
    {
        if (card.id == 0)
            return card.id = cardid[card.GetType()];
        return card.id;
    }
    public static Type GetCardType(int id)
    {
        return cardType[id];
    }
    public static void TriCreateCard(CardBase card)
    {
        cardSystem[GetCardId(card)].Create(card);
    }
    public static void TriClone(CardBase from, CardBase to)
    {
        cardSystem[GetCardId(from)].Clone(from, to);
    }
    public static string TriDescribe(CardBase card, string text)
    {
        return cardSystem[GetCardId(card)].ParseDescribe(card, text);
    }
    public class CardAndPriority
    {
        public CardBase card;
        public CardMsgHandler handheld;
        public int priority;
        public int liveCnt;
        public CardAndPriority(CardBase card, int priority, CardMsgHandler handheld, int liveCnt)
        {
            this.card = card;
            this.priority = priority;
            this.handheld = handheld;
            this.liveCnt = liveCnt;
        }
    }
    private static List<CardAndPriority> GetOneCardTriList(CardBase card, int msgType)
    {
        List<CardAndPriority> list = new List<CardAndPriority>();
        foreach (HandlerAndPriority handlerAndPriority in cardSystem[card.id].handler)
        {
            if (handlerAndPriority.msgType == msgType)
            {
                list.Add(new CardAndPriority(card, handlerAndPriority.priority, handlerAndPriority.handler, card.liveCnt));
            }
        }
        return list;
    }
    private static List<CardAndPriority> GetTriList(CardBase card, int msgType)
    {
        /*debug显示
        if (msgType == MsgType.MyUseMagicBefore)
        {
            Debug.Log(handlerAndPriority.cardAbandon + " " + msgType);
        }*/
        List<CardAndPriority> list;
        if (card is CObj_2 cobj && CardManager.InWorld(card) && MsgType.IsAddToObjMsg(msgType))
        {
            if (!cobj.msgTypeToHandler.TryGetValue(msgType, out list)) return list;
            for(int i=0;i<list.Count;i++)
            {
                if (!CardManager.CardValid(list[i].card) || list[i].liveCnt < list[i].card.liveCnt)//生命数不对，已经移动
                {
                    //Debug.Log("remove " + list[i].cardAbandon + " in " + cardAbandon);
                    list.RemoveAt(i);
                    i--;
                    continue;
                }
            }
            return new List<CardAndPriority>(list);
        }
        if (card.valid == 0) return null;

        list = new List<CardAndPriority>();

        // 先子节点再父节点，因为子节点同优先级可能会影响父节点
        if (card.activeComponents != null)
            foreach (CardBase cardChild in card.activeComponents)
            {
                List<CardAndPriority> listcom = GetTriList(cardChild, msgType);
                if (listcom != null)
                    list.AddRange(listcom);
            }

        list.AddRange(GetOneCardTriList(card, msgType));

        return list;
    }
    public static void SendMsg(CardBase cardfa, int msgType, MsgBase msg)
    {
        //Debug.Log(cardfa + " send " + msgType);
        //if (cardfa.valid <= 0) return;
        if (cardfa == null) return;
        
        List<CardAndPriority> list;

        //self信息只传给单一的卡
        if(MsgType.IsSelfMsg(msgType))
        {
            list = GetOneCardTriList(cardfa, msgType);
        }
        else
        {
            list = GetTriList(cardfa, msgType);
            if (list == null) return;

            // 时间更新不用优先级，不然卡死
            if (msgType != MsgType.Update && msgType != MsgType.FixedUpdate && msgType != MsgType.UpdateSec)
                list = list.OrderBy(x => -x.priority).ThenBy(x => -x.card.exPriority).ToList();
        }
        

        if (msgType == MsgType.OnItem && (msg as MsgOnItem).op == -1)//OnItem为卸下时反转优先级
            list.Reverse();

        UseTriList(msg, list, 0);
    }
    public static void UseTriList(MsgBase msg, List<CardAndPriority> list, int pos)
    {
        msg.triList = list;
        for (int i = pos; i < list.Count; i++)
        {
            if (msg.valid == false) break;

            CardAndPriority handlerAndPriority = list[i];
            msg.triPos = i;


            if (CardManager.CardValid(handlerAndPriority.card))
            {
                handlerAndPriority.handheld(handlerAndPriority.card, msg);
            }

        }
    }
    public static void SendMsgAll(int msgType, MsgBase msg)
    {
        SendMsg(CardManager.GetAllCards(), msgType, msg);
    }
    private static CardBase mainPlayer;//为了宝箱生成等事件的优化，只让player生效
    public static CardBase GetMainPlayer()
    {
        return mainPlayer;
    }
    public static void SetMainPlayer(CardBase card)
    {
        mainPlayer = card;
    }
    public static void SendMsgToPlayer(int msgType, MsgBase msg)
    {
        SendMsg(mainPlayer, msgType, msg);
    }
}
