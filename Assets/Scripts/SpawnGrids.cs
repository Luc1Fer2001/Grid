using System.Collections.Generic;
using UnityEngine;

public class SpawnGrids : MonoBehaviour
{
        [SerializeField] private Transform _mainHolder;
        [SerializeField] private Holder _leftHolder;
        [SerializeField] private Holder _rightHolder;
        [SerializeField] private Holder _holderPrefab;
        private List<Holder> _leftHolders = new List<Holder>();
        private List<Holder> _rightHolders = new List<Holder>();

        private void Start()
        {
                SpawnHolders(8);
        }

        private int GetPowTwo(int value)
        {
                if (value == 2) return 1;
                int currentPow = 1;
                while (value / 2 != 2)
                {
                        currentPow++;
                        value /= 2;
                }

                currentPow++;
                return currentPow;
        }

        // I know about the binary tree, but there was not enough time to study this issue for a good implementation
        private void SpawnHolders(int playerCount)
        {
                var playersOnOneSide = playerCount / 2;
                int countHolders = playersOnOneSide == 1 ? 1 : GetPowTwo(playersOnOneSide);////////////////// 1/2

                var countChildrenToHolder = countHolders;
                countHolders *= 2;
                for (int i = 0; i < countHolders; i++)
                {
                        var currentHolder = Instantiate(_holderPrefab, _mainHolder);
                        _mainHolder.GetChild(_mainHolder.childCount - 1).SetSiblingIndex(i + 1);
                        if (i < countChildrenToHolder)
                        {
                                _leftHolder.childrenHolders.Add(currentHolder);
                        }
                        else
                        {
                                _rightHolder.childrenHolders.Add(currentHolder);
                        }
                }
                _rightHolder.childrenHolders.Reverse();
                
                _leftHolders.Add(_leftHolder);
                _rightHolders.Add(_rightHolder);
                
                for (int i = 0; i < _leftHolder.childrenHolders.Count; i++)
                {
                        _leftHolders.Add(_leftHolder.childrenHolders[i]);
                        _leftHolder.childrenHolders[i].HolderIndex = i + 1;
                }
                
                for (int i = 0; i < _rightHolder.childrenHolders.Count; i++)
                {
                        _rightHolders.Add(_rightHolder.childrenHolders[i]);
                        _rightHolder.childrenHolders[i].HolderIndex = i + 1;
                }

                // _leftHolder.OnSpawnPairsComplete += SetInfoToPairs;
                // _rightHolder.OnSpawnPairsComplete += SetInfoToPairs;
                
                _leftHolder.SetInfoHolder(playersOnOneSide, 0);
                _rightHolder.SetInfoHolder(playersOnOneSide, 0);
        }
        
        private void SetInfoToPairs(bool isSideLeft)
        {
                List<Holder> holders = isSideLeft ? _leftHolders : _rightHolders;
                for (int i = 0; i < holders.Count; i++)
                {
                        for (int k = 0; k < holders[i].Pairs.Count; k++)
                        {
                                if(holders[i].Players.Count == 1) return;
                                var index = Mathf.Clamp(i + 1, 0, holders.Count - 1);
                                var clampedPlayers = Mathf.Clamp(k, 0, holders[index].Players.Count - 1);
                                holders[i].Pairs[k].NextPlayer = holders[index].Players[clampedPlayers];
                        }
                }

                if (isSideLeft)
                {
                        _leftHolder.OnSpawnPairsComplete -= SetInfoToPairs;
                }
                else
                {
                        _rightHolder.OnSpawnPairsComplete -= SetInfoToPairs;
                }
        }
}