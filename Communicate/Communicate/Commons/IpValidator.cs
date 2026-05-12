using System.Net;
using System.Net.Sockets;

namespace Communicate.Commons;

public class IpValidator
{
    /// <summary>
    /// 检查字符串是否为合法的 IP 地址（默认 IPv4 和 IPv6 都允许）
    /// </summary>
    /// <param name="ip">待验证的 IP 字符串</param>
    /// <param name="allowIpv6">是否允许 IPv6 地址，默认为 true</param>
    /// <param name="allowSpecial">是否允许 0.0.0.0、环回地址等特殊地址，默认为 true</param>
    /// <returns>合法返回 true，否则 false</returns>
    public static bool IsValidIpAddress(string ip, bool allowIpv6 = true, bool allowSpecial = true)
    {
        // 空字符串或空白直接非法
        if (string.IsNullOrWhiteSpace(ip))
            return false;

        // 使用系统方法进行基础格式解析
        if (!IPAddress.TryParse(ip, out IPAddress? address))
            return false;

        // 如果不允许 IPv6，且解析出的地址是 IPv6，则返回 false
        if (!allowIpv6 && address.AddressFamily == AddressFamily.InterNetworkV6)
            return false;

        // 如果不允许特殊地址，则过滤掉以下情况
        if (!allowSpecial)
        {
            // 0.0.0.0 (IPv4 任意地址) 或 [::] (IPv6 任意地址)
            if (address.Equals(IPAddress.Any) || address.Equals(IPAddress.IPv6Any))
                return false;

            // 环回地址 127.0.0.1 或 ::1
            if (address.Equals(IPAddress.Loopback) || address.Equals(IPAddress.IPv6Loopback))
                return false;

            // 还可以根据需要过滤广播地址等
            if (address.Equals(IPAddress.Broadcast))
                return false;
        }

        return true;
    }

    // 可选的：快速判断是否为 IPv4 合法地址
    public static bool IsValidIPv4(string ip, bool allowSpecial = true)
    {
        return IsValidIpAddress(ip, allowIpv6: false, allowSpecial);
    }
}