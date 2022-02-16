namespace WPF_3d_Edu.Models
{
    // Нулевая точка - внизу сзади слева

    public class Detal
    {
        public string Name { get; set; }

        public int? FixedLenght { get; set; }

        public int? FixedWidth { get; set; }

        public Material Material { get; set; } = new();

        public DetalOrientation Orientation { get; set; }

        public DetalMargins Margins { get; init; } = new();

        public Area Area { get; set; }
    }
}
