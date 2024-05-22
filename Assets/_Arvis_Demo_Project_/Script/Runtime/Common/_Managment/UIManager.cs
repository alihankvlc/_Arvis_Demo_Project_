using UnityEngine;
using Zenject;
using TMPro;
using _Arvis_Demo_Project_.Common._Database;
using _Arvis_Demo_Project_.Common._UI;
using _Arvis_Demo_Project_.Common._Building;

namespace _Arvis_Demo_Project_.Common._Managment
{
    public sealed class UIManager : MonoBehaviour
    {
        [Header("Building Menu Display Settings")]
        [SerializeField] private Transform _uiBuildingContentPlaceHolder;
        [Header("Player Stat Display Settings")]
        [SerializeField] private TextMeshProUGUI _gemTextMesh;
        [SerializeField] private TextMeshProUGUI _goldTextMesh;

        private BuildingDatabase _buildingDatabase;
        private UIBuilding.Factory _uiBuildingFactory;

        [Inject]
        public void Construct(BuildingDatabase buildingDatabase, UIBuilding.Factory uiBuildingFactory)
        {
            _buildingDatabase = buildingDatabase;
            _uiBuildingFactory = uiBuildingFactory;
        }

        private void Awake()
        {
            _buildingDatabase.Get_Data_List.ForEach(r => InitializeBuildingDisplay(r.Id));

            PlayerStatManager.OnChangePlayerGem += PlayerStatManager_OnChangePlayerGem;
            PlayerStatManager.OnChangePlayerGold += PlayerStatManager_OnChangePlayerGold;
        }

        private void PlayerStatManager_OnChangePlayerGold(int value)
        {
            _goldTextMesh.SetText(value.ToString());
        }

        private void PlayerStatManager_OnChangePlayerGem(int value)
        {
            _gemTextMesh.SetText(value.ToString());
        }

        public void InitializeBuildingDisplay(int buildingId)
        {
            UIBuilding uiBuilding = _uiBuildingFactory.Create();
            uiBuilding.transform.SetParent(_uiBuildingContentPlaceHolder, false);

            BuildingData data = _buildingDatabase.GetData(buildingId);

            if (data != null)
            {
                uiBuilding.Bind(data);
                return;
            }

            Debug.Log($"Not found {buildingId} in database");
        }

        private void OnDestroy()
        {
            PlayerStatManager.OnChangePlayerGem -= PlayerStatManager_OnChangePlayerGem;
            PlayerStatManager.OnChangePlayerGold -= PlayerStatManager_OnChangePlayerGold;
        }
    }
}
