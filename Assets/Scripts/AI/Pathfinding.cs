using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class Pathfinding
    {
        private PathNode[,] grid;
        private List<PathNode> openList;
        private List<PathNode> closedList;

        private const int MOVE_COST = 1;

        public Pathfinding(int width, int height)
        {
            grid = new PathNode[width, height];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    grid[x, y] = new PathNode(x, y);
                }
            }
        }

        private List<PathNode> FindPath(int startX, int startY, int endX, int endY)
        {
            var width = grid.GetLength(0);
            var height = grid.GetLength(1);
            
            if (startX > width || endX > width || startX < 0 || endX < 0 || startY > height || endY > height ||
                startY < 0 || endY < 0)
            {
                throw new Exception("point out of bounds");
            }
            
            openList = new List<PathNode> { grid[startX, startY] };
            closedList = new List<PathNode>();

            var startNode = grid[startX, startY];
            var endNode = grid[endX, endY];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < width; y++)
                {
                    var pathNode = grid[x, y];
                    pathNode.gCost = int.MaxValue;
                    pathNode.CalculateFCost();
                    pathNode.previousNode = null;
                }
            }

            startNode.gCost = 0;
            startNode.hCost = CalculateDistanceCost(startNode, endNode);
            startNode.CalculateFCost();

            while (openList.Count > 0)
            {
                var currentNode = GetLowerFCostNode(openList);
                if (currentNode == endNode)
                {
                    return CalculatePath(endNode);
                }

                openList.Remove(currentNode);
            }

            return null;
        }

        private List<PathNode> CalculatePath(PathNode endNode)
        {
            return null;
        }

        private int CalculateDistanceCost(PathNode a, PathNode b)
        {
            int xDistance = Mathf.Abs(a.x - b.x);
            int yDistance = Mathf.Abs(a.y - b.y);
            return MOVE_COST * (xDistance + yDistance);
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