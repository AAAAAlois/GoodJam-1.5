using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System;
using Sirenix.OdinInspector;
using System.Linq;

public class XLog : MonoBehaviour
{
    [Serializable]
    private struct Log
    {
        public enum LogType
        {
            TemperatureUp,
            TotalMoney,
        }
        public LogType type;
        public float value;
        public string content;
    };

    [Serializable]
    private struct UpgradeLog
    {
        public int upgradeIndex;
        public int upgradeLevel;
        public string content;
    }

    [SerializeField] private string defaultLog;
    [SerializeField] private List<Log> logs;
    [SerializeField] private List<UpgradeLog> upgradeLogs;
    [SerializeField] private List<TextMeshProUGUI> texts;
    [SerializeField] private float coolDownTime = 3.5f;

    private SortedList<float, string> temperatureUpLog = new SortedList<float, string>();
    private SortedList<int, string> moneyUpLog = new SortedList<int, string>();
    private int textIndex = 0;
    private Animator animator;
    private float showCD = 0;

    private int upgradeIndexToShow = -1;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        logs.ForEach(x =>
        {
            if(x.type == Log.LogType.TemperatureUp)
            {
                temperatureUpLog.Add(x.value, x.content);
            }
            else if(x.type == Log.LogType.TotalMoney)
            {
                moneyUpLog.Add(Mathf.RoundToInt(x.value), x.content);
            }
        });
        
        texts[textIndex].text = defaultLog;
        textIndex = (textIndex + 1) % texts.Count;
    }

    private void Start()
    {
        Score_manager.instance.OnUpgradeTech += CheckTechLog;
    }

    private void CheckTechLog(int index, int level)
    {
        if (upgradeIndexToShow != -1) return;
        UpgradeLog log = upgradeLogs.Where(x => x.upgradeIndex == index && x.upgradeLevel <= level).FirstOrDefault();
        if(log.content != "")
        {
            upgradeIndexToShow = upgradeLogs.IndexOf(log);
        }
    }

    private void Update()
    {
        if (showCD <= 0 && temperatureUpLog.Count > 0)
        {
            if (temperatureUpLog.Keys[0] <= Score_manager.instance.CO2 || Input.GetKeyDown(KeyCode.T))
            {
                texts[textIndex].text = temperatureUpLog.Values[0];
                temperatureUpLog.RemoveAt(0);
                textIndex = (textIndex + 1) % texts.Count;
                animator.SetTrigger("nextText");
                showCD = coolDownTime;
            }
        }

        if (showCD <= 0 && moneyUpLog.Count > 0)
        {
            if (moneyUpLog.Keys[0] <= Score_manager.instance.TotalGainedMoney || Input.GetKeyDown(KeyCode.M))
            {
                texts[textIndex].text = moneyUpLog.Values[0];
                moneyUpLog.RemoveAt(0);
                textIndex = (textIndex + 1) % texts.Count;
                animator.SetTrigger("nextText");
                showCD = coolDownTime;
            }
        }
        
        if (showCD <= 0 && upgradeIndexToShow != -1)
        {
            texts[textIndex].text = upgradeLogs[upgradeIndexToShow].content;
            upgradeLogs.RemoveAt(upgradeIndexToShow);
            textIndex = (textIndex + 1) % texts.Count;
            animator.SetTrigger("nextText");
            showCD = coolDownTime;
            upgradeIndexToShow = -1;
        }
        
        showCD -= Time.deltaTime;
    }
}
