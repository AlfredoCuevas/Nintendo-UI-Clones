using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterGrid : MonoBehaviour
{
    [SerializeField]
    private List<Character> _characters = new List<Character>();

    [SerializeField]
    private GameObject _cellPrefab;

    private void Start()
    {
        foreach (var character in _characters)
        {
            CreateCharacterCell(character);
        }
    }

    private void CreateCharacterCell(Character character)
    {
        CharacterCellComponents cell = Instantiate(_cellPrefab, transform).GetComponent<CharacterCellComponents>();

        cell.characterName.text = character.characterName; 
        cell.artwork.sprite = character.characterSprite;
    }
}
