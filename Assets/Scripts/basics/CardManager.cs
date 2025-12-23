using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static SystemManager;

[AttributeUsage(AttributeTargets.Field)]
public class NoActiveCard : Attribute
{
    
}
public static class CardManager
{
    
    static bool isInited = false;
    public static void Init()
    {
        if (isInited) return;
        isInited = true;
        
    }


    static CardBase world = CreateCard<CNull_0>();
    public static CObj_2 BondCardObj(GameObject obj, CObj_2 card)
    {
        CardObj cardObj = obj.AddComponent<CardObj>();
        cardObj.card = card;
        card.obj = cardObj;
        return card;
    }
    public static CObj_2 CreateCardObj(int id, CObj_2 card, Vector3 pos=default)
    {
        GameObject obj = GameObject.Instantiate(DataManager.GetConfig<DObj_2>(id).gameObject);
        if (SCmap_45.nowFloor != null)
            obj.transform.SetParent(SCmap_45.nowFloor.obj.transform);
        if (pos != default)
            obj.transform.position = pos;
        //cards.Add(cardAbandon);
        return BondCardObj(obj, card);
    }
    public static void AddToWorld(CardBase card)
    {
        AddComponent(world, card);
    }
    public static GameObject CreateObj(int id)
    {
        return GameObject.Instantiate(DataManager.GetConfig<DObj_2>(id).gameObject);
    }
    public static void RemoveCardObj(CardObj obj)
    {
        RemoveComponent(world, obj.card);
        obj.card.valid = 0;
        if(obj.gameObject!= null)
        {
            GameObject.Destroy(obj.gameObject);
        }
    }

    public static CardBase ContainedCnull(CardBase container)
    {
        CNull_0 card = new CNull_0();
        card.container = container;
        return card;
    }
    public static T CreateCard<T>() where T : CardBase
    {
        T card = (T)Activator.CreateInstance(typeof(T));
        card.id = SystemManager.GetCardId(card);
        card.uid = GetUID();
        SystemManager.TriCreateCard(card);
        return card;
    }
    public static CardBase CreateCard(int id)
    {
        CardBase card = (CardBase)Activator.CreateInstance(SystemManager.GetCardType(id));
        card.id = id;
        card.uid = GetUID();
        SystemManager.TriCreateCard(card);
        return card;
    }
    public static void DestroyCard(CardBase card)
    {
        if (card == null) return;
        if (card.container != null) OffContainedCard(card);

        RemoveComponent(card.parent, card);

        //Debug.Log(cardAbandon);
        card.valid = 0;
        List<CardBase> cards = WalkCard(card);
        foreach(CardBase cardc in cards) { cardc.valid = 0; }
    }
    public static int OffContainedCard(CardBase card)
    {
        if(card == null) return -1;
        if(card.container== null) return -1;

        MsgGetItem gmsg = new MsgGetItem(card, -1); gmsg.container = card.container;
        //Debug.Log(live);
        SystemManager.SendMsg(card.container, MsgType.SelfContainerGet, gmsg);
        return gmsg.pos;
    }
    public static void OnContainedCard(CardBase card, int onPos)
    {
        if (card == null) return;
        if (card.container == null) return;

        MsgGetItem gmsg = new MsgGetItem(card, 1, onPos); gmsg.container = card.container;
        //Debug.Log(live);
        SystemManager.SendMsg(card.container, MsgType.SelfContainerGet, gmsg);
    }
    //获取一张承载着所有的卡的卡
    public static CardBase GetAllCards()
    {
        return world;
    }
    public static bool InWorld(CardBase card)
    {
        if (card == world) return true;
        CardBase top = GetTop(card);
        if (top == null||top.parent==null) return false;
        return top.parent == world;
    }
    public static List<CardBase> WalkCard(CardBase card)
    {
        List<CardBase> ans = new List<CardBase>();
        if (card == null) return ans;
        ans.Add(card);
        if (card.activeComponents != null)
            foreach (CardBase com in card.activeComponents)
                ans.AddRange(WalkCard(com));
        return ans;
    }
    private static void AddHandlerToCobj(CardBase card)
    {
        if (!TryGetCobj(card, out var cobj)) return;

        foreach(CardBase com in WalkCard(card))
        {
            foreach (HandlerAndPriority hp in SystemManager.cardSystem[com.id].handler)
            {
                if (!MsgType.IsAddToObjMsg(hp.msgType)) continue;
                List<CardAndPriority> list = cobj.msgTypeToHandler.GetValueOrDefault(hp.msgType, new List<CardAndPriority>());
                list.Add(new CardAndPriority(com, hp.priority, hp.handler, com.liveCnt));
                cobj.msgTypeToHandler[hp.msgType] = list;
            }
        }
    }
    private static void RemoveHandlerToCobj(CardBase card)
    {
        if (!TryGetCobj(card, out var cobj)) return;
        foreach (CardBase com in WalkCard(card))
        {
            com.liveCnt++;//生命数增加，原有的handler丢弃效果
        }
    }
    private static void AddComponentInWorld(CardBase card, CardBase com)
    {
        if (InWorld(card))
        {
            bool showUI = Sshowui_42.InShowUI(com);// 防止在下述位置修改掉导致重复生成
            AddHandlerToCobj(com);
            SystemManager.SendMsg(com, MsgType.OnItem, new MsgOnItem() { item = com, op = 1 });
            if (showUI)
            {
                SystemManager.SendMsg(com, MsgType.OnShowUI, new MsgOnShowUI() { op = 1 });
            }
        }
    }
    private static void RemoveComponentInWorld(CardBase card, CardBase com)
    {
        if (InWorld(card))
        {
            bool showUI = Sshowui_42.InShowUI(com);
            if (showUI)
            {
                SystemManager.SendMsg(com, MsgType.OnShowUI, new MsgOnShowUI() { op = -1 });
            }
            SystemManager.SendMsg(com, MsgType.OnItem, new MsgOnItem() { item = com, op = -1 });
            RemoveHandlerToCobj(com);
        }
    }
    private const int ComponentExPriority = 10000;
    private static void AddComponentAtIndex(CardBase card, CardBase com)
    {
        if (card == null) return;
        if (com.parent != null)
        {
            Debug.LogWarning(com + "的parent为" + com.parent + "，被尝试加到了" + card + "上");
        }
        com.parent = card;
        if (card.components == null) card.components = new List<CardBase>();

        card.components.Add(com);

        ActiveComponent(card, com, -card.components.Count + ComponentExPriority);
    }
    public static void AddComponent(CardBase card, params CardBase[] coms)
    {
        if (card.components == null) card.components = new List<CardBase>();
        foreach(CardBase com in coms)
        {
            AddComponentAtIndex(card, com);
        }
    }
    // 同时也可以移除用active产生的组件内容
    public static void RemoveComponent(CardBase card, CardBase com)
    {
        if (card == null || com == null) return;

        InactiveComponent(card, com);

        if (card.components != null)
            card.components.Remove(com);
        com.parent = null;
    }

