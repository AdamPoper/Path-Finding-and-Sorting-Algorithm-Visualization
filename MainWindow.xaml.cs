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
using System.Runtime.InteropServices;
using System.Windows.Threading;

namespace Algorithm_Visualizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PathFindingAlgorithm aStarAlgo;
        public PathFindingAlgorithm dijktrasAlgo;
        public SortingAlgorithm bubbleAlgo;        
        public SortingAlgorithm selectionAlgo;
        public SortingAlgorithm insertionAlgo;        
        public int rows = 25;
        public int cols = 50;
        public int bars = 100;
        public MainWindow()
        {
            InitializeComponent();
            AllocConsole();
            Node.theTabs = tabs;            
            columnBox.Text = cols.ToString();
            rowBox.Text = rows.ToString();
            BarBox.Text = bars.ToString();
            obtainControlValues(ref cols, ref rows);
            aStarAlgo     = new AStarAlgorithm(aStarGrid, rows, cols);
            dijktrasAlgo  = new DijktrasAlgorithm(dijktrasGrid, rows, cols);
            bubbleAlgo    = new BubbleSortAlgorithm(bars, bubblePanel, tabs);                  
            selectionAlgo = new SelectionSortAlgorithm(bars, selectionPanel, tabs);
            insertionAlgo = new InsertionSortAlgorithm(bars, insertionPanel, tabs);
            InitNodes(aStarAlgo);    // idk why but this has to happen but cause of c# I guess idk     
            InitNodes(dijktrasAlgo);
            // add each algorithm application loop to the rendering composition so this will be the "game loop"            
            CompositionTarget.Rendering += aStarAlgo.AlgorithmLoop;
            CompositionTarget.Rendering += dijktrasAlgo.AlgorithmLoop;
            CompositionTarget.Rendering += bubbleAlgo.ApplicationLoop;
            CompositionTarget.Rendering += selectionAlgo.ApplicationLoop;
            CompositionTarget.Rendering += insertionAlgo.ApplicationLoop;
        }               
        public void InitNodes(PathFindingAlgorithm algo)
        {
            for (int i = 0; i < rows * cols; i++)
                algo.nodes[i].setColor(Color.FromArgb(255, 255, 255, 255));
        }
        // enable the console window for debugging purposes
        [DllImport("Kernel32")]
        public static extern void AllocConsole();

        public void updateSortingChanges(object sender, EventArgs e)
        {
            // if the new bar count comes back as different than the current one, 
            // then it gets changed and the algorithms are updated to reflect the changes           
            int newBars = bars;
            obtainControlValues(ref newBars);
            if(bars != newBars)
            {
                bars = newBars;               
                switch(tabs.SelectedIndex)
                {
                    case 2:
                        selectionAlgo.UpdateBarCount(bars);
                        break;
                    case 3:
                        bubbleAlgo.UpdateBarCount(bars);
                        break;
                    case 4:
                        insertionAlgo.UpdateBarCount(bars);
                        break;
                    default:
                        Console.WriteLine("Some Error");
                        break;
                }
            }
        }

        public void updateGridChanges(object sender, EventArgs e)
        {
            // update the grid if the column and row values have changed
            int newCols = cols;
            int newRows = rows;
            obtainControlValues(ref newCols, ref newRows);
            if(cols != newCols || rows != newRows)
            {
                cols = newCols;
                rows = newRows;
                // for A Star
                switch(tabs.SelectedIndex)
                {
                    case 0:
                        aStarAlgo.update(aStarGrid, rows, cols);
                        InitNodes(aStarAlgo);
                        break;
                    case 1:
                        dijktrasAlgo.update(dijktrasGrid, rows, cols);
                        InitNodes(dijktrasAlgo);
                        break;
                    default:
                        Console.WriteLine("Some Error");
                        break;

                }                
            }
        }
        /// <summary>
        /// Methods for getting the new values from the user controls. cols and rows for the pathfinding and number of bars for the sorting algorithms
        /// </summary>
        /// <param></param>
        public void obtainControlValues(ref int bar)
        {
            try
            {               
                bar = Int32.Parse(BarBox.Text);
            } catch(Exception e)
            {
                Console.WriteLine("Invalid Text Entry");
            }
        }
        public void obtainControlValues(ref int col, ref int row)
        {
            try
            {
                col = Int32.Parse(columnBox.Text);
                row = Int32.Parse(rowBox.Text);
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid Text Entry");
            }
        }       
    }
}
