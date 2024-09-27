using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    Button button;
    Weapon weapon;

    // Start is called before the first frame update
    void Start()
    {
        /*button = GetComponent<Button>();
        weapon = GetComponent<Weapon>();
        button.onClick.AddListener(OnClick);*/

    }

    void OnClick()
    {
        /*weapon.controller.CurrentWeapon = weapon;
        foreach (GameObject skill in weapon.skillist)
        {
            skill.SetActive(true);
        }*/
    }
}