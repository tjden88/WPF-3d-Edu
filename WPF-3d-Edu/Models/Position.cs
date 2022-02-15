namespace WPF_3d_Edu.Models;

public class Position
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }

    public override string ToString() => $"X={X} Y={Y} Z={Z}";
}