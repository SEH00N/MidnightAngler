using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public int shipLevel;

    private void Awake()
    {
        instance = this;
    }
}
