using _Arvis_Demo_Project_.Common._Board;
using _Arvis_Demo_Project_.Common._Building;
using _Arvis_Demo_Project_.Common._Framework;
using _Arvis_Demo_Project_Runtime._Factory;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


namespace _Arvis_Demo_Project_Runtime.Common
{
    public sealed class GameInitializer : Singleton<GameInitializer>
    {
        [Header("Board&Building Tile")]
        [SerializeField] private GameObject _tilePrefab;
        [SerializeField] private List<IDropable> _buildingTiles = new();

        [Inject] private BoardFactory _boardFactory;

        public List<IDropable> BuildingPlacementAreas => _buildingTiles;

        private void Start()
        {

            Board board = _boardFactory.CreateBoard(_tilePrefab, _buildingTiles);
            board?.CreateTile();
        }
    }
}

