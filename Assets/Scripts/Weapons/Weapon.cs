using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

public class Weapon : MonoBehaviour
{
    //[SerializeField] public List<GameObject> skillist;
    [SerializeField] public List<Skill> skillist;
    [SerializeField] private GameObject buttonList;
    GameObject[] buttons;
    AsyncOperationHandle<Texture2D> img;

    protected static bool[] availableSkills;
    protected CharacterController controller;

    public string weaponType;
    public bool isChosen = false;

    //RectTransform canvas;
    //List<AsyncOperationHandle<GameObject>> handles;

    // Start is called before the first frame update
    protected void Start()
    {
        //availableSkills = new bool[skillist.Count];
        /*canvas = this.transform.parent.GetComponent<RectTransform>();
        handles = new List<AsyncOperationHandle<GameObject>>();
        foreach(Skill skill in skillist)
        {
            handles.Add(new AsyncOperationHandle<GameObject>());
            handles[i] = Addressables.LoadAssetAsync<GameObject>(weapon_names[i]);
            handles[i].Completed += (AsyncOperationHandle<GameObject> operation) =>
            {
                if (operation.Status == AsyncOperationStatus.Succeeded)
                {
                    var s = Instantiate(operation.Result, canvas);
                    skilllist.Add(w.GetComponent<Skill>().SetController(controller));
                    int i = weapons.Count - 1;
                    float y = 50 + 100 * (i % 2 == 0 ? 1 : -1);
                    weapons[i].transform.position = new Vector3(50, y, 0);
                }
            };
        }*/
        //buttons = buttonList.GetComponentsInChildren<GameObject>();
        availableSkills = new bool[skillist.Count];
        for(int i = 0; i < skillist.Count; i++)
        {
            availableSkills[i] = false;
        }

        /*float x = 200f;
        foreach(GameObject skill in skillist)
        {
            GameObject s = Instantiate(skill, this.transform);
            s.GetComponent<Skill>().SetController(controller);
            s.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, 0);
            x += 100f;
        }*/
    }

    public Weapon SetController(CharacterController c)
    {
        controller = c;
        return this;
    }

    public virtual void Take()
    {
        controller.CurrentWeapon = this;
        isChosen = true;
        /*foreach (GameObject skill in skillist)
        {
            skill.SetActive(true);
            Debug.Log(skill.activeSelf);
        }*/
        for(int i = 0; i < skillist.Count; i++)
        {
            buttons[i].GetComponent<Button>().clicked += () => skillist[i].Prepare();
            if (img.IsValid())
                Addressables.Release(img);
            img = Addressables.LoadAssetAsync<Texture2D>(skillist[i].Name);
            img.Completed += (AsyncOperationHandle<Texture2D> operation) =>
            {
                buttons[i].GetComponent<Image>().image = operation.Result;
            };
        }
    }
    
    public virtual void Put()
    {
        isChosen = false;
        /*foreach (GameObject skill in skillist)
        {
            skill.SetActive(false);
            Debug.Log(skill.activeSelf);
        }*/
    }
}
