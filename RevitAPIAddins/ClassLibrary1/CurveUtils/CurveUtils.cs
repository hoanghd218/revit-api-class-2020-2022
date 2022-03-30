using Autodesk.Revit.DB;

namespace Utils.CurveUtils
{
   public static class CurveUtils
   {
      public static XYZ Direction(this Curve curve)
      {
         var sp = curve.SP();
         var ep = curve.EP();
         return (ep - sp).Normalize();
      }

      public static XYZ SP(this Curve curve)
      {
         return curve.GetEndPoint(0);
      }

      public static XYZ EP(this Curve curve)
      {
         return curve.GetEndPoint(1);
      }

   }
}
