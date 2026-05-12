using Communicate.Commons;

namespace Communicate.Transfer;

public interface BaseTransfer
{

    public void ConnectAsync(ConnectionParam param,CancellationToken ct);

    public bool SendAsync(byte[] bytes);

}