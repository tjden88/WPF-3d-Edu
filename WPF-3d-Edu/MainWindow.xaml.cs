using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using WPF_3d_Edu.Models;

namespace WPF_3d_Edu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Area.Detals = new List<Detal>()
            {
                DetalsFactory.CreateEmptyDetal(DetalOrientation.Horizontal),
                DetalsFactory.CreateEmptyDetal(DetalOrientation.Vertical),
                DetalsFactory.CreateEmptyDetal(DetalOrientation.Frontal),
            };

            listBox.ItemsSource = Area.DetalInfos;
        }

        public Area Area { get; } = new Area();

        public List<Detal> Detals { get; set; } = new List<Detal>()
        {
            DetalsFactory.CreateEmptyDetal(DetalOrientation.Horizontal),
            DetalsFactory.CreateEmptyDetal(DetalOrientation.Vertical),
        };

        private void AddDetal_Click(object Sender, RoutedEventArgs E)
        {
            var sw = Stopwatch.StartNew();
            var detal = DetalsFactory.CreateEmptyDetal((DetalOrientation)ComboBoxOrientation.SelectedIndex);
            detal.Margins = new()
            {
                Bottom = ParseOffset(bottomOffset.Text),
                Top = ParseOffset(topOffset.Text),
                Left = ParseOffset(leftOffset.Text),
                Right = ParseOffset(rightOffset.Text),
                Back = ParseOffset(backOffset.Text),
                Front = ParseOffset(frontOffset.Text),
            };

            Area.Detals.Add(detal);
            listBox.ItemsSource = Area.DetalInfos;
            TimerText.Text = sw.ElapsedMilliseconds.ToString();
        }

        private int? ParseOffset(string? value) => int.TryParse(value, out var result) ? result : null;

        private void ClearDetal_Click(object Sender, RoutedEventArgs E)
        {
            Area.Detals.Clear();
            listBox.ItemsSource = Area.DetalInfos;
        }
    }
}
