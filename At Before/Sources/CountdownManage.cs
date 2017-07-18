using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace At_Before.Sources
{
    class CountdownManage
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
                    foreach(var item in CountdownsSettings.Containers)
                    {
                        Countdown cacheCountdown = new Countdown()
                        {
                            Title = (string)item.Value.Values["Title"],
                            Date = (DateTimeOffset)item.Value.Values["Date"],
                            Classification = new Classification((ClassificationCase)Enum.Parse(typeof(ClassificationCase), (string)item.Value.Values["ClassificationCase"])),
                            ID = Convert.ToInt32(item.Value.Name),
                            Repeat = new Repeat((RepeatCase)Enum.Parse(typeof(RepeatCase),(string)item.Value.Values["RepeatCase"])),

                        };
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
                foreach(var Countdown in Countdowns)
                {
                    var CountdownSettings = CountdownsSettings.CreateContainer(Countdown.ID.ToString(), ApplicationDataCreateDisposition.Always);
                    if (CountdownsSettings.Containers.ContainsKey(Countdown.ID.ToString()))
                    {
                        CountdownSettings.Values["Title"] = Countdown.Title;
                        CountdownSettings.Values["Date"] = Countdown.Date;
                        CountdownSettings.Values["ClassificationCase"] = Countdown.Classification._Case;
                        CountdownSettings.Values["RepeatCase"] = Countdown.Repeat._Case;
                    }
                }
            }
        }


        public static List<Classification> GetClassifications()
        {
            List<Classification> Classifications = new List<Classification>
            {
                new Classification(ClassificationCase.Event),
                new Classification(ClassificationCase.Birthday),
                new Classification(ClassificationCase.Festival),
                new Classification(ClassificationCase.Entertainment),
                new Classification(ClassificationCase.Life),
                new Classification(ClassificationCase.Love),
                new Classification(ClassificationCase.Stuty),
                new Classification(ClassificationCase.Work),
                new Classification(ClassificationCase.Others)
            };
            return Classifications;
        }
        
        public static List<Repeat> GetRepeats()
        {
            List<Repeat> Repeats = new List<Repeat>()
            {
                new Repeat(RepeatCase.None),
                new Repeat(RepeatCase.EveryWeek),
                new Repeat(RepeatCase.EveryMonth),
                new Repeat(RepeatCase.EveryYear),
            };
            return Repeats;
        }
    }
}
