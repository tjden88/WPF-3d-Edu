using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
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


            var h1 = DetalsFactory.CreateEmptyDetal(DetalOrientation.Horizontal);
            h1.Name = "Дно";
            h1.Margins.Bottom = 0;


            var v1 = DetalsFactory.CreateEmptyDetal(DetalOrientation.Vertical);
            v1.Name = "Бок левый";
            v1.Margins.Left = 0;

            var v2 = DetalsFactory.CreateEmptyDetal(DetalOrientation.Vertical);
            v2.Name = "Бок правый";
            v2.Margins.Right = 0;

            var p1 = DetalsFactory.CreateEmptyDetal(DetalOrientation.Horizontal);
            p1.Name = "Планка фронт";
            p1.FixedWidth = 100;
            p1.Margins.Front = 0;
            p1.Margins.Back = null;
            p1.Margins.Top = 0;

            var p2 = DetalsFactory.CreateEmptyDetal(DetalOrientation.Horizontal);
            p2.Name = "Планка зад";
            p2.FixedWidth = 100;
            p2.Margins.Back = 0;
            p2.Margins.Front = null;
            p2.Margins.Top = 0;

            Area.Detals = new()
            {
                h1,
                v1,
                v2,
                p1,
                p2
            };

            listBox.ItemsSource = Area.Detals;
            Create3D();
        }

        public Area Area { get; } = new Area();

        private void AddDetal_Click(object Sender, RoutedEventArgs E)
        {
            var detal = new Detal()
            {
                Margins = new()
                {
                    Bottom = ParseOffset(bottomOffset.Text),
                    Top = ParseOffset(topOffset.Text),
                    Left = ParseOffset(leftOffset.Text),
                    Right = ParseOffset(rightOffset.Text),
                    Back = ParseOffset(backOffset.Text),
                    Front = ParseOffset(frontOffset.Text),

                },
                Orientation = (DetalOrientation) ComboBoxOrientation.SelectedIndex
            };

            Area.Detals.Add(detal);
            //listBox.ItemsSource = Area.Detals;
            Create3D();
        }

        private int? ParseOffset(string? value) => int.TryParse(value, out var result) ? result : null;

        private void ClearDetal_Click(object Sender, RoutedEventArgs E)
        {
            Area.Detals.Clear();
            //listBox.ItemsSource = Area.Detals;
            Create3D();
        }

        public void Create3D()
        {
            var sw = Stopwatch.StartNew();
            // Create a model group
            var modelGroup = new Model3DGroup();

            // Create some materials
            var greenMaterial = MaterialHelper.CreateMaterial(Colors.Green);
            var redMaterial = MaterialHelper.CreateMaterial(Colors.Red);
            var blueMaterial = MaterialHelper.CreateMaterial(Colors.Blue);
            var insideMaterial = MaterialHelper.CreateMaterial(Colors.Yellow);


            // Create a mesh builder and add a box to it

            var i = 0;

            foreach (var detalInfo in Area.DetalInfos)
            {
                var meshBuilder = new MeshBuilder(false, false);
                var locationPoint = new Point3D(detalInfo.Position.Z, detalInfo.Position.X, detalInfo.Position.Y);
                var size = new Size3D(detalInfo.Depth, detalInfo.Width, detalInfo.Height);

                var rect = new Rect3D(locationPoint, size);

                meshBuilder.AddBox(rect);

                // Create a mesh from the builder (and freeze it)
                var mesh = meshBuilder.ToMesh(true);

                var material = i == listBox.SelectedIndex ? greenMaterial : redMaterial;
                // Add 3 models to the group (using the same mesh, that's why we had to freeze it)
                modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh, Material = material, BackMaterial = insideMaterial });

                i++;
            }


            //modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh, Transform = new TranslateTransform3D(-2, 0, 0), Material = redMaterial, BackMaterial = insideMaterial });
            //modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh, Transform = new TranslateTransform3D(2, 0, 0), Material = blueMaterial, BackMaterial = insideMaterial });

            // Set the property, which will be bound to the Content property of the ModelVisual3D (see MainWindow.xaml)
            Model = modelGroup;
            OnPropertyChanged(nameof(Model));
            TimerText.Text = sw.ElapsedMilliseconds.ToString();
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

        private void DeleteDetal_Click(object Sender, RoutedEventArgs E)
        {
            Area.Detals.Remove((((Button) Sender).Tag as Detal)!);
        }
    }
}
