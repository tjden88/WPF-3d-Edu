using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WPF_3d_Edu.Models;

public class Area
{
    public int Width { get; set; } = 600;
    public int Height { get; set; } = 720;
    public int Depth { get; set; } = 520;

    public Position Position { get; set; } = new();

    public ObservableCollection<Detal> Detals { get; set; } = new ();

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

            info.Position.X = GetOffset(Width, info.Width, detal.Margins.Left, detal.Margins.Right);
            info.Position.Y = GetOffset(Height, info.Height, detal.Margins.Bottom, detal.Margins.Top);
            info.Position.Z = GetOffset(Depth, info.Depth, detal.Margins.Back, detal.Margins.Front);

            yield return info;
        }

    }

    private int GetOffset(int Bound, int DetalSize, int? FirstMargin, int? SecondMargin)
    {
        if (FirstMargin.HasValue)
        {
            return FirstMargin.Value;
        }

        if (SecondMargin.HasValue)
        {
            return Bound - DetalSize - SecondMargin.Value;
        }

        return (Bound - DetalSize) / 2;
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