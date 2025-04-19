using Supabase.Realtime.Interfaces;

namespace Supabase.Realtime.Events;

internal class RealtimeEvents : IRealtimeEvents
{
    public void Handle(IRealtimeChannel realtimeChannel, IRealtimeSocketResponse response)
    {
        throw new System.NotImplementedException();
    }

    public bool isEvent(IRealtimeSocketResponse response)
    {
        throw new System.NotImplementedException();
    }
}
