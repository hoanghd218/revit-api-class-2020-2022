using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using Utils.GeometryUtils;
using Utils.XYZUtils;
using Utils.CurveUtils;
using Utils.DoubleUtils;

namespace RevitAPIAddins.Formwork.Model
{
   public class ElementFormworkModel
   {
      public double FormWorkArea { get; set; }
      public Element Element { get; set; }
      public List<Solid> Solids { get; set; }

      //left and right
      public List<PlanarFace> LeftRightFaces { get; set; } = new List<PlanarFace>();

      //up and down
      public List<PlanarFace> OtherSideFaces { get; set; } = new List<PlanarFace>();

      public ElementFormworkModel(Element ele)
      {
         Element = ele;
         Solids = ele.GetAllSolids(true);
         var faces = Solids.Select(solid => solid.Faces).Flatten().Where(x => x is PlanarFace).Cast<PlanarFace>().ToList();

         //transform
         var familyInstance = ele as FamilyInstance;
         if (familyInstance != null)
         {
            var transform = familyInstance.GetTransform();

            //xVector of the family
            var rightVector = transform.OfVector(XYZ.BasisX);
            var upVector = transform.OfVector(XYZ.BasisY);
            foreach (var face in faces.Where(x => x.IsVerticalPlanarFace()))
            {
               if (face.FaceNormal.IsParallel(rightVector))
               {
                  LeftRightFaces.Add(face);
               }
               else
               {
                  OtherSideFaces.Add(face);
               }
            }

         }
      }

      public Solid GetSolidExtend2Sides(PlanarFace planarFace)
      {
         var normal = planarFace.FaceNormal;
         var extendVector = normal.CrossProduct(XYZ.BasisZ).Normalize();
         var edges = planarFace.EdgeLoops;
         var points = new List<XYZ>();
         foreach (EdgeArray edgeArray in edges)
         {
            foreach (Edge edge in edgeArray)
            {
               var curve = edge.AsCurve();
               var p1 = curve.SP();
               var p2 = curve.SP();
               points.Add(p1);
               points.Add(p2);
            }
         
         }

         //find z max z min

         var zMax = points.Max(x => x.Z);
         var zMin = points.Min(x => x.Z);

         //find the farthest point by extend vector

         var farthestPoint = points.GetFarthestPointByDirection(extendVector);
         var closestPoint = points.GetFarthestPointByDirection(-extendVector);

         //find p1 p2 extend
         var a = closestPoint.Add(-extendVector * 25.0.MmToFeet()).EditZ(zMin);
         var b = farthestPoint.Add(extendVector * 25.0.MmToFeet()).EditZ(zMin);
         var c = farthestPoint.Add(extendVector * 25.0.MmToFeet()).EditZ(zMax);
         var d = closestPoint.Add(-extendVector * 25.0.MmToFeet()).EditZ(zMax);

         var profiles = new List<CurveLoop>();
         var cl = new CurveLoop();
         cl.Append(a.CreateLine(b));
         cl.Append(b.CreateLine(c));
         cl.Append(c.CreateLine(d));
         cl.Append(d.CreateLine(a));
         profiles.Add(cl);

         return GeometryCreationUtilities.CreateExtrusionGeometry(profiles, planarFace.FaceNormal, 25.0.MmToFeet());

      }
   }
}
