using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Zenject;
using System.Collections;
using _Arvis_Demo_Project_.Common._Building;
using _Arvis_Demo_Project_.Common._Managment;

namespace _Arvis_Demo_Project_.Common._UI
{
    public sealed class UIBuilding : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<UIBuilding> { }

        [SerializeField] private Button _button;

        [SerializeField] private GameObject _gemContent;
        [SerializeField] private GameObject _goldContent;

        [SerializeField] private Image _buildingIcon;

        private int _buildingID;

        public int BuildingDataId => _buildingID;

        private TextMeshProUGUI _gemTextMesh;
        private TextMeshProUGUI _goldTextMesh;

        [Inject] IPlayerData _playerStat;

        private int _gemPrice;
        private int _goldPrice;

        private const float _checkPlayerStatDuration = 1f;

        private void Start()
        {
            StartCoroutine(CheckPlayerStat());
        }

        private IEnumerator CheckPlayerStat()
        {
            WaitForSeconds checkDelay = new(_checkPlayerStatDuration);
            while (true)
            {
                bool checkPlayerGold = _playerStat.Gold > 0 && (_playerStat.Gold >= _goldPrice) && _goldPrice > 0;
                bool checkPlayerGem = _playerStat.Gem > 0 && (_playerStat.Gem >= _gemPrice) && _gemPrice > 0;

                _gemTextMesh.color = checkPlayerGem ? Color.white : Color.red;
                _goldTextMesh.color = checkPlayerGold ? Color.white : Color.red;

                _button.interactable = checkPlayerGold | checkPlayerGem;

                yield return checkDelay;
            }
        }

        public void Bind(BuildingData buildingData)
        {
            _gemTextMesh = _gemContent.GetComponentInChildren<TextMeshProUGUI>();
            _goldTextMesh = _goldContent.GetComponentInChildren<TextMeshProUGUI>();

            if (_gemContent == null || _goldContent == null || _buildingIcon == null)
            {
                Debug.Log("UI components is null.");
                return;
            }

            _gemPrice = buildingData.Gem;
            _goldPrice = buildingData.Gold;

            _buildingIcon.sprite = buildingData.Icon;

            _gemContent.SetActive(buildingData.Gem > 0);
            _goldContent.SetActive(buildingData.Gold > 0);

            _gemTextMesh?.SetText(buildingData.Gem.ToString());
            _goldTextMesh?.SetText(buildingData.Gold.ToString());

            _buildingID = buildingData.Id;
        }
    }
}
