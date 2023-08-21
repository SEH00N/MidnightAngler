using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public enum Cost { money = 1, ruby = 2 }

public class ShipPanel : MonoBehaviour
{
    [Header("Image")]
    [SerializeField] private Sprite[] mapSprite;
    [SerializeField] private Sprite[] shipSprite;

    [SerializeField] private SpriteRenderer ship;
    [SerializeField] private Image map;
    [SerializeField] private Image nowShip;
    [SerializeField] private Image newShip;

    [Header("LevelText")]
    [SerializeField] private TextMeshProUGUI nowLevel;
    [SerializeField] private TextMeshProUGUI newLevel;

    [Header("Value")]
    [SerializeField] private int[] mapLevel;
    [SerializeField] private int[] moneyCost;
    [SerializeField] private int[] rubyCost;

    [Header("Button")]
    [SerializeField] private Button moneyBtn;
    [SerializeField] private Button rubyBtn;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI rubyText;

    private void Start()
    {
        MapUpdate();
        ShipUpdate();
        ButtonUpdate();
    }

    public void ShipLevelUp(int cost)
    {
        int money = DataTemp.Instance.money;
        int ruby = DataTemp.Instance.ruby;
        int shipLevel = DataTemp.Instance.shipLevel - 1;

        if (cost == 1)//돈
        {
            if (moneyCost[shipLevel] > money)//돈 부족
                return;
            money -= moneyCost[shipLevel];
        }
        else if (cost == 2)//루비
        {
            if (rubyCost[shipLevel] > ruby)//루비 부족
                return;
            ruby -= rubyCost[shipLevel];
        }
        DataTemp.Instance.money = money;
        DataTemp.Instance.ruby = ruby;
        DataTemp.Instance.shipLevel++;

        MapUpdate();
        ShipUpdate();
        ButtonUpdate();
    }

    void MapUpdate() //맵 이미지
    {
        for (int i = 0; i < mapLevel.Length; i++)
        {
            if (DataTemp.Instance.shipLevel >= mapLevel[i])
                map.sprite = mapSprite[i];
        }
    }

    void ShipUpdate() //배 레벨 이미지
    {
        int nowLevelcost = DataTemp.Instance.shipLevel;
        int newLevelcost = DataTemp.Instance.shipLevel + 1;

        nowLevel.text = $"Lv.{nowLevelcost}";
        newLevel.text = $"Lv.{newLevelcost}";

        for (int i = 0; i < mapLevel.Length; i++)
        {
            if (nowLevelcost >= mapLevel[i])
            {
                ship.sprite = shipSprite[i];
                nowShip.sprite = shipSprite[i];
            }
        }

        for (int i = 0; i < mapLevel.Length; i++)
        {
            if (newLevelcost >= mapLevel[i])
            {
                newShip.sprite = shipSprite[i];
            }
        }
    }

    void ButtonUpdate()
    {
        int money = DataTemp.Instance.money;
        int ruby = DataTemp.Instance.ruby;
        int shipLevel = DataTemp.Instance.shipLevel - 1;

        moneyBtn.GetComponent<Image>().color = moneyCost[shipLevel] > money
            ? Color.gray : Color.white;
        rubyBtn.GetComponent<Image>().color = moneyCost[shipLevel] > ruby
            ? Color.gray : Color.white;

        moneyText.text = string.Format("{0:#,###}", moneyCost[shipLevel]) + " k";
        rubyText.text = string.Format("{0:#,###}", rubyCost[shipLevel]) + " k";
    }
}
