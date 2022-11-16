using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Effect : MonoBehaviour
{
    public static Effect Instance;

    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private CanvasGroup _group;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        
        _group.alpha = Mathf.Lerp(_group.alpha, 0, Time.deltaTime * 3);
        transform.position += Vector3.up * Time.deltaTime * 200; 
        if(_group.alpha < .01d)
            Destroy(base.gameObject);
    }

    public void SetPosition(Vector2 position)
    {
        
        transform.position = position;
    }

    public void SetValue()
    {
        _text.text = "+" + EffectController.Instance._notationMethod.NotationMethods(EffectController.Instance._damage, "");
    }
}
