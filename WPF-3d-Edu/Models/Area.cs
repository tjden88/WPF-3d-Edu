using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Automation;

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
            var info = new Detal3DInfo() { Detal = detal }; // Подготовить деталь для обработки

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

            //// Установить смещения в соответствии с отступами
            //info.Position.X = GetOffset(AreaSize.Width, info.Width, detal.Margins.Left, detal.Margins.Right);
            //info.Position.Y = GetOffset(AreaSize.Height, info.Height, detal.Margins.Bottom, detal.Margins.Top);
            //info.Position.Z = GetOffset(AreaSize.Depth, info.Depth, detal.Margins.Back, detal.Margins.Front);

            infos.Add(PlaceDetal(info, infos));
        }

        return infos;
    }


    private Detal3DInfo PlaceDetal(Detal3DInfo info, List<Detal3DInfo> previousDetails)
    {
        // Устранение коллизий

        // Привязка по низу
        var maxBottomOffset = 0;

        if (info.Detal.Margins.Bottom is { })
        {
            maxBottomOffset = previousDetails
                .Where(p => p.Detal.Margins.Bottom is not null && p.Detal.Orientation != DetalOrientation.Vertical)
                .Select(p => p.Position.Y + p.Height)
                .DefaultIfEmpty()
                .Max();

            // Обрезать
            if(info.Detal.Orientation != DetalOrientation.Horizontal)
                info.Height -= maxBottomOffset;
        }


        // Привязка по верху
        var maxTopOffset = 0;

        if (info.Detal.Margins.Top is { })
        {
            maxTopOffset = previousDetails
                .Where(p => p.Detal.Margins.Top is not null && p.Detal.Orientation != DetalOrientation.Vertical)
                .Select(p => AreaSize.Height - p.Position.Y)
                .DefaultIfEmpty()
                .Max();

            // Обрезать
            if (info.Detal.Orientation != DetalOrientation.Horizontal)
                info.Height -= maxTopOffset;
        }

        var yOffset = maxBottomOffset + maxTopOffset;



        // Привязка по левому краю
        var maxLeftOffset = 0;

        if (info.Detal.Margins.Left is { })
        {
            maxLeftOffset = previousDetails
                .Where(p => p.Detal.Margins.Left is not null && p.Detal.Orientation != DetalOrientation.Horizontal)
                .Select(p => p.Position.X + p.Width)
                .DefaultIfEmpty()
                .Max();

            // Обрезать
            if (info.Detal.Orientation != DetalOrientation.Vertical)
                info.Width -= maxLeftOffset;
        }


        // Привязка по правому краю
        var maxRightOffset = 0;

        if (info.Detal.Margins.Right is { })
        {
            maxRightOffset = previousDetails
                .Where(p => p.Detal.Margins.Right is not null && p.Detal.Orientation != DetalOrientation.Horizontal)
                .Select(p => AreaSize.Width - p.Position.X)
                .DefaultIfEmpty()
                .Max();

            // Обрезать
            if (info.Detal.Orientation != DetalOrientation.Vertical)
                info.Width -= maxRightOffset;
        }

        var xOffset = maxLeftOffset + maxRightOffset;


        // Привязка по заду
        var maxBackOffset = 0;

        if (info.Detal.Margins.Back is { })
        {
            maxBackOffset = previousDetails
                .Where(p => p.Detal.Margins.Back is not null && p.Detal.Orientation == DetalOrientation.Frontal)
                .Select(p => p.Position.Z + p.Depth)
                .DefaultIfEmpty()
                .Max();

            // Обрезать
            if (info.Detal.Orientation != DetalOrientation.Frontal)
                info.Depth -= maxBackOffset;
        }


        // Привязка по переду
        var maxFrontOffset = 0;

        if (info.Detal.Margins.Front is { })
        {
            maxFrontOffset = previousDetails
                .Where(p => p.Detal.Margins.Front is not null && p.Detal.Orientation == DetalOrientation.Frontal)
                .Select(p => AreaSize.Depth - p.Position.Z)
                .DefaultIfEmpty()
                .Max();

            // Обрезать
            if (info.Detal.Orientation != DetalOrientation.Frontal)
                info.Depth -= maxFrontOffset;
        }

        var zOffset = maxBackOffset + maxFrontOffset;

        //// Установить смещения в соответствии с отступами
        info.Position.X = GetOffset(AreaSize.Width - xOffset, info.Width, info.Detal.Margins.Left, info.Detal.Margins.Right) + maxLeftOffset;
        info.Position.Y = GetOffset(AreaSize.Height - yOffset, info.Height, info.Detal.Margins.Bottom, info.Detal.Margins.Top) + maxBottomOffset;
        info.Position.Z = GetOffset(AreaSize.Depth - zOffset, info.Depth, info.Detal.Margins.Back, info.Detal.Margins.Front) + maxBackOffset;

        return info;
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
        if (FirstMargin is null && SecondMargin is null)
            return 0;

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