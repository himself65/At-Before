using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace At_Before.Sources
{
    public class FrameControl
    {
        public Frame RightFrame { get; set; }
        public Frame InViewFrame { get; set; }
        public Frame MainFrame { get; set; }
        public bool IsNarrow { get; set; }

        public static bool CanChangeFrame(Type PageType)
        {
            bool CanChange = false;
            List<Type> AllPageType = new List<Type>
            {
                typeof(Pages.CountdownPage.EditPage),
                typeof(Pages.CountdownPage.ShowOneCountdownPage),
            };
            foreach (var item in AllPageType)
            {
                if (item == PageType)
                {
                    CanChange = true;
                    break;
                }
            }

            return CanChange;
        }
    }
}
