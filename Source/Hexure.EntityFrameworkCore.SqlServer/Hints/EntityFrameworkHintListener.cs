using System;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Hexure.EntityFrameworkCore.SqlServer.Hints
{
    public class EntityFrameworkHintListener : IObserver<DiagnosticListener>
    {
        private readonly HintInterceptor _hintInterceptor = new HintInterceptor();

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(DiagnosticListener value)
        {
            if (value.Name == DbLoggerCategory.Name)
            {
                value.Subscribe(_hintInterceptor);
            }
        }
    }
}