using System;
using System.Collections.Generic;

namespace WPF_3d_Edu.Models
{
    // Нулевая точка - внизу сзади слева
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public override string ToString() => $"{X} - {Y} - {Z}";
    }

    public class DetalMargins
    {
        public int? Left { get; set; }
        public int? Right { get; set; }
        public int? Top { get; set; }
        public int? Bottom { get; set; }
        public int? Front { get; set; }
        public int? Back { get; set; }
    }

    public enum DetalOrientation
    {
        Horizontal,
        Vertical,
        Frontal
    }

    public class Detal
    {
        public string Name { get; set; }

        public int? FixedLenght { get; set; }

        public int? FixedWidth { get; set; }

        public Material Material { get; set; } = new();

        public DetalOrientation Orientation { get; set; }

        public DetalMargins Margins { get; set; } = new();

        public Area Area { get; set; }
    }

    public class Material
    {
        public string Name { get; set; } = "ЛДСП 16";
        public int Thickness { get; set; } = 16;
    }

    public class Area
    {
        public int Width { get; set; } = 600;
        public int Height { get; set; } = 720;
        public int Depth { get; set; } = 520;

        public Position Position { get; set; } = new();

        public ICollection<Detal> Detals { get; set; } = new List<Detal>();

        public void PlaceDetal(Detal Detal)
        {
            Detals.Add(Detal);
        }

        public IEnumerable<Detal3DInfo> DetalInfos => GetDetalInfos();

        private IEnumerable<Detal3DInfo> GetDetalInfos()
        {
            foreach (var detal in Detals)
            {
                var info = new Detal3DInfo();

                switch (detal.Orientation)
                {
                    case DetalOrientation.Horizontal:
                        info.Height = detal.Material.Thickness;
                        info.Width = detal.FixedLenght ?? GetSize(Width, detal.Margins.Left, detal.Margins.Right);
                        info.Depth = detal.FixedWidth ?? GetSize(Depth, detal.Margins.Back, detal.Margins.Front);
                        break;
                    case DetalOrientation.Vertical:
                        info.Width = detal.Material.Thickness;
                        info.Height = detal.FixedLenght ?? GetSize(Height, detal.Margins.Bottom, detal.Margins.Top);
                        info.Depth = detal.FixedWidth ?? GetSize(Depth, detal.Margins.Back, detal.Margins.Front);
                        break;
                    case DetalOrientation.Frontal:
                        info.Depth = detal.Material.Thickness;
                        info.Width = detal.FixedLenght ?? GetSize(Width, detal.Margins.Left, detal.Margins.Right);
                        info.Height = detal.FixedWidth ?? GetSize(Height, detal.Margins.Bottom, detal.Margins.Top);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                yield return info;
            }

        }

        private int GetSize(int Bound, int? FirstMargin, int? SecondMargin)
        {
            if (FirstMargin.HasValue)
            {
                Bound -= FirstMargin.Value;
            }

            if (SecondMargin.HasValue)
            {
                Bound -= SecondMargin.Value;
            }
            return Bound;
        }

        private (int w, int h, int d) GetDetalArea(Detal Detal)
        {
            return (Width, Height, Depth);
        }
    }

    public class Detal3DInfo
    {
        public int Width { get; set; } = 600;
        public int Height { get; set; } = 720;
        public int Depth { get; set; } = 520;

        public Position Position { get; set; } = new();
    }

    public static class DetalsFactory
    {
        public static Detal CreateEmptyDetal(DetalOrientation Orientation)
        {
            var detal = new Detal()
            {
                Orientation = Orientation
            };

            switch (Orientation)
            {
                case DetalOrientation.Horizontal:
                    detal.Margins = new()
                    {
                        Left = 0,
                        Right = 0,
                        Front = 0,
                        Back = 0
                    };
                    detal.Name = "Горизонт";
                    break;
                case DetalOrientation.Vertical:
                    detal.Margins = new()
                    {
                        Bottom = 0,
                        Top = 0,
                        Front = 0,
                        Back = 0
                    };
                    detal.Name = "Вертикаль";
                    break;
                case DetalOrientation.Frontal:
                    detal.Margins = new()
                    {
                        Bottom = 0,
                        Top = 0,
                        Left = 0,
                        Right = 0,
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
