using System;
using System.Collections.Generic;
using Arena;
using UnityEngine;

namespace AI
{
    public class Pathfinding
    {
        private readonly PathNode[,] _grid;
        private List<PathNode> _openList;
        private List<PathNode> _closedList;
        private readonly int _width;
        private readonly int _height;

        private const int MoveCost = 1;
        
        public Pathfinding()
        {
            var arena = GameArena.Instance;
            var width = arena.Grid.width;
            var height = arena.Grid.height;
            _grid = new PathNode[width, height];
            _width = width;
            _height = height;
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    _grid[x, y] = new PathNode(x, y, arena.Grid[x, y] == null);
                }
            }
        }

        private List<PathNode> FindPath(int startX, int startY, int endX, int endY)
        {

            if (startX > _width || endX > _width || startX < 0 || endX < 0 || startY > _height || endY > _height ||
                startY < 0 || endY < 0)
            {
                throw new Exception("point out of bounds");
            }
            
            _openList = new List<PathNode> { _grid[startX, startY] };
            _closedList = new List<PathNode>();

            var startNode = _grid[startX, startY];
            var endNode = _grid[endX, endY];

            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    var pathNode = _grid[x, y];
                    pathNode.gCost = int.MaxValue;
                    pathNode.CalculateFCost();
                    pathNode.previousNode = null;
                }
            }

            startNode.gCost = 0;
            startNode.hCost = CalculateDistanceCost(startNode, endNode);
            startNode.CalculateFCost();

            while (_openList.Count > 0)
            {
                var currentNode = GetLowerFCostNode(_openList);
                if (currentNode == endNode)
                {
                    return CalculatePath(endNode);
                }

                _openList.Remove(currentNode);
                _closedList.Add(currentNode);

                foreach (var neighbourNode in GetNeighbourList(currentNode))
                {
                    if (_closedList.Contains(neighbourNode)) continue;
                    if (!neighbourNode.isWalkable)
                    {
                        _closedList.Add(neighbourNode);
                        continue;
                    }
                    var tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                    if (tentativeGCost >= neighbourNode.gCost) continue;
                    neighbourNode.previousNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!_openList.Contains(neighbourNode))
                    {
                        _openList.Add(neighbourNode);
                    }
                }
            }

            return null;
        }

        private List<PathNode> GetNeighbourList(PathNode currentNode)
        {
            var neighbourList = new List<PathNode>();
            var currentX = currentNode.x;
            var currentY = currentNode.y;

            if (currentX - 1 >= 0)
            {
                neighbourList.Add(_grid[currentX - 1, currentY]);
            }
            if (currentX + 1 < _width)
            {
                neighbourList.Add(_grid[currentX + 1, currentY]);
            }
            if (currentY - 1 >= 0)
            {
                neighbourList.Add(_grid[currentX, currentY - 1]);
            }
            if (currentY + 1 < _height)
            {
                neighbourList.Add(_grid[currentX, currentY + 1]);
            }
            
            return neighbourList;
        }
        
        private List<PathNode> CalculatePath(PathNode endNode)
        {
            var path = new List<PathNode>();
            var currentNode = endNode;
            while (currentNode.previousNode != null)
            {
                path.Add(currentNode.previousNode);
                currentNode = currentNode.previousNode;
            }
            path.Reverse();
            return path;
        }

        private int CalculateDistanceCost(PathNode a, PathNode b)
        {
            var xDistance = Mathf.Abs(a.x - b.x);
            var yDistance = Mathf.Abs(a.y - b.y);
            return MoveCost * (xDistance + yDistance);
        }

        private PathNode GetLowerFCostNode(List<PathNode> pathNodeList)
        {
            var lowestFCostNode = pathNodeList[0];
            for (var i = 1; i < pathNodeList.Count; i++)
            {
                var currentNode = pathNodeList[i];
                if (currentNode.fCost < lowestFCostNode.fCost)
                {
                    lowestFCostNode = currentNode;
                }
            }

            return lowestFCostNode;
        }
    }
}