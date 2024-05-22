using _Arvis_Demo_Project_.Common._Framework;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace _Arvis_Demo_Project_.Common._Database
{
    public interface IData
    {
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }
    }

    public abstract class Database<T> : DeletableScriptableObject where T : IData
    {
        [SerializeField, InlineEditor, Searchable] protected List<T> Datas = new List<T>();
        [SerializeField, ReadOnly] protected Dictionary<int, T> Cache = new Dictionary<int, T>();

        public List<T> Get_Data_List => Datas;
        public bool ContainsItem(int id) => Cache.ContainsKey(id);

        public virtual void Init()
        {
            Cache = Datas.ToDictionary(r => r.Id);
        }

#if UNITY_EDITOR
        public void AddItem(T item)
        {
            if (!Datas.Contains(item) && !Cache.ContainsKey(item.Id))
            {
                Datas.Add(item);
                Cache[item.Id] = item;
                EditorUtility.SetDirty(this);
            }
        }

        public void RemoveItem(int id)
        {
            if (Cache.TryGetValue(id, out T item))
            {
                Datas.Remove(item);
                Cache.Remove(id);
                EditorUtility.SetDirty(this);
            }
        }
#endif

    }
}
