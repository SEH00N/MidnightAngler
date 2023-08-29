using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishArea : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI boatLvText;
    [SerializeField] private int[] areaLevel;

    private void Awake()
    {
        BoatLevelSetting();
    }

    public void AreaSetting(int area)
    {
        if (DataTemp.Instance.shipLevel < areaLevel[area - 1]) return;
        Debug.Log("¾ÆÀ×");
        DataTemp.Instance.area = area;
        BoatLevelSetting();
    }

    private void BoatLevelSetting()
        => boatLvText.text = DataTemp.Instance.shipLevel.ToString();
}
