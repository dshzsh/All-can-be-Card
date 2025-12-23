using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;
using Newtonsoft.Json;

public class CGRbase_1 : Citem_33
{

}
public class DGRbase_1 : DataBase
{

}
public class DFilterTag : DataBase
{
    public string tag;

    [JsonIgnore]
    public int tagID;
    public override void Init(int id)
    {
        base.Init(id);
        tagID = MyTag.StringToTag(tag);
    }
}
public class SGRbase_1 : Sitem_33
{
    public static HashSet<int> GetFilteredItems()
    {
        return filteredItems;
    }
    private static HashSet<int> filteredItems = new HashSet<int>();

    public override void Create(CardBase _card)
    {
        base.Create(_card);
        AddComponent(_card, CreateCard<CGQwfxx_15>());
    }
    public override void Init()
    {
        base.Init();
        DFilterTag dFilterTag = DataManager.GetConfig<DFilterTag>(id);
        if (dFilterTag != null)
        {
            AddHandle(mTSummonRandItem, SummonRandItem);
            filteredItems.UnionWith(MyTag.GetPool(dFilterTag.tagID));
        }
    }
    void SummonRandItem(CardBase _card, MsgBase _msg)
    {
        MsgSummonRandItem msg = _msg as MsgSummonRandItem;
        DFilterTag dFilterTag = DataManager.GetConfig<DFilterTag>(id);

        msg.filterGood.ExceptWith(MyTag.GetPool(dFilterTag.tagID));
    }

    public override Color GetColor(CardBase _card)
    {
        return GoodUIColor.Rule;
    }
}