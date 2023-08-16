using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishdexPanelBtn : MonoBehaviour
{
    [SerializeField] private List<GameObject> rarePanel;

    public void ActiveEncyclopediaPanel(GameObject activePanel)
    {
        foreach(GameObject allPanel in rarePanel)
            allPanel.SetActive(false);

        activePanel.SetActive(true);
    }
}
