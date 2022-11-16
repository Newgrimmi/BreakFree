using UnityEngine;
using TMPro;

public class AmplifierPrefs : MonoBehaviour

{
    [SerializeField] private NotationMethod _notationMethod;

    [Header("Level")]
    [SerializeField] private TextMeshProUGUI _levelClick;
    [SerializeField] private TextMeshProUGUI _levelCritClick;
    [SerializeField] private TextMeshProUGUI _levelIdleDamage;
    [SerializeField] private TextMeshProUGUI _levelCritClickDamage;
    [SerializeField] private TextMeshProUGUI _levelTimeCreate;

    [Header("Price")]
    [SerializeField] private TextMeshProUGUI _levelClickPrice;
    [SerializeField] private TextMeshProUGUI _levelCritClickPrice;
    [SerializeField] private TextMeshProUGUI _levelIdleDamagePrice;
    [SerializeField] private TextMeshProUGUI _levelCritClickDamagePrice;
    [SerializeField] private TextMeshProUGUI _levelTimeCreatePrice;

    private CanvasGroup _group;
    private DamageAmplifier amplifier;

    private void Start()
    {
        UpdateUI();
    }
    private void Update()
    {
        if (amplifier.Level < amplifier.MaxLevel)
        {
            _group.alpha = MoneyCounter.Instance.CurrentMoney >= amplifier.Price ? 1 : .5f;
        } 
        else
        {
            _group.alpha = .5f;
        }
    }
    public void SetData(DamageAmplifier amplifier)
    {
        _group = GetComponentInChildren<CanvasGroup>();
        this.amplifier = amplifier;
        UpdateUI();
    }

    public void BuyUpgrade()
    {
        if(amplifier.Level < amplifier.MaxLevel)
        {
            if (MoneyCounter.Instance.CurrentMoney < amplifier.Price)
                return;

            MoneyCounter.Instance.CurrentMoney -= amplifier.Price;
            amplifier.LevelUp();
            UpdateUI();
            MoneyCounter.Instance.UpdateUI();
        }
    }

    private void UpdateUI()
        {
            _levelClick.text ="+" + MoneyCounter.Instance.amplifiers[0].Level * 100  + "%";
            _levelCritClick.text = "+" + MoneyCounter.Instance.amplifiers[1].Level + "%";
            _levelIdleDamage.text = "+" + MoneyCounter.Instance.amplifiers[2].Level * 50 + "%";
            _levelCritClickDamage.text = "x" + (25+MoneyCounter.Instance.amplifiers[3].Level * 5);
            _levelTimeCreate.text = "- " + MoneyCounter.Instance.amplifiers[4].Level*0.5;

            _levelClickPrice.text = _notationMethod.NotationMethods(MoneyCounter.Instance.amplifiers[0].Price, "");
            _levelCritClickPrice.text = _notationMethod.NotationMethods(MoneyCounter.Instance.amplifiers[1].Price, "");
            _levelIdleDamagePrice.text = _notationMethod.NotationMethods(MoneyCounter.Instance.amplifiers[2].Price, "");
            _levelCritClickDamagePrice.text = _notationMethod.NotationMethods(MoneyCounter.Instance.amplifiers[3].Price, "");
            _levelTimeCreatePrice.text = _notationMethod.NotationMethods(MoneyCounter.Instance.amplifiers[4].Price, "");
        }
}
