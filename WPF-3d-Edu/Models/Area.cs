using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WPF_3d_Edu.Models;

public class AreaSize
{
    public int Width { get; set; } = 600;
    public int Height { get; set; } = 720;
    public int Depth { get; set; } = 520;

}

public class Area
{
    public AreaSize AreaSize { get; set; } = new();

    public Position Position { get; set; } = new();

    public ObservableCollection<Detal> Detals { get; set; } = new();


    public IEnumerable<Detal3DInfo> DetalInfos => GetDetalInfos();

    private IEnumerable<Detal3DInfo> GetDetalInfos()
    {
        var infos = new List<Detal3DInfo>(); // Список обработанных деталей
        foreach (var detal in Detals)
        {
            infos.Add(PlaceDetal(detal, infos, AreaSize));
        }

        return infos;
    }


    private Detal3DInfo PlaceDetal(Detal detal, List<Detal3DInfo> previousDetails, AreaSize areaSize)
    {

        if (!previousDetails.Any())
        {
            var info = new Detal3DInfo();

            switch (detal.Orientation)
            {
                case DetalOrientation.Horizontal:
                    info.Height = detal.Material.Thickness;
                    info.Width = detal.FixedLenght ?? GetSize(areaSize.Width, detal.Margins.Left, detal.Margins.Right);
                    info.Depth = detal.FixedWidth ?? GetSize(areaSize.Depth, detal.Margins.Back, detal.Margins.Front);
                    break;
                case DetalOrientation.Vertical:
                    info.Width = detal.Material.Thickness;
                    info.Height = detal.FixedLenght ?? GetSize(areaSize.Height, detal.Margins.Bottom, detal.Margins.Top);
                    info.Depth = detal.FixedWidth ?? GetSize(areaSize.Depth, detal.Margins.Back, detal.Margins.Front);
                    break;
                case DetalOrientation.Frontal:
                    info.Depth = detal.Material.Thickness;
                    info.Width = detal.FixedLenght ?? GetSize(areaSize.Width, detal.Margins.Left, detal.Margins.Right);
                    info.Height = detal.FixedWidth ?? GetSize(areaSize.Height, detal.Margins.Bottom, detal.Margins.Top);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            info.Position.X = GetOffset(areaSize.Width, info.Width, detal.Margins.Left, detal.Margins.Right);
            info.Position.Y = GetOffset(areaSize.Height, info.Height, detal.Margins.Bottom, detal.Margins.Top);
            info.Position.Z = GetOffset(areaSize.Depth, info.Depth, detal.Margins.Back, detal.Margins.Front);

            return info;
        }

        var newAreaSize = CheckCollision(detal, previousDetails.Last());

        return PlaceDetal(detal, previousDetails.Take(previousDetails.Count - 1).ToList(), newAreaSize);

    }

    private AreaSize CheckCollision(Detal detal, Detal3DInfo info)
    {
        return new AreaSize();
    }


    private int GetOffset(int Bound, int DetalSize, int? FirstMargin, int? SecondMargin) // Получить смещение детали отностиельно начала координат
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

    private int GetSize(int Bound, int? FirstMargin, int? SecondMargin) // Получить размер детали
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

}