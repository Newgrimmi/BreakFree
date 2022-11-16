using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyCounter : MonoBehaviour
{
    public static MoneyCounter Instance;

    [SerializeField] private NotationMethod _notationMethod;
    [SerializeField] private TextMeshProUGUI _currentMoney;
    [SerializeField] private TextMeshProUGUI _idleMoneyCount;

    public List<AmplifierPrefs> amplifierPrefs;
    public List<DamageAmplifier> amplifiers;

    public float CurrentMoney;
    public float IdleMoney;

    private void Awake()
    {
        Instance = this;
        amplifiers = new List<DamageAmplifier>()
        {
            new DamageAmplifier(DamageAmplifier.AmplifierType.PLUS_CLICK_DAMAGE, 0, 1f, 100, 100, 1000),
            new DamageAmplifier(DamageAmplifier.AmplifierType.CLICK_CRIT, 0, 1f, 500, 1000, 50),
            new DamageAmplifier(DamageAmplifier.AmplifierType.PLUS_PASSIVE_DAMAGE, 0, 0.5f, 500, 500, 1000),
            new DamageAmplifier(DamageAmplifier.AmplifierType.PLUS_CLICK_CRIT_DAMAGE, 0, 5f, 1000, 1000, 1000),
            new DamageAmplifier(DamageAmplifier.AmplifierType.REDUCE_CREATE_TIME, 0, 0.5f, 50000, 100000, 15),
        };
        for (int i = 0; i < amplifierPrefs.Count; i++)
            amplifierPrefs[i].SetData(amplifiers[i]);
    }

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        _currentMoney.text = _notationMethod.NotationMethods(CurrentMoney, "");
        _idleMoneyCount.text = _notationMethod.NotationMethods(IdleMoney / 5, "") + "/сек";
    }
}
