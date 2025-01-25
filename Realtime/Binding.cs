using Supabase.Realtime.Interfaces;
using Supabase.Realtime.PostgresChanges;

namespace Supabase.Realtime;

public class Binding
{
    public int? Id { get; set; }
    
    public PostgresChangesOptions Options { get; set; }
    
    public IRealtimeChannel.PostgresChangesHandler? Handler { get; set; }
}