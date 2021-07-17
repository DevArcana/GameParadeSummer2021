﻿namespace AI
{
    public class PathNode
    {
        public readonly int x;
        public readonly int y;

        public int gCost;
        public int hCost;
        public int fCost;

        public readonly bool isWalkable;

        public PathNode previousNode;
        
        public PathNode(int x, int y, bool isWalkable = true)
        {
            this.x = x;
            this.y = y;
            this.isWalkable = isWalkable;
        }

        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }
    }
}