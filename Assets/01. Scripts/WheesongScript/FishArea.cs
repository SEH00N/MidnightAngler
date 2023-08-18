using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishArea : MonoBehaviour
{
    //[SerializeField] private Button[] areaBtns;

    //private void Awake()
    //{
    //    for (int i = 0; i < areaBtns.Length; i++)
    //    {
    //        areaBtns[i].onClick.AddListener(() => AreaSetting(i + 1));
    //    }
    //}

    public void AreaSetting(int area)
    {
        DataTemp.Instance.areaTemp.area = area;
    }
}
