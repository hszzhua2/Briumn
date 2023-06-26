using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BIMBOX.Revit.Tuna.Views;

namespace BIMBOX.Revit.Tuna.Commands
{
    [Transaction(TransactionMode.Manual)]
    class Face2Face : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiDoc = commandData.Application.ActiveUIDocument;
            var doc = uiDoc.Document;
            var faceReference = uiDoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Face, "选取面");
            var wallofFace = doc.GetElement(faceReference) as Wall;
            var face = wallofFace.GetGeometryObjectFromReference(faceReference) as Face;

            var wallTypes = from element in new FilteredElementCollector(doc).
                            OfClass(typeof(WallType))
                            let type = element as WallType
                            select type;
            var faceConfigWin = new FaceConfig(wallTypes.ToList());
            var result = faceConfigWin.ShowDialog();
            if (result.HasValue && result.Value)
            {
                var tran = new Transaction(doc, "创建面");
                tran.Start();
                CreateFace(doc, face, wallofFace, faceConfigWin.SelectedWallType);
                tran.Commit();
                return Result.Succeeded;
            }
            return Result.Cancelled;
        }
        private void CreateFace(Document doc, Face? face, Wall wallofFace, WallType selectedWallType)
        {
            var profile = new List<Curve>();
            var openngArrays = new List<CurveArray>();
            var width = selectedWallType.Width;
            ExtractFaceOutline(face, width, ref profile, ref openngArrays);
            var wall = Wall.Create(doc, profile, selectedWallType.Id, wallofFace.LevelId, false);
            wall.get_Parameter(BuiltInParameter.WALL_BASE_OFFSET).Set(0);
            foreach (var item in openngArrays)
            {
                doc.Create.NewOpening(wall, item.get_Item(0).GetEndPoint(0),
                    item.get_Item(1).GetEndPoint(1));
            }
        }
        private void ExtractFaceOutline(Face? face, double width, ref List<Curve> profile, ref List<CurveArray> openngArrays)
        {
            var curveLoops = face.GetEdgesAsCurveLoops();
            var normal = (face as PlanarFace)?.FaceNormal;
            if (normal == null) throw new ArgumentException("面生面的功能暂时不支持曲面");
            var translation = Transform.CreateTranslation(normal *  width/2);
            int i = 0;
            foreach ( var curveLoop in curveLoops.OrderByDescending(x=> x.GetExactLength()))

            {
                curveLoop.Transform(translation);
                var array = new
                    CurveArray();   
                foreach (var curve in curveLoop)
                {
                    if (i == 0)
                        profile.Add(curve);
                    else array.Append(curve);
                }
                if (i != 0) openngArrays.Add(array);
                i++;
            }
        }
    }
}
