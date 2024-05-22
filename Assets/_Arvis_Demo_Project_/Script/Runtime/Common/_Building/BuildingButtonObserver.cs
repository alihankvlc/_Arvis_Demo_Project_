using _Arvis_Demo_Project_.Common._Managment;
using UnityEngine;
using Zenject;

namespace _Arvis_Demo_Project_.Common._Building
{
    public interface IBuildingButtonObserver
    {
        void OnBuildingButtonClicked(int dataId);
    }

    public sealed class BuildingButtonObserver : MonoBehaviour, IBuildingButtonObserver
    {
        [Inject] IBuildingButtonNotifier _buttonRegisterHandler;
        [Inject] IDraggableManager _placementManager;

        private void Start()
        {
            _buttonRegisterHandler?.RegisterObserver(this);
        }

        private void OnDestroy()
        {
            _buttonRegisterHandler?.UnregisterObserver(this);
        }

        public void OnBuildingButtonClicked(int dataId)
        {
            _placementManager?.OnBuildingCardClicked(dataId);
        }
    }
}
