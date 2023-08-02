using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;

    public int money;
    public int ruby;

    private void Awake()
    {
        instance = this;
    }
}
