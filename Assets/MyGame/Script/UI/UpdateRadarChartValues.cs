using UnityEngine;
using System;
using System.Collections.Generic;

// 确保这个脚本和RadarPolygon在同一个namespace下，或者在这里引用对应的namespace
namespace UnityEngine.UI.Extensions
{
    public class ModifyRadarChartValues : MonoBehaviour
    {
        public RadarPolygon radarChart1; // 第一个 RadarPolygon 组件
        public RadarPolygon radarChart2; // 第二个 RadarPolygon 组件
        public SpiritBagManager spiritBagManager;

        void Start()
        {
            spiritBagManager.BeastSelected += UpdateRadarChartValues;
            spiritBagManager.TriggerBeastSelected();
            RadarPolygon radarChart = GetComponent<RadarPolygon>();

        }

        void OnDestroy()
        {    
            spiritBagManager.BeastSelected -= UpdateRadarChartValues;
        }

        private void UpdateRadarChartValues(SpiritualBeast beast)
        {

            Dictionary<string, int> statLimits = BeastGenerator.GetStatLimits(beast.ethnicity);
            radarChart1.value = Backgroundpic(statLimits);
            radarChart2.value = Currpic(statLimits, beast);

            radarChart1.SetVerticesDirty(); 
            radarChart2.SetVerticesDirty(); 
   
        }

        private float[] Backgroundpic(Dictionary<string, int> statLimits)
        {
            float maxLimit = 530f; // 设置最大值用于归一化
            float hp = statLimits.ContainsKey("Hp") ? statLimits["Hp"] / maxLimit : 0f;
            float ap = statLimits.ContainsKey("Ap") ? statLimits["Ap"] / maxLimit : 0f;
            float attack = statLimits.ContainsKey("Attack") ? statLimits["Attack"] / maxLimit : 0f;
            float armor = statLimits.ContainsKey("Armor") ? statLimits["Armor"] / maxLimit : 0f;
            float mr = statLimits.ContainsKey("Mr") ? statLimits["Mr"] / maxLimit : 0f;
            float speed = statLimits.ContainsKey("Speed") ? statLimits["Speed"] / maxLimit : 0f;

        
            return new float[] { ap, hp, attack, armor, speed, mr};
        }

        private float[] Currpic(Dictionary<string, int> statLimits, SpiritualBeast beast)
        {
            float maxLimit = 530f; // 设置最大值用于归一化
            float hp = statLimits.ContainsKey("Hp") ? (float)beast.Hp / maxLimit : 0f;
            float ap = statLimits.ContainsKey("Ap") ? (float)beast.Ap / maxLimit: 0f;
            float attack = statLimits.ContainsKey("Attack") ? (float)beast.Attack / maxLimit : 0f;
            float armor = statLimits.ContainsKey("Armor") ? (float)beast.Armor / maxLimit : 0f;
            float mr = statLimits.ContainsKey("Mr") ? (float)beast.Mr / maxLimit : 0f;
            float speed = statLimits.ContainsKey("Speed") ? (float)beast.Speed / maxLimit : 0f;
            return new float[] { ap, hp, attack, armor, speed, mr};

        }
    }
}
