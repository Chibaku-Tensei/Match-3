using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private Color selectedColor;
        [SerializeField] private Color deselectedColor;
        public int x;
        public int y;

        public Image icon;

        private Item _item;

        private Tile[] _neighbours;

        private Image _selfImage;

        public Tile Left => x > 0 ? Board.Instance.Tiles[x - 1, y] : null;
        public Tile Top => y > 0 ? Board.Instance.Tiles[x, y - 1] : null;
        public Tile Right => x < Board.Instance.Width - 1 ? Board.Instance.Tiles[x + 1, y] : null;
        public Tile Bottom => y < Board.Instance.Height - 1 ? Board.Instance.Tiles[x, y + 1] : null;

        public Tile[] Neighbours
        {
            get
            {
                _neighbours = new[] { Left, Right, Top, Bottom };

                return _neighbours;
            }
        }
        

        public Item Item
        {
            get => _item;
            set
            {
                if (_item == value) return;

                _item = value;

                icon.sprite = _item.sprite;
                icon.material = _item.materialColor;
            }
        }

        public Button button;

        private void Start()
        {
            button.onClick.AddListener(Select);

            _selfImage = GetComponent<Image>();
        }

        public void Select()
        {
            Board.Instance.Select(this).Forget();

            _selfImage.color = selectedColor;
        }

        public void Deselect()
        {
            _selfImage.color = deselectedColor;
        }

        public List<Tile> GetMatchTile()
        {
            var match1 = FindAllMatches(new[] { Left, Right });
            var match2 = FindAllMatches(new[] { Top, Bottom }, 2);
            List<Tile> result = new List<Tile>();
            
            // if (match1.Count > 2)
            //     result = new List<Tile>(match1);
            //
            // if (match2.Count > 2)
            //     result.AddRange(match2);
            
            result.AddRange(match1);
                
            result.AddRange(match2);

            return result.Distinct().ToList();
        }

        public List<Tile> FindMatch(Tile dir, int index)
        {
            List<Tile> matchingTiles = new List<Tile>();

            if (dir == null)
            {
                return matchingTiles;
            }

            try
            {
                while (dir.Item == Item)
                {
                    matchingTiles.Add(dir);
                    dir = dir.Neighbours[index];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            

            return matchingTiles;
        }

        public List<Tile> FindAllMatches(Tile[] dir, int offset = 0)
        {
            List<Tile> matchingTiles = new List<Tile>() {this, };

            for (int i = 0; i < dir.Length; ++i)
            {
                matchingTiles.AddRange(FindMatch(dir[i], i + offset));
            }

            return matchingTiles.Distinct().ToList();
        }

        public List<Tile> GetAllAdjacentTiles()
        {
            var result = new List<Tile> { this };

            for (int i = 0; i < Neighbours.Length; ++i)
            {
                if (Neighbours[i] == null)
                {
                    continue;
                }
                
                result.Add(Neighbours[i]);
            }

            return result;
        }
    }
}

