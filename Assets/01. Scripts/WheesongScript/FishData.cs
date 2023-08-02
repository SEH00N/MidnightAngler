using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fish", menuName = "SO/Fish")]
public class FishData : ScriptableObject
{
    [SerializeField] private Sprite fishSprite;
    [SerializeField] private int fishRating;
    [SerializeField] private string fishName;
    [SerializeField] private string fishInformation;

    public Sprite FishSprite { get => fishSprite; set { fishSprite = value; } }
    public int FishRating { get => fishRating; set { fishRating = value; } }
    public string FishName { get => fishName; set { fishName = value; } }
    public string FishInformation { get => fishInformation; set { fishInformation = value; } }
}
