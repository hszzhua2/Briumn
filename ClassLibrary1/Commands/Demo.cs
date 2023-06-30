using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMBOX.Revit.Tuna.Commands
{

    [Transaction(TransactionMode.Manual)]
    public class Demo : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            Reference reference = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);
            ImportInstance instance = doc.GetElement(reference) as ImportInstance;
            GeometryElement geometryElement = instance.get_Geometry(new Options());
            Transform transform = instance.GetTransform();


            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfCategory(BuiltInCategory.OST_StructuralFraming);

            FamilySymbol familySymbol = (FamilySymbol)collector.FirstElement();
            int marknem = 0;

            foreach (var geometry in geometryElement)
            {

                using (Transaction transaction = new Transaction(doc))
                {
                    transaction.Start("创建梁");
                    if (!familySymbol.IsActive)
                    { familySymbol.Activate(); }
                    var a = (GeometryInstance)geometry;
                    foreach (PolyLine geo in a.SymbolGeometry)
                    {

                        Plane plane = Plane.Create(new Frame(XYZ.Zero, XYZ.BasisX, XYZ.BasisY, XYZ.BasisZ));
                        SketchPlane sketchPlane = SketchPlane.Create(doc, plane);
                        var points = geo.GetCoordinates();
                        List<XYZ> twoDpoints = new List<XYZ>();
                        //System.Windows.MessageBox.Show($"当前多段线有{count}个点");
                        List<double> width = new List<double>();
                        CurveArray curves = new CurveArray();
                        for (int i = 0; i < points.Count - 1; i++)
                        {
                            XYZ pt1 = points[i];
                            XYZ pt2 = points[i + 1];

                            //XYZ pt1=new XYZ(strart1.X, strart1.Y, 0);
                            //XYZ pt2 = new XYZ(strart2.X, strart2.Y, 0);
                            Line line = Line.CreateBound(pt1, pt2);
                            var beam = doc.Create.NewFamilyInstance(line.CreateTransformed(transform), familySymbol, doc.ActiveView.GenLevel, Autodesk.Revit.DB.Structure.StructuralType.Beam);
                            beam.get_Parameter(BuiltInParameter.DOOR_NUMBER).Set(marknem.ToString());
                            StructuralFramingUtils.DisallowJoinAtEnd(beam, 0);
                            StructuralFramingUtils.DisallowJoinAtEnd(beam, 1);
                            width.Add(1);
                            curves.Append(line);
                            twoDpoints.Add(pt1);


                        }
                        width.Add(1);
                        if (points.Count == width.Count)
                        {
                            //NurbSpline nurbSpline = (NurbSpline)NurbSpline.CreateCurve(points, width);
                            HermiteSpline hermiteSpline = (HermiteSpline)HermiteSpline.Create(points, false);
                            HermiteSpline hermiteSpline1 = (HermiteSpline)HermiteSpline.Create(twoDpoints, false);
                            doc.Create.NewFamilyInstance(hermiteSpline.CreateTransformed(transform), familySymbol, (Level)doc.GetElement(new ElementId(13071)), Autodesk.Revit.DB.Structure.StructuralType.Beam);
                        }


                        transaction.Commit();

                    }


                }

            }
            return Result.Succeeded;
        }
    }
}