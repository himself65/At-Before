using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;

namespace At_Before.Sources
{
    
    public class Countdown
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTimeOffset Date { get; set; }
        public Classification Classification { get; set; }
        public Repeat Repeat { get; set; }
    }
    public class Classification
    {
        private static string[] States = {"Event","Life","Love","Birthday","Festival","Entertainment","Work","Stuty","Others"};
        private static string[] Translation = { "事件", "生活", "爱情", "生日", "节日", "娱乐", "工作", "学习", "其他" };

        public ClassificationCase Case { get; set; }
        public string _Case { get; private set; }
        public string _Case_cn {
            get;
            private set;
        }
        public Classification(ClassificationCase Case)
        {
            this.Case = Case;
            var state = Enum.Format(typeof(ClassificationCase), (int)Case, "G");
            var i = 0;
            foreach(var item in States)
            {
                if (state == item)
                    break;
                i++;
            }
            _Case = States[i];
            _Case_cn = Translation[i];
        }
        public Classification()
        {
            Case = ClassificationCase.Event;
            _Case = States[0];
            _Case_cn = Translation[0];
        }
    }
    public class Repeat
    {
        public RepeatCase Case { get; set; }
        public string _Case { get => Enum.Format(typeof(RepeatCase), Case, "G"); }
        public string _Case_cn { get => Translation[Convert.ToInt32(Case)]; }
        private static string[] Translation = { "不重复", "每年", "每月", "每周" };
        
        public Repeat(RepeatCase Case)
        {
            this.Case = Case;
        }
    }

    public enum ClassificationCase
    {
        Event,
        Life,
        Love,
        Birthday,
        Festival,
        Entertainment,
        Work,
        Stuty,
        Others
    }
    public enum RepeatCase
    {
        None,
        EveryYear,
        EveryMonth,
        EveryWeek
    };
}
