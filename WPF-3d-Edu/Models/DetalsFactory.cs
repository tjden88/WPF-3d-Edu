using System;

namespace WPF_3d_Edu.Models;

public static class DetalsFactory
{
    public static Detal CreateEmptyDetal(DetalOrientation Orientation)
    {
        Detal detal;

        switch (Orientation)
        {
            case DetalOrientation.Horizontal:
                detal = new()
                {
                    Margins = new()
                    {
                        Left = 0,
                        Right = 0,
                        Front = 0,
                        Back = 0
                    },
                    Name = "Горизонт"
                };
                break;
            case DetalOrientation.Vertical:
                detal = new()
                {
                    Margins = new()
                    {
                        Bottom = 0,
                        Top = 0,
                        Front = 0,
                        Back = 0
                    },
                    Name = "Вертикаль"
                };
                break;
            case DetalOrientation.Frontal:
                detal = new()
                {
                    Margins = new()
                    {
                        Bottom = 0,
                        Top = 0,
                        Left = 0,
                        Right = 0,
                    },
                    Name = "Фронт"
                };
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(Orientation), Orientation, null);
        }

        detal.Orientation = Orientation;

        return detal;
    }
}