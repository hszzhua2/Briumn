using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Diagnostics;


    [Transaction(TransactionMode.Manual)]
    public class OpenURLCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // 确定URL地址（BIMObject）
            string url = "https://www.bimobject.com/en";
            try
            {
                // 用默认浏览器打开网址
                Process.Start(url);
                return Result.Succeeded;
            }
            catch
            {
                // 异常处理
                return Result.Failed;
            }
        }
    }

