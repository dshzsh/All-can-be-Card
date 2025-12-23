using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoodSelectFit : MonoBehaviour
{
    const int midBackAdd = 5 + 28;//加上像素看上去会好看一点
    public GameObject pre;
    public GameObject end;
    public GameObject mid;

    public RectTransform scrollView;
    float maxScrollViewHeight;
    public RectTransform goodSeleted;
    float maxGoodSeletedHeight;
    //为了能够调整大小以后立刻生效对于presee的位置修正，只会设置一次
    public RectTransform goodPreSee;
    private bool preSeePosSetted = false;

    public bool resizeByContent;
    List<GameObject> mids = new List<GameObject>();

    RectTransform preRect,endRect;
    TextMeshProUGUI preTxt;
    TextMeshProUGUI endTxt;
    //List<TextMeshProUGUI> midTxts;
    //List<RectTransform> midRects;
    bool initted = false;
    
    public void Init()
    {
        if(initted) { return; }
        initted = true;
        preTxt = pre.GetComponent<TextMeshProUGUI>();
        endTxt = end.GetComponent<TextMeshProUGUI>();
        preRect= pre.GetComponent<RectTransform>();
        endRect= end.GetComponent<RectTransform>();
        mid.SetActive(false);
        for (int i=0;i<=5;i++)
        {
            mids.Add(Instantiate(mid, mid.transform));
            mids[i].transform.SetParent(mid.transform.parent);
        }
        maxScrollViewHeight = scrollView.sizeDelta.y;
        maxGoodSeletedHeight = goodSeleted.sizeDelta.y;
    }
    private void LateUpdate()
    {
        if (!initted) Init();
        //fit大小
        float interval = 12f;
        float nowhigh = 0f;
        nowhigh += preRect.sizeDelta.y + interval;
        for(int i=0;i<=5;i++)
        {
            if (mids[i].activeSelf == false) break;
            float high = mids[i].transform.GetChild(2).GetComponent<RectTransform>().sizeDelta.y + midBackAdd;
            RectTransform rect = mids[i].transform.GetChild(0).GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(0, high);
            RectTransform rectFa = mids[i].GetComponent<RectTransform>();
            rectFa.localPosition = new Vector3(0, -nowhigh, 0);
            nowhigh += high + interval;
        }
        nowhigh += interval;
        endRect.localPosition = new Vector3(0, -nowhigh, 0);
        nowhigh += endRect.sizeDelta.y + interval;
        GetComponent<RectTransform>().sizeDelta = new Vector2(0, nowhigh);
        if (!resizeByContent) return;
        if(nowhigh>maxScrollViewHeight) { nowhigh = maxScrollViewHeight; }
        scrollView.sizeDelta = new Vector2(scrollView.sizeDelta.x, nowhigh);
        goodSeleted.sizeDelta = new Vector2(goodSeleted.sizeDelta.x, maxGoodSeletedHeight + nowhigh - maxScrollViewHeight);
        if(goodPreSee!=null)
        {
            goodPreSee.sizeDelta = new Vector2(goodSeleted.sizeDelta.x, maxGoodSeletedHeight + nowhigh - maxScrollViewHeight);
            if(!preSeePosSetted)
            {
                preSeePosSetted = true;
                GoodPreSee.SetPreSeePos(goodPreSee);
            }
        }
    }
    private void ReadMid(string input)
    {
        //Debug.Log(input);

        // 正则表达式来匹配 "标题#颜色代码{页面}" 的格式
        string pattern = @"(?<title>[^#]+)#(?<color>[0-9A-Fa-f]{6})\{(?<page>[^\}]+)\}";

        MatchCollection matches = Regex.Matches(input, pattern);

        int i = 0;
        foreach (Match match in matches)
        {
            string title = match.Groups["title"].Value;
            string color = match.Groups["color"].Value;
            string page = match.Groups["page"].Value;
            if (page.Trim('\n').Length == 0) continue;

            string text = title + page;
            Color tcolor = MyRGB.StrToColor("#" + color);tcolor.a = 0.8f;
            mids[i].transform.GetChild(0).GetComponent<Image>().color = tcolor;
            mids[i].transform.GetChild(1).GetComponent<Image>().color =
                 MyRGB.ToFadeColor(MyRGB.ToFadeColor(tcolor, 0.15f), 0.1f, true);
            mids[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = " " + text.Trim('\n');
            mids[i].SetActive(true);
            i++;
        }
        for (int j = i; j <= 5; j++) mids[j].SetActive(false);
    }
    public void Set(string txt, bool resizeByContent)
    {
        if (!initted) Init();
        this.resizeByContent = resizeByContent;
        txt = txt + "\n";
        string preText = "";
        string endText = "";

        for (int j = 0; j <= 5; j++) mids[j].SetActive(false);

        // 找到第一个右花括号 } 的位置
        int preTextEndIndex = txt.IndexOf('}');
        // 找到最后一个左花括号 { 的位置
        int endTextStartIndex = txt.LastIndexOf('}');
        int endJudge = txt.LastIndexOf('{');
        if(endJudge!=-1&&endJudge>endTextStartIndex)
        {
            txt = txt + "}\n";//漏了最后一个}，自动补全
            endTextStartIndex = txt.Length - 1;
        }
        
        if (preTextEndIndex==-1)
             preText= txt;
        else//匹配成功那么两个都会有的
        {
            preText = txt.Substring(0, preTextEndIndex);
            endText = txt.Substring(endTextStartIndex + 1);
            if(preTextEndIndex!=endTextStartIndex)
            {
                string middleContent = txt.Substring(preTextEndIndex + 1, endTextStartIndex - preTextEndIndex);
                ReadMid(middleContent);
            }
        }
        preTxt.text = preText.TrimEnd('\n');
        
        endTxt.text = endText.TrimStart('\n');
    }
}
