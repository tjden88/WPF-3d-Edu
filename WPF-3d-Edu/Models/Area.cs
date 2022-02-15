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
            var info = new Detal3DInfo(); // Подготовить деталь для обработки

            switch (detal.Orientation) // Установить размеры по всей области
            {
                case DetalOrientation.Horizontal:
                    info.Height = detal.Material.Thickness;
                    info.Width = detal.FixedLenght ?? GetSize(AreaSize.Width, detal.Margins.Left, detal.Margins.Right);
                    info.Depth = detal.FixedWidth ?? GetSize(AreaSize.Depth, detal.Margins.Back, detal.Margins.Front);
                    break;
                case DetalOrientation.Vertical:
                    info.Width = detal.Material.Thickness;
                    info.Height = detal.FixedLenght ?? GetSize(AreaSize.Height, detal.Margins.Bottom, detal.Margins.Top);
                    info.Depth = detal.FixedWidth ?? GetSize(AreaSize.Depth, detal.Margins.Back, detal.Margins.Front);
                    break;
                case DetalOrientation.Frontal:
                    info.Depth = detal.Material.Thickness;
                    info.Width = detal.FixedLenght ?? GetSize(AreaSize.Width, detal.Margins.Left, detal.Margins.Right);
                    info.Height = detal.FixedWidth ?? GetSize(AreaSize.Height, detal.Margins.Bottom, detal.Margins.Top);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Установить смещения в соответствии с отступами
            info.Position.X = GetOffset(AreaSize.Width, info.Width, detal.Margins.Left, detal.Margins.Right);
            info.Position.Y = GetOffset(AreaSize.Height, info.Height, detal.Margins.Bottom, detal.Margins.Top);
            info.Position.Z = GetOffset(AreaSize.Depth, info.Depth, detal.Margins.Back, detal.Margins.Front);

            infos.Add(PlaceDetal(info, infos, AreaSize));
        }

        return infos;
    }


    private Detal3DInfo PlaceDetal(Detal3DInfo info, List<Detal3DInfo> previousDetails, AreaSize areaSize)
    {

        if (!previousDetails.Any())
        {
            return info;
        }

        // Устранить коллизии и скорректировать область

        var prevInfo = previousDetails.Last();

        // По ширине

        var prevWidthFrom = prevInfo.Position.X;
        var prevWidthTo = prevWidthFrom + prevInfo.Width;

        var currentWidthFrom = info.Position.X;
        var currentWidthTo = currentWidthFrom + info.Width;

        if (currentWidthFrom < prevWidthTo && currentWidthTo > prevWidthFrom) // Коллизия слева
        {
            info.Position.X += prevInfo.Width;
        }

        if (currentWidthTo > prevWidthFrom && currentWidthFrom < prevWidthTo) // Коллизия справа
        {
            info.Position.X -= prevInfo.Width;
        }


        return PlaceDetal(info, previousDetails.Take(previousDetails.Count - 1).ToList(), areaSize);

    }

    private AreaSize CheckCollision(Detal3DInfo current, Detal3DInfo previous)
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