using System.Collections.Generic;
using Autodesk.Revit.DB;
using Utils.XYZUtils;

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


      public static bool IsVerticalPlanarFace(this PlanarFace planarFace)
      {
         var normal = planarFace.FaceNormal;
         return normal.IsPerpendicular(XYZ.BasisZ);
      }


      public static DirectShape CreateDirectShape(this Solid solid)
      {
         var geo = new List<GeometryObject>() { solid};
       
         var directShape = DirectShape.CreateElement(ActiveDocument.ActiveDocument.Document, new ElementId(BuiltInCategory.OST_GenericModel));
         directShape.SetShape(geo);

         return directShape;
      }

      public  static Solid CreateOriginalSolidFromPlanarFace(this PlanarFace planarFace, double thickness = 0.082020997375)
      {
         var curveLoops = planarFace.GetEdgesAsCurveLoops();
         var solid = GeometryCreationUtilities.CreateExtrusionGeometry(curveLoops, planarFace.FaceNormal, thickness);

         return solid;
      }
   }
}
