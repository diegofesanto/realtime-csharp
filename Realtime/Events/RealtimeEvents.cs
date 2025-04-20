using System.Collections.Generic;
using System.Linq;
using Supabase.Realtime.Interfaces;
using Supabase.Realtime.Socket;

namespace Supabase.Realtime.Events;

internal class RealtimeEvents
{
    private List<IRealtimeEvents> _events = [];

    public void Add(IRealtimeEvents events)
    {
        _events.Add(events);
    }

    public IRealtimeEvents? Get(RealtimeChannel realtimeChannel, SocketResponse response)
    {
        return IsEvent(response);
    }

    private IRealtimeEvents? IsEvent(IRealtimeSocketResponse response)
    {
        return _events.FirstOrDefault(e => e.isEvent(response));
    }
}