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

    [SerializeField]
    private Transform _player2Slot;

    [SerializeField]
    private Sprite _transparentSquare;

    private void Start()
    {
        foreach (var character in _characters)
        {
            CreateCharacterCell(character);
        }

        int RandomIndex = Random.Range(0, _characters.Count);
        RandomPlayer2Character(_characters[RandomIndex]);
    }

    public void ShowCharacterInSlot(int player, Character character)
    {
        // Get character info
        Sprite artworkSprite =  character ? character.characterSprite : _transparentSquare;
        Sprite gameIconSprite = character ? character.characterGameIcon : _transparentSquare;
        string charName =       character ? character.characterName : string.Empty;
        string playerNickname = character ? "Player " + player.ToString() : "Player " + player.ToString();
        string playerNumber =   character ? "P" + player.ToString() : "P" + player.ToString();

        // Assign character info into the player slot
        PlayerSlotComponents slot = _playerSlot.GetComponent<PlayerSlotComponents>();
        
        slot.characterIcon.sprite = gameIconSprite;
        slot.characterName.text = charName;
        slot.playerNickname.text = playerNickname;
        slot.playerNumber.text = playerNumber;

        Sequence s = DOTween.Sequence();
        float centerX = -235f;
        s.Append(slot.characterImage.transform.DOLocalMoveX(centerX-300, .1f));
        s.AppendCallback(() => slot.characterImage.sprite = artworkSprite);
        s.Append(slot.characterImage.transform.DOLocalMoveX(centerX+300, 0));
        s.Append(slot.characterImage.transform.DOLocalMoveX(centerX, 0.2f));
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

    private void RandomPlayer2Character(Character character)
    {
        // Get character info
        Sprite artworkSprite = character ? character.characterSprite : _transparentSquare;
        Sprite gameIconSprite = character ? character.characterGameIcon : _transparentSquare;
        string charName = character ? character.characterName : string.Empty;

        // Assign character info into the player slot
        PlayerSlotComponents slot = _player2Slot.GetComponent<PlayerSlotComponents>();
        slot.characterImage.sprite = artworkSprite;
        slot.characterIcon.sprite = gameIconSprite;
        slot.characterName.text = charName;
    }
}
