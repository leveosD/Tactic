using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

public class AimSkill : Skill
{
    //[SerializeField] GameObject aimPrefab;
    GameObject aim;
    GameObject bullet;
    AsyncOperationHandle<GameObject> aimHandle;
    AsyncOperationHandle<GameObject> bulletHandle;
    string aimPath = "Assets/Prefabs/Skills/WeaponSkills/Firearms/Aim.prefab";
    protected string bulletPath = "Assets/Prefabs/Other/Bullet.prefab";

    protected Gun weapon;
    protected int bulletsCost = 1;
    protected float delay = 0.5f;
    protected float bulletSpeed = 3f;

    protected void Start()
    {
        
    }

    public override void Prepare()
    {
        base.Prepare();
        aimHandle = Addressables.LoadAssetAsync<GameObject>(aimPath);
        aimHandle.Completed += (AsyncOperationHandle<GameObject> operation) =>
        {
            if (operation.Status == AsyncOperationStatus.Succeeded)
            {
                aim = Instantiate(operation.Result, controller.transform);
            }
        };
    }

    public override IEnumerator DoSkill(Vector3 t)
    {
        controller.busy = true;
        if(bulletsCost > weapon.bullets)
            yield return StartCoroutine(weapon.Reload());
        Vector3 direction = new Vector3(t.x - controller.transform.position.x, t.y - controller.transform.position.y, 0);
        //RaycastHit2D hit = Physics2D.Raycast((Vector2)controller.transform.position, direction);
        float ang = (float)Math.Atan(t.y / t.x);
        bulletHandle = Addressables.LoadAssetAsync<GameObject>(bulletPath);
        yield return bulletHandle;
        for (int i = 0; i < bulletsCost; i++)
        {
            bullet = Instantiate(bulletHandle.Result, aim.transform);
            //bullet.transform.rotation = new Vector3(0, 0, ang);
            float tiltAroundZ = Input.GetAxis("Horizontal") * ang;
            Quaternion quater = Quaternion.Euler(0, 0, tiltAroundZ);
            transform.rotation = Quaternion.Slerp(transform.rotation, quater, Time.deltaTime * bulletSpeed);
            StartCoroutine(bullet.GetComponent<Bullet>().Shot(direction));
            yield return new WaitForSeconds(delay);
        }
        controller.busy = false;
        Destroy(aim);
        Addressables.Release(aimHandle);
        yield return null;
    }

    public Skill SetController(CharacterController c)
    {
        controller = c;
        if (controller.weapons[0].weaponType == "Gun")
            weapon = (Gun)controller.weapons[0];
        else if(controller.weapons[1].weaponType == "Gun")
            weapon = (Gun)controller.weapons[1];
        return this;
    }
}