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
using System.Windows.Threading;

namespace Algorithm_Visualizer
{
    public class SortingAlgorithm
    {
        public Rectangle[] rectangles;
        public StackPanel panel;
        protected TabControl tabs;
        public int count;
        protected bool started;       

        public SortingAlgorithm()
        {            
        }
        public SortingAlgorithm(int count, StackPanel panel)
        {
            this.count = count;
            this.panel = panel;
            started = false;
            Init();
        }
        protected void swap(int index1, int index2)
        {
            double temp = rectangles[index1].Height;
            rectangles[index1].Height = rectangles[index2].Height;
            rectangles[index2].Height = temp;
        }
        public virtual void ApplicationLoop(object sender, EventArgs e)
        {
        }
        protected void Init()
        {
            rectangles = new Rectangle[this.count];
            var random = new Random();
            for (int i = 0; i < this.count; i++)
            {
                rectangles[i] = new Rectangle();
                rectangles[i].Fill = new SolidColorBrush(Node.Blue);
                rectangles[i].Stroke = Brushes.Black;
                rectangles[i].Width = panel.Width / count;
                rectangles[i].Height = random.NextDouble() * panel.Height;
                rectangles[i].VerticalAlignment = VerticalAlignment.Bottom;
                panel.Children.Add(rectangles[i]);
            }
        }
        public void UpdateBarCount(int count)
        {
            this.count = count;
            panel.Children.Clear();
            Init();
        }
    }
    public class BubbleSortAlgorithm : SortingAlgorithm
    {
        private int index;
        public BubbleSortAlgorithm(int count, StackPanel panel, TabControl tb)
        {
            this.count = count;
            this.panel = panel;
            this.tabs = tb;
            this.started = false;
            index = 0;
            Init();
        }
        public override void ApplicationLoop(object sender, EventArgs e)
        {
            // doesn't use nested loops because the application loop is already doing the first loop but the algorithm is still that same
            if (Keyboard.IsKeyDown(Key.Space) && tabs.SelectedIndex == 3)
                started = true;            
            if(started)
            {          
                for(int j = 0; j < count; j++)
                    if (rectangles[index].Height < rectangles[j].Height)
                        swap(index, j);                                   
                index++;
                if (index == count)
                    started = false;               
            }
        }
    }
    public class SelectionSortAlgorithm : SortingAlgorithm
    {
        private int minIndex;
        private int index;
        public SelectionSortAlgorithm(int count, StackPanel panel, TabControl tb)
        {
            this.count = count;
            this.panel = panel;
            this.tabs = tb;
            started = false;
            index = 0;
            Init();
        }
        public override void ApplicationLoop(object sender, EventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Space) && tabs.SelectedIndex == 2)
                started = true;
            if(started)
            {
                minIndex = index;
                for(int j = index + 1; j < count; j++)
                    if(rectangles[j].Height < rectangles[minIndex].Height)
                        minIndex = j;
                swap(index, minIndex);
                index++;
                if (index == count)
                    started = false;
            }
        }
    }
    public class InsertionSortAlgorithm : SortingAlgorithm
    {
        private int index;
        public InsertionSortAlgorithm(int count, StackPanel panel, TabControl tb)
        {
            this.count = count;
            this.panel = panel;
            this.tabs = tb;
            this.index = 1;
            Init();
        }
        public override void ApplicationLoop(object sender, EventArgs e)
        {            
            if (Keyboard.IsKeyDown(Key.Space) && tabs.SelectedIndex == 4)
                started = true;
            if(started)
            {                
                var current = rectangles[index].Height;
                int j = index - 1;
                while(j >= 0 && rectangles[j].Height > current)
                {
                    rectangles[j + 1].Height = rectangles[j].Height;
                    j--;
                }
                index++;
                rectangles[j + 1].Height = current;
                if (index == count)
                    started = false;
            }
        }
    }
}
