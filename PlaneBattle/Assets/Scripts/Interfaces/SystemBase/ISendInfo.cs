
/// <summary>
/// 一个接口，为物体的脚本提供设置实体类对象方法
/// </summary>
public interface ISendInfo
{
    /// <summary>
    /// 设置实体类对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="vFrom"></param>
    void SetInfo(string vFrom);
}
