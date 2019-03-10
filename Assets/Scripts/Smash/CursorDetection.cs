using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class CursorDetection : MonoBehaviour
{
    [SerializeField]
    private CharacterGrid _characterGrid;

    [SerializeField]
    private Transform _token;

    [SerializeField]
    private Vector3 _positionOffset;

    private GraphicRaycaster _graphicRaycaster;
    private PointerEventData _pointerEventData = new PointerEventData(null);

    private Transform _currentCharacter;

    [SerializeField]
    private bool _hasToken;
    

    void Start()
    {
        _graphicRaycaster = GetComponentInParent<GraphicRaycaster>();

        _hasToken = true;
        SetCurrentCharacter(null);
    }

    void Update()
    {
        _pointerEventData.position = Camera.main.WorldToScreenPoint(transform.position);
        List<RaycastResult> results = new List<RaycastResult>();
        _graphicRaycaster.Raycast(_pointerEventData, results);

        if (_hasToken)
        {
            if (results.Count > 0)
            {
                for (int i = 0; i < results.Count; i++)
                {
                    if (results[i].gameObject.tag == "Character Cell")
                    {
                        if(results[i].gameObject.transform != _currentCharacter) SetCurrentCharacter(results[i].gameObject.transform);
                        break;
                    }
                    else if (i == results.Count-1)
                    {
                        SetCurrentCharacter(null);
                    }
                }
            }
            else if (_currentCharacter != null)
            {
                SetCurrentCharacter(null);
            }
        }

        // Read User Input for putting down the token
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(_currentCharacter != null && _hasToken)
            {
                _hasToken = false;
                _token.DOPunchScale( Vector3.one * 0.2f, .2f, 0, 0);
                _characterGrid.CharacterConfirm();
            }
            else if (!_hasToken)
            {
                // _hasToken = true after the tween
                Tweener tween =_token.DOMove(transform.position - _positionOffset, 0.05f).OnComplete(()=>_hasToken = true);
            }
        }
    }

    private void LateUpdate()
    {
        UpdateTokenPosition();
    }

    private void SetCurrentCharacter(Transform characterCell)
    {
        _currentCharacter = characterCell;

        if (characterCell)
        {
            _characterGrid.ShowCharacterInSlot(1, _currentCharacter.GetComponent<CharacterCellComponents>().myCharacter);
        }
        else
        {
            _characterGrid.ShowCharacterInSlot(1, null);
        }
    }

    private void UpdateTokenPosition()
    {
        if (_hasToken)
        {
            _token.transform.position = transform.position - _positionOffset;
        }
    }
}
