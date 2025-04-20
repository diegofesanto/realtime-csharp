using Newtonsoft.Json;
using Supabase.Realtime.Handlers;
using Supabase.Realtime.Interfaces;
using Supabase.Realtime.Socket;
using Supabase.Realtime.Socket.Responses;

namespace Supabase.Realtime.Events;

internal class SystemEvents : IRealtimeEvents
{
    public void Handle(RealtimeChannel realtimeChannel, SocketResponse response)
    {
        if (!realtimeChannel.IsJoining)
            return;

        SocketResponse<PhoenixResponse>? obj = JsonConvert.DeserializeObject<
            SocketResponse<PhoenixResponse>
        >(response.Json!, realtimeChannel.Options.SerializerSettings);

        if (obj?.Payload?.Status == null)
            return;

        SystemHandle? handler = (SystemHandle?)
            realtimeChannel.Handlers.GetService(typeof(SystemHandle));
        handler?.Handler(obj.Payload.Status, response, realtimeChannel);
    }

    public bool isEvent(IRealtimeSocketResponse response)
    {
        return response.Event == Constants.EventType.System;
    }
}