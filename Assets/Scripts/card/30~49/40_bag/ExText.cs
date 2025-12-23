using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExText : MonoBehaviour, IPointerClickHandler
{
    public const int CYCLE_LENGTH = 100;
    public static CardBase[] bagGoodsDes = new CardBase[CYCLE_LENGTH];
    public static int despo = 0;//上面那个循环队列的队尾指针
    public static GameObject preSeeGoodUIPrefab;
    TextMeshProUGUI pTextMeshPro;

    private void Awake()
    {
        pTextMeshPro = GetComponent<TextMeshProUGUI>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        int linkIndex =
        TMP_TextUtilities.FindIntersectingLink(pTextMeshPro, eventData.position, null);
        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = pTextMeshPro.textInfo.linkInfo[linkIndex];
            int id = Convert.ToInt32(linkInfo.GetLinkID());
            if (id < 0) return;
            //card的id内容为替换后的cards内的位置
            GoodPreSee.SetPreSee(cards[id]);
            //Debug.Log(linkInfo.GetLinkID());
        }
    }

    private List<PreSeeGoodUI> goodUIs = new();
    private List<CardBase> cards = new();
    /// <summary>
    /// 从字符串中提取所有<link=数字>中的数字
    /// </summary>
    /// <param name="text">输入文本</param>
    /// <returns>提取到的数字列表</returns>
    private List<int> ExtractLinkNumbers()
    {
        string text = pTextMeshPro.text;
        //Debug.Log(text);

        List<int> numbers = new List<int>();

        // 定义正则表达式模式
        string pattern = @"<link=(\d+)>";

        string result = Regex.Replace(text, pattern, match =>
        {
            if (match.Groups.Count == 0 || !int.TryParse(match.Groups[1].Value, out int num))
                return $"<link={-1}>";
            if (num < CYCLE_LENGTH)
                numbers.Add(num);
            else num -= CYCLE_LENGTH;
            CardBase card = bagGoodsDes[num];
            cards.Add(card);
            // 返回重新编号后的格式
            return $"<link={cards.Count - 1}>";
        });
        pTextMeshPro.text = result;
        return numbers;
    }
    //横着：40+n*76
    //竖着：-80+n*(-90)
    //每行四个，每个空3行
    private void OnEnable()
    {
        //只会匹配正数
        List<int> numbers = ExtractLinkNumbers();

        string text = pTextMeshPro.text;

        if (numbers.Count == 0) return;

        for (int i = 0; i < numbers.Count; i++)
        {
            CardBase card = bagGoodsDes[numbers[i]];
            if (i < goodUIs.Count)
            {
                goodUIs[i].SetCard(card);
                goodUIs[i].gameObject.SetActive(true);
            }
            else
            {
                PreSeeGoodUI goodUI = UIBasic.GiveUI(preSeeGoodUIPrefab).GetComponent<PreSeeGoodUI>();
                goodUI.transform.SetParent(transform);
                goodUI.SetCard(card);
                goodUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(40 + i % 4 * 76, -80 - i / 4 * 90);
                goodUIs.Add(goodUI);
            }
        }

        //插入\n
        int firstNewLineIndex = text.IndexOf('\n');
        if (firstNewLineIndex != -1)
        {
            // 创建要插入的换行符字符串
            string newLines = new string('\n', ((numbers.Count - 1) / 4 + 1) * 3);
            // 在第一个换行符后插入
            pTextMeshPro.text = text.Insert(firstNewLineIndex + 1, newLines);
        }
        
    }
    private void OnDisable()
    {
        //删除card的预览的goodUI
        foreach(var goodUI in goodUIs)
        {
            goodUI.gameObject.SetActive(false);
        }
        cards.Clear();
    }
    public void Refresh()
    {
        OnDisable();
        OnEnable();
    }
    public static int AddCard(CardBase card, bool givePreSeeGoodUI = false)
    {
        despo = (despo + 1) % CYCLE_LENGTH;
        bagGoodsDes[despo] = card;
        //加上一个CYCLE_LENGTH标记为不突出显示的内容
        if (!givePreSeeGoodUI) despo += CYCLE_LENGTH;
        return despo;
    }
}
