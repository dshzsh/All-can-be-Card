using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Sitem_33;

[Serializable]
public class CardData
{
    public int id = 0;
    public string rawname = "null";
    public string name = "空";
    public string description = "空";
    public List<DataBase> configs = null;
}
public class MetaData
{
    public CardField cardField;
    public string fName;
    public string folder = "";
    public string headString;
}
public static class DataManager
{
    public static string dataPath = "0_data";
    private static bool isInited = false;
    private static List<CardData> cardDatas = new List<CardData>();
    private static List<int> idField= new List<int>();//对应了该领域下的id为0的数据的起始物理id
    private static List<CardField> nameField= new List<CardField>();
    private static Dictionary<CardField, string> fieldName = new Dictionary<CardField, string>();
    private static Dictionary<CardField, string> fieldFolder = new Dictionary<CardField, string>();
    private static Dictionary<CardField, int> fieldId = new Dictionary<CardField, int>();
    public static int VidToPid(int vid, CardField field)
    {
        return fieldId[field] + vid;
    }
    public static void PidToVid(int pid, out int vid, out CardField field)
    {
        int ii = 0;
        while (idField.Count > ii + 1 && pid >= idField[ii + 1]) ii++;
        //Debug.Log(ii + " " + idField[ii] + " " + nameField[ii]);
        vid = pid - idField[ii];
        field = nameField[ii];
    }
    public static string GetIdFolderName(int id)
    {
        int start = -10;
        while (start + 19 < id) start += 20;
        if (start < 0) return "0~9";
        return start + "~" + (start + 19);
    }
    public static string GetResourceFolder(int vid, CardField field)
    {
        string folderAndName= fieldName[field];
        if (fieldFolder.TryGetValue(field, out var folder))
            folderAndName = folder + folderAndName;
        return folderAndName + "/" + GetIdFolderName(vid) + "/" + vid + "_" + cardDatas[VidToPid(vid, field)].rawname;
    }
    private static void ParseRef(string path, out CardField field, out int vid)
    {
        string[] ss = path.Split('*');
        string ff = ss[1];
        field = CardField.card;
        foreach (var value in fieldName)
        {
            if (value.Value.Equals(ff))
            {
                field = value.Key;
                break;
            }
        }
        vid = int.Parse(ss[2]);
    }
    public static T LoadResource<T>(int pid, string path) where T : UnityEngine.Object
    {
        if (path == null) return null;
        if (path[0]=='*') //引用其他id的内容
        {
            ParseRef(path, out var field, out var vid);
            string[] ss = path.Split('*');
            path = ss[3];
            return LoadResource<T>(vid, path, field);
            //Debug.Log(vid + " " + path);
        }
        PidToVid(pid, out int vvid, out CardField vfield);
        T ans = Resources.Load<T>(GetResourceFolder(vvid, vfield) + "/" + path);
        if(ans==null)
        {
            //Debug.LogWarning("加载资源出现问题：" + pid + " path:" + GetResourceFolder(vvid, vfield) + "/" + path);
        }
        return ans;
    }
    public static T LoadResource<T>(int vid, string path, CardField field) where T : UnityEngine.Object
    {
        if (path == null) return null;
        
        return Resources.Load<T>(GetResourceFolder(vid, field) + "/" + path);
    }
    public static void Init()
    {
        if (isInited) return;
        isInited = true;

        //读入主文件
        {
            TextAsset meta = Resources.Load<TextAsset>(dataPath + "/meta");
            List<MetaData> datas = MyTool.JsonDeserializeObject<List<MetaData>>(meta.text);
            foreach(MetaData data in datas)
            {
                data.cardField = Enum.Parse<CardField>(data.fName);
                fieldName[data.cardField] = data.fName;
                fieldFolder[data.cardField] = data.folder;
            }
        }

        foreach (var _field in fieldName)
        {
            string field = _field.Value;
            TextAsset dataMeta = Resources.Load<TextAsset>(dataPath + "/" + fieldFolder[_field.Key] + field);
            List<CardData> data = MyTool.JsonDeserializeObject<List<CardData>>(dataMeta.text);
            fieldId[_field.Key] = cardDatas.Count;
            idField.Add(cardDatas.Count);
            nameField.Add(_field.Key);
            cardDatas.AddRange(data);
        }

        // 加载数据
        for (int i = 0; i < cardDatas.Count; i++)
        {
            if (cardDatas[i].configs == null) continue;
            foreach(DataBase dataBase in cardDatas[i].configs)
            {
                dataBase.Init(i);
            }
        }
    }
    public static int GetCardCount()
    {
        return cardDatas.Count;
    }
    public static int GetVid(int pid)
    {
        return cardDatas[pid].id;
    }
    public static string GetRawName(int id)
    {
        return cardDatas[id].rawname;
    }
    public static string GetName(int id)
    {
        return cardDatas[id].name;
    }
    public static string GetDescirbe(int id)
    {
        return cardDatas[id].description;
    }
    public static object GetVariableValue(CardBase card, string variableName)
    {
        object value = null;
        var fieldInfo = card.GetType().GetField(variableName);
        if (fieldInfo != null) // cardAbandon 中的值
        {
            value = fieldInfo.GetValue(card);
            return value;
        }

