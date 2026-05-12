using System.Net;
using System.Net.Sockets;
using Communicate.Commons;

namespace Communicate.Transfer;

public class TcpTransfer : BaseTransfer
{
    private Socket client;
    private TcpConnectionParam connectionParam;
    public bool IsConnected { get; set; }
    
    private int connectingFlag = 0;
    private int sendingFlag = 0;
    
    private int sendBuffSize;
    private int receiveBuffSize;
    private byte[] sendBuffer;
    private byte[] receiveBuffer;

    private readonly SocketAsyncEventArgs connectionEvent;
    private readonly SocketAsyncEventArgs receiveEvent;
    private readonly SocketAsyncEventArgs sendEvent;

    public TcpTransfer(int bufferSize = 1024 * 1024)
    {
        sendBuffSize = bufferSize;
        receiveBuffSize = bufferSize;

        connectionEvent = new SocketAsyncEventArgs();
        connectionEvent.Completed += ClientCallback;
        connectionEvent.DisconnectReuseSocket = true;

        sendEvent = new SocketAsyncEventArgs();
        sendBuffer =  new byte[sendBuffSize];
        sendEvent.SetBuffer(sendBuffer, 0, bufferSize);
        sendEvent.Completed += ClientCallback;

        receiveEvent = new SocketAsyncEventArgs();
        receiveBuffer = new byte[receiveBuffSize];
        receiveEvent.SetBuffer(receiveBuffer, 0, bufferSize);
        receiveEvent.Completed += ClientCallback;
    }

    private void ClientCallback(object? sender, SocketAsyncEventArgs e)
    {
        var operation = e.LastOperation;
        switch (operation)
        {
            case SocketAsyncOperation.Receive:
                ProcessReceive(e);
                break;
            case SocketAsyncOperation.Send:
                ProcessSend(e);
                break;
            case SocketAsyncOperation.Connect:
                if (e.SocketError == SocketError.Success)
                {
                    Console.WriteLine($"Connected to {connectionParam.Host}:{connectionParam.Port} successfully");
                    if (client.ReceiveAsync(receiveEvent))
                    {
                        Console.WriteLine($"{connectionParam.Host}:{connectionParam.Port} sent");
                    }
                    else
                    {
                        Console.WriteLine($"{connectionParam.Host}:{connectionParam.Port} failed");
                    }
                }
                else
                {
                    Console.WriteLine($"Failed to connect to {connectionParam.Host}:{connectionParam.Port}");
                }

                break;
            case SocketAsyncOperation.Disconnect:
                Console.WriteLine($"Disconnected from {connectionParam.Host}:{connectionParam.Port}");
                break;
        }
    }

    private void ProcessSend(SocketAsyncEventArgs socketAsyncEventArgs)
    {
        Console.WriteLine($"{connectionParam.Host}:{connectionParam.Port} sent");
    }

    private void ProcessReceive(SocketAsyncEventArgs socketAsyncEventArgs)
    {
        Console.WriteLine($"{connectionParam.Host}:{connectionParam.Port} received");
    }

    public void ConnectAsync(ConnectionParam param, CancellationToken ct)
    {
        try
        {
            // 如果connectingFlag=0，则替换为1，如果哪种情况，都返回修改前的原始值
            if (Interlocked.CompareExchange(ref connectingFlag, 1, 0) != 0)
            {
                throw new Exception("正在连接中......");
            }

            connectionParam = param as TcpConnectionParam;
            if (connectionParam == null)
            {
                throw new ArgumentException("The connection param must be of type TcpConnectionParam");
            }

            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            if (!IpValidator.IsValidIPv4(connectionParam.Host, false))
            {
                throw new ArgumentException("The host is not a valid IPv4 address");
            }

            IPAddress ipAddress = IPAddress.Parse(connectionParam.Host);
            IPEndPoint endPoint = new IPEndPoint(ipAddress, connectionParam.Port);
            connectionEvent.RemoteEndPoint = endPoint;
            var connectRes = client.ConnectAsync(connectionEvent);
            if (!connectRes)
            {
                IsConnected = false;
            }
            IsConnected = true;
        }
        catch (Exception e)
        {
            IsConnected = false;
            client.Dispose();
            client = null;
            Console.WriteLine("connect error: " + e.Message);
        }
        finally
        {
            Interlocked.Exchange(ref connectingFlag, 0);
        }

    }

    public bool SendAsync(byte[] bytes)
    {
        if (Interlocked.CompareExchange(ref sendingFlag, 1, 0) != 0)
        {
            return false;
        }

        try
        {
            if (client == null)
                throw new Exception("client is null");
            if (!IsConnected)
                throw new Exception("client is not connected");
            if (bytes.Length < 1)
                throw new Exception("data can not be null or empty");
            byte[] sendBytes = bytes;
            if(sendBytes.Length > sendBuffSize)
                throw new Exception("send buffer is too large");
            sendBytes.CopyTo(sendBuffer, 0);
            sendEvent.SetBuffer(0, sendBytes.Length);

            client.SendAsync(sendEvent);
        }
        catch (Exception e)
        {
            Console.WriteLine("send error: " + e.Message);
        }
        finally
        {
            Interlocked.Exchange(ref sendingFlag, 0);
        }
        return true;
    }

    
}