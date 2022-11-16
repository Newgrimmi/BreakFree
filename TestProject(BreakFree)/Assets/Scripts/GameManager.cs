using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Camera _camera;
    [SerializeField] private Canvas _canvas;
    
    private Vector3 _createPosition;
    private bool isCanCreate;
    private float _countIdleMoney;

    public DamageAmplifier amplifier;
    public GameObject[] _units;
    public GameObject[] _levelUnit;
    public GameObject _unitsFolder;
    public Vector3 _canvasTransformCorrection;
    public int BeginNumberUnit;
    public int MaxUnitToScreen;
    public float IdleMoney;

    private void Awake()
    {
            Instance = this;
    }

    private void Start()
    {
        BeginNumberUnit = 0;
        _canvasTransformCorrection = new Vector3(540, 1170, 0);
        LoadGame();
    }

    private void Update()
    {
        _units = GameObject.FindGameObjectsWithTag("Unit");
        for (int i = 0; i < _units.Length; i++)
        {
            _countIdleMoney += _units[i].GetComponent<UnitController>().IdleDamage;
        }
        IdleMoney = _countIdleMoney;
        _countIdleMoney = 0;
        MoneyCounter.Instance.IdleMoney = IdleMoney;
        MoneyCounter.Instance.UpdateUI();
        MaxUnitAmount();

    }

    private void LoadGame()
    {
        
        if (SaveManager.Instance.hasLoaded)
        {
            MoneyCounter.Instance.CurrentMoney = SaveManager.Instance.AutoSave.Money;
        }
        for (int i = 0; i < SaveManager.Instance.AutoSave.CurrentUnitForLoad.Length; i++)
        {
            GameObject randomPos = Instantiate(_levelUnit[SaveManager.Instance.AutoSave.CurrentUnitForLoad[i]],
                SaveManager.Instance.AutoSave.RespawnPositionUnitForLoad[i], Quaternion.identity) as GameObject;
            randomPos.transform.SetParent(_unitsFolder.transform, false);
        }
        for (int i = 0; i < SaveManager.Instance.AutoSave.CurrentLevelAmplifiers.Length; i++)
        {
            MoneyCounter.Instance.amplifiers[i].Level = SaveManager.Instance.AutoSave.CurrentLevelAmplifiers[i];
        }
        CalculateOfflineIncome();
    }

    private void CalculateOfflineIncome()
    {
        var lastPlayedTime = DateTime.Parse(SaveManager.Instance.AutoSave.LastData);
        /*if (lastPlayedTime == null)
            return;*/
        int maxOfflineTime = 24 * 60 * 60;
        double OfflineTime = (DateTime.UtcNow - lastPlayedTime).TotalSeconds;

        if(OfflineTime > maxOfflineTime)
        {
            OfflineTime = maxOfflineTime;
        }
        float TotalIdleIncome = (float)OfflineTime * SaveManager.Instance.AutoSave.LastIdleDamage;
        MoneyCounter.Instance.CurrentMoney += TotalIdleIncome;
    }

    private void CreateUnit()
    {
        if (TimeCreateManager.Instance.Tick)
        {
            GameObject randomPos = Instantiate(_levelUnit[BeginNumberUnit], _levelUnit[BeginNumberUnit].transform.position = _createPosition, Quaternion.identity) as GameObject;
            randomPos.transform.SetParent(_unitsFolder.transform, false);
        }
    }

    private void CanCreateUnit()
    {
        _createPosition = new Vector3(UnityEngine.Random.Range(-300, 300), UnityEngine.Random.Range(-900, 500), 0);
        Ray ray = new Ray(_camera.transform.position, _createPosition + _canvasTransformCorrection - _camera.transform.position);
       
        if (Physics.Raycast(ray))
        {
            CheckUnitPosition();
            if (isCanCreate)
            {
                CreateUnit();
            }
            else
            {
                CheckUnitPosition();
            }
        }
        else
        {
            CreateUnit();
        }

    }

    private void CheckUnitPosition()
    {
        isCanCreate = false;
        for (int i = 0; i < _units.Length; i++)
        {
            if (_units[i].transform.position == _createPosition)
            {
                _createPosition = new Vector3(UnityEngine.Random.Range(-300, 300), UnityEngine.Random.Range(-900, 500), 0);
            }
            else
            {
                isCanCreate = true;
            }
        }
    }

    private void MaxUnitAmount()
    {
        if (_units.Length < MaxUnitToScreen)
        {
            CanCreateUnit();
        }
    }

    private void SaveInfo()
    {

        for (int i = 0; i < _units.Length; i++)
        {
            SaveManager.Instance.AutoSave.CurrentUnitForLoad[i] = _units[i].transform.gameObject.GetComponent<UnitController>().CurrentUnit;
            SaveManager.Instance.AutoSave.RespawnPositionUnitForLoad[i] = _units[i].transform.gameObject.transform.position - _canvasTransformCorrection;
        }
        for (int i = 0; i < MoneyCounter.Instance.amplifierPrefs.Count; i++)
        {
            
            SaveManager.Instance.AutoSave.CurrentLevelAmplifiers[i] = MoneyCounter.Instance.amplifiers[i].Level;
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            SaveGame();
        }
    }

    public void SaveGame()
    {
        SaveManager.Instance.AutoSave.LastIdleDamage = IdleMoney/5;
        SaveManager.Instance.AutoSave.LastData = DateTime.UtcNow.ToString();
        SaveManager.Instance.AutoSave.Money = MoneyCounter.Instance.CurrentMoney;
        SaveManager.Instance.AutoSave.CurrentLevelAmplifiers = new int[MoneyCounter.Instance.amplifierPrefs.Count];
        SaveManager.Instance.AutoSave.RespawnPositionUnitForLoad = new Vector3[GameManager.Instance._units.Length];
        SaveManager.Instance.AutoSave.CurrentUnitForLoad = new int[GameManager.Instance._units.Length];
        SaveInfo();
        SaveManager.Instance.Save();
        //UnityEditor.EditorApplication.isPlaying = false; // ¬ключать, если проверка будет на юнити
        Application.Quit();
    }
}
