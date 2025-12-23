using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class CGBbase_1 : Citem_33
{

}
public class DGBbase_1 : DataBase
{
    public float changeRate;

    public static float SchangeRate;
    public override void Init(int id)
    {
        SchangeRate = changeRate;
    }
}
public class DGBbuildTag : DataBase
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
public class SGBbase_1 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(mTSummonRandItem, SummonRandItem);
    }
    void SummonRandItem(CardBase _card, MsgBase _msg)
    {
        CGBbase_1 card = _card as CGBbase_1;
        MsgSummonRandItem msg = _msg as MsgSummonRandItem;
        DGBbuildTag config = DataManager.GetConfig<DGBbuildTag>(id);

        if (MyRandom.RandPer(DGBbase_1.SchangeRate, true))
        {
            msg.highPriGood.UnionWith(MyTag.GetPool(config.tagID));
        }
    }
    public override Color GetColor(CardBase _card)
    {
        return GoodUIColor.Rule;
    }
}