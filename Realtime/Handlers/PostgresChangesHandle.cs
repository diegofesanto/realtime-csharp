using System.Collections.Generic;
using System.Linq;
using Supabase.Realtime.Interfaces;
using Supabase.Realtime.PostgresChanges;
using static Supabase.Realtime.PostgresChanges.PostgresChangesOptions;

namespace Supabase.Realtime.Handlers;

internal class PostgresChangesHandle : IHandlers
{
    private IList<Binding> _bindings;
    private IRealtimeChannel _realtimeChannel;

    public PostgresChangesHandle(IList<Binding> bindings, IRealtimeChannel realtimeChannel)
    {
        _bindings = bindings;
        _realtimeChannel = realtimeChannel;
    }

    public void Handle(ListenType eventType, PostgresChangesResponse response)
    {
        var all = _bindings.FirstOrDefault(b =>
        {
            if (b.Options == null && response.Payload == null && b.Handler == null)
                return false;

            return response.Payload != null
                && response.Payload.Ids.Contains(b.Id)
                && eventType != ListenType.All
                && b.ListenType == ListenType.All;
        });

        if (all != null)
        {
            all.Handler?.Invoke(_realtimeChannel, response);
            return;
        }

        // Invoke all specific handler if possible
        _bindings
            .ToList()
            .ForEach(binding =>
            {
                if (binding.ListenType != eventType)
                    return;
                if (binding.Options == null || response.Payload == null || binding.Handler == null)
                    return;

                if (response.Payload.Ids.Contains(binding.Id))
                    binding.Handler.Invoke(_realtimeChannel, response);
            });
    }
}