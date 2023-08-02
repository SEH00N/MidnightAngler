using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishSolt : MonoBehaviour
{
    public Sprite fishSprite;
    public string fishName;
    public string fishInformation;
    public int cnt;

    private Button btn;
    private Image slotImage;
    private TextMeshProUGUI cntText;

    private Image fishImage;
    private TextMeshProUGUI fishNameText;
    private TextMeshProUGUI fishInformationText;

    private void Awake()
    {
        btn = GetComponent<Button>();
        slotImage = transform.GetChild(0).GetComponent<Image>();
        cntText = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();

        btn.onClick.AddListener(OpenInformation);

        fishImage = GameObject.Find("FishInformationSlot").GetComponent<Image>();
        fishNameText = GameObject.Find("FishInformationName").GetComponent<TextMeshProUGUI>();
        fishInformationText = GameObject.Find("FishInformationText").GetComponent<TextMeshProUGUI>();
    }

    public void OnEnable()
    {
        StartCoroutine(SlotSetting());
    }

    void OpenInformation()
    {
        if (cnt <= 0) return;

        fishImage.sprite = slotImage.sprite;
        fishNameText.text = fishName;
        fishInformationText.text = fishInformation;
    }

    IEnumerator SlotSetting()
    {
        yield return new WaitForSeconds(0.01f);
        slotImage.sprite = cnt > 0 ? fishSprite : Resources.Load<Sprite>("Image/beenfish");
        cntText.text = cnt.ToString();
    }
}
