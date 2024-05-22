using _Arvis_Demo_Project_.Common._Building;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Arvis_Demo_Project_.Common._Other
{
    public class PlacementPreview : SerializedMonoBehaviour
    {
        [SerializeField] private Transform _placeHolder;
        [SerializeField] private GameObject _previewObject;

        [SerializeField] private Dictionary<ShapeType, List<GameObject>> _previewCache = new Dictionary<ShapeType, List<GameObject>>();
        [SerializeField] private ShapeType _type;

        private void Start()
        {
            foreach (ShapeType shapeType in Enum.GetValues(typeof(ShapeType)))
                InitPreview(shapeType);
        }

        public void InitPreview(ShapeType shapeType)
        {
            if (_previewCache.ContainsKey(shapeType))
                return;

            Vector2Int[] positions = BuildingShapeData.Cells[shapeType];
            List<GameObject> previewObjects = new List<GameObject>();

            foreach (Vector2Int position in positions)
            {
                GameObject obj = Instantiate(_previewObject, new Vector3(position.x, position.y, 0), Quaternion.identity);
                obj.transform.SetParent(_placeHolder, false);
                obj.SetActive(false);
                previewObjects.Add(obj);
            }

            _previewCache.Add(shapeType, previewObjects);
        }

        public void ShowPreview(ShapeType shapeType)
        {
            HideAllPreviews();

            if (_previewCache.TryGetValue(shapeType, out List<GameObject> previewObjects))
            {
                previewObjects.ForEach(r => r.SetActive(true));
            }
        }

        public void HideAllPreviews()
        {
            foreach (KeyValuePair<ShapeType, List<GameObject>> kvp in _previewCache)
                kvp.Value.ForEach(r => r.SetActive(false));
        }

        public void ChangeColor(Color color)
        {
            foreach (KeyValuePair<ShapeType, List<GameObject>> kvp in _previewCache)
                kvp.Value.ForEach(r =>
                {
                    SpriteRenderer renderer = r.GetComponent<SpriteRenderer>();
                    renderer.color = color;
                });
        }
    }
}
