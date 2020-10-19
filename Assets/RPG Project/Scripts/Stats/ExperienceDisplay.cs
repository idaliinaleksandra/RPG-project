using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        BaseStats baseStats;
        Experience exp;

        [SerializeField] Text levelText;
        [SerializeField] Text expText;

        private void Awake()
        {
            baseStats = GameObject.Find("Player").GetComponent<BaseStats>();
            exp = GameObject.Find("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            levelText.text = String.Format("LEVEL {0:0}", baseStats.GetLevel());
            expText.text = String.Format("EXP: {0:0}", exp.GetEXP());
        }
    }
}
