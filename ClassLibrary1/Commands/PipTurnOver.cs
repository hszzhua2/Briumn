using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.Attributes;
using System.Windows.Forms;



[Transaction(TransactionMode.Manual)]
    public class PipTurnOver : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            try
            {
                UIApplication uiapp = commandData.Application;
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uiapp.ActiveUIDocument.Document;
                Selection sel = uidoc.Selection;

                double h = 1000;
                Reference reference = sel.PickObject(ObjectType.PointOnElement, "请选择第一个翻弯点");

                //选择Pipe上的第一个点（GlobalPoint获取点击的点）
                var reffirstpoint = reference.GlobalPoint;
                var mepcurev = doc.GetElement(reference) as MEPCurve;
                var Pickmep0locationCurve1 = (mepcurev.Location as LocationCurve).Curve as Line;//获取管线的LocationCurve
                Pickmep0locationCurve1.MakeUnbound();//将管线变成射线
                var proj0 = Pickmep0locationCurve1.Project(reffirstpoint).XYZPoint;//将点投影到射线上获取该点的坐标

                //选择Pipe上的第二个点
                reference = sel.PickObject(ObjectType.PointOnElement, "请选择第二个翻弯点");
                var refsecondpoint = reference.GlobalPoint;
                var proj1 = Pickmep0locationCurve1.Project(refsecondpoint).XYZPoint;//将点投影到射线上获取该点的坐标

                //生成管道线
                var stratpoint = (mepcurev.Location as LocationCurve).Curve.GetEndPoint(0);
                var endpoint = (mepcurev.Location as LocationCurve).Curve.GetEndPoint(1);
                var line1point = Nearpoint(proj0, stratpoint, endpoint);//比较开始点和刚点的两边
                var line2point = Nearpoint(proj1, stratpoint, endpoint);

                //5根管的LocationCurve
                var line1 = Line.CreateBound(proj0, line1point);//第一根管的LocationCurve
                var UPline2 = Line.CreateBound(proj0, new XYZ(proj0.X, proj0.Y, proj0.Z + h / 304.8));//第二根管的LocationCurve（翻弯侧管）

                var line3 = Line.CreateBound(new XYZ(proj0.X, proj0.Y, proj0.Z + h / 304.8), new XYZ(proj1.X, proj1.Y, proj1.Z + h / 304.8));//第三根管的LocationCurve（翻弯管）

                var UPline4 = Line.CreateBound(proj1, new XYZ(proj1.X, proj1.Y, proj1.Z + h / 304.8));//第四根管的LocationCurve（翻弯侧管）
                var line5 = Line.CreateBound(proj1, line2point);//第五根管的LocationCurve

                Transaction tr = new Transaction(doc);
                tr.Start("一键翻弯");
                //复制管道
                var newmepcurev1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, mepcurev.Id, new XYZ(0, 0, 0)).FirstOrDefault()) as MEPCurve;
                var newmepcurev2 = doc.GetElement(ElementTransformUtils.CopyElement(doc, mepcurev.Id, new XYZ(0, 0, 0)).FirstOrDefault()) as MEPCurve;
                var newmepcurev3 = doc.GetElement(ElementTransformUtils.CopyElement(doc, mepcurev.Id, new XYZ(0, 0, 0)).FirstOrDefault()) as MEPCurve;
                var newmepcurev4 = doc.GetElement(ElementTransformUtils.CopyElement(doc, mepcurev.Id, new XYZ(0, 0, 0)).FirstOrDefault()) as MEPCurve;
                var newmepcurev5 = doc.GetElement(ElementTransformUtils.CopyElement(doc, mepcurev.Id, new XYZ(0, 0, 0)).FirstOrDefault()) as MEPCurve;

                //变换管道LocationCurve
                (newmepcurev1.Location as LocationCurve).Curve = line1;
                (newmepcurev2.Location as LocationCurve).Curve = UPline2;
                (newmepcurev3.Location as LocationCurve).Curve = line3;
                (newmepcurev4.Location as LocationCurve).Curve = UPline4;
                (newmepcurev5.Location as LocationCurve).Curve = line5;
                //生成管件
                TwoMEPCurveCeateElbow(doc, newmepcurev1, newmepcurev2);
                TwoMEPCurveCeateElbow(doc, newmepcurev2, newmepcurev3);
                TwoMEPCurveCeateElbow(doc, newmepcurev3, newmepcurev4);
                TwoMEPCurveCeateElbow(doc, newmepcurev4, newmepcurev5);

                doc.Delete(mepcurev.Id);//删除原来的管道
                tr.Commit();

            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {


            }
            return Result.Succeeded;
        }
        //方法
        public static XYZ Nearpoint(XYZ pickpoint, XYZ firpiont, XYZ secpoint)
        {

            if (firpiont.DistanceTo(pickpoint) > secpoint.DistanceTo(pickpoint))
            {
                return secpoint;
            }
            else
            {
                return firpiont;
            }

        }
        public static void TwoMEPCurveCeateElbow(Document doc, MEPCurve pipecurve1, MEPCurve pipecurve2)
        {
            double mindistance = double.MaxValue;
            Connector connector1, connector2;
            connector1 = null;
            connector2 = null;
            foreach (Connector con1 in pipecurve1.ConnectorManager.Connectors)
            {
                foreach (Connector con2 in pipecurve2.ConnectorManager.Connectors)
                {
                    var dis = con1.Origin.DistanceTo(con2.Origin);
                    if (dis < mindistance)
                    {
                        mindistance = dis;
                        connector1 = con1;
                        connector2 = con2;
                    }
                }
            }
        if (connector1 != null && connector2 != null)
        {
            doc.Create.NewElbowFitting(connector1, connector2);
        }

        else
        { 
            System.Windows.MessageBox.Show("Error! Please Check your Selection.");
        }
    }
}
