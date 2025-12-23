using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class Ctime_41 : CardBase
{

}

public class Stime_41: SystemBase
{
    private static int pauseCnt = 0;
    public static float timeSpeed = 1f;
    public static void PauseTime(int op)
    {
        pauseCnt += op;
        if (pauseCnt == 1)
            Time.timeScale = 0;
        else if (pauseCnt == 0)
            Time.timeScale = timeSpeed;
    }
}