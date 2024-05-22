using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace _Arvis_Demo_Project_.Common._Building
{
    public interface IBuildingButtonNotifier
    {
        void RegisterObserver(IBuildingButtonObserver observer);
        void UnregisterObserver(IBuildingButtonObserver observer);
        void NotifyObservers(int buildingDataId);
    }

    public sealed class BuildingButtonNotifier : SerializedMonoBehaviour, IBuildingButtonNotifier
    {
        [SerializeField] private readonly List<IBuildingButtonObserver> _observers = new List<IBuildingButtonObserver>();

        public void RegisterObserver(IBuildingButtonObserver observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }

        public void UnregisterObserver(IBuildingButtonObserver observer)
        {
            if (_observers.Contains(observer))
                _observers.Remove(observer);

        }

        public void NotifyObservers(int buildingDataId)
        {
            _observers.ForEach(r => r.OnBuildingButtonClicked(buildingDataId));
        }

        public void OnButtonClicked(int buildingDataId)
        {
            NotifyObservers(buildingDataId);
        }
    }
}
