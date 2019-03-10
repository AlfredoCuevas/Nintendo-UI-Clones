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

        TextMeshProUGUI name = cell.characterName;
        Image artwork = cell.artwork;

        name.text = character.characterName; 
        artwork.sprite = character.characterSprite;

        Vector2 pixelSize = new Vector2(artwork.sprite.texture.width, artwork.sprite.texture.height);
        Vector2 pixelPivot = artwork.sprite.pivot;
        Vector2 uiPivot = new Vector2(pixelPivot.x / pixelSize.x, pixelPivot.y / pixelSize.y);

        artwork.GetComponent<RectTransform>().pivot = uiPivot;
        artwork.GetComponent<RectTransform>().sizeDelta *= character.zoom;
    }
}
