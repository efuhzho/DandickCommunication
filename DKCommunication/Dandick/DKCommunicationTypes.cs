namespace DKCommunication.Dandick
{
    /// <summary>
    /// 协议类型枚举
    /// </summary>
    public enum DKCommunicationType:byte
    {
        /// <summary>
        /// 支持DK-PTS系列
        /// </summary>
        DK55CommunicationType = 55,

        /// <summary>
        /// 支持DK-34B1、DK-34B2系列
        /// </summary>
        DK81CommunicationType = 81
    }
}
