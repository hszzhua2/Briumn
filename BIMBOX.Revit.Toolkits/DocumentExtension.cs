using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;


namespace BIMBOX.Revit.Toolkit.Extension
{
    public static class DocumentExtension
    {
        public static void NewTransaction(this Document document, string name, Action action)
        {
            using (Transaction ts = new Transaction(document, name))
            {
                ts.Start();
                action?.Invoke();
                ts.Commit();
            }
        }
    }
}
