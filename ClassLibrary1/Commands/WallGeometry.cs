using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.Formula.PTG;

namespace BIMBOX.Revit.Tuna.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class WallGeometry : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            //计算墙的面积
            Reference reference = uiDoc.Selection.PickObject(ObjectType.Element, "选择墙");
            if (reference == null)
                return Result.Cancelled;

            Wall wall = doc.GetElement(reference) as Wall;
            if (wall == null)
                return Result.Failed;

            double area = GetArea(wall);
            double volumn = GetVolumn(wall);
            int triangleCount = GetTriangleCount(wall, 0.5);
            TaskDialog.Show("revit", $"{wall.Name}墙面积为：{area}平方米，体积为{volumn}立方米");
            return Result.Succeeded;
        }
        /// <summary>
        /// 获取几何元素的面积
        /// </summary>
        /// <param name="element"></param>
        /// <returns>平方米</returns>
        public static double GetArea(Element element)
        {
            double area = 0;
            GeometryElement geometryElement = element.get_Geometry(new Options());
            foreach (GeometryObject geometryObject in geometryElement)
            {
                if (geometryObject is Solid)
                {
                    Solid solid = geometryObject as Solid;
                    foreach (Face face in solid.Faces)
                    {
                        area += face.Area;
                    }
                }
            }
            area = UnitUtils.Convert(area, UnitTypeId.Feet, UnitTypeId.Meters);
            return area;
        }

        /// <summary>
        /// 获取几何元素的体积
        /// </summary>
        /// <param name="element"></param>
        /// <returns>立方米</returns>
        public static double GetVolumn(Element element)
        {
            double volumn = 0;
            GeometryElement geometryElement = element.get_Geometry(new Options());
            foreach (GeometryObject geometryObject in geometryElement)
            {
                if (geometryObject is Solid)
                {
                    Solid solid = geometryObject as Solid;
                    volumn += solid.Volume;
                }
            }
            volumn = UnitUtils.Convert(volumn, UnitTypeId.Feet, UnitTypeId.Meters);
            return volumn;
        }

        /// <summary>
        /// 获取几何元素三角网格数量
        /// </summary>
        /// <param name="element"></param>
        /// <param name="levelOfDetail"></param>
        /// <returns></returns>
        public static int GetTriangleCount(Element element, double levelOfDetail)
        {
            int triangleCount = 0;
            GeometryElement geometryElement = element.get_Geometry(new Options());
            foreach (GeometryObject geometryObject in geometryElement)
            {
                if (geometryObject is Solid)
                {
                    Solid solid = geometryObject as Solid;
                    foreach (Face face in solid.Faces)
                    {
                        Mesh mesh = face.Triangulate(levelOfDetail);
                        triangleCount += mesh.NumTriangles;
                    }
                }
            }
            return triangleCount;
        }
    }

}
