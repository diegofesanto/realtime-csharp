using Supabase.Realtime.Interfaces;
using Supabase.Realtime.Socket;

namespace Supabase.Realtime.Events;

internal interface IRealtimeEvents
{
    void Handle(IRealtimeChannel realtimeChannel, SocketResponse response);

    bool isEvent(IRealtimeSocketResponse response);
}
