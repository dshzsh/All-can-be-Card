using Newtonsoft.Json;
using PsychoticLab;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class MyTool
{
    public static float BigFloat = 1000000;
    public static float SmallFloat = 0.0001f;
    public static int BigInt = 1000000;
    
    public static T JsonDeserializeObject<T>(string json)
    {
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };
        return JsonConvert.DeserializeObject<T>(json, settings);
    }
    public static object JsonDeserializeObject(string json, System.Type t)
    {
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };
        return JsonConvert.DeserializeObject(json, t, settings);
    }
    public static string JsonSerializeObject(object obj)
    {
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };
        return JsonConvert.SerializeObject(obj, Formatting.Indented, settings);
    }
    public static bool Assert(bool condition, string msg)
    {
        if(!condition) { Debug.LogWarning(msg); }
        return condition;
    }
    public static bool IntervalTime(float interval, ref float time, float dtime)
    {
        time += dtime;
        if (time < interval) 
            return false;
        time -= interval;
        return true;
    }
    public static bool IntervalTimeSec(int interval, ref int time)
    {
        time += 1;
        if (time < interval)
            return false;
        time = 0;
        return true;
    }

    public static Vector3 RandScatter(Vector3 dir, float scatterRange, bool onlyMax = false)
    {
        // 生成随机的球面偏移角度
        float angle = Random.Range(-scatterRange, scatterRange);  // 生成偏移角度
        if (onlyMax) angle = scatterRange;
        float phi = Random.Range(0f, 360);  // 随机生成一个绕z轴的旋转角度

        // 选择一个任意的向量（例如 Vector3.up），并计算它与 dir 的叉积
        Vector3 perpendicular = Vector3.Cross(dir, Vector3.up);

        // 如果 dir 已经是垂直于 Vector3.up，叉积结果将为零向量，所以下面要做一个检查
        if (perpendicular.magnitude < 0.0001f)
        {
            // 如果 dir 已经接近 (0,1,0) 或 (0,-1,0)，使用不同的基准向量进行计算
            perpendicular = Vector3.Cross(dir, Vector3.right);
        }

        // 计算旋转矩阵
        Quaternion rotation = Quaternion.AngleAxis(angle, perpendicular);  // 生成绕dir方向旋转的四元数
        Quaternion rotation2 = Quaternion.AngleAxis(phi, dir); // 绕y轴的旋转


        return rotation2 * (rotation * dir);
    }
    public static Vector3 Vector3Multiply(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }
    public static string SuperNumText(float num)
    {
        if (num < 0) return "-" + SuperNumText(-num);
        if (num <= 0.05f)
            return string.Format("{0:0.000}", num);
        if (num <= 100f)
            return string.Format("{0:0.00}", num);
        if (num <= 10000f)
            return string.Format("{0:0.0}", num);
        if (num <= 1e7)
            return string.Format("{0:0.0}K", num / 1e3);
        if (num <= 1e10)
            return string.Format("{0:0.0}M", num / 1e6);
        if (num <= 1e13)
            return string.Format("{0:0.0}G", num / 1e9);
        return "无限大";
    }
    public static int FloatToIntRandomly(float x)
    {
        int ans = Mathf.FloorToInt(x);
        if (MyRandom.RandPer(x - ans)) ans++;
        return ans;
    }
}
public static class MyPhysics
{
    //标准都是希望一个物体受到力以后希望的移动多少m
    public static float ImpulseForceMul = 3f;
    public static int ObsLayMask = LayerMask.GetMask("obstacle", "ground", "live");
    public static int hitMask = ~LayerMask.GetMask("ghost");
    public static void GiveImpulseForce(Rigidbody robj, Vector3 dir, float force, Vector3 pos = default)
    {
        if (pos == default)
        {
            robj.AddForce(force * ImpulseForceMul * dir, ForceMode.Impulse);
        }
        else
            robj.AddForceAtPosition(force * ImpulseForceMul * dir, pos, ForceMode.Impulse);
        //Debug.Log($"rbobj:{robj},dir:{dir},force:{force}");
    }
    public static void GiveForce(Rigidbody robj, Vector3 dir, float force, Vector3 pos = default)
    {
        if (pos == default)
        {
            robj.AddForce(force * dir, ForceMode.Force);
            //Debug.Log($"rbobj:{robj},dir:{dir},force:{force}");
        }
        else
            robj.AddForceAtPosition(force * dir, pos, ForceMode.Force);
    }
    public static void GiveForceToCard(CObj_2 card, Vector3 dir, float force, float time = 0.1f)
    {
        CFmustMove_4 buff = CardManager.CreateCard<CFmustMove_4>();
        float mass = card.obj.rbody == null ? MyTool.BigFloat : card.obj.rbody.mass;
        buff.mustMoveVelocity = force / time / mass * dir;
        Sbuff_35.GiveBuff(Sysw_26.GetYsw(card), card, new MsgBeBuff(buff, time));
    }

}
public static class MyMath
{
    public static float BigFloat = 1000000;
    public static float SmallFloat = 0.0001f;
    public static int BigInt = 1000000;
    public static bool FloatEqual(float a, float b)
    {
        return (a - b) >= -0.001f && (a - b) <= 0.001f;
    }
    public static Vector3 V3zeroYNor(Vector3 v)
    {
        Vector3 ans = v;ans.y = 0;
        return ans.normalized;
    }
    public static Vector3 Vector3Multiply(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }
    public static Vector3 Vector3Div(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
    }
    /// <summary>
    /// 从a1->a2映射到b1->b2,输入值为p
    /// </summary>
    /// <param name="a1"></param>
    /// <param name="a2"></param>
    /// <param name="b1"></param>
    /// <param name="b2"></param>
    /// <param name="p"></param>
    /// <returns></returns>
    public static float LinerMap(float a1, float a2, float b1, float b2, float p)
    {
        if (a1 > a2)
        {
            float t = a1; a1 = a2; a2 = t;
            t = b1; b1 = b2; b2 = t;
        }
        if (p < a1) p = a1; if (p > a2) p = a2;
        return (p - a1) / (a2 - a1) * (b2 - b1) + b1;
    }
    public static Vector3 GetRandomPointOnCircle(Vector3 pos, Vector3 dir, float r)
    {
        // 生成一个垂直于法线的随机向量
        Vector3 randomVector = Vector3.Cross(dir, Random.insideUnitSphere).normalized;

        // 如果随机向量与法线平行(极小概率)，重新生成
        if (randomVector.magnitude < 0.0001f)
        {
            randomVector = Vector3.Cross(dir, Random.insideUnitSphere).normalized;
        }

        // 创建第二个垂直向量
        Vector3 perpendicularVector = Vector3.Cross(dir, randomVector).normalized;

        // 生成随机角度
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);

