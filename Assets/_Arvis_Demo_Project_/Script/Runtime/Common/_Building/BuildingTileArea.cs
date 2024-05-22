using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace _Arvis_Demo_Project_.Common._Building
{
    public interface IDropable
    {
        public Transform Transform { get; }
        public Vector2Int Position { get; }
        public bool IsOccupied { get; }
        public void OnDrop(int droppedId, Action action);
        public void SetOccupied(bool occupied);
    }

    public sealed class BuildingTileArea : MonoBehaviour, IDropable
    {
        [SerializeField, ReadOnly] private Vector2Int _position;
        [SerializeField, ReadOnly] private bool _isOccupied;
        [SerializeField, ReadOnly] private int _droppedId;

        public Vector2Int Position => _position;
        public bool IsOccupied => _isOccupied;

        public Transform Transform => transform;

        public void Init(Vector2Int position)
        {
            _position = position;
        }

        public void OnDrop(int droppedId, Action action)
        {
            _isOccupied = true;
            _droppedId = droppedId;

            action?.Invoke();
        }

        public void SetOccupied(bool occupied)
        {
            _isOccupied = occupied;
        }
    }
}