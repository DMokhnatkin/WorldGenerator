
namespace Map.MapModels.Points
{
    public interface IMapPoint
    {
        float Height { get; set; }

        float HeightAfterWaterErosion { get; set; }

        bool IsGenerated { get; }

        MapPointNatureConfig NatureConf { get; }
    }
}
