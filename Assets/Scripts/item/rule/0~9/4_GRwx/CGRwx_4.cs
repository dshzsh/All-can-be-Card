using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGRwx_4 : CGRbase_1
{

}
public class DGRwx_4 : DataBase
{
    public string[] wxStrs;
    public string[] lightColorStrs;

    public static Color[] lightColors = new Color[10];
    public static int[] wxTags = new int[10];
    public static Dictionary<int, int> TagToWxId = new();
    public override void Init(int id)
    {
        for (int i = 0; i <= 5; i++)
        {
            wxTags[i] = MyTag.StringToTag(wxStrs[i]);
            TagToWxId[wxTags[i]] = i;
            if(ColorUtility.TryParseHtmlString(lightColorStrs[i], out var color))
            {
                lightColors[i] = color;
            }
            else
            {
                Debug.LogWarning("颜色不合法");
            }
        }

    }
}
public class SGRwx_4 : SGRbase_1
{
    public static class WxTag
    {
        public static int jin = MyTag.StringToTag("jin");
        public static int mu = MyTag.StringToTag("mu");
        public static int shui = MyTag.StringToTag("shui");
        public static int huo = MyTag.StringToTag("huo");
        public static int tu = MyTag.StringToTag("tu");
    }
    private static int GetWxId(int tagID)
    {
        return DGRwx_4.TagToWxId[tagID];
    }
    public static int GetItemWxTag(int id)
    {
        foreach (int tagID in MyTag.GetCardTag(id))
        {
            for (int i = 1; i <= 5; i++)
            {
                if (tagID == DGRwx_4.wxTags[i])
                    return tagID;
            }
        }
        return DGRwx_4.wxTags[0];
    }
    public static int GetItemWxTag(CardBase card)
    {
        return GetItemWxTag(card.id);
    }
    public static Color GetLightColor(int wxTag)
    {
        int wx = GetWxId(wxTag);
        return DGRwx_4.lightColors[wx];
    }
    public static Color GetItemLightColor(CardBase card)
    {
        int wxTag = GetItemWxTag(card);
        int wx = GetWxId(wxTag);
        return DGRwx_4.lightColors[wx];
    }

    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CGRwx_4 card = _card as CGRwx_4;
        MsgOnItem msg = _msg as MsgOnItem;
        DGRwx_4 config = basicConfig as DGRwx_4;

        Debug.Log(msg.op);
    }
}