using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyTag
{
    public static class CardTag
    {
        public static int magic = StringToTag("magic");
        public static int good = StringToTag("good");
        public static int Pnormal = StringToTag("Pnormal");
        public static int PbossGood = StringToTag("PbossGood");
        public static int Pcurse = StringToTag("Pcurse");
        public static int qhstone = StringToTag("qhstone");
        public static int test = StringToTag("test");
        public static int empty = StringToTag("empty");
        public static int floor = StringToTag("floor");
        public static int npc = StringToTag("npc");
        public static int enemy = StringToTag("enemy");
        public static int env = StringToTag("env");
        public static int build = StringToTag("build");
        public static int rule = StringToTag("Prule");
    }
    private static Dictionary<int, HashSet<int>> pools = new Dictionary<int, HashSet<int>>();
    private static HashSet<int>[] cardTag = new HashSet<int>[1000];
    /// <summary>
    /// 出现哈希冲突的概率应该几乎不可能
    /// </summary>
    public static int StringToTag(string ss)
    {
        int ans = 0;
        for (int i = 0; i < 6 && i < ss.Length; i++)
        {
            ans *= 26;
            ans += ss[i] - 'a';
        }
        return ans;
    }
    public static void AddToPool(int id, List<string> list)
    {
        HashSet<int> tags = new HashSet<int>();
        foreach(string s in list)
        {
            int tagID = StringToTag(s);
            HashSet<int> ints = pools.GetValueOrDefault(tagID, new HashSet<int>());
            ints.Add(id);
            pools[tagID] = ints;
            tags.Add(tagID);
        }
        //Debug.Log(id);
        cardTag[id] = tags;
    }
    public static HashSet<int> GetPool(int tagID)
    {
        return pools.GetValueOrDefault(tagID, new HashSet<int>());
    }
    public static HashSet<int> GetPoolWithTags(params int[] tagIDs)
    {
        if (tagIDs == null || tagIDs.Length == 0) return new HashSet<int>();
        HashSet<int> ans = GetPool(tagIDs[0]);
        ans = new HashSet<int>(ans);
        if (tagIDs.Length == 1) return ans;
        for (int i=1;i<tagIDs.Length;i++)
        {
            ans.IntersectWith(GetPool(tagIDs[i]));
        }
        return ans;
    }
    public static HashSet<int> GetCardTag(int id)
    {
        if (cardTag[id] == null) return new HashSet<int>();
        return cardTag[id];
    }
    public static bool HaveTag(int id, int tagID)
    {
        return GetCardTag(id).Contains(tagID);
    }
}
public class Dtag : DataBase
{
    public List<string> tags = new List<string>();

    public override void Init(int id)
    {
        MyTag.AddToPool(id, tags);
    }
}