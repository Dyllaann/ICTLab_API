using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTable2.Notifier;

namespace TimeTable2.Tests.Resources
{
    public class TestNotifier : INotifier
    {
        public Task Notify(string userId, string title, string message, string @from)
        {
            return Task.FromResult(true);
        }
    }
}
