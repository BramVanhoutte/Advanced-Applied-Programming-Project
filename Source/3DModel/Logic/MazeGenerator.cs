using Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class MazeGenerator
    {
        public List<Wall> wallList = new List<Wall>();
        public List<Node> nodeList = new List<Node>();
        private Stack<Node> path = new Stack<Node>();
        private Stack<Node> pathFinal = new Stack<Node>();
        private Stack<Stack<Node>> paths = new Stack<Stack<Node>>();

        private bool backtracking = false;
        private Random random = new Random();
        public MazeGenerator()
        {
            GenerateWalls();
            GenerateNodes();
            VisitNode(nodeList[0]);
        }

        private void GenerateWalls()
        {
            wallList.Add(new Wall(0, 0, 1, 49));
            wallList.Add(new Wall(0, 0, 49, 1));
            wallList.Add(new Wall(49, 49, 48, 0));
            wallList.Add(new Wall(0, 48, 49, 49));

            for(int i = 4; i <= 49; i+=4 )
            {
                for (int j = 4; j <= 49; j += 4)
                {
                    wallList.Add(new Wall(j, i-4, j + 1, i + 1));
                    wallList.Add(new Wall(j - 4, i, j + 1, i + 1));
                }
            }
        }

        private void GenerateNodes()
        {

            for (int i = 0; i < 47; i += 4)
            {
                for (int j = 0; j < 47; j += 4)
                {
                    nodeList.Add(new Node(j, i, j + 5, i + 5));
                }
            }
        }
        private void VisitNode(Node visit)
        {
            
            nodeList.FirstOrDefault(p => p.nodeX == visit.nodeX && p.nodeY == visit.nodeY).VisitedNode();
            List<Node> adjacentNodes = new List<Node>();
            Node next = null;
            foreach (Node node in nodeList)
            {
                if(node.nodeX == visit.nodeX)
                {
                    if(node.nodeY == visit.nodeY - 4.0 || node.nodeY == visit.nodeY + 4.0)
                    {
                        adjacentNodes.Add(node);
                    }
                }
                else if(node.nodeY == visit.nodeY)
                {
                    if (node.nodeX == visit.nodeX - 4.0 || node.nodeX == visit.nodeX + 4.0)
                    {
                        adjacentNodes.Add(node);
                    }
                }
            }
            while(true)
            {
                if(!adjacentNodes.Any())
                {
                    next = null;
                    break;
                }

                Node temp = adjacentNodes[random.Next(0, adjacentNodes.Count)];

                if(!temp.Visited)
                {
                    next = temp;
                    break;
                }
                else
                {
                    adjacentNodes.Remove(temp);
                }
            }

            if(!(next == null))
            {
                backtracking = false;
                path.Push(visit);
                if(next.nodeX == 46.5 && next.nodeY == 46.5)
                {
                    pathFinal = new Stack<Node>(path);
                    pathFinal.Push(next);
                }
                if (next.nodeX == visit.nodeX)
                {
                    double maxY = next.nodeY > visit.nodeY ? next.nodeY : visit.nodeY;
                    double minY = next.nodeY < visit.nodeY ? next.nodeY : visit.nodeY;
                    foreach (Wall wall in wallList)
                    {
                        if(wall.wallX == next.nodeX)
                        {
                            if (wall.wallY > minY && wall.wallY < maxY)
                            {
                                wallList.Remove(wall);
                                break;
                            }
                        }
                    }
                }
                else if (next.nodeY == visit.nodeY)
                {
                    double maxX = next.nodeX > visit.nodeX ? next.nodeX : visit.nodeX;
                    double minX = next.nodeX < visit.nodeX ? next.nodeX : visit.nodeX;
                    foreach (Wall wall in wallList)
                    {
                        if (wall.wallY == next.nodeY)
                        {
                            if (wall.wallX > minX && wall.wallX < maxX)
                            {
                                wallList.Remove(wall);
                                break;
                            }
                        }
                    }
                }
                VisitNode(next);
            }
            else
            {
                if(!backtracking) {
                    backtracking = true;
                }
                if(path.Count == 0)
                {
                    FinalizeGeneration();
                    return;
                }
                VisitNode(path.Pop());
            }
        }

        private void FinalizeGeneration()
        {
            while (paths.Count > 0)
            {
                Stack<Node> temp = paths.Pop();
                if(temp.Count > pathFinal.Count)
                {
                    pathFinal = new Stack<Node>(temp);
                }
            }
            bool first = true;
            while(pathFinal.Count > 0)
            {
                if (first)
                {
                    Node temp = pathFinal.Pop();
                    nodeList.FirstOrDefault(p => p.nodeX == temp.nodeX && p.nodeY == temp.nodeY).PathNode();
                    nodeList.FirstOrDefault(p => p.nodeX == temp.nodeX && p.nodeY == temp.nodeY).EndNode();
                    first = false;
                }
                else
                {
                    Node temp = pathFinal.Pop();
                    nodeList.FirstOrDefault(p => p.nodeX == temp.nodeX && p.nodeY == temp.nodeY).PathNode();
                }
            }

            foreach(Node node in nodeList)
            {
                node.CreateShape();
            }
        }
    }

    
}
