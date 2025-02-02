using Microsoft.Xna.Framework;
using MonoGizmo.Abstraction;
using System.Collections.Generic;

namespace MonoGizmo.DataStructures
{
    // QuadTree used for efficient ISelectable selection
    internal sealed class QuadTree
    {
        private const int MAX_OBJECTS = 10;
        private const int MAX_LEVELS = 5;
        private readonly int _level;
        private readonly List<ISelectable> _objects;
        private readonly Rectangle _bounds;
        private readonly QuadTree[] _nodes;

        public QuadTree(int level, Rectangle bounds)
        {
            _level = level;
            _objects = new List<ISelectable>();
            _bounds = bounds;
            _nodes = new QuadTree[4];
        }

        public void Clear()
        {
            _objects.Clear();
            for (int i = 0; i < _nodes.Length; i++)
            {
                if (_nodes[i] != null)
                {
                    _nodes[i].Clear();
                    _nodes[i] = null;
                }
            }
        }

        private void Split()
        {
            int subWidth = _bounds.Width / 2;
            int subHeight = _bounds.Height / 2;
            int x = _bounds.X;
            int y = _bounds.Y;
            _nodes[0] = new QuadTree(_level + 1, new Rectangle(x + subWidth, y, subWidth, subHeight));     // Top-right
            _nodes[1] = new QuadTree(_level + 1, new Rectangle(x, y, subWidth, subHeight));               // Top-left
            _nodes[2] = new QuadTree(_level + 1, new Rectangle(x, y + subHeight, subWidth, subHeight));   // Bottom-left
            _nodes[3] = new QuadTree(_level + 1, new Rectangle(x + subWidth, y + subHeight, subWidth, subHeight)); // Bottom-right
        }

        private int GetIndex(Vector2 position)
        {
            int index = -1;
            float verticalMidpoint = _bounds.X + (_bounds.Width / 2f);
            float horizontalMidpoint = _bounds.Y + (_bounds.Height / 2f);

            bool topQuadrant = position.Y < horizontalMidpoint;
            bool bottomQuadrant = position.Y >= horizontalMidpoint;

            if (position.X >= verticalMidpoint)
            {
                if (topQuadrant)
                {
                    index = 0; // Top-right
                }
                else if (bottomQuadrant)
                {
                    index = 3; // Bottom-right
                }
            }
            else if (position.X < verticalMidpoint)
            {
                if (topQuadrant)
                {
                    index = 1; // Top-left
                }
                else if (bottomQuadrant)
                {
                    index = 2; // Bottom-left
                }
            }

            return index;
        }

        public void Insert(ISelectable selectable)
        {
            if (_nodes[0] != null)
            {
                int index = GetIndex(selectable.Center);
                if (index != -1)
                {
                    _nodes[index].Insert(selectable);
                    return;
                }
            }

            _objects.Add(selectable);

            if (_objects.Count > MAX_OBJECTS && _level < MAX_LEVELS)
            {
                if (_nodes[0] == null)
                {
                    Split();
                }

                int i = 0;
                while (i < _objects.Count)
                {
                    int index = GetIndex(_objects[i].Center);
                    if (index != -1)
                    {
                        _nodes[index].Insert(_objects[i]);
                        _objects.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }

        public List<ISelectable> Retrieve(List<ISelectable> returnObjects, Vector2 position)
        {
            int index = GetIndex(position);
            if (index != -1 && _nodes[0] != null)
            {
                _nodes[index].Retrieve(returnObjects, position);
            }

            returnObjects.AddRange(_objects);
            return returnObjects;
        }
    }
}
