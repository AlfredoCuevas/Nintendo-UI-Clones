using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class CursorDetection : MonoBehaviour
{
    [SerializeField]
    private CharacterGrid _characterGrid;

    [SerializeField]
    private Transform _token;

    [SerializeField]
    private Image _backButton;

    [SerializeField]
    private Vector3 _positionOffset;

    private GraphicRaycaster _graphicRaycaster;
    private PointerEventData _pointerEventData = new PointerEventData(null);

    private Transform _currentCharacter;

    [SerializeField]
    private bool _hasToken;
    private bool _hoveringBackButton = false;

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

        // Select the current character cell
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

        // Implementing cursor hovering on the back button
        for( int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.tag == "Back Button")
            {
                if (!_hoveringBackButton)
                {
                    _hoveringBackButton = true;
                    AudioManager.Instance.PlayOneShot("Cell Hovered");
                    _backButton.transform.DOPunchScale(Vector3.one * .1f, .1f, 0, 0);
                }
                break;
            }
            else if( i == results.Count - 1)
            {
                _hoveringBackButton = false;
                _backButton.DOComplete();
                _backButton.color = Color.white;
            }
        }

        // Read User Input for putting down the token
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_hoveringBackButton)
            {
                AudioManager.Instance.PlayOneShot("Character Selected");
                _backButton.DOColor(Color.red, 0.1f).OnComplete(() => LeaveScene());
                return;
            }
            else if(_currentCharacter != null && _hasToken)
            {
                _hasToken = false;
                _token.DOPunchScale( Vector3.one * 0.2f, .2f, 0, 0);
                AudioManager.Instance.PlayOneShot("Character Selected");
                AudioManager.Instance.Play(_currentCharacter.gameObject.name);
                _characterGrid.CharacterConfirm();
            }
            else if (!_hasToken)
            {
                // _hasToken = true after the tween
                Tweener tween =_token.DOMove(transform.position - _positionOffset, 0.05f).OnComplete(()=>_hasToken = true);
                AudioManager.Instance.PlayOneShot("Token Return");
            }
            else
            {
                AudioManager.Instance.PlayOneShot("Can Not Select");
            }
        }
    }

    private void LateUpdate()
    {
        UpdateTokenPosition();
    }

    private void SetCurrentCharacter(Transform nextCharacterCell)
    {
        // Turn on and off Border Animations for the character grid cells
        if (nextCharacterCell == null && _currentCharacter != null)
        {
            _currentCharacter.GetComponent<CharacterCellComponents>().border.DOKill();
            _currentCharacter.GetComponent<CharacterCellComponents>().border.color = Color.clear;
        }
        else if (nextCharacterCell != null && _currentCharacter != null)
        {
            _currentCharacter.GetComponent<CharacterCellComponents>().border.DOKill();
            _currentCharacter.GetComponent<CharacterCellComponents>().border.color = Color.clear;

            nextCharacterCell.GetComponent<CharacterCellComponents>().border.color = Color.white;
            nextCharacterCell.GetComponent<CharacterCellComponents>().border.DOColor(Color.red, 1).SetLoops(-1);
        }
        else if(nextCharacterCell != null && _currentCharacter == null)
        {
            nextCharacterCell.GetComponent<CharacterCellComponents>().border.color = Color.white;
            nextCharacterCell.GetComponent<CharacterCellComponents>().border.DOColor(Color.red, 1).SetLoops(-1);
        }

        _currentCharacter = nextCharacterCell;

        if (nextCharacterCell)
        {
            _characterGrid.ShowCharacterInSlot(1, _currentCharacter.GetComponent<CharacterCellComponents>().myCharacter);
            AudioManager.Instance.PlayOneShot("Cell Hovered");
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

    private void LeaveScene()
    {
        AudioManager.Instance.FadeOutSound("Smash Background Music", .5f);
        SceneManager.LoadScene(0);
    }
}
