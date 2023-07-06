using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB.ExtensibleStorage;

namespace BIMBOX.Revit.Tuna.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class CreateSchemaCommand : IExternalCommand
    {
        private readonly string schemaGUID = "379B2606-721D-4636-B532-9307D250E90B";
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //创建扩展
            using (SchemaBuilder schemaBuilder = new SchemaBuilder(new Guid(schemaGUID)))
            {
                //定义构造信息
                schemaBuilder
                    .SetApplicationGUID(commandData.Application.ActiveAddInId.GetGUID())
                    .SetVendorId("ADSK")
                    .SetSchemaName("BriumnTest")
                    .SetDocumentation("Briumn SetDocumentation 测试数据")
                    .SetReadAccessLevel(AccessLevel.Public)
                    .SetWriteAccessLevel(AccessLevel.Public);
                //定义字段

                //自定义简单字段
                schemaBuilder.AddSimpleField("byte", typeof(byte));
                schemaBuilder.AddSimpleField("int", typeof(int));
                schemaBuilder.AddSimpleField("short", typeof(short));
                var fieldBuilder =  schemaBuilder.AddSimpleField("double", typeof(double));
                schemaBuilder.AddSimpleField("float", typeof(float));
                schemaBuilder.AddSimpleField("bool", typeof(bool));
                schemaBuilder.AddSimpleField("string", typeof(string));
                schemaBuilder.AddSimpleField("guid", typeof(Guid));
                schemaBuilder.AddSimpleField("element", typeof(ElementId));

                //schemaBuilder.AddSimpleField("entity", typeof(Entity)).SetSubSchemaGUID(new Guid(schemaGUID));

                //结束构造，生成数据架构
               
            }


                return Result.Succeeded;
        }
    }
}
