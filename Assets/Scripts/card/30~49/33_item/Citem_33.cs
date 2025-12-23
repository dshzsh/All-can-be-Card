using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardManager;
using static SystemManager;

public class Citem_33 : CardBase
{
    public bool getted = false;
    public float pow = 1;

    [JsonIgnore]
    public GameObject _onTx;
}
public class DonTx: DataBase
{
    public string onTxName;
    public bool isPar;//是粒子，额外维护

    [JsonIgnore]
    public GameObject onTx;
    public override void Init(int id)
    {
        onTx = DataManager.LoadResource<GameObject>(id, onTxName);
    }
}
public class Ditem: DataBase
{
    public int rare = 0;
    public bool canGetFromTreature = true;

    public static HashSet<int> CantGetPool = new();
    public override void Init(int id)
    {
        base.Init(id);
        if(canGetFromTreature)
        {
            HashSet<int> ints = Sitem_33.itemPool.GetValueOrDefault(rare, new HashSet<int>());
            ints.Add(id);
            Sitem_33.itemPool[rare] = ints;
        }
        else CantGetPool.Add(id);
    }
}
public class DbasicAtt: DataBase
{
    public class AttAndReviseData
    {
        public string att;
        public BasicAtt revise;
        public AttAndRevise ToRevise()
        {
            return new AttAndRevise(BasicAttID.GetAttId(att), revise);
        }
    }
    public List<AttAndReviseData> attAndReviseDatas;
    public class AttAndValueData
    {
        public string att;
        public float value;//希望通过这个属性带来多少的提升
        public AttAndRevise ToRevise()
        {
            int attId = BasicAttID.GetAttId(att);
            BasicAtt basicAtt = new BasicAtt(value / BasicAttID.attValue[attId]);
            return new AttAndRevise(attId, basicAtt);
        }
    }
    public class AttAndValue
    {
        public int attId;
        public float value;//希望通过这个属性带来多少的提升
        public AttAndRevise ToRevise()
        {
            BasicAtt basicAtt = new BasicAtt(value / BasicAttID.attValue[attId]);
            return new AttAndRevise(attId, basicAtt);
        }
        public AttAndValue(int attId, float value)
        {
            this.attId = attId;
            this.value = value;
        }
    }
    public List<AttAndValueData> attAndValueDatas;

    [JsonIgnore]
    public List<AttAndRevise> attAndRevises;
    public override void Init(int id)
    {
        attAndRevises = new List<AttAndRevise>();
        if(attAndReviseDatas != null)
            foreach(AttAndReviseData attAndReviseData in attAndReviseDatas)
            {
                attAndRevises.Add(attAndReviseData.ToRevise());
            }
        if (attAndValueDatas != null)
            foreach (var attAndValueData in attAndValueDatas)
            {
                attAndRevises.Add(attAndValueData.ToRevise());
            }
    }
}
public class Dcolor : DataBase
{
    public float r = 0;
    public float g = 0;
    public float b = 0;
    public float a = 1;

    [JsonIgnore]
    public Color color;
    public override void Init(int id)
    {
        color = new Color(r, g, b, a);
    }
}
public class Sitem_33: SystemBase
{
    public static int mTSummonRandItem = MsgType.ParseMsgType(CardField.card, 33, 0);
    public class MsgSummonRandItem : MsgBase
    {
        public HashSet<int> filterGood;
        public HashSet<int> highPriGood;

        public MsgSummonRandItem()
        {
            filterGood = SGRbase_1.GetFilteredItems();
            highPriGood = new HashSet<int>();
        }
    }

