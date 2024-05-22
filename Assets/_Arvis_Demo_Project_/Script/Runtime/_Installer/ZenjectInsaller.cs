using Zenject;
using UnityEngine;
using _Arvis_Demo_Project_.Common._UI;
using _Arvis_Demo_Project_.Common._Building;
using _Arvis_Demo_Project_.Common._Managment;
using _Arvis_Demo_Project_Runtime._Factory;
using _Arvis_Demo_Project_.Common._Database;

namespace _Arvis_Demo_Project_Runtime._Installer
{
    public sealed class ZenjectInsaller : MonoInstaller
    {
        [SerializeField] private UIBuilding _uiBuildingPrefab;
        [SerializeField] private BuildingButtonNotifier _uIButtonRegisterProvider;
        [SerializeField] private PlacementManager _placementManager;
        [SerializeField] private PlayerStatManager _playerStatManager;

        public override void InstallBindings()
        {
            Container.Bind<BoardFactory>().AsSingle();

            Container.Bind<BuildingDatabase>().FromInstance(BuildingDatabase.Instance).AsSingle();

            Container.Bind<IBuildingButtonNotifier>().FromInstance(_uIButtonRegisterProvider).AsSingle();
            Container.Bind<IDraggableManager>().FromInstance(_placementManager).AsSingle();
            Container.Bind<IPlayerData>().FromInstance(_playerStatManager).AsSingle();
            Container.BindFactory<UIBuilding, UIBuilding.Factory>().FromComponentInNewPrefab(_uiBuildingPrefab);
        }
    }
}