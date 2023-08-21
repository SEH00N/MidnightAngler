using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DataTemp : MonoBehaviour
{
    public static DataTemp Instance;

    public int money;
    public int ruby;
    public int shipLevel;
    public int area;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
}
