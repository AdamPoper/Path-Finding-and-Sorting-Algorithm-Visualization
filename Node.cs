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
    public class Node
    {
        public int row { get; set; }
        public int col { get; set; }
        public Rectangle rect;
        public static Color Blue { get; }
        public static Color Red { get; }
        public static Color Black { get; }
        public static Color Yellow { get; }
        public static Color Green { get; }
        public static Color Purple { get; }
        public static Color White { get; }
        public bool blocked { get; set; }
        public bool isStart { get; set; }
        public bool isEnd { get; set; }
        public float g { get; set; }  /// <summary>
        public float h { get; set; }  ///  These values are for the a star algorithm    
        public float f { get; set; }  /// </summary>
        public float dist { get; set; }  // for dijkstras
        public static PathFindingAlgorithm.Algorithms activeAlgorithm;
        public Node parent;
        public Node()
        {
            this.rect = new Rectangle();
            this.blocked = false;
            this.rect.MouseLeftButtonDown += new MouseButtonEventHandler(this.OnClick);

        }
        static Node()
        {
            Blue = Color.FromArgb(255, 0, 0, 255);
            Red = Color.FromArgb(255, 255, 0, 0);
            Black = Color.FromArgb(255, 0, 0, 0);
            Green = Color.FromArgb(255, 0, 255, 0);
            Yellow = Color.FromArgb(255, 255, 255, 0);
            Purple = Color.FromArgb(255, 255, 0, 255);
            White = Color.FromArgb(255, 255, 255, 255);
        }
        public Node(int x, int y)
        {
            this.rect = new Rectangle();
            this.blocked = false;
            this.col = x;
            this.row = y;
            this.rect.MouseLeftButtonDown += new MouseButtonEventHandler(this.OnClick);
            this.rect.MouseEnter += new MouseEventHandler(this.MouseOver);
            this.isEnd = false;
            this.isStart = false;
            this.f = 0.0f;
            this.g = 0.0f;
            this.h = 0.0f;
        }
        public void setColor(Color c)
        {
            this.rect.Fill = new SolidColorBrush(c);
        }
        public void OnClick(object sender, MouseButtonEventArgs e)
        {
            MouseClickHandle();
        }  
        public void MouseOver(object sender, MouseEventArgs e)
        {
            MouseClickHandle();
        }
        public void MouseClickHandle()
        {
            // there's lots of inheritence going on here and mouse and keyboard inputs must be handled by the Node class
            // this function needs to filter out which algorithm is being performed so the events are handled properly

            // For A Star
            if (Mouse.LeftButton == MouseButtonState.Pressed) 
            {
                if (Keyboard.IsKeyDown(Key.S) && !AStarAlgorithm.isStartDefined)
                {
                    setColor(Node.Blue);
                    this.isStart = true;
                    Console.WriteLine("Setting the start node");
                }
                else if (Keyboard.IsKeyDown(Key.E) && !AStarAlgorithm.isEndDefined)
                {
                    setColor(Node.Purple);
                    this.isEnd = true;                   
                    Console.WriteLine("Setting the end node");                   
                }
                else
                {
                    setColor(Node.Black);
                    this.blocked = true;
                }               
            }
            // For dijkstras
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                if (Keyboard.IsKeyDown(Key.S) && !DijktrasAlgorithm.isStartDefined)
                {
                    setColor(Node.Blue);
                    this.isStart = true;
                    Console.WriteLine("Setting the start node");
                }
                else if (Keyboard.IsKeyDown(Key.E) && !DijktrasAlgorithm.isEndDefined)
                {
                    setColor(Node.Purple);
                    this.isEnd = true;
                    Console.WriteLine("Setting the end node");
                }
                else
                {
                    setColor(Node.Black);
                    this.blocked = true;
                }
            }
        }       
    }
}
