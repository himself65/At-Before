using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using At_Before.Pages;

namespace At_Before.Sources
{
    public static class ListItemManage
    {
        public static List<HambugerItem> GetHambugerListItems()
        {
            List<HambugerItem> lists = new List<HambugerItem>()
            {
                new HambugerItem(){ Title="主页", Icon="\uE13D", Page=typeof(ShowCountdownPage) },
                new HambugerItem(){ Title="日历", Icon="\uE163", Page=typeof(HomePage) },
            };
            return lists;
        }

        public static List<HambugerItem> GetSettingListItem()
        {
            List<HambugerItem> lists = new List<HambugerItem>()
            {
                new HambugerItem(){ Title="设置", Icon="\uE115", Page=typeof(SettingPage) },
            };

            return lists;
        }

    }
}
