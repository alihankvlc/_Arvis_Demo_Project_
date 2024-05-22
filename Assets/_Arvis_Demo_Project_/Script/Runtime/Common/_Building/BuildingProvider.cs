using Sirenix.OdinInspector;
using Zenject;
using _Arvis_Demo_Project_.Common._Managment;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

namespace _Arvis_Demo_Project_.Common._Building
{

    public sealed class BuildingProvider : MonoBehaviour
    {
        [SerializeField, InlineEditor, ReadOnly] private BuildingData _data;
        [SerializeField, ReadOnly] private List<Vector2Int> _occupiedPositions;

        [SerializeField] private TextMeshProUGUI _productionTextMesh;

        [SerializeField] private Slider _gemSlider;
        [SerializeField] private Slider _goldSlider;

        [SerializeField] private Canvas _canvas;
        [SerializeField] private Animator _animator;

        [Inject] private IPlayerData _playerData;

        public BuildingData Data => _data;

        private float _gemProductionDuration;
        private float _goldProductionDuration;

        private float _productionGemTimer;
        private float _productionGoldTimer;

        private const float ProductSliderThreshold = 0.99f;
        private const int SliderDivisor = 10;
        private readonly int ProductAnimHashId = Animator.StringToHash("Product");

        private void Start()
        {
            InitializeUI();
        }

        public void Init(BuildingData data, List<Vector2Int> occupiedPosition)
        {
            _occupiedPositions = occupiedPosition;
            _data = data;

            _gemProductionDuration = _data.GemProductionSpeed;
            _goldProductionDuration = _data.GoldProductionSpeed;
        }

        private void Update()
        {
            UpdateProductionTimers();
            UpdateProductionSliders();

            if (_data.GemProductionRate > 0 && _productionGemTimer >= _gemProductionDuration)
                ProduceResource(StatType.Gem, _data.GemProductionRate, ref _productionGemTimer);

            if (_data.GoldProductionRate > 0 && _productionGoldTimer >= _goldProductionDuration)
                ProduceResource(StatType.Gold, _data.GoldProductionRate, ref _productionGoldTimer);
        }

        private void InitializeUI()
        {
            _canvas.gameObject.SetActive(true);
            _gemSlider.gameObject.SetActive(_data.GemProductionRate > 0);
            _goldSlider.gameObject.SetActive(_data.GoldProductionRate > 0);
        }

        private void UpdateProductionTimers()
        {
            _productionGemTimer += Time.deltaTime;
            _productionGoldTimer += Time.deltaTime;
        }

        private void UpdateProductionSliders()
        {
            _gemSlider.value = _productionGemTimer / SliderDivisor;
            _goldSlider.value = _productionGoldTimer / SliderDivisor;

            if (_gemSlider.value >= ProductSliderThreshold)
                TriggerProductionAnimation(_data.GemProductionRate);

            if (_goldSlider.value >= ProductSliderThreshold)
                TriggerProductionAnimation(_data.GoldProductionRate);
        }

        private void TriggerProductionAnimation(int productionRate)
        {
            _productionTextMesh.SetText($"+{productionRate}");
            _animator.SetTrigger(ProductAnimHashId);
        }

        private void ProduceResource(StatType statType, int productionRate, ref float timer)
        {
            _playerData.UpdateStat(statType, productionRate);
            timer = 0;
        }
    }
}
