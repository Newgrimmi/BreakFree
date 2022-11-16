using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeCreateManager : MonoBehaviour
{
    public static TimeCreateManager Instance { get; private set;  }

    [SerializeField] private TMP_Text timeForCreateText;

    private Image img;

    private float currentTime;

    public float TimeForCreate;
    public bool Tick;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        TimeForCreate = 10 - MoneyCounter.Instance.amplifiers[4].Value;
        img = GetComponent<Image>();
        currentTime = TimeForCreate ;    
        
    }

    void Update()
    {
        TimeForCreate = 10 - MoneyCounter.Instance.amplifiers[4].Value;
        Tick = false;
        if(GameManager.Instance._units.Length < GameManager.Instance.MaxUnitToScreen)
        {
            currentTime -= Time.deltaTime;
            timeForCreateText.text = Mathf.Round(currentTime).ToString();

            if (currentTime <= 0)
            {
                Tick = true;
                currentTime = TimeForCreate;// - MoneyCounter.Instance.amplifiers[4].Value;
            }

            img.fillAmount = currentTime / TimeForCreate ;
        }
        
    }
}
