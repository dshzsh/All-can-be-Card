using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MGQCewcc_63_goodui : MonoBehaviour
{
    public Image back, time, timeCover;
    public GameObject separation;

    public List<GameObject> separations = new List<GameObject>();
    public int maxShowCnt = 15;

    CGQCewcc_63 card;
    public void Set(CGQCewcc_63 card)
    {
        this.card = card;
        Update();
    }
    private void GiveSeparation(int si, float pos)
    {
        if(separations.Count<=si)
        {
            GameObject obj = GameObject.Instantiate(separation, separation.transform.parent);
            separations.Add(obj);
        }

        const float maxPos = 100;
        separations[si].transform.localPosition = new Vector3(pos * maxPos - maxPos / 2, 0);
        separations[si].SetActive(true);
    }
    private void Update()
    {
        if(!CardManager.CardValid(card))
        {
            card = null;
            return;
        }
        float max = card.exTimeMax * card.magicCd * card.pow;if (max <= 0) max = 1;
        time.transform.localScale = new Vector3(card.exTime / max, 1, 1);
        float magicCd = card.magicCd;if (magicCd <= 0) magicCd = 1;
        // cover条的逻辑是当前已经完整地存储了几个完整一个cd
        float coverTime = Mathf.Floor(card.exTime / magicCd) * magicCd;
        timeCover.transform.localScale = new Vector3(coverTime / max, 1, 1);
        // 根据每一个能出现完整cd的位置切下分割线
        int maxSepCnt = Mathf.FloorToInt(max / magicCd);
        if (maxSepCnt > maxShowCnt) return; // 过多层数就不显示
        for (int i = 1; i <= maxSepCnt; i++)
        {
            // print($"{magicCd} {max}");
            GiveSeparation(i-1, i * magicCd / max);
        }
        for (int i = maxSepCnt; i < separations.Count; i++)
            separations[i].SetActive(false);
    }
}