    public static Dictionary<int, HashSet<int>> itemPool = new Dictionary<int, HashSet<int>>();
    public static int GetRandomItem(int rare = -1, params int[] needTags)
    {
        if (rare == -1) rare = SNtreasure_1.SummonRare();
        if(!itemPool.ContainsKey(rare)) { return 0; }
        HashSet<int> goodPool = MyTag.GetPoolWithTags(needTags);
        goodPool.IntersectWith(itemPool[rare]);

        MsgSummonRandItem smsg = new MsgSummonRandItem();
        SendMsgToPlayer(mTSummonRandItem, smsg);

        goodPool.ExceptWith(smsg.filterGood);
        smsg.highPriGood.IntersectWith(goodPool);
        if(smsg.highPriGood.Count > 0)
            return MyRandom.RandomInList(new List<int>(smsg.highPriGood));

        return MyRandom.RandomInList(new List<int>(goodPool));
    }
    public static List<int> GetNoRepeatRandomItems(int num, int rare, params int[] needTags)
    {
        List<int> cardToSums = new();
        for (int i = 0; i < num; i++)
        {
            int tid = 0;
            const int tryCnt = 10;
            for (int tt = 0; tt < tryCnt; tt++)
            {
                tid = Sitem_33.GetRandomItem(rare, needTags);
                if (tid == 0) break;
                bool noRep = true;
                for (int j = 0; j < i; j++)
                    if (tid == cardToSums[j])
                    {
                        noRep = false;
                        break;
                    }
                if (noRep)
                    break;
            }
            cardToSums.Add(tid);
        }
        return cardToSums;
    }
    public static class MaskColor
    {
        public static string ZD = "#2C2C57";
        public static string BD = "#204350";
        public static string MK = "#3D2650";//铭刻
        public static string HQ = "#5A4614";//获取
        public static string JH = "#2C2C57";//交互
        public static string Import = "#5E2F33";
    }
    public static class GoodUIColor
    {
        public static Color Rule = new(0.4056603f, 0.6554546f, 1);
        public static Color Magic = new(0.6470588f, 0.9590101f, 1);
        public static Color QhStone = new(0.6470588f, 0.7178496f, 1);
        public static Color Buff = new(0.8655301f, 0.5803922f, 1f);
        public static Color Item = new(1, 0.8679429f, 0.6470588f);
        public static Color Obj = new(0.6708112f, 1, 0.5792453f);
        public static Color Other = new(0.8566037f, 0.8566037f, 0.8566037f);
        public static Color Curse = new(0.6f, 0.6f, 0.6f);
    }
    public static class RareColor
    {
        public static Color[] quality_color = new Color[7] { Color.gray, Color.white, Color.green, new Color(43f / 255, 249f / 255, 1), new Color(115f / 255, 43f / 255, 245f / 255), Color.red, Color.black };
        public static string[] quality_str = { "一般", "普通", "优秀", "稀有", "史诗", "传说", "不存在" };
        public static string[] quality_colorstr = { "#7F7F7FFF", "#FFFFFFFF", "#00FF00FF", "#2B8BFFFF", "#732BF5FF", "#FF0000FF", "#000000FF" };
        public static Color GetRareColor(CardBase card)
        {
            Ditem ditem = DataManager.GetConfig<Ditem>(card.id);
            if (ditem == null) return Color.clear;
            return GetRareColor(ditem.rare);
        }
        public static Color GetRareColor(int rare)
        {
            rare = System.Math.Clamp(rare, 0, 6);
            return quality_color[rare];
        }
    }
    public static class SpecialTextColor
    {
        public static string Important = ColorUtility.ToHtmlStringRGB(MyRGB.ToFadeColor(Color.red, 0.5f));
    }
    public static bool IsMagic(CardBase card)
    {
        return card is Cmagicbase_17;
    }
    public static bool IsGood(CardBase card)
    {
        return MyTag.HaveTag(card.id, MyTag.CardTag.good);
    }
    public static class HealthShow
    {
        public class HealthShowData
        {
            public string att;
            public string color;
            public string describe;
        }
        public static Dictionary<int, string> healthColor = new();
        public static Dictionary<int, string> healthDescribe = new();
        public static void Add(string key, string color, string des)
        {
            int keyId = BasicAttID.GetAttId(key);
            healthColor.Add(keyId, MyRGB.ColorToStr(MyRGB.ToFadeColor(MyRGB.StrToColor(color))));
            healthDescribe.Add(keyId, des);
        }
    }

