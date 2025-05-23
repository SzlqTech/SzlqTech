using System.ComponentModel;

namespace SzlqTech.Common.EnumType
{
    public enum DecimalPointShiftType:short
    {
        [Description("左移三位")]
        LeftThree = -3,
        [Description("左移两位")]
        LeftTwo = -2,
        [Description("左移一位")]
        LeftOne = -1,
        [Description("无")]
        None = 0,
        [Description("右移一位")]
        RightOne = 1,
        [Description("右移两位")]
        RightTwo = 2,
        [Description("右移三位")]
        RightThree = 3,
    }
}
