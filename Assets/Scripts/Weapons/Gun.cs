using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    int maxBullets;
    public int bullets;

    public IEnumerator Reload()
    {
        if(bullets == 0)
        {
            bullets = maxBullets;
            controller.animator.SetBool("attack", true);
            yield return new WaitForSeconds(1);
            controller.animator.SetBool("attack", false);
        }
    }

    protected void Start()
    {
        base.Start();
        weaponType = "Gun";
        maxBullets = 6;
        bullets = maxBullets;
    }
}
