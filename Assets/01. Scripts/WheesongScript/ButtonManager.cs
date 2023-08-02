using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public void OnActiveBtn(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void OffActiveBtn(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void OnOffActiveBtn(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }
}
