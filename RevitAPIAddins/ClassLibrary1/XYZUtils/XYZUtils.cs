using Autodesk.Revit.DB;
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
   }
}
