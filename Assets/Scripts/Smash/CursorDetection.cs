using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorDetection : MonoBehaviour
{
    [SerializeField]
    private CharacterGrid _characterGrid;

    private GraphicRaycaster _graphicRaycaster;
    private PointerEventData _pointerEventData = new PointerEventData(null);

    private Transform currentCharacter;

    void Start()
    {
        _graphicRaycaster = GetComponentInParent<GraphicRaycaster>();
        SetCurrentCharacter(null);
    }

    void Update()
    {
        _pointerEventData.position = Camera.main.WorldToScreenPoint(transform.position);
        List<RaycastResult> results = new List<RaycastResult>();
        _graphicRaycaster.Raycast(_pointerEventData, results);

        if(results.Count > 0)
        {
            for(int i = 0; i < results.Count; i++)
            {
                if (results[i].gameObject.tag == "Character Cell" && results[i].gameObject.transform != currentCharacter)
                {
                    SetCurrentCharacter(results[i].gameObject.transform);
                    break;
                }
            }
        }
        else if( currentCharacter != null)
        {
            SetCurrentCharacter(null);
        }
    }

    private void SetCurrentCharacter(Transform characterCell)
    {
        currentCharacter = characterCell;

        if (characterCell)
        {
            _characterGrid.ShowCharacterInSlot(1, currentCharacter.GetComponent<CharacterCellComponents>().myCharacter);
        }
        else
        {
            _characterGrid.ShowCharacterInSlot(1, null);
        }
    }
}
