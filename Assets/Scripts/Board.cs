using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Base;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public sealed class Board : MonoBehaviour
    {
        public static Board Instance { get; private set; }

        public Row[] rows;

        public Tile[,] Tiles { get; private set; }

        public int Width => Tiles.GetLength(0);
        public int Height => Tiles.GetLength(1);

        private readonly List<Tile> _selectedTile = new List<Tile>();

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Tiles = new Tile[rows.Max(row => row.tiles.Length), rows.Length];
            
            StartBoard();
        }

        public void StartBoard()
        {
            int itemDatabaseLength = ItemDatabase.Items.Length;
            Item[] previousLeft = new Item[Height];
            Item previousAbove = null;

            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    Tile tile = rows[y].tiles[x];

                    tile.x = x;
                    tile.y = y;

                    List<Item> possibleItems = ItemDatabase.Items.ToList();
                    possibleItems.Remove(previousLeft[y]);
                    //possibleItems.Remove(previousAbove);
                    tile.Item = possibleItems[Random.Range(0, possibleItems.Count)];
                    possibleItems.Shuffle();
                    
                    Tiles[x, y] = tile;

                    previousLeft[y] = tile.Item;
                    previousAbove = tile.Item;
                }  
            }
        }

        public async UniTaskVoid Select(Tile tile)
        {
            if (!_selectedTile.Contains(tile))
                _selectedTile.Add(tile);

            if (_selectedTile.Count < 2) return;
            
            if (!_selectedTile[0].GetAllAdjacentTiles().Contains(_selectedTile[1]))
            {
                _selectedTile[0].Deselect();
                _selectedTile.RemoveAt(0);
                return;
            }

            await Swap(_selectedTile[0], _selectedTile[1]);
            
            _selectedTile[0].Deselect();
            _selectedTile[1].Deselect();

            if (CanExplode())
            {
                Explode().Forget();
            }
            else
            {
                await Swap(_selectedTile[0], _selectedTile[1]);
            }
            
            _selectedTile.Clear();
        }

        private async UniTask Swap(Tile tile1, Tile tile2)
        {
            var icon1 = tile1.icon;
            var icon2 = tile2.icon;

            var transform1 = icon1.transform;
            var transform2 = icon2.transform;

            var sequence = DOTween.Sequence();

            sequence.Join(transform1.DOMove(transform2.position, .25f))
                .Join(transform2.DOMove(transform1.position, .25f));
            
            await sequence.Play().AsyncWaitForCompletion();
            
            transform1.SetParent(tile2.transform);
            transform2.SetParent(tile1.transform);

            tile1.icon = icon2;
            tile2.icon = icon1;

            (tile1.Item, tile2.Item) = (tile2.Item, tile1.Item);
        }

        private bool CanExplode()
        {
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    if (Tiles[x, y].GetMatchTile().Skip(1).Count() >= 2)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private async UniTask Explode()
        {
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    var tile = Tiles[x, y];

                    var connectedTile = tile.GetMatchTile();

                    if (connectedTile.Skip(1).Count() < 2) continue;

                    var deflateSequence = DOTween.Sequence();

                    foreach (var a in connectedTile) deflateSequence.Join(a.icon.transform.DOScale(Vector3.zero, .25f));

                    await deflateSequence.Play().AsyncWaitForCompletion();

                    var inflateSequence = DOTween.Sequence();

                    foreach (var a in connectedTile)
                    {
                        a.Item = ItemDatabase.Items[Random.Range(0, ItemDatabase.Items.Length)];
                        inflateSequence.Join(a.icon.transform.DOScale(Vector3.one, .25f));
                    }

                    await inflateSequence.Play().AsyncWaitForCompletion();
                    
                    UIManager.Instance.Scored(100);

                    x = 0;
                    y = 0;
                }
            }
        }
    }
}

