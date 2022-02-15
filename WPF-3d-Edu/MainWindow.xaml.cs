using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using WPF_3d_Edu.Annotations;
using WPF_3d_Edu.Models;

namespace WPF_3d_Edu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
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
            Create3D();
            TimerText.Text = sw.ElapsedMilliseconds.ToString();
        }

        private int? ParseOffset(string? value) => int.TryParse(value, out var result) ? result : null;

        private void ClearDetal_Click(object Sender, RoutedEventArgs E)
        {
            Area.Detals.Clear();
            listBox.ItemsSource = Area.DetalInfos;
            Create3D();
        }

        public void Create3D()
        {
            // Create a model group
            var modelGroup = new Model3DGroup();

            // Create a mesh builder and add a box to it
            var meshBuilder = new MeshBuilder(false, false);

            foreach (var detalInfo in Area.DetalInfos)
            {
                var locationPoint = new Point3D(detalInfo.Position.Z, detalInfo.Position.X, detalInfo.Position.Y);
                var size = new Size3D(detalInfo.Depth, detalInfo.Width, detalInfo.Height);

                var rect = new Rect3D(locationPoint, size);

                meshBuilder.AddBox(rect);

            }


            // Create a mesh from the builder (and freeze it)
            var mesh = meshBuilder.ToMesh(true);

            // Create some materials
            var greenMaterial = MaterialHelper.CreateMaterial(Colors.Green);
            var redMaterial = MaterialHelper.CreateMaterial(Colors.Red);
            var blueMaterial = MaterialHelper.CreateMaterial(Colors.Blue);
            var insideMaterial = MaterialHelper.CreateMaterial(Colors.Yellow);

            // Add 3 models to the group (using the same mesh, that's why we had to freeze it)
            modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh, Material = greenMaterial, BackMaterial = insideMaterial });
            //modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh, Transform = new TranslateTransform3D(-2, 0, 0), Material = redMaterial, BackMaterial = insideMaterial });
            //modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh, Transform = new TranslateTransform3D(2, 0, 0), Material = blueMaterial, BackMaterial = insideMaterial });

            // Set the property, which will be bound to the Content property of the ModelVisual3D (see MainWindow.xaml)
            Model = modelGroup;
            OnPropertyChanged(nameof(Model));
        }

        public Model3D Model { get; set; }

        private void Create3d_Click(object Sender, RoutedEventArgs E)
        {
            Create3D();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
