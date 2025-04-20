using Supabase.Realtime.Exceptions;
using Supabase.Realtime.Socket;
using static Supabase.Realtime.Constants;

namespace Supabase.Realtime.Handlers;

internal class SystemHandle : IHandlers
{
    public void Handler(
        string phoenixStatus,
        SocketResponse response,
        RealtimeChannel realtimeChannel
    )
    {
        if (phoenixStatus == PhoenixStatusOk)
        {
            realtimeChannel.NotifyStateChanged(ChannelState.Joined);
        }
        if (phoenixStatus == PhoenixStatusError)
        {
            realtimeChannel.NotifyErrorOccurred(
                new RealtimeException(response.Json)
                {
                    Reason = FailureHint.Reason.ChannelJoinFailure,
                }
            );
        }
    }
}