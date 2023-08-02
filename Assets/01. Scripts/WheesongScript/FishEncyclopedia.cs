using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishEncyclopedia : MonoBehaviour
{
    [SerializeField] private List<GameObject> rarePanel;

    private void Awake()
    {
        //PlayerPrefs.SetInt("A", 0);
        //PlayerPrefs.SetInt("B", 0);
        //PlayerPrefs.SetInt("C", 0);
        //PlayerPrefs.SetInt("D", 0);
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //    AddFish("A");
        //if (Input.GetKeyDown(KeyCode.W))
        //    AddFish("B");
        //if (Input.GetKeyDown(KeyCode.E))
        //    AddFish("C");
        //if (Input.GetKeyDown(KeyCode.R))
        //    AddFish("D");
    }

    public void OnEncyclopedia()
    {
        for (int i = 0; i < 5; i++)
        {
            int j = 0;
            foreach (FishData fish in Resources.LoadAll<FishData>($"Rare{i + 1}"))
            {
                FishSolt fishSolt = rarePanel[i].transform.GetChild(j).GetComponent<FishSolt>();
                fishSolt.cnt = PlayerPrefs.GetInt(fish.FishName);
                fishSolt.fishSprite = fish.FishSprite;
                fishSolt.fishName = fish.FishName;
                fishSolt.fishInformation = fish.FishInformation;
                j++;
            }
        }
    }

    public void AddFish(string fishName)
    {
        if (PlayerPrefs.GetInt(fishName) <= 0)//새로운 물고기냐
        {
            PlayerPrefs.SetInt(fishName, 1);
        }
        else//숫자만 늘리냐
        {
            PlayerPrefs.SetInt(fishName, PlayerPrefs.GetInt(fishName) + 1);
        }
        PlayerPrefs.Save();
    }
}