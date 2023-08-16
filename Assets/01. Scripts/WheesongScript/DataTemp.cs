using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MoneyTemp
{
    public int money;
    public int ruby;

    public MoneyTemp(int money, int ruby)
    {
        this.money = money;
        this.ruby = ruby;
    }
}

public struct LevelTemp
{
    public int shipLevel;

    public LevelTemp(int shipLevel)
    {
        this.shipLevel = shipLevel;
    }
}

public struct AreaTemp
{
    public int area;

    public AreaTemp(int area)
    {
        this.area = area;
    }
}

public class DataTemp : MonoBehaviour
{
    public static DataTemp Instance;

    public MoneyTemp moneyTemp;
    public LevelTemp levelTemp;
    public AreaTemp areaTemp;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        moneyTemp = new MoneyTemp(999999, 9999);
        levelTemp = new LevelTemp(1);
        areaTemp = new AreaTemp(1);
    }
}
