using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace CountdownClass
{
    public static partial class CountdownManage
    {
        /// <summary>
        /// 读取当地文件
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<Countdown> GetCountdowns()
        {
            ObservableCollection<Countdown> Countdowns = new ObservableCollection<Countdown>();
            try
            {
                //localSettings保存位置
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

                StorageFolder localFolder = ApplicationData.Current.LocalFolder;

                ApplicationDataContainer CountdownsSettings = localSettings.CreateContainer("Data", ApplicationDataCreateDisposition.Always);
                if (localSettings.Containers.ContainsKey("Data"))
                {
                    foreach (var item in CountdownsSettings.Containers)
                    {
                        Countdown cacheCountdown = new Countdown();
                        #region 读取内容
                        if (ExistInContainers(CountdownsSettings, "Title")) 
                        {
                            cacheCountdown.Title = (string)item.Value.Values["Title"];
                        }
                        if (ExistInContainers(CountdownsSettings, "Date")) 
                        {
                            cacheCountdown.Date = (DateTimeOffset)item.Value.Values["Date"];
                        }
                        if (ExistInContainers(CountdownsSettings, "ClassificationCase")) 
                        {
                            cacheCountdown.Classification = new Classification((ClassificationCase)Enum.Parse(typeof(ClassificationCase), (string)item.Value.Values["ClassificationCase"]));
                        }
                        if (ExistInContainers(CountdownsSettings, "RepeatCase")) 
                        {
                            cacheCountdown.Repeat = new Repeat((RepeatCase)Enum.Parse(typeof(RepeatCase), (string)item.Value.Values["RepeatCase"]));
                        }
                        if (ExistInContainers(CountdownsSettings, "AllDay")) 
                        {
                            cacheCountdown.AllDay = Boolean.Parse(item.Value.Values["AllDay"].ToString());
                        }
                        cacheCountdown.ID = Convert.ToInt32(item.Value.Name);
                        #endregion
                        #region 其他
                        ////将储存在setting里的内容转换为Countdown类
                        //cacheCountdown = new Countdown()
                        //{
                        //    Title = (string)item.Value.Values["Title"],
                        //    Date = (DateTimeOffset)item.Value.Values["Date"],
                        //    Classification = new Classification((ClassificationCase)Enum.Parse(typeof(ClassificationCase), (string)item.Value.Values["ClassificationCase"])),
                        //    ID = Convert.ToInt32(item.Value.Name),
                        //    Repeat = new Repeat((RepeatCase)Enum.Parse(typeof(RepeatCase), (string)item.Value.Values["RepeatCase"])),
                        //    AllDay = Boolean.Parse(item.Value.Values["AllDay"].ToString()),
                        //};
                        #endregion
                        Countdowns.Add(cacheCountdown);
                    }
                }
            }
            catch
            {
                throw new Exception("Error");
            }

            return Countdowns;
        }
        
        /// <summary>
        /// 判断是否存在名字
        /// </summary>
        /// <param name="Container"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        private static bool ExistInContainers(ApplicationDataContainer Container, string Name)
        {
            //如果存在元素
            if (Container.Containers.Any())
            {
                //判断是否容器中存在元素
                if (Container.Containers.First().Value.Values[Name]!=null)
                {
                    return true;
                }
            }
            else
            {
                throw new ArgumentNullException();
            }
            return false;
        }

        /// <summary>
        /// 从容器中获取值的String值
        /// </summary>
        /// <param name="Container"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        private static string GetStringFromContainers(ApplicationDataContainer Container, string Name)
        {
            throw new Exception();
        }


        /// <summary>
        /// 
        /// 用途:将集合内容保存到当地
        /// </summary>
        /// <param name="Countdowns"></param>
        /// <returns></returns>
        public static void SaveCountdowns(ObservableCollection<Countdown> Countdowns)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            StorageFolder localFolder = ApplicationData.Current.LocalFolder;

            ApplicationDataContainer CountdownsSettings = localSettings.CreateContainer("Data", ApplicationDataCreateDisposition.Always);
            if (localSettings.Containers.ContainsKey("Data"))
            {
                foreach (var Countdown in Countdowns)
                {
                    var CountdownSettings = CountdownsSettings.CreateContainer(Countdown.ID.ToString(), ApplicationDataCreateDisposition.Always);
                    if (CountdownsSettings.Containers.ContainsKey(Countdown.ID.ToString()))
                    {
                        CountdownSettings.Values["Title"] = Countdown.Title;
                        CountdownSettings.Values["Date"] = Countdown.Date;
                        CountdownSettings.Values["ClassificationCase"] = Countdown.Classification._Case;
                        CountdownSettings.Values["RepeatCase"] = Countdown.Repeat._Case;
                        CountdownSettings.Values["AllDay"] = Countdown.AllDay.ToString();
                    }
                }
            }
        }

        /// <summary>
        /// 根据ID删除指定的数据
        /// </summary>
        /// <param name="DeleteItem"></param>
        public static void DeleteCountdownFromList(Countdown DeleteItem)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            StorageFolder localFolder = ApplicationData.Current.LocalFolder;

            ApplicationDataContainer CountdownsSettings = localSettings.CreateContainer("Data", ApplicationDataCreateDisposition.Always);
            if (localSettings.Containers.ContainsKey("Data"))
            {
                if (DeleteItem != null)
                    if (CountdownsSettings.Containers.ContainsKey(Convert.ToString(DeleteItem.ID)))
                    {
                        CountdownsSettings.DeleteContainer(Convert.ToString(DeleteItem.ID));
                    }
                    else
                        throw new Exception();
                else
                    throw new NullReferenceException();
            }
        }

        /// <summary>
        /// 由于交换元素位置
        /// </summary>
        /// <param name="List"></param>
        /// 交换元素的列表
        /// <param name="Positon">
        /// 交换的位置
        /// </param>
        /// <param name="PrePositon">
        /// 原先的位置
        /// </param>
        public static void Swap(ObservableCollection<Countdown> List, int Positon,int PrePositon)
        {
            if (Positon == PrePositon)
                return;
            if (Positon < 0 || PrePositon > List.Count - 1 || Positon > List.Count - 1)
                throw new ArgumentOutOfRangeException();
            var item = List[PrePositon];
            List.Insert(Positon, item);
        }
    }
}
