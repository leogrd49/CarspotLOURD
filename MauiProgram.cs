using Microsoft.Extensions.Logging;
using CarspotLourd.Services;
using Supabase;
using Supabase.Interfaces;
using Supabase.Gotrue;
using Supabase.Realtime;
using Supabase.Storage;

namespace CarspotLourd;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Configuration Supabase
        var url = builder.Configuration["SupabaseUrl"] ?? "https://your-supabase-url.supabase.co";
        var key = builder.Configuration["SupabaseKey"] ?? "your-supabase-key";
        
        var options = new SupabaseOptions
        {
            AutoRefreshToken = true
        };
        
        var client = new Supabase.Client(url, key, options);
        builder.Services.AddSingleton(client);
        
        // Enregistrer les services pour l'injection de dépendance
        builder.Services.AddSingleton<SupabaseDataService>();
        builder.Services.AddSingleton<SpotService>();
        builder.Services.AddSingleton<InstagramFeedService>();

        return builder.Build();
    }
}
