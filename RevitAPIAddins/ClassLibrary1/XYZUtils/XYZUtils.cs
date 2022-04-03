using Autodesk.Revit.DB;
using MoreLinq;
using System.Collections.Generic;
using System.Linq;
using Utils.DoubleUtils;

namespace Utils.XYZUtils
{
   public static class XYZUtils
   {
      public static bool IsParallel(this XYZ vector1, XYZ vector2)
      {

         return vector1.CrossProduct(vector2).GetLength().IsEqual(0);
      }

      public static bool IsPerpendicular(this XYZ vector1, XYZ vector2)
      {

         return vector1.DotProduct(vector2).IsEqual(0);
      }

      public static XYZ GetFarthestPointByDirection(this List<XYZ> points, XYZ vector)
      {
         return points.MaxBy(x => x.DotProduct(vector)).First();
      }

      public static XYZ EditZ(this XYZ point, double z)
      {
         return new XYZ(point.X, point.Y, z);
      }
   }
}
