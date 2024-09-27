using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Skills
{
    public class SkillList
    {
        /*private static SkillList instance;
        public List<Skill> skills;

        public SkillList()
        {
            GameObject parent = new GameObject("SkillParent");
            skills = new();
            Addressables.LoadAssetsAsync<GameObject>("Skills", (skill) =>
            {
                GameObject skillobj = Object.Instantiate(skill, parent.transform);
                skillobj.name = skill.name;
                skills.Add(skillobj.GetComponent<Skill>());
            });
        }

        public static SkillList GetInstance()
        {
            return instance ??= new SkillList();
        }*/
        public List<Skill> skills;


    }
}