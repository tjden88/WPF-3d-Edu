using System.Collections.Generic;
using System.Windows;
using WPF_3d_Edu.Models;

namespace WPF_3d_Edu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Area _Area = new Area();

        public MainWindow()
        {
            InitializeComponent();
            _Area.Detals = new List<Detal>()
            {
                DetalsFactory.CreateEmptyDetal(DetalOrientation.Horizontal),
                DetalsFactory.CreateEmptyDetal(DetalOrientation.Vertical),
                DetalsFactory.CreateEmptyDetal(DetalOrientation.Frontal),
            };

            listBox.ItemsSource = Area.DetalInfos;
        }

        public Area Area => _Area;

        public List<Detal> Detals { get; set; } = new List<Detal>()
        {
            DetalsFactory.CreateEmptyDetal(DetalOrientation.Horizontal),
            DetalsFactory.CreateEmptyDetal(DetalOrientation.Vertical),
        };
    }
}
