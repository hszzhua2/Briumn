using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;

namespace BIMBOX.Revit.Tuna.Services
{
    internal class ExternalEventService : IExternalEventHandler
    {
        private readonly ExternalEvent _externalEvent;
        private Action<UIApplication>? _action;

        public ExternalEventService()
        {
            _externalEvent = ExternalEvent.Create(this);
        }

        public void Execute(UIApplication app)
        {
            if (_action == null)
            {
                return;
            }
            _action.Invoke(app);
        }

        public string GetName() => "External Event Services";

        public void Raise(Action<UIApplication> action)
        {
            _action = action;
            _externalEvent.Raise();
        }
    }
}
