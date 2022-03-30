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
   }
}