        var propertyInfo = card.GetType().GetProperty(variableName);

        if (propertyInfo!=null)
        {
            value = propertyInfo.GetValue(card);
            return value;
        }

        if (cardDatas[card.id].configs != null) // data 中的值
        {
            foreach (DataBase config in cardDatas[card.id].configs)
            {
                fieldInfo = config.GetType().GetField(variableName);
                if (fieldInfo != null)
                {
                    value = fieldInfo.GetValue(config);
                    break;
                }
            }
        }
        return value;
    }
    public static string GetBasicAttText(BasicAtt basicAtt, int healthMul)
    {
        if (healthMul == 0) return basicAtt.ToString();
        CGAattbase_1 attCard = CardManager.CreateCard(BasicAttID.GetAttPid(healthMul)) as CGAattbase_1;
        attCard.bvalue = basicAtt;
        string describe = CardManager.Cstr(attCard, noUnderline: true);
        string ans = string.Format("{0}{1}", describe, basicAtt.ReviseString(BasicAttID.attData[healthMul].percentShow));
        ans = "<color=" + HealthShow.healthColor[healthMul] + ">" + ans + "</color>";
        return ans;
    }
    /// <summary>
    /// 获取倍率字段，如“100%攻击力”
    /// </summary>
    public static string GetHealthText(float num, int healthMul, CardBase card, bool percentShow = false)
    {
        string ans = string.Format("{0:0.0}%{1}", num * 100, HealthShow.healthDescribe[healthMul]);
        if (Shealth_4.HaveHealth(card) && BasicAttID.isLiveAtt[healthMul])
        {
            float att = Shealth_4.GetAttf(card, healthMul);
            
            if(percentShow)
                ans += "(" + MyTool.SuperNumText(num * att * 100) + "%)";
            else
                ans += "(" + MyTool.SuperNumText(num * att) + ")";
        }
        ans = "<color=" + HealthShow.healthColor[healthMul] + ">" + ans + "</color>";
        return ans;
    }
    public static string GetLuckOddsText(float num, CardBase card)
    {
        string ans = string.Format("{0:0.0}%", num * 100);
        if (Shealth_4.HaveHealth(card))
        {
            float luckedOdds = SGAluck_13.LuckedOdds(card, num);

            ans += "(幸运结算后:" + string.Format("{0:0.0}%", luckedOdds * 100) + ")";
        }
        else
            ans += "(受幸运影响)";
        ans = "<color=" + HealthShow.healthColor[BasicAttID.luck] + ">" + ans + "</color>";
        return ans;
    }
    public interface IParseStringWithPow
    {
        public string ToStringWithPow(CardBase card, float pow);
    }
    public static string GetFloatText(float num)
    {
        string value = MyTool.SuperNumText(num);

        //再加一个显示表示这是数值的颜色
        value = "<color=#B2DEEFFF>" + value + "</color>";
        return value;
    }
    public static string ParseVariableWithPow(CardBase card, string text, float tpow, bool squareBrackets)
    {
        string des = text;
        string regex = @"([\+\-*!@#%,\w]+)";

        if (squareBrackets) regex = "\\[" + regex + "\\]";
        else regex = "\\{" + regex + "\\}";

        var matches = System.Text.RegularExpressions.Regex.Matches(des, regex);

        foreach (System.Text.RegularExpressions.Match match in matches)
        {
            string matchText = match.Groups[1].Value;
            string[] _ss = matchText.Split(',');
            List<string> ss = new List<string>(_ss);
            float pow = tpow;

            string variableName = "";
            string spOp = "";
            bool percentShow = false;

            // 处理特殊的后置字符: -是负pow，%是百分比显示
            for (int i=0; i<ss.Count; i++)
            {
                string s = ss[i];
                if(s.Equals("-"))
                {
                    pow = 1 / pow;
                    ss.RemoveAt(i);
                    i--;
                }
                else if(s.Equals("%"))
                {
                    percentShow = true;
                    ss.RemoveAt(i);
                    i--;
                }
            }

            if (ss.Count == 1) variableName = ss[0];//形如{var}
            else if (ss.Count == 2)// 形如{cardAbandon,*[field]*[vid]}
            {
                spOp = ss[1];
                variableName = ss[0];
            }

            //Debug.Log(variableName + " " + spOp);
            object value = null;

            if (variableName.Equals("card"))//引用基本卡牌，仅供预览(如寒冷)
            {
                ParseRef(spOp, out var field, out var vid);
                CardBase _card = CardManager.CreateCard(VidToPid(vid, field));
                if (squareBrackets && _card is Citem_33 citem) citem.pow *= pow;
                value = CardManager.Cstr(_card);
            }
            else if (variableName.Equals("useluck"))//基于幸运的概率结算
            {
                float fvalue = (float)GetVariableValue(card, spOp);

                value = GetLuckOddsText(fvalue, card);
            }
            else
            {
                //Debug.Log(cardAbandon.GetType().GetProperty(variableName));
                // 通过反射获取变量值
                value = GetVariableValue(card, variableName);

                if (value is int ivalue) // 统一转换成 float 的显示逻辑
                    value = ivalue * 1f;

                if (value is float fvalue)
                {
                    if (ss.Count == 1) // 形如{damage}
                    {
                        if (percentShow) // 形如{speedUp,%}
                            value = string.Format("{0:0.0}%", fvalue * pow * 100);
                        else value = MyTool.SuperNumText(fvalue * pow);

                        //再加一个显示表示这是数值的颜色
                        value = "<color=#B2DEEFFF>" + value + "</color>";
                    }
                    else // 形如{damage,Atk}
                    {
                        value = GetHealthText(fvalue * pow, BasicAttID.GetAttId(spOp), card, percentShow);
                    }
                }
                else if (value is CardBase vcard)//可以操作装配的内容,spOp为+则显示小框，++可以装配。better:只有+不能装配拖拽
                {
                    value = CardManager.Cstr(vcard, spOp.Equals("+") || spOp.Equals("++"));
                }
                else if (value is BasicAtt basicAtt)//形如[basicAtt,cost]
                {
                    value = GetBasicAttText(basicAtt.WithPow(pow), BasicAttID.GetAttId(spOp));
                }
                else if (value is AttAndRevise attRevise)//形如[basicAtt]
                {
                    value = GetBasicAttText(attRevise.revise.WithPow(pow), attRevise.attID);
                }
                else if (value is List<AttAndRevise> attRevises)//形如[basicAtt]
                {
                    value = "";
                    foreach (var att in attRevises)
                        value += GetBasicAttText(att.revise.WithPow(pow), att.attID) + "\n";
                }
                else if (value is Color color)
                {
                    value = "<color=" + MyRGB.ColorToStr(color) + ">" + MyRGB.ColorToStr(color) + "</color>";
                }
                else if (value is IParseStringWithPow iparse)
                {
                    value = iparse.ToStringWithPow(card, pow);
                }
            }


            if (value != null)
            {
                if (squareBrackets)
                {
                    string ans = value.ToString();
                    //Debug.Log(ans+" "+ ans[0].Equals('<'));
                    if (ans.StartsWith("<color") && ans.EndsWith("</color>"))//形如<color=#123456></color>
                    {
                        ans = ans.Insert(17, "[");
                        ans = ans.Insert(ans.Length - 8, "]");
                    }
                    else ans = "[" + ans + "]";
                    //Debug.Log(ans);
                    des = des.Replace("[" + matchText + "]", ans);
                }
                else
                    des = des.Replace("{" + matchText + "}", value.ToString());
            }
                
        }
        return des;
    }
    public static string KeyWordReplace(string ans)
    {
        ans = ans.Replace("[主动]", ImportStr("主动", MaskColor.ZD));
        ans = ans.Replace("[被动]", ImportStr("被动", MaskColor.BD));
        ans = ans.Replace("[交互]", ImportStr("交互", MaskColor.JH));
        ans = ans.Replace("[获取]", ImportStr("获取", MaskColor.HQ));
        ans = ans.Replace("[C重要]", "#" + Sitem_33.SpecialTextColor.Important);

        int tpos = ans.LastIndexOf("\n\n");
        if (tpos != -1) ans = ans.Insert(tpos, "}");//解释词
        else ans = ans + "}";//如果没有两连换行，则认为是无末尾的情况

        return ans;
    }
    public static string GetDescirbe(CardBase card)
    {
        int id = SystemManager.GetCardId(card);
        string des = cardDatas[id].description;

        des = "[被动]\n" + des;
        des = KeyWordReplace(des);

        des = ParseVariableWithPow(card, des, 1, false);//有时可能只有一个前括号的显示，这个是显示框的格式的锅

        des = SystemManager.TriDescribe(card, des);
        MsgParseDescribe pmsg = new MsgParseDescribe(card, des);
        SystemManager.SendMsg(card, MsgType.ParseDescribe, pmsg);
        SystemManager.SendMsg(card, MsgType.SelfParseDescribe, pmsg);
        des = pmsg.text;

        return des;
    }
    public static T GetConfig<T>(int id) where T : DataBase
    {
        if (cardDatas[id].configs == null) return null;
        foreach(DataBase config in cardDatas[id].configs)
        {
            if(config is T) return (T)config;
        }
        return null;
    }
    public static bool TryGetConfig<T>(int id, out T config) where T : DataBase
    {
        config = GetConfig<T>(id);
        return (config != null);
    }
    public static List<DataBase> GetConfigs(int id)
    {
        return cardDatas[id].configs;
    }
}
