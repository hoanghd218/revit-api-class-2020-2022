using System;

namespace Utils.DoubleUtils
{
   public static class DoubleUtils
   {
      private static double tolerance = 0.00001;
      public static bool IsEqual(this double number1, double number2)
      {
         // 0.001  --0.0010000001
         var delta = Math.Abs(number1 - number2);
         if (delta < tolerance)
         {
            return true;
         }

         return false;

      }
   }
}
