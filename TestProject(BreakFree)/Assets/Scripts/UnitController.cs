using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UnitController : MonoBehaviour,  IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private int NextUnit;

    public DamageAmplifier amplifier;
    public GameObject _unit;
    public int CurrentUnit;
    public float Damage;
    public float IdleDamage;
    public float CritChance;
    public float CritDamage;
    
    private GameObject _unitsFolder;
    private CanvasGroup _canvasGroup;
    private Canvas _gameMenuCanvas;
    private RectTransform _rectTransform;
    private Vector3 _beginPos;
    private Vector3 _normalScale;
    private Vector3 _clickedScale;
   
    private void Awake()
    {
        _normalScale = transform.localScale;
        _clickedScale = new Vector3(_normalScale.x - .05f, _normalScale.y - .05f, _normalScale.z);
    }

    private void Start()
    {
        StartParametres();
    }

    private void Update()
    {
        Damage = Mathf.Pow(2, CurrentUnit) + Mathf.Pow(2, CurrentUnit) * MoneyCounter.Instance.amplifiers[0].Value;
        IdleDamage = Mathf.Pow(2, CurrentUnit) + Mathf.Pow(2, CurrentUnit) * MoneyCounter.Instance.amplifiers[2].Value;
        CritDamage = Damage * (25 + MoneyCounter.Instance.amplifiers[3].Value);
        CritChance = 100 - MoneyCounter.Instance.amplifiers[1].Value; 
    }

    private void StartParametres()
    {
        
        StartCoroutine(IdleDamageDealer());
        CurrentUnit = NextUnit - 1;
        _unit = this.gameObject;
        _rectTransform = GetComponent<RectTransform>();
        _unitsFolder = GameManager.Instance._unitsFolder;
        _gameMenuCanvas = GetComponentInParent<Canvas>();
        _beginPos = _rectTransform.anchoredPosition;
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Click()
    {

        if (Random.Range(0, 101) > CritChance)
        {
            Damage = CritDamage;
            EffectController.Instance.CreateCritClickEffect();
        }
        else
        {
            EffectController.Instance.CreateClickEffect();
        }
        EffectController.Instance._damage = _unit.transform.gameObject.GetComponent<UnitController>().Damage;
        Effect.Instance.SetValue();
        DamageDealer();

    }

    private void DamageDealer()
    {
        MoneyCounter.Instance.CurrentMoney += Damage;
        MoneyCounter.Instance.UpdateUI();
    }

    private void DamageDealerIdle()
    {
        MoneyCounter.Instance.CurrentMoney += IdleDamage;
        MoneyCounter.Instance.UpdateUI();
    }

    private IEnumerator IdleDamageDealer()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            DamageDealerIdle();
        }
    }

    public void SetData(DamageAmplifier amplifier)
    {
        this.amplifier = amplifier;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _rectTransform.SetAsLastSibling();
        _canvasGroup.blocksRaycasts = false;
        
    }

    public void OnDrag(PointerEventData eventData)
    { 
        _rectTransform.anchoredPosition += eventData.delta/_gameMenuCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = _beginPos;
        _canvasGroup.blocksRaycasts = true;
       
    }

    public void OnDrop(PointerEventData eventData)
    {
        var otherUnitName = eventData.pointerDrag.name;
        if (_unit.name == otherUnitName)
        {
            Destroy(eventData.pointerDrag);
            Destroy(_unit);
            GameObject randomPos = Instantiate(GameManager.Instance._levelUnit[NextUnit], _rectTransform.anchoredPosition, Quaternion.identity) as GameObject;
            randomPos.transform.SetParent(_unitsFolder.transform, false);

        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = _clickedScale;
        EffectController.Instance.CurUnit = _unit;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
        transform.localScale = _normalScale;
        Click();
        
    }
}
