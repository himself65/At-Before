using CountdownClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountdownClass
{
    public static partial class CountdownManage
    {
        #region 基本不会变的代码

        /// <summary>
        /// 用于返回Classificaion的list，在xaml中显示
        /// </summary>
        /// <returns></returns>
        public static List<Classification> GetClassifications()
        {
            List<Classification> Classifications = new List<Classification>
            {
                new Classification(ClassificationCase.Event),
                new Classification(ClassificationCase.Life),
                new Classification(ClassificationCase.Love),
                new Classification(ClassificationCase.Birthday),
                new Classification(ClassificationCase.Festival),
                new Classification(ClassificationCase.Entertainment),
                new Classification(ClassificationCase.Work),
                new Classification(ClassificationCase.Stuty),
                new Classification(ClassificationCase.Others)
            };
            return Classifications;
        }

        /// <summary>
        /// 用于返回Repeat的list，在xaml中显示
        /// </summary>
        /// <returns></returns>
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
        #endregion
    }
}