    public override void Create(CardBase _card)
    {
        base.Create(_card);
        if (MyTag.GetCardTag(_card.id).Contains(MyTag.CardTag.Pcurse))
        {
            AddComponent(_card, CreateCard<CGQwfxx_15>());
        }
    }
    public override void Init()
    {
        if (DataManager.GetConfig<DonTx>(id) != null)
            AddHandle(MsgType.OnItem, OnTx, HandlerPriority.After);
        if (DataManager.GetConfig<DbasicAtt>(id) != null)
            AddHandle(MsgType.OnItem, OnBasicAtt);
    }
    public void OnTx(CardBase _card,MsgBase _msg)
    {
        Citem_33 card = _card as Citem_33;
        MsgOnItem msg = _msg as MsgOnItem;
        DonTx config = DataManager.GetConfig<DonTx>(id);

        if (!TryGetCobj(card, out var cobj)) return;

        GiveOnTx(cobj, config.onTx, ref card._onTx, msg.op, config.isPar);
    }
    public void OnBasicAtt(CardBase _card, MsgBase _msg)
    {
        Citem_33 card = _card as Citem_33;
        MsgOnItem msg = _msg as MsgOnItem;
        DbasicAtt config = DataManager.GetConfig<DbasicAtt>(id);

        foreach(var att in config.attAndRevises)
        {
            att.UseOnLive(card, msg.op, card.pow);
        }
    }
    public static void GiveOnTx(CObj_2 cobj, GameObject obj, ref GameObject ontx, int op, bool isPar =false, bool isCenter=true)
    {
        //Debug.Log(op);
        if (op == 1)
        {
            ontx = GameObject.Instantiate(obj, cobj.obj.transform);
            if (isCenter) ontx.transform.position += cobj.centerOffset;
        }
        else
        {
            if (ontx != null)
            {
                if (!isPar)
                    GameObject.Destroy(ontx);
                else
                {
                    foreach (ParticleSystem par in ontx.GetComponentsInChildren<ParticleSystem>())
                        par.Stop(false, ParticleSystemStopBehavior.StopEmitting);
                }
            }
        }
    }
    public static void GiveInstantTx(CardBase card, GameObject prefab)
    {
        if (!TryGetCobj(card, out var cobj)) return;

        GameObject ontx = GameObject.Instantiate(prefab, cobj.obj.transform);
        ontx.transform.position += cobj.centerOffset;
    }
    public override string ParseDescribe(CardBase _card, string text)
    {
        //用“[]”包括的变量默认为受到pow影响的描述，自动乘上pow
        Citem_33 card = _card as Citem_33;

        text = ParseVariable(card, text);
        {
            //debug
            text = InsertEnd(text, "物品威力：" + string.Format("{0:0.00}", card.pow));

            Ditem ditem = DataManager.GetConfig<Ditem>(id);
            if (ditem != null)
            {
                text = InsertEnd(text, "物品稀有度：" + ditem.rare + "\n能否获取：" + ditem.canGetFromTreature);
            }
            Dtag dtag = DataManager.GetConfig<Dtag>(id);
            if (dtag != null)
            {
                string tags = "";
                foreach (string tag in dtag.tags) tags += tag + " ";
                text = InsertEnd(text, "物品tag：" + tags);
            }
        }
        
        text = AttAndReviseString(card) + text;
        return base.ParseDescribe(_card, text);
    }
    public static string AttAndReviseString(Citem_33 card)
    {
        DbasicAtt config = DataManager.GetConfig<DbasicAtt>(card.id);
        if (config == null) return "";
        string ans = "";
        foreach(var attData in config.attAndRevises)
        {
            ans += DataManager.GetBasicAttText(attData.revise.WithPow(card.pow), attData.attID) + "\n";
        }
        return ans;
    }
    public static string ParseVariable(Citem_33 card, string text)
    {
        return DataManager.ParseVariableWithPow(card, text, card.pow, true);
    }
    
    public static string ImportStr(string title, string color)
    {
        return "}<size=28><b>" + title + "</b></size>" + color + "{";
    }
    public static string InsertField(string text, string title, string color, string content)
    {
        int firstPos = text.IndexOf("}");
        if(firstPos == -1) //没有已经有的块，直接插入
        {
            text = text + ImportStr(title, color) + "\n" + content + "\n\n";
        }
        else
        {
            text = text.Insert(firstPos, ImportStr(title, color) + "\n" + content);
        }
        return text;
    }
    public static string InsertEnd(string text, string content)
    {
        return text + "\n" + content;
    }
    public static string InsertFront(string text, string content)
    {
        return content + "\n" + text;
    }
    public override Color GetColor(CardBase _card)
    {
        if (MyTag.GetCardTag(_card.id).Contains(MyTag.CardTag.Pcurse))
            return GoodUIColor.Curse;
        return GoodUIColor.Item;
    }
}