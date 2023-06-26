using System;

namespace BIMBOX.Revit.Toolkit.Extension
{
    /// <summary>
    /// 表示医院的床位规模。
    /// </summary>
    public enum HospitalSize
    {
        /// <summary>
        /// 200床以下。
        /// </summary>
        Below200Beds,

        /// <summary>
        /// 200床~499床。
        /// </summary>
        Beds200To499,

        /// <summary>
        /// 500床~799床。
        /// </summary>
        Beds500To799,

        /// <summary>
        /// 800床~1199床。
        /// </summary>
        Beds800To1199,

        /// <summary>
        /// 1200床~1500床。
        /// </summary>
        Beds1200To1500,

        /// <summary>
        /// 1500床以上。
        /// </summary>
        BedsAbove1500
    }

    /// <summary>
    /// 表示医院的级别。
    /// </summary>
    public enum HospitalLevel
    {
        /// <summary>
        /// 二级综合医院。
        /// </summary>
        Level2,

        /// <summary>
        /// 三级甲等医院。
        /// </summary>
        Level3A,

        /// <summary>
        /// 三级乙等医院。
        /// </summary>
        Level3B
    }

    /// <summary>
    /// 规模指标
    /// </summary>
    public class PlanningData
    {
        /// <summary>
        /// 获取医院规模：床位数
        /// </summary>
        public static HospitalSize GetHospitalSize(int bedCount)
        {
            if (bedCount < 200)
            {
                return HospitalSize.Below200Beds;
            }
            else if (bedCount >= 200 && bedCount < 500)
            {
                return HospitalSize.Beds200To499;
            }
            else if (bedCount >= 500 && bedCount < 800)
            {
                return HospitalSize.Beds500To799;
            }
            else if (bedCount >= 800 && bedCount < 1200)
            {
                return HospitalSize.Beds800To1199;
            }
            else if (bedCount >= 1200 && bedCount <= 1500)
            {
                return HospitalSize.Beds1200To1500;
            }
            else
            {
                return HospitalSize.BedsAbove1500;
            }
        }

        /// <summary>
        /// 获取医院规模：规模等级
        /// </summary>
        public static HospitalLevel GetHospitalLevel(HospitalSize size)
        {
            if (size == HospitalSize.Below200Beds || size == HospitalSize.Beds200To499)
            {
                return HospitalLevel.Level2;
            }
            else
            {
                return HospitalLevel.Level3A;
            }
        }
        /// <summary>
        /// 获取医院规模：床位数
        /// </summary>
        public static HospitalLevel GetHospitalLevel(int bedCount)
        {
            HospitalSize size = GetHospitalSize(bedCount);
            return GetHospitalLevel(size);
        }
    }
}