        // 计算圆上的点
        Vector3 ans = pos + r * (Mathf.Cos(randomAngle) * randomVector + Mathf.Sin(randomAngle) * perpendicularVector);

        return ans;
    }
    /// <summary>
    /// 角度计算从x轴正方向逆时针
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="dir"></param>
    /// <param name="forwardDist"></param>
    /// <param name="cornerDist"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static (Vector3 npos, Vector3 ndir) CalculateViewport(Vector3 pos, Vector3 dir,float forwardDist, float cornerDist, float angle)
    {
        Vector3 forward = dir.normalized;
        Vector3 right = -Vector3.Cross(forward, Vector3.up).normalized;
        Vector3 up = Vector3.Cross(forward, right).normalized;

        angle = angle * Mathf.Deg2Rad;

        Vector3 offsetToCorner = right * Mathf.Cos(angle) + up * Mathf.Sin(angle);

        Vector3 npos = pos + forward * forwardDist + offsetToCorner * cornerDist;

        Vector3 topPos = pos + forward * forwardDist - offsetToCorner * cornerDist;
        Vector3 ndir = (topPos - npos).normalized;

        return (npos, ndir);
    }


}
public static class MyRandom
{
    public static T RandomInList<T>(List<T> list, bool useMapRand = false)
    {
        if (list.Count == 0) return default;
        return list[Random.Range(0, list.Count)];
    }
    const int NoRepTryCnt = 10;
    public static List<int> NoRepRandom(int maxNum, int num, bool useMapRand = false)
    {
        List<int> cardToSums = new();
        for (int i = 0; i < num; i++)
        {
            int tid = Random.Range(0, maxNum);
            for (int tt = 0; tt < NoRepTryCnt; tt++)
            {
                bool noRep = true;
                for (int j = 0; j < i; j++)
                    if (tid == cardToSums[j])
                    {
                        noRep = false;
                        break;
                    }
                if (noRep)
                    break;
                tid = Random.Range(0, maxNum);
            }
            cardToSums.Add(tid);
        }
        return cardToSums;
    }
    public static List<T> NoRepRandomInList<T>(List<T> list, int num, bool useMapRand = false)
    {
        List<int> indexs = NoRepRandom(list.Count, num, useMapRand);
        List<T> ans = new List<T>();
        foreach(int index in indexs)
        {
            ans.Add(list[index]);
        }
        return ans;
    }
    public static bool RandPer(float percent, bool useMapRand = false)
    {
        return Random.Range(0f, 1f) <= percent;
    }
    public static Color RandColor()
    {
        return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }
    public static T GetRandomKeyByWeight<T>(Dictionary<T, float> weightedDictionary)
    {
        // 计算总权重
        float totalWeight = 0f;
        foreach (var pair in weightedDictionary)
        {
            totalWeight += pair.Value;
        }

        // 生成随机数
        float randomValue = Random.Range(0f, totalWeight);
        float currentWeight = 0f;

        // 遍历字典找到对应的key
        T key = default;
        foreach (var pair in weightedDictionary)
        {
            currentWeight += pair.Value;
            key = pair.Key;
            if (randomValue <= currentWeight)
            {
                return pair.Key;
            }
        }

        // 如果由于浮点精度问题没有返回，返回最后一个key
        return key;
    }
    public static Vector3 UpSphere()
    {
        Vector3 ans = Random.insideUnitSphere.normalized;
        if (ans.y < 0) ans.y = -ans.y;
        return ans;
    }
}
public static class MyRGB
{
    public static Color RGB(int r, int g, int b)
    {
        return new Color(r / 255f, g / 255f, b / 255f);
    }
    public static Color RGB(int r, int g, int b, int a)
    {
        return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
    }
    public static Color StrToColor(string x)
    {
        if (x.Length == 7) x += "FF";
        return RGB(Convert.ToInt32(x.Substring(1, 2), 16), Convert.ToInt32(x.Substring(3, 2), 16), Convert.ToInt32(x.Substring(5, 2), 16), Convert.ToInt32(x.Substring(7, 2), 16));
    }
    static string CtoStr(float x)
    {
        if (x > 1) x = 1; if (x < 0) x = 0;
        int t = (int)(x * 255);
        return string.Format("{0:X2}", t);
    }
    public static string ColorToStr(Color c)
    {
        return "#" + CtoStr(c.r) + CtoStr(c.g) + CtoStr(c.b) + CtoStr(c.a);
    }
    public const float SoftConst = 0.7f;
    public static Color ToFadeColor(Color c, float soft = SoftConst, bool onlymax = false)
    {
        Color color = c;
        if (onlymax)
        {
            float max = Mathf.Max(c.r, c.g, c.b);
            if (max == c.r) color.r += (1 - color.r) * soft;
            if (max == c.g) color.g += (1 - color.g) * soft;
            if (max == c.b) color.b += (1 - color.b) * soft;
            return color;
        }
        color.r += (1 - color.r) * soft;
        color.g += (1 - color.g) * soft;
        color.b += (1 - color.b) * soft;
        return color;
    }
    public static Color ToSaturateColor(Color c, float sat = 0.4f)
    {
        Color color = c;
        float min = Mathf.Min(color.r, color.g, color.b) * 0.8f;
        color -= new Color(min, min, min);
        float max = Mathf.Max(color.r, color.g, color.b);
        color /= max;
        color *= sat;
        color.a = c.a;
        return color;
    }
}