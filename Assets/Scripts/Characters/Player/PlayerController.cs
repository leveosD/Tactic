using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Pathfinding;

public class PlayerController : CharacterController
{
    protected string[] weapon_names;
    RectTransform canvas;
    AsyncOperationHandle<GameObject> coldarmHandle;
    AsyncOperationHandle<GameObject> firearmHandle;
    //AsyncOperationHandle<GameObject> handle;

    //[SerializeField] private MouseBehaviour mouse;

    // Start is called before the first frame update
    protected void Start()
    {
        //временно закоминтил, потом вернуть
        base.Start();
        canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
        weapon_names = new string[] { "Sword", "Pistol" };
        coldarmHandle = Addressables.LoadAssetAsync<GameObject>(weapon_names[0]);
        coldarmHandle.Completed += (AsyncOperationHandle<GameObject> operation) =>
        {
            if (operation.Status == AsyncOperationStatus.Succeeded)
            {
                var w = Instantiate(operation.Result, canvas);
                weapons.Add(w.GetComponent<Weapon>().SetController(this));
                //weapons[0].transform.position = new Vector3(150, 100, 0);
            }
        };

        firearmHandle = Addressables.LoadAssetAsync<GameObject>(weapon_names[1]);
        firearmHandle.Completed += (AsyncOperationHandle<GameObject> operation) =>
        {
            if (operation.Status == AsyncOperationStatus.Succeeded)
            {
                var w = Instantiate(operation.Result, canvas);
                weapons.Add(w.GetComponent<Weapon>().SetController(this));
                w.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 150);
                //w.transform.position += new Vector3(0, 150, 0);
            }
        };
        CurrentWeapon = weapons[0];
        //Start from character controller
        points = 3;
        steps_per_action = 5;
        steps = points * steps_per_action;
        //mode = CharacterEvent.STAND;
        //movement = GetComponent<MovementBehaviour>();
        //ai = GetComponent<AILerp>();
        //ai.canMove = false;
        //seeker = GetComponent<Seeker>();
        //rb = GetComponent<Rigidbody2D>();
        //col = GetComponent<BoxCollider2D> ();
    }

    // Update is called once per frame
    void Update()
    {
        /*foreach (var handle in handles)
        {
            Debug.Log(handle);
        }
        Debug.Log("+++++++++++++++");*/
    }

    void OnDestroy()
    {
        Addressables.Release(coldarmHandle);
        //Addressables.Release(firearmHandle);
    }
}
