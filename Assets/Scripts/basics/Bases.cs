using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;

public class CardBase
{
    public int uid = 0;
    public int id;
    public int exPriority = 0;

    [JsonIgnore]
    public int liveCnt = 0;
    [JsonIgnore]
    public CardBase parent;
    [JsonIgnore]
    public CardBase container;
    [JsonIgnore]
    public List<CardBase> components;
    [JsonIgnore]
    public List<CardBase> activeComponents;// 额外的不存储在component内的激活的内容
    [JsonIgnore]
    public int valid = 1;
}
[Serializable]
public class DataBase
{
    public virtual void Init(int id)
    {

    }
}
public class SystemBase
{
    public int id;
    public DataBase basicConfig;

    public List<HandlerAndPriority> handler = new List<HandlerAndPriority>();
    public void AddHandle(int type, CardMsgHandler chandler, int priority = 0)
    {
        HandlerAndPriority hp = new();
        hp.handler = chandler;
        hp.priority = priority;
        hp.msgType = type;
        hp.islocal = false;
        
        handler.Add(hp);
    }
    public virtual void Init() { }
    public virtual void Create(CardBase _card) { }
    public virtual void Clone(CardBase _from, CardBase _to) 
    {
        
    }
    public virtual string ParseDescribe(CardBase _card, string text) { return text; }
    public virtual Color GetColor(CardBase _card)
    {
        return Sitem_33.GoodUIColor.Other;
    }

}
public class MsgBase
{
    public bool valid = true;

    public List<CardAndPriority> triList = new List<CardAndPriority>();
    public int triPos = 0;
}

public class MsgBaseWithTag: MsgBase
{
    private Dictionary<int, CardBase> dict = new Dictionary<int, CardBase>();

    public CardBase GetTag(int tag)
    {
        return dict[tag];
    }
    public void AddTag(CardBase tag)
    {
        dict[tag.id] = tag;
    }
    public bool TryGetTag(int id, out CardBase tag)
    {
        return dict.TryGetValue(id, out tag);
    }
    public bool HaveTag(int id)
    {
        return dict.ContainsKey(id);
    }
    public bool TryGetTag<T>(out T tag) where T : CardBase
    {
        int id = GetTypeId(typeof(T));
        CardBase ans;
        if(!dict.TryGetValue(id, out ans))
        {
            tag = null;
            return false;
        }
        tag = ans as T;
        return tag != null;
    }
}