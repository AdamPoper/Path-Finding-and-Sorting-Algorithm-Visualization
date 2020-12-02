using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Algorithm_Visualizer
{
    /// <summary>
    /// An algorithm contains the neccessary UI stuff, the grid layout, rows, and columns, and formatting of the nodes
    /// I figured it didn't matter where I put it so it goes in the algorithm class just so it's easy
    /// </summary>
    public class PathFindingAlgorithm
    {
        public enum Algorithms
        {
            A_STAR = 0,
            DIJKSTRAS = 1
        }
        public Grid grid;
        public List<Node> nodes;
        public int rows { get; set; }
        public int cols { get; set; }
        public Algorithms algoType;
        public PathFindingAlgorithm()
        {
            this.rows = 10;
            this.cols = 10;            
        }
        public PathFindingAlgorithm(Grid grid, int rows, int cols)
        {
            this.grid = grid;
            this.rows = rows;
            this.cols = cols;
            this.grid.Width = 600;
            this.grid.Height = 600;
            Init();
        }
        protected void Init()
        {            
            for (int i = 0; i < cols; i++)
                this.grid.ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 0; i < rows; i++)
                this.grid.RowDefinitions.Add(new RowDefinition());
            nodes = new List<Node>();
            int n = 0;
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    Node node = new Node(x, y);
                    node.rect.Stroke = Brushes.Black;
                    Grid.SetColumn(node.rect, x);
                    Grid.SetRow(node.rect, y);
                    this.grid.Children.Add(node.rect);
                    nodes.Add(node);
                    n++;
                }
            }
        }
        // find the neighbors of the current 
        public void findNeighbors(Node currentNode, List<Node> open, List<Node> closed, Node startNode, Node endNode)
        {
            List<Node> neighbors = new List<Node>();
            { // up
                foreach (Node n in nodes)
                    if (n.col == currentNode.col && n.row == currentNode.row - 1)
                        neighbors.Add(n);               
            }
            { // down
                foreach (Node n in nodes)
                    if (n.col == currentNode.col && n.row == currentNode.row + 1)
                        neighbors.Add(n);               
            }
            { // left
                foreach (Node n in nodes)
                    if (n.col == currentNode.col - 1 && n.row == currentNode.row)
                        neighbors.Add(n);                
            }
            { // right
                foreach (Node n in nodes)
                    if (n.col == currentNode.col + 1 && n.row == currentNode.row)
                        neighbors.Add(n);                
            }
            // neighbors only get added to the open list if they are not already part of the open list or closed list and is not blocked
            foreach(Node n in neighbors)
            {
                if (!closed.Contains(n) && !open.Contains(n) && !n.blocked)
                {
                    float newGCost = calculateGCost(n, startNode);
                    if(newGCost < n.g)    // only update it's gCost if the new gCost is less than its current gCost
                        n.g = newGCost;
                    n.h = calculateHCost(n, endNode);
                    n.f = n.g + n.h;
                    n.parent = currentNode;
                    n.setColor(Node.Green);
                    open.Add(n);                                                       
                }
            }
        }
        // both of these use pythagorus
        public float calculateGCost(Node node, Node startNode)
        {
            float x = Math.Abs(startNode.col - node.col);
            float y = Math.Abs(startNode.row - node.row);
            return (float)Math.Sqrt((x * x) + (y * y));
        }
        public float calculateHCost(Node node,Node endNode)
        {
            float x = Math.Abs(endNode.col - node.col);
            float y = Math.Abs(endNode.row - node.row);
            return (float)Math.Sqrt((x * x) + (y * y));
        }
        public void retracePath(Node start, Node end)
        {
            Node n = end;
            while(n != start)
            {
                n.setColor(Node.Yellow);
                n = n.parent;
            }
        }
        public virtual void AlgorithmLoop(object sender, EventArgs e) { }        
    }    
  
    public class AStarAlgorithm : PathFindingAlgorithm
    {
        public static bool isStartDefined { get; set; }
        public static bool isEndDefined { get; set; }
        public static bool finished { get; set; }
        public Node currentNode;
        public Node startNode;
        public Node endNode;
        public List<Node> open;
        private List<Node> closed;

        public AStarAlgorithm()
        {
            this.rows = 10;
            this.cols = 10;
            this.algoType = Algorithms.A_STAR;
            this.open = new List<Node>();
            this.closed = new List<Node>();
            Init();                        
        }
        static AStarAlgorithm()
        {
            isEndDefined = false;
            isStartDefined = false;
            finished = false;
        }
        public AStarAlgorithm(Grid grid, int rows, int cols)
        {
            this.grid = grid;
            this.rows = rows;
            this.cols = cols;
            this.grid.Width = 1600;
            this.grid.Height = 800;
            this.algoType = Algorithms.A_STAR;
            this.open   = new List<Node>();
            this.closed = new List<Node>();
            Init();
        }
        public void DefineStartEnd()
        {
            foreach(Node n in this.nodes)
            {
                if (n.isEnd)
                {
                    endNode = n;
                    isEndDefined = true;
                }
                if(n.isStart)
                {
                    startNode = n;
                    isStartDefined = true;
                }
            }
        }
        public override void AlgorithmLoop(object sender, EventArgs e)
        {                      
            if (!isStartDefined || !isEndDefined)
            {                              
                DefineStartEnd();
                if (isStartDefined && isEndDefined)
                {
                    currentNode = startNode;
                    open.Add(currentNode);                    
                }
            }               
            if(open.Count > 0 && !finished)
            {                
                Node lowestNode = open[0];
                foreach(Node node in open)
                    if (node.f < lowestNode.f)
                        lowestNode = node;
                currentNode = lowestNode;
                currentNode.setColor(Node.Red);
                open.Remove(currentNode);                    
                closed.Add(currentNode);
                if (currentNode == endNode)
                {
                    finished = true;
                    retracePath(startNode, endNode);
                }
                if (!finished)
                    findNeighbors(currentNode, open, closed, startNode, endNode);
            }
        }       
    }
}
