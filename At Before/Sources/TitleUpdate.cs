using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.UI.Notifications;
using Windows.Web.Syndication;

namespace At_Before.Sources
{
    public sealed class TitleUpdate
    {

    }
    //public sealed class TitleUpdate:IBackgroundTask
    //{
    //    public void Run(IBackgroundTaskInstance taskInstance)
    //    {
    //        var deferral = taskInstance.GetDeferral();

    //        var List = GetLastestMessage();

    //        deferral.Complete();
    //    }
        
    //    public Task<List<string>> GetLastestMessage()
    //    {

    //    }
    //}
}
