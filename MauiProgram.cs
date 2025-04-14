using Microsoft.Extensions.Logging;
using CarspotLourd.Services;
using Microsoft.Extensions.Configuration;
using System.Reflection;

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

        builder.Services.AddMauiBlazorWebView();
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        string supabaseUrl = "https://vdvqdycepxncqzdgqdee.supabase.co";
        string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InZkdnFkeWNlcHhuY3F6ZGdxZGVlIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDM2MDI4NDgsImV4cCI6MjA1OTE3ODg0OH0.qatJcK6-o31ZDU8odJq01csRZaikKK4iBx7tcYNq_0w";  // Remplacez par votre clé réelle

        Console.WriteLine($"Initialisation des services Supabase avec URL: {supabaseUrl}");
        
        // Enregistrer les deux services
        builder.Services.AddSingleton(new SupabaseService(supabaseUrl, supabaseKey));
        builder.Services.AddSingleton(new HttpSupabaseService(supabaseUrl, supabaseKey));

        return builder.Build();
    }
}