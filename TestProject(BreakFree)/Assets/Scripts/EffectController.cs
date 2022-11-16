using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public static EffectController Instance;

    [SerializeField] private Effect _effectClickPref;
    [SerializeField] private Effect _effectCritClickPref;
    [SerializeField] private Camera _camera;
    [SerializeField] private Canvas _gameMenuCanvas;

    public GameObject CurUnit;
    public NotationMethod _notationMethod;

    public float _damage;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        _notationMethod.NotationMethods(_damage, "");
    }

    public void CreateClickEffect()
    {
        var pref = Instantiate(_effectClickPref, CurUnit.transform);
        pref.transform.SetParent(_gameMenuCanvas.transform, false);
        pref.SetPosition(Input.mousePosition);
        pref.SetValue();
    }

    public void CreateCritClickEffect()
    {
        var pref = Instantiate(_effectCritClickPref, CurUnit.transform);
        pref.transform.SetParent(_gameMenuCanvas.transform, false);
        pref.SetPosition(Input.mousePosition);
        pref.SetValue();
    }
}
