using System;
using System.Collections.Generic;
using UnityEngine;

public class Holder : MonoBehaviour
{
        [SerializeField] private Pair _pairPrefab;
        [SerializeField] private Player _playerPrefab;
        private int _playerCountOneSide;

        public int HolderIndex;
        public bool IsSideLeft;
        public List<Pair> Pairs = new List<Pair>();
        public List<Holder> childrenHolders = new List<Holder>();
        public List<Player> Players = new List<Player>();
        public event Action<bool> OnSpawnPairsComplete;

        public void SetInfoHolder(int playerCountOneSide, int holderIndex)
        {
                _playerCountOneSide = playerCountOneSide;
                HolderIndex = holderIndex;
                SpawnAllPairs();
        }

        private void SpawnAllPairs()
        {
                int pairsCount;
                int playersCountInPair;
                if (_playerCountOneSide > 1)
                {
                        pairsCount = _playerCountOneSide / 2;
                        playersCountInPair = 2;
                }
                else
                {
                        pairsCount = 1;
                        playersCountInPair = 1;
                }
                SpawnPairs(this, pairsCount, playersCountInPair);
                int requirePairsCount = (pairsCount / 2) == 0 ? 1 : pairsCount / 2;
                for (int i = 0; i < childrenHolders.Count; i++)
                {
                        SpawnPairs(childrenHolders[i], requirePairsCount, 
                                i == childrenHolders.Count - 1 ? 1 : 2);
                        
                        if (requirePairsCount != 1)
                        {
                                requirePairsCount /= 2;  
                        }
                }
                OnSpawnPairsComplete?.Invoke(IsSideLeft);
        }
        
        private void SpawnPairs(Holder currentHolder, int pairsCount, int playersCountInPair)
        {
                for (int k = 0; k < pairsCount; k++)
                {
                        var currentPair = Instantiate(_pairPrefab, currentHolder.transform);
                        for (int j = 0; j < playersCountInPair; j++)
                        {
                                var currentPlayer = Instantiate(_playerPrefab, currentPair.transform);
                                currentPair.Players.Add(currentPlayer);
                                currentPair.PairIndex = k;
                                currentPair.HolderIndex = currentHolder.HolderIndex;
                        }
                        currentHolder.Pairs.Add(currentPair);
                        foreach (var player in currentPair.Players)
                        {
                                Players.Add(player);
                        }
                }
        }
}