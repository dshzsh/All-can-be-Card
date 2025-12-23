using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardManager;
using static Sitem_33;
using static SystemManager;

public class CGAattbase_1 : CardBase
{
    public BasicAtt bvalue;
}
public class DGAattbase_1 : DataBase
{
    public string att;
    public string color;
    public float value;
    public bool percentShow = false;
    public bool isLiveAtt = true;//是否是生物属性，排除耗魔等内容
    public override void Init(int id)
    {
        int vid = DataManager.GetVid(id);
        att = att.ToLower();
        BasicAttID.stringToAtt.Add(att, vid);
        BasicAttID.isLiveAtt[vid] = isLiveAtt;
        BasicAttID.attValue[vid] = value;
        BasicAttID.attData[vid] = this;
        Sitem_33.HealthShow.Add(att, color, DataManager.GetName(id));
    }
}
public class BasicAtt: ICloneAble
{
    public float value = 0;//基础数值，也是最先加算
    public float DirectMul = 0;//直接增加的百分比数值,+20%+30%=+50%
    public float finalMul = 1;//最终乘的一个数,*200%*200%=*400%
    public float finalAdd = 0;//在最终乘之后直接加上的数值
    object ICloneAble.Clone()
    {
        return new BasicAtt(this);
    }
    public BasicAtt() { }
    public BasicAtt(float value)
    {
        this.value = value;
    }
    public BasicAtt(BasicAtt other)
    {
        value = other.value;
        DirectMul = other.DirectMul;
        finalMul = other.finalMul;
        finalAdd = other.finalAdd;
    }
    public float GetValue()
    {
        return value * (1 + DirectMul) * finalMul + finalAdd;
    }
    public BasicAtt BeOnAtt(BasicAtt other, int op)
    {
        value += other.value * op;
        DirectMul += other.DirectMul * op;
        finalAdd += other.finalAdd * op;
        if (op == 1)
        {
            finalMul *= other.finalMul;
        }
        else
        {
            finalMul /= other.finalMul;
        }
        return this;
    }
    public float UseAttTo(float other, int op = 1)
    {
        if (op == 1)
        {
            other += value;
            other *= DirectMul + 1;
            other *= finalMul;
            other += finalAdd;
        }
        else
        {
            other -= finalAdd;
            other /= finalMul;
            other /= DirectMul + 1;
            other -= value;
        }
        return other;
    }
    public BasicAtt UseAttTo(BasicAtt other, int op)
    {
        return other.BeOnAtt(this, op);
    }
    public BasicAtt WithPow(float pow)
    {
        BasicAtt other = new BasicAtt();
        other.value = value * pow;
        other.DirectMul = DirectMul * pow;
        other.finalMul = (finalMul - 1) * pow + 1;
        other.finalAdd = finalAdd * pow;
        return other;
    }
    public override string ToString()
    {
        return ReviseString();
    }
    public string AttString()
    {
        return MyTool.SuperNumText(GetValue());
    }
    public string ReviseString(bool percentShow = false)
    {
        string ans = "";
        if (value != 0)
        {
            if (percentShow)
            {
                ans += (value > 0 ? "+" : "") + string.Format("{0:0.0}%", value * 100);
            }
            else
                ans += (value > 0 ? "+" : "") + MyTool.SuperNumText(value);
        }
        if (DirectMul != 0) ans += (DirectMul > 0 ? "+" : "") + string.Format("{0:0.0}%", DirectMul * 100);
        if (finalMul != 1) ans += string.Format("*{0:0.0}%", finalMul * 100);
        if (finalAdd != 0) ans += (finalAdd > 0 ? "+" : "") + MyTool.SuperNumText(finalAdd) + "F";
        if (ans.Length == 0) ans = "+0";
        return ans;
    }
    public bool IsNoRevise()
    {
        return value == 0 && DirectMul == 0 && finalMul == 1 && finalAdd == 0;
    }
}
public static class BasicAttID
{
    public const int MAX_ATT_NUM = 21;
    public const int none = 0;
    public const int healthMax = 2;
    public const int manaMax = 3;
    public const int atk = 4;
    public const int def = 5;
    public const int atkSpeed = 6;
    public const int speed = 7;
    public const int cost = 8;
    public const int cd = 9;
    public const int pow = 10;
    public const int crit = 11;
    public const int critDam = 12;
    public const int luck = 13;
    public const int cdSpeed = 14;
    public const int usePow = 15;
    public const int useDis = 16;
    public const int size = 17;

    public static Dictionary<string, int> stringToAtt = new();
    public static bool[] isLiveAtt = new bool[MAX_ATT_NUM];
    public static float[] attValue = new float[MAX_ATT_NUM];
    public static DGAattbase_1[] attData = new DGAattbase_1[MAX_ATT_NUM];
    public static int GetAttId(string attName)
    {
        attName = attName.ToLower();
        return stringToAtt[attName];
    }
    public static int GetAttPid(int vid)
    {
        return DataManager.VidToPid(vid, CardField.att);
    }
    public static bool NeedCard(int vid)
    {
        return SystemManager.cardSystem[GetAttPid(vid)].handler.Count > 0;
    }
    public static string ColoredAttText(int vid)
    {
        return "<color=" + HealthShow.healthColor[vid] + ">" + HealthShow.healthDescribe[vid] + "</color>";
    }
}
public class AttAndRevise
{
    public int attID;
    public BasicAtt revise;
    public AttAndRevise() { }
    public AttAndRevise(int att, BasicAtt revise)
    {
        this.attID = att;
        this.revise = revise;
    }
    public void UseOnLive(CardBase card, int op, float pow = 1)
    {
        if (card == null) return;
        BasicAtt revise = this.revise.WithPow(pow);
        if (attID == BasicAttID.size)// 大小其实不是真实的属性，统一在这里只是方便使用而已
        {
            if(TryGetCobj(card, out var cobj))
            {
                float oldSize = cobj.size.GetValue();
                cobj.size.BeOnAtt(revise, op);
                float newSize = cobj.size.GetValue();
                cobj.obj.transform.localScale *= newSize / oldSize;
            }
        }
        else
        {
            Shealth_4.GetAtt(card, attID).BeOnAtt(revise, op);
        }
    }
    public override string ToString()
    {
        return DataManager.GetBasicAttText(revise, attID);
    }
    public AttAndRevise WithPow(float pow)
    {
        BasicAtt revise = this.revise.WithPow(pow);
        return new AttAndRevise(attID, revise);
    }
}
public class SGAattbase_1 : SystemBase
{
    public override Color GetColor(CardBase _card)
    {
        return MyRGB.StrToColor(Sitem_33.HealthShow.healthColor[DataManager.GetVid(_card.id)]);
    }
}