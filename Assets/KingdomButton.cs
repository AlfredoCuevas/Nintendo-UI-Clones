using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class KingdomButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;

    [SerializeField]
    private Image _rectangle;

    [SerializeField]
    private Color _rectHoverColor;

    [SerializeField]
    private Image _circle;

    public void Start()
    {
        _rectangle.color = Color.clear;
        _text.color = Color.white;
        _circle.color = Color.white;
    }

    public void ManuallySelect()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void Selected()
    {
        _rectangle.DOColor(Color.white, .5f);
        _text.DOColor(Color.black, .5f);
        _circle.DOColor(Color.red, .5f);

        _rectangle.transform.DOComplete();
        _rectangle.transform.DOPunchScale(Vector3.one * .3f, .2f, 20, 1);
    }

    public void Deselected()
    {
        _rectangle.DOColor(Color.clear, .5f);
        _text.DOColor(Color.white, .5f);
        _circle.DOColor(Color.white, .5f);
    }

    public void MouseHovering()
    {
        if(EventSystem.current.currentSelectedGameObject != gameObject)
        {
            _rectangle.DOColor(_rectHoverColor, .5f);
        }
    }

    public void MouseNotHovering()
    {
        if(EventSystem.current.currentSelectedGameObject != gameObject)
        {
            _rectangle.DOColor(Color.clear, .5f);
            _text.DOColor(Color.white, .5f);
            _circle.DOColor(Color.white, .5f);
        }
    }

}
