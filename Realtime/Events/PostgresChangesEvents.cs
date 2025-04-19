using Newtonsoft.Json;
using Supabase.Realtime.Interfaces;
using Supabase.Realtime.PostgresChanges;
using Supabase.Realtime.Socket;

namespace Supabase.Realtime.Events;

internal class PostgresChangesEvents : IRealtimeEvents
{
    public void Handle(IRealtimeChannel realtimeChannel, SocketResponse response)
    {
        var deserialized = JsonConvert.DeserializeObject<PostgresChangesResponse>(
            response.Json!,
            realtimeChannel.Options.SerializerSettings
        );

        if (deserialized?.Payload?.Data == null)
            return;

        deserialized.Json = response.Json;
        deserialized.SerializerSettings = realtimeChannel.Options.SerializerSettings;

        // Invoke '*' listener
    }

    public bool isEvent(IRealtimeSocketResponse response)
    {
        return response.Event == Constants.EventType.PostgresChanges;
    }
}
