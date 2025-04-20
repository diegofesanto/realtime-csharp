using Newtonsoft.Json;
using Supabase.Realtime.Handlers;
using Supabase.Realtime.Interfaces;
using Supabase.Realtime.PostgresChanges;
using Supabase.Realtime.Socket;
using static Supabase.Realtime.Constants;
using static Supabase.Realtime.PostgresChanges.PostgresChangesOptions;

namespace Supabase.Realtime.Events;

internal class PostgresChangesEvents : IRealtimeEvents
{
    public void Handle(RealtimeChannel realtimeChannel, SocketResponse response)
    {
        PostgresChangesResponse? deserialized = Parse(realtimeChannel, response);
        if (deserialized == null)
            return;

        ListenType listenType = ConvertEventType(deserialized.Payload!.Data!.Type, deserialized);
        PostgresChangesHandle? handler = (PostgresChangesHandle?)
            realtimeChannel.Handlers.GetService(typeof(PostgresChangesHandle));
        handler?.Handle(listenType, deserialized);
    }

    private PostgresChangesResponse? Parse(RealtimeChannel realtimeChannel, SocketResponse response)
    {
        PostgresChangesResponse? deserialized =
            JsonConvert.DeserializeObject<PostgresChangesResponse>(
                response.Json!,
                realtimeChannel.Options.SerializerSettings
            );

        if (deserialized?.Payload?.Data == null)
            return null;

        deserialized.Json = response.Json;
        deserialized.SerializerSettings = realtimeChannel.Options.SerializerSettings;

        return deserialized;
    }

    private ListenType ConvertEventType(EventType eventType, PostgresChangesResponse response)
    {
        return eventType switch
        {
            EventType.Insert => ListenType.Inserts,
            EventType.Delete => ListenType.Deletes,
            EventType.Update => ListenType.Updates,
            _ => ListenType.All,
        };
    }

    public bool isEvent(IRealtimeSocketResponse response)
    {
        return response.Event == Constants.EventType.PostgresChanges;
    }
}
