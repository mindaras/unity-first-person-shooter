using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text ammo;

    public void UpdateAmmo(int current, int total)
    {
        ammo.text = $"{current} / {total}";
    }
}
