using System.Windows;

namespace WMP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {  this.DataContext = new ViewModel();
            InitializeComponent();   
        }
    }
}