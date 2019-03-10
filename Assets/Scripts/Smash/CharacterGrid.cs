using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterGrid : MonoBehaviour
{
    [SerializeField]
    private List<Character> _characters = new List<Character>();

    [SerializeField]
    private GameObject _cellPrefab;

    [SerializeField]
    private Transform _playerSlot;

    private void Start()
    {
        foreach (var character in _characters)
        {
            CreateCharacterCell(character);
        }
    }

    public void ShowCharacterInSlot(int player, Character character)
    {
        // Get character info
        Sprite artworkSprite =  character ? character.characterSprite : null;
        string charName =       character ? character.characterName : string.Empty;
        string playerNickname = character ? "Player " + player.ToString() : string.Empty;
        string playerNumber =   character ? "P" + player.ToString() : string.Empty;

        // Assign character info into the player slot
        PlayerSlotComponents slot = _playerSlot.GetComponent<PlayerSlotComponents>();
        slot.characterImage.sprite = artworkSprite;
        slot.characterName.text = charName;
        slot.playerNickname.text = playerNickname;
        slot.playerNumber.text = playerNumber;
    }

    public void CharacterConfirm()
    {
        _playerSlot.DOPunchPosition(Vector3.down * 5, 0.5f, 4, 0);
    }

    private void CreateCharacterCell(Character character)
    {
        CharacterCellComponents cell = Instantiate(_cellPrefab, transform).GetComponent<CharacterCellComponents>();
        cell.gameObject.name = character.characterName;
        cell.myCharacter = character;

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
