using System.ComponentModel;

namespace api.Models.AdContainer
{
    public enum AdContainerAspectRatio
    {
        [Description("1x1")]
        Type1X1,

        [Description("4x3")]
        Type4X3,

        [Description("16x9")]
        Type16X9
    }
}