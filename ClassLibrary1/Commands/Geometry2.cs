using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMBOX.Revit.Tuna.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class Geometry2 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            //计算墙的面积
            Reference reference1 = uiDoc.Selection.PickObject(ObjectType.Element, "选择第一条模型线");
            if (reference1 == null)
                return Result.Cancelled;

            Reference reference2 = uiDoc.Selection.PickObject(ObjectType.Element, "选择第二条模型线");
            if (reference2 == null)
                return Result.Cancelled;

            ModelCurve modelCurve1 = doc.GetElement(reference1) as ModelCurve;
            ModelCurve modelCurve2 = doc.GetElement(reference2) as ModelCurve;
            List<XYZ> points = GetIntersectionPoint(modelCurve1.GeometryCurve, modelCurve2.GeometryCurve);

            using (Transaction trans = new Transaction(doc, "创建立柱"))
            {
                trans.Start();
                FamilySymbol familySymbol = GetFamilySymbol(doc, "305x305x97UC");
                if (!familySymbol.IsActive)
                    familySymbol.Activate();
                foreach (XYZ point in points)
                {
                    FamilyInstance familyInstance = doc.Create.NewFamilyInstance(point, familySymbol, GetLevel(doc, "建模-首层平面图"), StructuralType.NonStructural);


                }
                trans.Commit();
            }

            return Result.Succeeded;
        }

        /// <summary>
        /// 获取两条曲线的交点
        /// </summary>
        /// <param name="curve1"></param>
        /// <param name="curve2"></param>
        /// <returns></returns>
        public static List<XYZ> GetIntersectionPoint(Curve curve1, Curve curve2)
        {
            List<XYZ> intersectionPoints = new List<XYZ>();
            IntersectionResultArray resultArray = new IntersectionResultArray();
            SetComparisonResult result = curve1.Intersect(curve2, out resultArray);
            if (result == SetComparisonResult.Overlap)//相交但是不重合
            {
                foreach (IntersectionResult intersectionResult in resultArray)
                {
                    intersectionPoints.Add(intersectionResult.XYZPoint);
                }
            }
            return intersectionPoints;
        }
        /// <summary>
        /// 获取指定名称的族类型
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="familySymbolName"></param>
        /// <returns></returns>
        public static FamilySymbol GetFamilySymbol(Document doc, string familySymbolName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector = collector.OfClass(typeof(FamilySymbol));

            var query = from element in collector
                        where element.Name == familySymbolName
                        select element;

            List<Element> elements = query.ToList();
            if (elements.Count < 1)
                return null;
            return elements[0] as FamilySymbol;
        }
        /// <summary>
        /// 获取指定名称的标高
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="levelName">标高名称</param>
        /// <returns></returns>
        public static Level GetLevel(Document doc, string levelName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector = collector.OfClass(typeof(Level));

            var query = from element in collector
                        where element.Name == levelName
                        select element;

            List<Element> elements = query.ToList();
            if (elements.Count < 1)
                return null;
            return elements[0] as Level;
        }
    }
}
