using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    Button button;
    Skill skill;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        skill = GetComponent<Skill>();
        button.onClick.AddListener(skill.Prepare);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
