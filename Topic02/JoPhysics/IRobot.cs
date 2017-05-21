namespace GDI_framework
{
    public interface IRobot
    {
        double acceleration { get; set; }
        double maxspeed { get; set; }
        double speed { get; set; }
        double straal { get; set; }
        int x { get; set; }
        int y { get; set; }
    }
}