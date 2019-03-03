using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KingdomSelect : MonoBehaviour
{
    [SerializeField]
    private List<Kingdom> _kingdoms = new List<Kingdom>();

    [SerializeField]
    private Transform _sphereModel;

    [SerializeField]
    private Transform _kingdomPivotPrefab;

    [SerializeField]
    private Transform _uiContainer;

    [SerializeField]
    private Transform _kingdomButtonPrefab;

    private Button _firstButton;

    //-------------------------------------

    private void Start()
    {
        foreach (Kingdom kingdom in _kingdoms)
        {
            SpawnKingdomPoint(kingdom);
        }
        
        // Select the first kindom
        LookAtKingdom(_kingdoms[0]);
        _firstButton.GetComponent<KingdomButton>().ManuallySelect();
    }

    public void LookAtKingdom(Kingdom kingdom)
    {
        Transform mainCamera = Camera.main.transform;
        Transform cameraPivot = mainCamera.parent.parent;

        cameraPivot.DOLocalRotate(new Vector3(kingdom.xAngle, kingdom.yAngle, 0), 1, RotateMode.Fast);
    }

    private void SpawnKingdomPoint(Kingdom kingdom)
    {
        Transform tempKingdom = Instantiate(_kingdomPivotPrefab, _sphereModel);
        tempKingdom.localEulerAngles = new Vector3(kingdom.xAngle, kingdom.yAngle, 0);
        tempKingdom.name = kingdom.name;

        SpawnKingdomButton(kingdom);
    }

    private void SpawnKingdomButton(Kingdom kingdom)
    {
        Button kingdomButton = Instantiate(_kingdomButtonPrefab, _uiContainer).GetComponent<Button>();
        kingdomButton.name = kingdom.name + " Button";
        kingdomButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = kingdom.name;
        kingdomButton.onClick.AddListener(() => LookAtKingdom(kingdom));

        if (!_firstButton) _firstButton = kingdomButton;
    }

    [Serializable]
    public class Kingdom
    {
        public string name;
        public float xAngle, yAngle;
    }
}
