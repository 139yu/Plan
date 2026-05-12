namespace Communicate.Commons;

public enum CommunicationType
{
    Tcp,
    Udp,
    ModbusTcp,
    ModbusRtu
}

public enum EnumStopBits
{
    One,
    Two,
}

public class ConnectionParam
{
    
}

public class TcpConnectionParam: ConnectionParam
{
    public string Host { get; set; }
    public int Port { get; set; }
}