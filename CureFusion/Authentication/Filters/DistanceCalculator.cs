namespace CureFusion.Authentication.Filters;

public static class DistanceCalculator
{
    public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371e3; // Radius of the Earth in meters
        var phi1 = lat1 * Math.PI / 180; // Convert latitude to radians
        var phi2 = lat2 * Math.PI / 180; // Convert latitude to radians
        var deltaPhi = (lat2 - lat1) * Math.PI / 180; // Difference in latitude in radians
        var deltaLambda = (lon2 - lon1) * Math.PI / 180; // Difference in longitude in radians
        var a = Math.Sin(deltaPhi / 2) * Math.Sin(deltaPhi / 2) +
                Math.Cos(phi1) * Math.Cos(phi2) *
                Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c/1000; // Distance in kilometers
    }
}
