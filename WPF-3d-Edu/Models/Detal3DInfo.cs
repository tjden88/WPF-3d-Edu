namespace WPF_3d_Edu.Models;

public class Detal3DInfo
{
    public int Width { get; set; } = 600;
    public int Height { get; set; } = 720;
    public int Depth { get; set; } = 520;

    public Position Position { get; set; } = new();

    public Detal Detal { get; set; }
}