    public static bool TryGetComponent<T>(CardBase card, out T com) where T : CardBase
    {
        com = null;
        if(card is T t) { com= t; return true; }
        if(card.components == null) return false;
        foreach(CardBase child in card.components)
        {
            if(child is T tt)
            {
                com = tt; return true;
            }
        }
        return false;
    }

    public static void ActiveComponent(CardBase card, CardBase com, int exPriority = 0)
    {
        if (card == null) return;
        if (com.parent != null && com.parent != card)
        {
            Debug.LogWarning(com + "的parent为" + com.parent + "，被尝试加到了" + card + "上");
        }
        com.parent = card;
        com.exPriority = exPriority;
        if (card.activeComponents == null) card.activeComponents = new List<CardBase>();

        card.activeComponents.Add(com);

        AddComponentInWorld(card, com);
    }
    public static void InactiveComponent(CardBase card, CardBase com)
    {
        if (card == null || com == null || card.activeComponents == null) return;

        RemoveComponentInWorld(card, com);

        card.activeComponents.Remove(com);
        com.parent = null;
    }

    public static CardBase GetTop(CardBase card)
    {
        if(card == null) return null;
        int tryCnt = 30;
        while(card.parent != null && card.parent!=world && tryCnt>0) { card = card.parent;tryCnt--; }
        if (tryCnt < 0) Debug.LogWarning("过于长的递归！");
        return card;
    }
    public static bool IsParentCard(CardBase card, CardBase parent)
    {
        if (card == null || parent == null) return false;
        while (card != null)
        {
            //Debug.Log(cardAbandon + " " + parent);
            if (card == parent) return true;
            card = card.parent;
        }
        return false;
    }
    public static bool TryGetCobj(CardBase card, out CObj_2 obj, bool needRbody = false)
    {
        obj = GetTop(card) as CObj_2;
        if (obj == null || obj.valid <= 0) return false;
        if (needRbody && obj.obj.rbody == null) return false;
        return true;
    }
    public static bool TryGetClive(CardBase card, out Clive_19 obj)
    {
        obj = GetTop(card) as Clive_19;
        if (obj == null || obj.valid <= 0) return false;
        return true;
    }
    public static int GetTeam(CardBase card)
    {
        if (TryGetCobj(card,out var obj)) 
            return obj.team;
        return -1;
    }
    public static void ReplaceCard(CardBase from, CardBase to)// 将from的位置改为to，from清除亲子关系
    {
        Debug.Log("uncomplete and unsafe");

        if (from == to) return;
        if (from == null || to == null)
        {
            Debug.LogError("空的替换调用");
            return;
        }

        if(from.components!=null)
        {
            AddComponent(to, from.components.ToArray());
            from.components = null;
        }

        if(from.parent!=null)
        {
            AddComponent(from.parent, to);
            RemoveComponent(from.parent, from);
        }
    }
    public interface ICloneAble
    {
        object Clone();
    }
    public static void CopySingleCard(CardBase card, CardBase newCard)
    {
        // 获取所有字段
        FieldInfo[] fields = card.GetType().GetFields();

        // 复制字段的值
        foreach (var field in fields)
        {
            // 复制每个字段的值
            if (Attribute.IsDefined(field, typeof(JsonIgnoreAttribute)))
                continue;

            if(field.FieldType == typeof(ICloneAble))
            {
                field.SetValue(newCard, (field.GetValue(card) as ICloneAble).Clone());
                continue;
            }

            // card的内容，额外操作
            if (field.FieldType == typeof(CardBase))
            {
                CardBase old = field.GetValue(card) as CardBase;

                CardBase newc = NewCopy(old);

                field.SetValue(newCard, newc);
                if (Attribute.IsDefined(field, typeof(NoActiveCard)))
                    continue;
                ActiveComponent(newCard, newc);
                continue;
            }

            if (field.FieldType == typeof(List<CardBase>))
            {
                List<CardBase> olds = field.GetValue(card) as List<CardBase>;

                List<CardBase> newcs = new List<CardBase>();

                foreach (CardBase old in olds)
                {
                    newcs.Add(NewCopy(old));
                }

                field.SetValue(newCard, newcs);

                if (Attribute.IsDefined(field, typeof(NoActiveCard)))
                    continue;
                foreach (CardBase newc in newcs)
                {
                    ActiveComponent(newCard, newc);
                }
                continue;
            }

            field.SetValue(newCard, field.GetValue(card));
        }

        SystemManager.TriClone(card, newCard);
    }
    public static CardBase NewCopy(CardBase card)
    {
        // 对于在运行的“热”物体，先卸下再拷贝能防止on的效果重复触发
        if(!InWorld(card))
            return NewCopyColdly(card);

        CardBase pa = card.parent;
        int exPri = card.exPriority;

        InactiveComponent(pa, card);
        CardBase copy = NewCopyColdly(card);
        ActiveComponent(pa, card, exPri);

        return copy;
    }
    private static CardBase NewCopyColdly(CardBase card)
    {
        // 创建一个新的 CardBase 实例
        CardBase newCard = (CardBase)Activator.CreateInstance(card.GetType());
        CopySingleCard(card, newCard);
        newCard.uid = GetUID();

        newCard.parent = null;
        newCard.components = new List<CardBase>();
        if (card.components != null)
        {
            foreach (CardBase com in card.components)
            {
                AddComponent(newCard, NewCopyColdly(com));
            }
        }
        return newCard;
    }
    public static string Cstr(CardBase card, bool givePreSeeGoodUI = false, bool noString = false, bool noUnderline = false)
    {
        string ans = "" + DataManager.GetName(card.id) + "";
        int dp = ExText.AddCard(card, givePreSeeGoodUI);
        if (noString) ans = "";
        if (!noUnderline) ans = "<u>" + ans + "</u>";
        ans = "<link=" + dp + ">" + ans + "</link>";
        return ans;
    }
    public static bool CardValid(CardBase card)
    {
        if (card == null) return false;
        return card.valid > 0;
    }
    public static Color GetCardColor(CardBase card)
    {
        return cardSystem[card.id].GetColor(card);
    }
    public static Color GetCardColor(int id)
    {
        return cardSystem[id].GetColor(CreateCard(id));
    }

    static int uidCnt = 1;
    /// <summary>
    /// 可以用来做各种事情的独一的id，比如领域持续更新的buff效果
    /// </summary>
    /// <returns></returns>
    public static int GetUID()
    {
        return uidCnt++;
    }
}
