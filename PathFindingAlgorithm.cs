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
    /// A pathfinding algorithm contains the neccessary UI stuff, the grid layout, rows, and columns, and formatting of the nodes
    /// I figured it didn't matter where I put it so it goes in the path finding algorithm class just so it's easy
    /// </summary>
    public class PathFindingAlgorithm
    {
        public enum Algorithms
        {
            A_STAR = 0,
            DIJKSTRAS = 1
        }
        protected Grid grid;
        public List<Node> nodes;
        public int rows { get; set; }
        public int cols { get; set; }
        public Algorithms algoType;
        
        protected Node currentNode;
        protected Node startNode;
        protected Node endNode;
        protected List<Node> open;
        protected List<Node> closed;
        public PathFindingAlgorithm()
        {
            rows = 10;
            cols = 10;            
        }
        public PathFindingAlgorithm(Grid grid, int rows, int cols)
        {
            this.grid = grid;
            this.rows = rows;
            this.cols = cols;
            this.grid.Width = 1600;
            this.grid.Height = 800;
            Init();
        }
        protected void Init()
        {            
            for (int i = 0; i < cols; i++)
                this.grid.ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 0; i < rows; i++)
                this.grid.RowDefinitions.Add(new RowDefinition());
            nodes = new List<Node>();            
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
                }
            }
        }
        // find the neighbors of the current 
        protected List<Node> findNeighbors(Node currentNode)
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
            
            return neighbors;
        }
        // both of these use pythagorus
        protected float calculateGCost(Node node, Node startNode)
        {
            float x = Math.Abs(startNode.col - node.col);
            float y = Math.Abs(startNode.row - node.row);
            return (float)Math.Sqrt((x * x) + (y * y));
        }
        protected float calculateHCost(Node node,Node endNode)
        {
            float x = Math.Abs(endNode.col - node.col);
            float y = Math.Abs(endNode.row - node.row);
            return (float)Math.Sqrt((x * x) + (y * y));
        }
        protected void retracePath(Node start, Node end)
        {
            Node n = end;
            while(n != start)
            {
                n.setColor(Node.Yellow);
                n = n.parent;
            }
        }
        // doesn't use the Init method because some slightly different stuff has to happen
        public virtual void update(Grid grid, int rows, int cols)
        {
            this.grid.ColumnDefinitions.Clear();
            this.grid.RowDefinitions.Clear();
            this.rows = rows;
            this.cols = cols;
            open = new List<Node>();
            closed = new List<Node>();
            for (int i = 0; i < cols; i++)
                this.grid.ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 0; i < rows; i++)
                this.grid.RowDefinitions.Add(new RowDefinition());

            nodes.Clear();
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
                }
            }
        }
        public virtual void AlgorithmLoop(object sender, EventArgs e) { }               
    }    
  
    public class AStarAlgorithm : PathFindingAlgorithm
    {
        public static bool isStartDefined { get; set; }
        public static bool isEndDefined { get; set; }
        public static bool finished { get; set; }
        public AStarAlgorithm()
        {
            rows = 10;
            cols = 10;
            algoType = Algorithms.A_STAR;
            open = new List<Node>();
            closed = new List<Node>();
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
            algoType = Algorithms.A_STAR;
            open     = new List<Node>();
            closed   = new List<Node>();
            Init();
        }
        public override void update(Grid grid, int rows, int cols)
        {
            base.update(grid, rows, cols);
        }
        private void DefineStartEnd()
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
                {
                    List<Node> neighbors = findNeighbors(currentNode);
                    // neighbors only get added to the open list if they are not already part of the open list or closed list and is not blocked
                    foreach (Node n in neighbors)
                    {
                        if (!closed.Contains(n) && !open.Contains(n) && !n.blocked)
                        {
                            float newGCost = calculateGCost(n, startNode);
                            if (newGCost < n.g)    // only update it's gCost if the new gCost is less than its current gCost
                                n.g = newGCost;
                            n.h = calculateHCost(n, endNode);
                            n.f = n.g + n.h;
                            n.parent = currentNode;
                            n.setColor(Node.Green);
                            open.Add(n);
                        }
                    }
                }
            }
        }       
    }
    public class DijktrasAlgorithm : PathFindingAlgorithm
    {
        public static bool isStartDefined { get; set; }
        public static bool isEndDefined { get; set; }
        public static bool finished { get; set; }
        public DijktrasAlgorithm(Grid grid, int rows, int cols)
        {
            this.grid = grid;
            this.rows = rows;
            this.cols = cols;
            this.grid.Width  = 1600;
            this.grid.Height = 800;
            this.algoType = Algorithms.DIJKSTRAS;
            closed = new List<Node>();
            open = new List<Node>();
            Init();
            foreach (Node n in nodes)
                open.Add(n);
        }
        static DijktrasAlgorithm()
        {
            isEndDefined = false;
            isStartDefined = false;
            finished = true;
        }
        public override void update(Grid grid, int rows, int cols)
        {
            base.update(grid, rows, cols);
            foreach (Node n in nodes)
                open.Add(n);
        }
        /// <summary>
        /// the A Star and the dijkstras versions of this method are a little different and they need access to the static variables
        /// so i just made them as sepatate private methods
        /// </summary>
        private void DefineStartEnd()
        {
            foreach (Node n in this.nodes)
            {
                if (n.isEnd)
                {
                    endNode = n;
                    isEndDefined = true;
                }
                if (n.isStart)
                {
                    if(!isStartDefined)
                        foreach(Node v in open)
                            if(!v.isStart)
                                v.dist = float.MaxValue;                   
                    startNode = n;
                    startNode.dist = 0;
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
                    finished = false;
                }
            }            
            if(open.Count > 0 && !finished)
            {            
                Node lowestDistNode = nodes[0];
                foreach (Node n in open)
                    if (n.dist <= lowestDistNode.dist && !closed.Contains(n))
                        lowestDistNode = n;
                currentNode = lowestDistNode;
                currentNode.setColor(Node.Red);
                open.Remove(currentNode);
                closed.Add(currentNode);
                if (currentNode == endNode)
                {
                    finished = true;                   
                    retracePath(startNode, endNode);
                }
                else
                {                    
                    List<Node> neighbors = findNeighbors(currentNode);                    
                    foreach(Node n in neighbors)
                    {
                        if (n.blocked || closed.Contains(n) || (n.col == 0 && n.row == 0)) // node (0, 0) causes problems for some reason so it doesn't check it
                            continue;
                        n.dist = currentNode.dist + 1.0f;
                        n.setColor(Node.Green);
                        // no need to find an alternative shorter distance becaus they all have distance of 1                        
                        n.parent = currentNode;
                    }
                }
            }
        }        
    }
}
