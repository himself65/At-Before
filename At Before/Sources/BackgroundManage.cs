using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace At_Before.Sources
{
    public static class BackgroundManage
    {
        public static List<Background> GetBackgroundPapers()
        {
            List<Background> items = new List<Background>()
            {
                new Background(){ ImageFile="appx://Assets/Background/Cloth.jpg", Name="Cloth",}

            };


            return items;
        }
    }
}
