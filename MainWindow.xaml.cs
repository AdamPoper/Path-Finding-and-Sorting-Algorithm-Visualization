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
        public PathFindingAlgorithm pathAlgo;
        public SortingAlgorithm bubbleAlgo;
        public SortingAlgorithm selectionAlgo;
        public SortingAlgorithm insertionAlgo;
        public int rows = 50;
        public int cols = 100;
        public MainWindow()
        {
            InitializeComponent();
            AllocConsole();            
            pathAlgo = new AStarAlgorithm(aStarGrid, rows, cols);
            bubbleAlgo = new BubbleSortAlgorithm(1000, bubblePanel, tabs);                  
            selectionAlgo = new SelectionSortAlgorithm(1000, selectionPanel, tabs);
            insertionAlgo = new InsertionSortAlgorithm(1000, insertionPanel, tabs);
            InitNodes(pathAlgo);    // idk why but this has to happen but cause of c# I guess idk            
            // add the algorithm application loop to the rendering composition so this will be the "game loop"            
            CompositionTarget.Rendering += pathAlgo.AlgorithmLoop;            
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
        
    }
}
