using _Arvis_Demo_Project_.Common._Building;
using _Arvis_Demo_Project_.Common._Database;
using UnityEditor;
using UnityEngine;

namespace _Arvis_Demo_Project_.Editor
{
    public class BuildingMenuItem : EditorWindow
    {
        private string _buildingName;
        private string _buildingDescription;
        private int _buildingId;

        private int _buildingGemPrice;
        private int _buildingGoldPrice;

        private int _buildingGemProductionRate;
        private int _buildingGoldProductionRate;

        private int _buildingGemProductionSpeed;
        private int _buildingGoldProductionSpeed;

        private Sprite _buildingIcon;

        private string _folderPath = "Assets/_Arvis_Demo_Project_/ScriptableObject/BuildingData";

        private Vector2Int[] _buildingShape = new Vector2Int[0];
        private ShapeType _shapeType;

        private bool _isAddingToDatabase;
        private Vector2 _scrollPosition;


        [MenuItem("Tools/Building Menu Item Creator")]
        public static void ShowWindow()
        {
            GetWindow<BuildingMenuItem>();
        }

        private void OnGUI()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            GUILayout.Label("Create BuildingData", EditorStyles.boldLabel);

            GUILayout.Space(5);
            EditorGUILayout.LabelField("Data Settings", EditorStyles.boldLabel);
            _isAddingToDatabase = EditorGUILayout.Toggle("Add to Database", _isAddingToDatabase);

            _buildingName = EditorGUILayout.TextField("Name", _buildingName);
            _buildingDescription = EditorGUILayout.TextField("Description", _buildingDescription);
            _buildingId = EditorGUILayout.IntField("ID", _buildingId);

            GUILayout.Space(5);
            EditorGUILayout.LabelField("Price Settings", EditorStyles.boldLabel);

            _buildingGemPrice = EditorGUILayout.IntSlider("Gem Price", _buildingGemPrice, 0, 999);
            _buildingGoldPrice = EditorGUILayout.IntSlider("Gold Price", _buildingGoldPrice, 0, 999);

            GUILayout.Space(5);
            EditorGUILayout.LabelField("Production Settings", EditorStyles.boldLabel);

            _buildingGemProductionRate = EditorGUILayout.IntField("Gem Production Rate", _buildingGemProductionRate);
            _buildingGemProductionSpeed = EditorGUILayout.IntSlider("Gem Production Speed", _buildingGemProductionSpeed, 0, 30);
            _buildingGoldProductionRate = EditorGUILayout.IntField("Gold Production Rate", _buildingGoldProductionRate);
            _buildingGoldProductionSpeed = EditorGUILayout.IntSlider("Gold Production Speed", _buildingGoldProductionSpeed, 0, 30);

            GUILayout.Space(5);
            EditorGUILayout.LabelField("Display Settings", EditorStyles.boldLabel);

            _buildingIcon = EditorGUILayout.ObjectField("Icon", _buildingIcon, typeof(Sprite), false) as Sprite;

            _folderPath = EditorGUILayout.TextField("Folder Path", _folderPath);

            GUILayout.Space(5);
            EditorGUILayout.LabelField("Shape Settings", EditorStyles.boldLabel);

            _shapeType = (ShapeType)EditorGUILayout.EnumPopup("Shape Type", _shapeType);

            GUILayout.Space(5);

            if (GUILayout.Button("Create") && IsCreatable())
                CreateBuildingData();
            else if (GUILayout.Button("Reset Variables"))
                ResetAllVariables();

            EditorGUILayout.EndScrollView();
        }

        private void CreateBuildingData()
        {
            if (!AssetDatabase.IsValidFolder(_folderPath))
            {
                ThrowDebugMessage($"Folder path is not valid");
                return;
            }

            string path = _folderPath + "/" + _buildingName + ".asset";

            if (AssetDatabase.LoadAssetAtPath(path, typeof(BuildingData)))
                return;

            if (_isAddingToDatabase)
            {
                if (BuildingDatabase.Instance.ContainsItem(_buildingId))
                {
                    ThrowDebugMessage($"There is a data with the same id {_buildingId}");
                    return;
                }
            }

            BuildingData buildingData = CreateInstance<BuildingData>();

            buildingData.SetDataSettings(_buildingId, _buildingName, _buildingDescription);
            buildingData.SetPriceSettings(_buildingGemPrice, _buildingGoldPrice);
            buildingData.SetProductionSettings(_buildingGemProductionRate, _buildingGoldProductionRate,
                _buildingGemProductionSpeed, _buildingGoldProductionSpeed);

            buildingData.SetDisplaySettings(_buildingIcon);
            buildingData.SetShapePosition(_shapeType, _buildingShape);

            AssetDatabase.CreateAsset(buildingData, path);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = buildingData;


            if (_isAddingToDatabase && !BuildingDatabase.Instance.ContainsItem(_buildingId))
                BuildingDatabase.Instance.AddItem(buildingData);

            ResetAllVariables();
        }

        private void ResetAllVariables()
        {
            _buildingName = "";
            _buildingDescription = "";
            _buildingId = 0;

            _buildingGemPrice = 0;
            _buildingGoldPrice = 0;

            _buildingGemProductionRate = 0;
            _buildingGemProductionSpeed = 0;
            _buildingGoldProductionRate = 0;
            _buildingGoldProductionSpeed = 0;

            _shapeType = ShapeType.None;
            _buildingIcon = null;
        }

        private bool IsCreatable()
        {
            if (_buildingId <= 0)
            {
                ThrowDebugMessage($"Building ID must be greater than zero");
                return false;
            }

            if (string.IsNullOrEmpty(_buildingName) || _buildingName.Length < 1)
            {
                ThrowDebugMessage($"Building name is not be null and name lengt must be greater than zero");
                return false;
            }

            return true;
        }

        private void ThrowDebugMessage(string message)
        {
            string info = $"<color=cyan>{message}</color>";
            Debug.LogWarning(info);
        }
    }
}
