using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountdownClass
{
    public class Countdown
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTimeOffset Date { get; set; }
        public bool AllDay { get; set; }
        public Classification Classification { get; set; }
        public Repeat Repeat { get; set; }
        public TimeSpan EndLine { get => Date - DateTimeOffset.Now; }
    }
    public class Classification
    {
        private static string[] States = { "Event", "Life", "Love", "Birthday", "Festival", "Entertainment", "Work", "Stuty", "Others" };
        private static string[] Translation = { "事件", "生活", "爱情", "生日", "节日", "娱乐", "工作", "学习", "其他" };

        public ClassificationCase Case { get; set; }
        public string _Case { get => Enum.Format(typeof(ClassificationCase), Case, "G"); }
        public string _Case_cn{ get => Translation[Convert.ToInt32(Case)]; }
        public Classification(ClassificationCase Case)
        {
            this.Case = Case;
        }
        public Classification()
        {
            this.Case = ClassificationCase.Event;
        }
    }
    public class Repeat
    {
        public RepeatCase Case { get; set; }
        public string _Case { get => Enum.Format(typeof(RepeatCase), Case, "G"); }
        public string _Case_cn { get => Translation[Convert.ToInt32(Case)]; }
        private static string[] Translation = { "不重复", "每周", "每月", "每年" };

        public Repeat(RepeatCase Case)
        {
            this.Case = Case;
        }
        public Repeat()
        {
            this.Case = RepeatCase.None;
        }
    }

    /// <summary>
    /// Enum数据
    /// </summary>
    public enum ClassificationCase
    {
        Event = 0,
        Life = 1,
        Love = 2,
        Birthday = 3,
        Festival = 4,
        Entertainment = 5,
        Work = 6,
        Stuty = 7,
        Others = 8
    }
    public enum RepeatCase
    {
        None,
        EveryWeek,
        EveryMonth,
        EveryYear,
    };
}
