using _Arvis_Demo_Project_.Common._Database;
using _Arvis_Demo_Project_.Common._Framework;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace _Arvis_Demo_Project_.Common._Building
{
    [CreateAssetMenu(fileName = "New_Building", menuName = "Arvis_Project/Create Handle Building Data")]
    public sealed class BuildingData : DeletableScriptableObject, IData
    {
        [Header("Data Settings")]
        [SerializeField] private int _buildingId;
        [SerializeField] private string _buildingName;
        [Multiline][SerializeField] private string _buildingDescription;

        [Space, Header("Price Settings")]
        [Range(0, 999)][SerializeField] private int _buildingGemPrice;
        [Range(0, 999)][SerializeField] private int _buildingGoldPrice;

        [Space, Header("Production Settings")]
        [SerializeField] private int _gemProductionRate;
        [Range(0, 30)][SerializeField] private int _gemProductionSpeed;
        [SerializeField] private int _goldProductionRate;
        [Range(0f, 30f)][SerializeField] private int _goldProductionSpeed;

        [Space, Header("Display Settings")]
        [SerializeField] private Sprite _buildingIcon;

        [Space, Header("Building Shape Settings")]
        [SerializeField] private ShapeType _shapeType;
        [SerializeField, ReadOnly] private Vector2Int[] _shapePosition;
        public int Id
        {
            get => _buildingId;
            private set => _buildingId = value;
        }

        public string Name
        {
            get => _buildingName;
            private set => _buildingName = value;
        }
        public string Description
        {
            get => _buildingDescription;
            private set => _buildingDescription = value;
        }
        public int Gem
        {
            get => _buildingGemPrice;
            set => _buildingGemPrice = value;
        }

        public int Gold
        {
            get => _buildingGoldPrice;
            set => _buildingGoldPrice = value;
        }

        public int GemProductionRate
        {
            get => _gemProductionRate;
            private set => _gemProductionRate = value;
        }
        public int GoldProductionRate
        {
            get => _goldProductionRate;
            private set => _goldProductionRate = value;
        }

        public int GemProductionSpeed
        {
            get => _gemProductionSpeed;
            private set => _gemProductionSpeed = value;
        }

        public int GoldProductionSpeed
        {
            get => _goldProductionSpeed;
            private set => _goldProductionSpeed = value;
        }

        public Sprite Icon
        {
            get => _buildingIcon;
            private set => _buildingIcon = value;
        }
        public ShapeType ShapeType
        {
            get => _shapeType;
            private set
            {
                _shapeType = value;
            }
        }
        public Vector2Int[] Cells => _shapePosition;

#if UNITY_EDITOR
        [SerializeField, Multiline] private string _debuggingNote;

        protected override void OnDestroy()
        {
            BuildingDatabase.Instance.RemoveItem(_buildingId);
        }

        [Button("Destory", ButtonStyle.FoldoutButton)]
        private void DestroyInstance()
        {
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(this));
        }

        [Button("Add To Database"), ShowIf("@!BuildingDatabase.Instance.ContainsItem(_buildingId)")]
        private void AddToDatabase()
        {
            BuildingDatabase.Instance.AddItem(this);
        }
        [Button("Update Shape Settings")]
        private void UpdateShapeSettings()
        {
            if (BuildingShapeData.Cells.ContainsKey(_shapeType))
            {
                _shapePosition = BuildingShapeData.Cells[_shapeType];
            }
        }

#endif
        public void SetDataSettings(int id, string name = "null", string description = "null")
        {
            _buildingId = id;
            _buildingName = name;
            _buildingDescription = description;
        }

        public void SetPriceSettings(int gemPrice, int goldPrice)
        {
            _buildingGemPrice = gemPrice;
            _buildingGoldPrice = goldPrice;
        }

        public void SetProductionSettings(int gemProductionRate, int goldProductionRate, int gemProductionSpeed, int goldProductionSpeed)
        {
            _gemProductionRate = gemProductionRate;
            _goldProductionRate = goldProductionRate;

            _gemProductionSpeed = gemProductionSpeed;
            _goldProductionSpeed = goldProductionSpeed;
        }

        public void SetDisplaySettings(Sprite icon)
        {
            _buildingIcon = icon;
        }

        public void SetShapePosition(ShapeType shapeType, Vector2Int[] vector2Ints)
        {
            if (BuildingShapeData.Cells.ContainsKey(_shapeType))
            {
                _shapeType = shapeType;
                _shapePosition = BuildingShapeData.Cells[_shapeType];
            }
        }
    }
}