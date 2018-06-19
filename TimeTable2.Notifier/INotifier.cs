using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTable2.Notifier
{
    public interface INotifier
    {
        Task Notify(string userId, string title, string message, string from);
    }
}
