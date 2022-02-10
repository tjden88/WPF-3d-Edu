using System.Windows;
using System.Windows.Media.Media3D;

namespace WPF_3d_Edu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Detal Detal = new();
        public MainWindow()
        {
            InitializeComponent();
        }

        public Model3D Model => Detal.ToGeometryModel3D();
    }
}
