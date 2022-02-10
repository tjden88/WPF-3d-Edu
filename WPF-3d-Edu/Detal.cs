using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace WPF_3d_Edu
{
    internal class Detal
    {
        public int Width { get; set; } = 50;
        public int Height { get; set; } = 70;
        public int Thickness { get; set; } = 20;

        private Point3DCollection GetAnglePoints => new()
        {
            new Point3D(0, 0, 0),
            new Point3D(Width, 0, 0),
            new Point3D(Width, Height, 0),
            new Point3D(0, Height, 0),

            new Point3D(0, 0, Thickness),
            new Point3D(Width, 0, Thickness),
            new Point3D(Width, Height, Thickness),
            new Point3D(0, Height, Thickness),

        };

        public MeshGeometry3D BuildMesh()
        {
            var angles = GetAnglePoints;
            MeshGeometry3D mesh = new MeshGeometry3D
            {

                Positions = new()
                {
                    angles[0],
                    angles[1],
                    angles[2],
                    angles[3],

                    angles[4],
                    angles[5],
                    angles[6],
                    angles[7],

                    angles[0],
                    angles[1],
                    angles[5],
                    angles[4],

                    angles[3],
                    angles[2],
                    angles[6],
                    angles[7],

                    angles[0],
                    angles[3],
                    angles[7],
                    angles[4],

                    angles[1],
                    angles[5],
                    angles[6],
                    angles[2]
                }
            };

            mesh.TriangleIndices = new()
            {
                0,3,1,
                1,3,2,

                4,5,6,
                6,7,4,

                8,9,11,
                11,9,10,

                12,15,13,
                13,15,14,

                16,19,17,
                17,19,18,

                20,23,21,
                21,23,22

            };

            return mesh;
        }

        public GeometryModel3D ToGeometryModel3D()
        {
            GeometryModel3D model = new GeometryModel3D
            {
                Geometry = BuildMesh(),
                Material = new DiffuseMaterial(Brushes.CadetBlue),
                BackMaterial = new DiffuseMaterial(Brushes.MidnightBlue)
            };

            return model;
        }
    }
}
