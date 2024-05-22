using _Arvis_Demo_Project_.Common._Building;
using System.Collections.Generic;
using UnityEngine;

namespace _Arvis_Demo_Project_.Common._Board
{
    public sealed class Board
    {
        private const int _width = 10;
        private const int _height = 10;

        private const string _tilePlaceHolderName = "Tile_Place_Holder";

        private List<IDropable> _buildingTileAreaList = new();


        private GameObject _tilePrefab;

        public Board(GameObject tilePrefab, List<IDropable> tileAreas)
        {
            _buildingTileAreaList = tileAreas;
            _tilePrefab = tilePrefab;
        }

        public void CreateTile()
        {
            GameObject placeHolder = new(_tilePlaceHolderName);

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    Vector2Int position = new(x, y);
                    BuildingTile tile = new(_tilePrefab, x, y, placeHolder.transform, position, _buildingTileAreaList);
                }
            }
        }
    }
}