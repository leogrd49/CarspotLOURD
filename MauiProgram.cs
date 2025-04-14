using Microsoft.Extensions.Logging;
using CarspotLourd.Services;

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

        // Enregistrer le service SupabaseDataService pour l'injection de dépendance si nécessaire
        builder.Services.AddSingleton<SupabaseDataService>();

        return builder.Build();
    }
}
