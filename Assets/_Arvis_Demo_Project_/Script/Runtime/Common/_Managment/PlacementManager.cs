using _Arvis_Demo_Project_.Common._Building;
using _Arvis_Demo_Project_.Common._Database;
using _Arvis_Demo_Project_.Common._Other;
using _Arvis_Demo_Project_Runtime.Common;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace _Arvis_Demo_Project_.Common._Managment
{
    public interface IDraggableManager
    {
        void OnBuildingCardClicked(int dataId);
    }

    [System.Serializable]
    public class BoardMappingData
    {
        public int DataId;
        public Vector2Int Position;
        public List<Vector2Int> OccupiedPositions = new();
    }

    public sealed class PlacementManager : MonoBehaviour, IDraggableManager
    {
        [SerializeField] private GameObject _spawnBuildingPrefab;
        [SerializeField] private SpriteRenderer _tempDraggableObject;

        [Inject] private BuildingDatabase _buildingDatabase;
        [Inject] private IPlayerData _playerStat;

        [System.Serializable]
        public struct BuildingPlacementData
        {
            public BoardMappingData[] BuildingPlacementDatas;
        }

        [SerializeField] private List<BoardMappingData> _buildingBoardMapDatas = new();

        private Camera _mainCamera;

        [SerializeField] private PlacementPreview _placementView;

        [SerializeField] private BuildingPlacementData _buildingPlacementDatas;

        private bool _isPlacing;
        private int _tempBuildingDataId;

        private void Start()
        {
            _mainCamera = Camera.main;

            _buildingPlacementDatas = SaveManager.Instance.Load<BuildingPlacementData>("placementData");
            if (_buildingPlacementDatas.BuildingPlacementDatas != null)
            {
                _buildingBoardMapDatas = _buildingPlacementDatas.BuildingPlacementDatas.ToList();

                foreach (var boardData in _buildingBoardMapDatas)
                {
                    IDropable dropableComponent = GameInitializer.Instance.BuildingPlacementAreas.FirstOrDefault(r => r.Position == boardData.Position);
                    InitializeBuildingData(boardData.DataId, dropableComponent, boardData.OccupiedPositions);
                }
            }
        }

        private void OnApplicationQuit()
        {
            _buildingPlacementDatas.BuildingPlacementDatas = _buildingBoardMapDatas.ToArray();
            SaveManager.Instance.Save(_buildingPlacementDatas, "placementData");
        }

        private void Update()
        {
            if (_isPlacing)
                OnDrag();
        }

        public void OnBuildingCardClicked(int dataId)
        {
            if (!_isPlacing)
            {
                _tempBuildingDataId = dataId;
                OnDragStart(dataId);
            }
        }

        private void OnDragStart(int dataId)
        {
            BuildingData data = _buildingDatabase.GetData(dataId);
            _placementView.ShowPreview(data.ShapeType);

            _isPlacing = true;

            _tempDraggableObject.sprite = data.Icon;


            _tempDraggableObject.gameObject.SetActive(true);
        }

        private void OnDrag()
        {
            if (_tempDraggableObject.gameObject.activeSelf)
            {
                Vector3 mousePosition = Input.mousePosition;
                mousePosition.z = _mainCamera.nearClipPlane;

                Ray ray = _mainCamera.ScreenPointToRay(mousePosition);
                RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, ray.direction);

                if (hitInfo.collider != null && hitInfo.collider.TryGetComponent(out IDropable dropable))
                {
                    BuildingData data = _buildingDatabase.GetData(_tempBuildingDataId);
                    Vector2Int[] cells = data.Cells;

                    Vector3 position = new Vector3(dropable.Position.x, dropable.Position.y, 0);
                    _tempDraggableObject.transform.position = position;

                    bool canPlace = cells.All(cell =>
                    {
                        Vector2Int offset = new Vector2Int(dropable.Position.x + cell.x, dropable.Position.y + cell.y);
                        IDropable dropableComponent = GameInitializer.Instance.BuildingPlacementAreas.FirstOrDefault(r => r.Position == offset);
                        return dropableComponent != null && !dropableComponent.IsOccupied;
                    });

                    bool checkPlayerGold = _playerStat.Gold > 0 && (_playerStat.Gold >= data.Gold) && data.Gold > 0;
                    bool checkPlayerGem = _playerStat.Gem > 0 && (_playerStat.Gem >= data.Gem) && data.Gem > 0;

                    _placementView.ChangeColor(canPlace && checkPlayerGold | checkPlayerGem ? Color.yellow : Color.red);

                    if (Input.GetMouseButtonDown(0))
                    {
                        if (canPlace && checkPlayerGold | checkPlayerGem)
                        {
                            List<Vector2Int> occupiedPositions = new();

                            foreach (Vector2Int cell in cells)
                            {
                                Vector2Int offset = new Vector2Int(dropable.Position.x + cell.x, dropable.Position.y + cell.y);
                                IDropable dropableComponent = GameInitializer.Instance.BuildingPlacementAreas.FirstOrDefault(r => r.Position == offset);
                                dropableComponent?.SetOccupied(true);
                                occupiedPositions.Add(dropableComponent.Position);
                            }

                            _placementView.ChangeColor(Color.white);
                            dropable.OnDrop(_tempBuildingDataId, () => OnPlace(data, dropable.Position, occupiedPositions, checkPlayerGold));
                        }
                        else OnDragReset();
                    }
                }
                else _tempDraggableObject.transform.position = _mainCamera.ScreenToWorldPoint(mousePosition);

                if (Input.GetMouseButtonDown(1))
                    OnDragReset();
            }
        }

        private void OnPlace(BuildingData data, Vector2Int position, List<Vector2Int> occupiedPositions, bool checkPlayerGold)
        {
            GameObject building = Instantiate(_spawnBuildingPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);

            BuildingProvider buildingProvider = building.GetComponent<BuildingProvider>();
            buildingProvider.enabled = true;

            buildingProvider.Init(data, occupiedPositions);

            building.AddComponent<ZenAutoInjecter>().ContainerSource = ZenAutoInjecter.ContainerSources.SceneContext;

            building.transform.name = data.Name;

            BoardMappingData boardData = new BoardMappingData
            {
                DataId = data.Id,
                Position = position,
                OccupiedPositions = occupiedPositions,
            };

            _buildingBoardMapDatas.Add(boardData);

            building.SetActive(true);

            OnDragReset();

            if (checkPlayerGold)
            {
                _playerStat.UpdateStat(StatType.Gold, -data.Gold);
                return;
            }


            _playerStat.UpdateStat(StatType.Gem, -data.Gem);

        }

        private void InitializeBuildingData(int dataId, IDropable dropable, List<Vector2Int> occupiedPositions)
        {
            BuildingData data = _buildingDatabase.GetData(dataId);
            GameObject building = Instantiate(_spawnBuildingPrefab, new Vector3(dropable.Position.x, dropable.Position.y, 0), Quaternion.identity);

            SpriteRenderer renderer = building.GetComponent<SpriteRenderer>();
            renderer.sprite = data.Icon;

            PlacementPreview placementPreview = building.GetComponent<PlacementPreview>();
            placementPreview.ShowPreview(data.ShapeType);

            BuildingProvider buildingProvider = building.GetComponent<BuildingProvider>();
            buildingProvider.enabled = true;

            buildingProvider.Init(data, occupiedPositions);

            occupiedPositions.ForEach(position =>
            {
                IDropable dropableComponent = GameInitializer.Instance.BuildingPlacementAreas.FirstOrDefault(r => r.Position == position);
                dropableComponent.SetOccupied(true);
            });

            building.AddComponent<ZenAutoInjecter>().ContainerSource = ZenAutoInjecter.ContainerSources.SceneContext;
            building.transform.name = data.Name;

            dropable.SetOccupied(true);
            building.SetActive(true);
        }

        private void OnDragReset()
        {
            _tempBuildingDataId = 0;
            _tempDraggableObject.gameObject.SetActive(false);
            _isPlacing = false;
        }
    }
}
