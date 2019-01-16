namespace YoYo.ABP.Common.VierificationCode
{
    /// <summary>
    ///     验证码类型
    /// </summary>
    public enum ValidateCodeType
    {
        /// <summary>
        ///纯数字
        /// </summary>
        Number = 0,
        /// <summary>
        /// 纯字母
        /// </summary>
        English = 1,
        /// <summary>
        /// 数值与字母的组合
        /// </summary>
        NumberAndLetter = 2,
        /// <summary>
        /// 汉字
        /// </summary>
        Hanzi = 3
    }
}