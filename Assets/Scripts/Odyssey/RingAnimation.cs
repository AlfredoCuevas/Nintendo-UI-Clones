using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RingAnimation : MonoBehaviour
{
    [SerializeField]
    private float delay;

    [SerializeField]
    private float duration;

    private RectTransform _rectTransform;
    private Image _image;
    private Vector2 _size;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _image.DOFade(0,0);

        _size = _rectTransform.sizeDelta;
        _rectTransform.sizeDelta = _size / 4f;

        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(delay);
        Animate();
    }

    private void Animate()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_rectTransform.DOSizeDelta(_size, duration).SetEase(Ease.OutCirc));
        seq.Join(_image.DOFade(1, duration / 3));
        seq.Join(_image.DOFade(0, duration / 4).SetDelay(duration / 1.5f));
        seq.SetLoops(-1);
    }
}
