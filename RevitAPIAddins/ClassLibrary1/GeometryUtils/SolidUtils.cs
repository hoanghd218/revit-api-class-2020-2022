using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace Utils.GeometryUtils
{
   public static class GeometryUtils
   {
      public static List<Solid> GetAllSolids(this Element instance, bool transformedSolid = false, View view = null)
      {
         List<Solid> solidList = new List<Solid>();
         if (instance == null)
            return solidList;
         GeometryElement geometryElement = instance.get_Geometry(new Options()
         {
            ComputeReferences = true
         });

         foreach (GeometryObject geometryObject1 in geometryElement)
         {
            GeometryInstance geometryInstance = geometryObject1 as GeometryInstance;
            if (null != geometryInstance)
            {
               var tf = geometryInstance.Transform;
               foreach (GeometryObject geometryObject2 in geometryInstance.GetSymbolGeometry())
               {
                  Solid solid = geometryObject2 as Solid;
                  if (!(null == solid) && solid.Faces.Size != 0 && solid.Edges.Size != 0)
                  {
                     if (transformedSolid)
                     {
                        solidList.Add(SolidUtils.CreateTransformed(solid, tf));
                     }
                     else
                     {
                        solidList.Add(solid);
                     }

                  }
               }
            }
            Solid solid1 = geometryObject1 as Solid;
            if (!(null == solid1) && solid1.Faces.Size != 0)
               solidList.Add(solid1);
         }
         return solidList;
      }
   }
}
