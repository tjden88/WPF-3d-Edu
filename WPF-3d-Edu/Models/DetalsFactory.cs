using System;

namespace WPF_3d_Edu.Models;

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