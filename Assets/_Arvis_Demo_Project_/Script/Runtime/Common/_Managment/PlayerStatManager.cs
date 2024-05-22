using System;
using UnityEngine.Events;
using UnityEngine;

namespace _Arvis_Demo_Project_.Common._Managment
{
    public enum StatType { Gem, Gold }
    public interface IPlayerData
    {
        public int Gem { get; }
        public int Gold { get; }

        public void UpdateStat(StatType type, int amount);
    }

    public sealed class PlayerStatManager : MonoBehaviour, IPlayerData
    {
        [System.Serializable]
        public struct PlayerData
        {
            public int Gem;
            public int Gold;
            public bool IsInitializeGemAndGold;
        }

        [SerializeField] private int _gem;
        [SerializeField] private int _gold;

        [SerializeField] private UnityEvent OnChangePlayerGemEvent;
        [SerializeField] private UnityEvent OnChangePlayerGoldEvent;

        [SerializeField] private PlayerData _data = new PlayerData();

        private bool _isInitGemAndGold;

        private const int _initGem = 10;
        private const int _initGold = 50;

        public delegate void OnChangePlayerStatDelegate(int value);


        public static event OnChangePlayerStatDelegate OnChangePlayerGem;
        public static event OnChangePlayerStatDelegate OnChangePlayerGold;

        public int Gem
        {
            get => _gem;
            private set => SetStatAndActionInvoke(ref _gem, value, UnityEvent_OnChangePlayerGem);
        }

        public int Gold
        {
            get => _gold;
            private set => SetStatAndActionInvoke(ref _gold, value, UnityEvent_OnChangePlayerGold);
        }

        private void Start()
        {
            _data = SaveManager.Instance.Load<PlayerData>();
            _isInitGemAndGold = _data.IsInitializeGemAndGold;

            if (!_isInitGemAndGold)
            {
                UpdateStat(StatType.Gem, _initGem);
                UpdateStat(StatType.Gold, _initGold);

                _isInitGemAndGold = true;
                return;
            }

            Gem = _data.Gem;
            Gold = _data.Gold;
        }

        private void OnApplicationQuit()
        {
            _data.IsInitializeGemAndGold = _isInitGemAndGold;
            _data.Gold = Gold;
            _data.Gem = Gem;

            SaveManager.Instance.Save(_data);
        }

        public void UpdateStat(StatType type, int amount)
        {
            switch (type)
            {
                case StatType.Gem:
                    UpdateGem(amount);
                    break;
                case StatType.Gold:
                    UpdateGold(amount);
                    break;
            }
        }
        private void UpdateGold(int amount)
        {
            Gold += amount;
            Gold = Mathf.Clamp(Gold, 0, int.MaxValue);
        }

        private void UpdateGem(int amount)
        {
            Gem += amount;
            Gem = Mathf.Clamp(Gem, 0, int.MaxValue);
        }

        private void SetStatAndActionInvoke(ref int field, int value, Action action = null)
        {
            if (field != value)
            {
                field = value;
                action?.Invoke();
            }
        }

        private void UnityEvent_OnChangePlayerGem()
        {
            OnChangePlayerGem?.Invoke(_gem);
            OnChangePlayerGemEvent?.Invoke();
        }

        private void UnityEvent_OnChangePlayerGold()
        {
            OnChangePlayerGold?.Invoke(_gold);
            OnChangePlayerGoldEvent?.Invoke();
        }
    }
}
