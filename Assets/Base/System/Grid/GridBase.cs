using System.Collections;
using System.Collections.Generic;
using Base;
using UnityEngine;

namespace Base
{
    public class GridBase
    {
        private int _width, _height;
        private List<int> _gridList;

        public List<int> GridList => _gridList;
        public GridBase(int width, int height)
        {
            _width = width;
            _height = height;
        
            _gridList = new List<int>(width * height);
        }

        public void Add(int value)
        {
            _gridList.Add(value);
        }

        public void Add(int index, int value)
        {
            _gridList[index] = value;
        }
        
        public void Add(int x, int y, int value)
        {
            int index = _width * y + x;
            _gridList[index] = value;
        }

        public int Get(int x, int y)
        {
            x = Mathf.Clamp(x, 0, _width - 1);
            y = Mathf.Clamp(y, 0, _height - 1);
            return _gridList[_width * y + x];
        }
    }
}

