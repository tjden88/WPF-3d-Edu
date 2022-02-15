using System;
using System.Windows.Media.Media3D;

namespace WPF_3d_Edu.Models
{
    internal class DetalAnchors
    {
        public bool Left { get; set; }
        public bool Right { get; set; }
        public bool Top { get; set; }
        public bool Bottom { get; set; }
        public bool Front { get; set; }
        public bool Back { get; set; }
    }

    internal enum DetalOrientation
    {
        Horizontal,
        Vertical,
        Frontal
    }

    internal class Detal
    {
        public string Name { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public int Thickness => Material.Thickness;

        public Material Material { get; set; } = new();

        public DetalOrientation Orientation { get; set; }

        public DetalAnchors Anchors { get; set; } = new();
    }

    internal class Material
    {
        public string Name { get; set; } = "ЛДСП 16";
        public int Thickness { get; set; } = 16;
    }

    internal class Area
    {
        public int Width { get; set; } = 600;
        public int Height { get; set; } = 720;
        public int Depth { get; set; } = 520;

        public Point3D Position { get; set; }
    }

    internal static class DetalsFactory
    {
        public static Detal EmptyDetal(DetalOrientation Orientation, Area Area)
        {
            var detal = new Detal();

            switch (Orientation)
            {
                case DetalOrientation.Horizontal:
                    detal.Anchors = new()
                    {
                        Left = true,
                        Right = true,
                        Front = true,
                        Back = true
                    };
                    detal.Name = "Горизонт";
                    break;
                case DetalOrientation.Vertical:
                    detal.Anchors = new()
                    {
                        Bottom = true,
                        Top = true,
                        Front = true,
                        Back = true
                    };
                    detal.Name = "Вертикаль";
                    break;
                case DetalOrientation.Frontal:
                    detal.Anchors = new()
                    {
                        Bottom = true,
                        Top = true,
                        Left = true,
                        Right = true,
                    };
                    detal.Name = "Фронт";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Orientation), Orientation, null);
            }


            return detal;
        }
    }
}
