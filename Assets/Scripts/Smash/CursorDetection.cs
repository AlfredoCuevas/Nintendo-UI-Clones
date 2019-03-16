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

        // Currently working here trying to get back button color to function as expected
        // try debugging when all colors are applied. 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(LeaveScene());
            return;
        }
        else
        {
            // check if back button is being hovered
            for (int i = 0; i < results.Count; i++)
            {
                if (results[i].gameObject.tag == "Back Button")
                {
                    _backButton.color = Color.red;
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        Debug.Log("F Pressed Once");
                        StartCoroutine(LeaveScene());
                        return;
                    }
                    break;
                }
                _backButton.color = Color.white;
            }
        } 

        // Read User Input for putting down the token
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(_currentCharacter != null && _hasToken)
            {
                _hasToken = false;
                _token.DOPunchScale( Vector3.one * 0.2f, .2f, 0, 0);
                AudioManager.Instance.PlayOneShot("Character Selected");
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

    private IEnumerator LeaveScene()
    {
        _backButton.DOComplete();
        _backButton.color = Color.red;
        _backButton.DOColor(Color.gray, 1f);
        
        float timer = 0f;
        while (Input.GetKey(KeyCode.F) || Input.GetKey(KeyCode.Escape))
        {
            timer += Time.deltaTime;
            if(timer > 1f)
            {
                AudioManager.Instance.FadeOutSound("Smash Background Music", 0.5f);
                _backButton.transform.DOPunchScale(Vector3.one, 0.01f, 0, 0);
                SceneManager.LoadScene(0);
                break;
            }
            yield return null;
        }
        _backButton.DOComplete();
        _backButton.color = Color.white;
    }
}
