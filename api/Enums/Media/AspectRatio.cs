using System.ComponentModel;

namespace api.Enums.Media
{
    public enum AspectRatio
    {
        [Description("1x1")]
        Aspect1X1 = 1,

        [Description("4x3")]
        Aspect4X3,

        [Description("16x9")]
        Aspect16X9
    }
}