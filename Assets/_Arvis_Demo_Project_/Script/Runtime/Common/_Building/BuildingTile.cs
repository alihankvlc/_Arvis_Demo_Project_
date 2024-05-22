using System.Collections.Generic;
using UnityEngine;

namespace _Arvis_Demo_Project_.Common._Building
{
    public sealed class BuildingTile
    {
        public int Width;
        public int Height;

        public BuildingTile(GameObject tile, int width, int height, Transform placeHolder, Vector2Int vector2Int, List<IDropable> areaList)
        {
            Create(tile, width, height, placeHolder, vector2Int, areaList);
        }

        private void Create(GameObject tile, int width, int height, Transform placeHolder, Vector2Int vector2Int, List<IDropable> areaList)
        {
            Vector2 position = new(width, height);
            GameObject outputTileObject = Object.Instantiate(tile, position, Quaternion.identity);
            BuildingTileArea tileArea = outputTileObject.AddComponent<BuildingTileArea>();
            IDropable dropableComponent = tileArea.GetComponent<IDropable>();


            tileArea.Init(vector2Int);

            string tileTransformName = $"Building_Tile_[X={vector2Int.x}] [Y={vector2Int.y}]";
            outputTileObject.name = tileTransformName;

            outputTileObject.transform.SetParent(placeHolder, false);
            areaList.Add(dropableComponent);
        }

    }
}