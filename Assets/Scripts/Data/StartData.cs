using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//오류날 경우, 한 줄 짜리 SingleJson으로 변환시켜주기.
[Serializable]
public class StartData
{
    public int ID;
    public int curFatigue;
    public int curStress;
    public int curHunger;
    public int maxFatigue;
    public int maxStress;
    public int maxHunger;
    public int curFloor;
    public int curRoom;
    public bool isFirstStart;

}

