using System;
using ClientModels;

namespace ClientModels
{
    public static class LocationHelper
    {
        static public double Distance(Location pointA, Location pointB)
        {
            //d = ((x2 - x1)2 + (y2 - y1)2 + (z2 - z1)2)1/2 
            var range = Math.Sqrt(Math.Pow(pointA.StarPosX - pointB.StarPosX, 2) + Math.Pow(pointA.StarPosY - pointB.StarPosY, 2) + Math.Pow(pointA.StarPosZ - pointB.StarPosZ, 2));
            return range;
        }
    }
}
