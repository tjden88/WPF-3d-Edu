using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace WPF_3d_Edu;

internal class Detal
{
    public int Width { get; set; } = 50;
    public int Height { get; set; } = 70;
    public int Thickness { get; set; } = 20;

    public Model3D ToGeometryModel3D()
    {
        // Create a model group
        var modelGroup = new Model3DGroup();

        // Create a mesh builder and add a box to it
        var meshBuilder = new MeshBuilder(false, false);
        meshBuilder.AddBox(new Point3D(0, 0, 1), Width, Height, Thickness);

        // Create a mesh from the builder (and freeze it)
        var mesh = meshBuilder.ToMesh(true);

        // Create some materials
        var material = MaterialHelper.CreateMaterial(Brushes.DimGray, Brushes.CadetBlue);
        // Add 3 models to the group (using the same mesh, that's why we had to freeze it)
        modelGroup.Children.Add(new GeometryModel3D
        {
            Geometry = mesh,
            Material = material
        });

        return modelGroup;

    }

}