using _Arvis_Demo_Project_.Common._UI;
using UnityEngine;
using UnityEngine.UI;

namespace _Arvis_Demo_Project_.Common._Building
{
    public sealed class BuildingButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private BuildingButtonObserver _provider;
        private UIBuilding _uiBuilding;

        private void Start()
        {
            _provider = GetComponent<BuildingButtonObserver>();
            _uiBuilding = GetComponent<UIBuilding>();

            _button.onClick.AddListener(OnButtonClicked);
        }

        public void OnButtonClicked()
        {
            _provider?.OnBuildingButtonClicked(_uiBuilding.BuildingDataId);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

    }
}
