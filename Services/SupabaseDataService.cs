using CarspotLourd.Models;
using Supabase;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace CarspotLourd.Services
{
    public class SupabaseDataService
    {
        private readonly string _supabaseUrl;
        private readonly string _supabaseKey;
        private Client _supabaseClient;

        public SupabaseDataService()
        {
            _supabaseUrl = "https://vdvqdycepxncqzdgqdee.supabase.co";
            _supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InZkdnFkeWNlcHhuY3F6ZGdxZGVlIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDM2MDI4NDgsImV4cCI6MjA1OTE3ODg0OH0.qatJcK6-o31ZDU8odJq01csRZaikKK4iBx7tcYNq_0w";
            
            // Configurer TLS pour assurer la compatibilité
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

            InitializeClient();
        }

        private void InitializeClient()
        {
            try
            {
                var options = new SupabaseOptions
                {
                    AutoRefreshToken = true,
                    AutoConnectRealtime = true
                };

                _supabaseClient = new Client(_supabaseUrl, _supabaseKey, options);
                Console.WriteLine("Client Supabase initialisé avec succès");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'initialisation du client Supabase: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
            }
        }

        public async Task<List<UserCollection>> GetUserCollectionsAsync()
        {
            try
            {
                if (_supabaseClient == null)
                {
                    InitializeClient();
                    if (_supabaseClient == null)
                    {
                        return GetDummyData(); // Retourner des données fictives si l'initialisation échoue
                    }
                }

                var response = await _supabaseClient
                    .From<UserCollection>()
                    .Select("*")
                    .Get();

                Console.WriteLine($"Récupération réussie! {response.Models.Count} collections trouvées.");
                return response.Models;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des collections: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                return GetDummyData(); // Retourner des données fictives en cas d'erreur
            }
        }

        public string GenerateHtmlTable(List<UserCollection> collections)
        {
            StringBuilder htmlBuilder = new StringBuilder();
            htmlBuilder.AppendLine("<!DOCTYPE html>");
            htmlBuilder.AppendLine("<html lang=\"fr\">");
            htmlBuilder.AppendLine("<head>");
            htmlBuilder.AppendLine("    <meta charset=\"utf-8\" />");
            htmlBuilder.AppendLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no, viewport-fit=cover\" />");
            htmlBuilder.AppendLine("    <title>CarspotLourd</title>");
            htmlBuilder.AppendLine("    <link rel=\"stylesheet\" href=\"https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css\" />");
            htmlBuilder.AppendLine("    <style>");
            htmlBuilder.AppendLine("        body {");
            htmlBuilder.AppendLine("            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;");
            htmlBuilder.AppendLine("            padding: 20px;");
            htmlBuilder.AppendLine("            background-color: #f8f9fa;");
            htmlBuilder.AppendLine("        }");
            htmlBuilder.AppendLine("        ");
            htmlBuilder.AppendLine("        .container {");
            htmlBuilder.AppendLine("            background-color: white;");
            htmlBuilder.AppendLine("            border-radius: 8px;");
            htmlBuilder.AppendLine("            padding: 20px;");
            htmlBuilder.AppendLine("            box-shadow: 0 0 10px rgba(0,0,0,0.1);");
            htmlBuilder.AppendLine("        }");
            htmlBuilder.AppendLine("        ");
            htmlBuilder.AppendLine("        h3 {");
            htmlBuilder.AppendLine("            margin-bottom: 20px;");
            htmlBuilder.AppendLine("            color: #333;");
            htmlBuilder.AppendLine("        }");
            htmlBuilder.AppendLine("        ");
            htmlBuilder.AppendLine("        table {");
            htmlBuilder.AppendLine("            width: 100%;");
            htmlBuilder.AppendLine("        }");
            htmlBuilder.AppendLine("        ");
            htmlBuilder.AppendLine("        th {");
            htmlBuilder.AppendLine("            background-color: #f2f2f2;");
            htmlBuilder.AppendLine("        }");
            htmlBuilder.AppendLine("        ");
            htmlBuilder.AppendLine("        .table-responsive {");
            htmlBuilder.AppendLine("            overflow-x: auto;");
            htmlBuilder.AppendLine("        }");
            htmlBuilder.AppendLine("    </style>");
            htmlBuilder.AppendLine("</head>");
            htmlBuilder.AppendLine("<body>");
            htmlBuilder.AppendLine("    <div class=\"container\">");
            htmlBuilder.AppendLine("        <h3>Collections d'utilisateurs</h3>");
            htmlBuilder.AppendLine("        ");
            htmlBuilder.AppendLine("        <div class=\"table-responsive\">");
            htmlBuilder.AppendLine("            <table class=\"table table-striped\">");
            htmlBuilder.AppendLine("                <thead>");
            htmlBuilder.AppendLine("                    <tr>");
            htmlBuilder.AppendLine("                        <th>ID</th>");
            htmlBuilder.AppendLine("                        <th>User ID</th>");
            htmlBuilder.AppendLine("                        <th>Model ID</th>");
            htmlBuilder.AppendLine("                        <th>Spotted</th>");
            htmlBuilder.AppendLine("                        <th>Created At</th>");
            htmlBuilder.AppendLine("                        <th>Location</th>");
            htmlBuilder.AppendLine("                        <th>Is Public</th>");
            htmlBuilder.AppendLine("                        <th>Superspot</th>");
            htmlBuilder.AppendLine("                    </tr>");
            htmlBuilder.AppendLine("                </thead>");
            htmlBuilder.AppendLine("                <tbody>");

            if (collections != null && collections.Count > 0)
            {
                foreach (var item in collections)
                {
                    htmlBuilder.AppendLine("                    <tr>");
                    htmlBuilder.AppendLine($"                        <td>{item.Id}</td>");
                    htmlBuilder.AppendLine($"                        <td>{item.UserId}</td>");
                    htmlBuilder.AppendLine($"                        <td>{item.ModelId}</td>");
                    htmlBuilder.AppendLine($"                        <td>{(item.Spotted ? "Oui" : "Non")}</td>");
                    htmlBuilder.AppendLine($"                        <td>{item.CreatedAt.ToString("yyyy-MM-dd")}</td>");
                    htmlBuilder.AppendLine($"                        <td>{item.Location}</td>");
                    htmlBuilder.AppendLine($"                        <td>{(item.IsPublic ? "Oui" : "Non")}</td>");
                    htmlBuilder.AppendLine($"                        <td>{(item.Superspot ? "Oui" : "Non")}</td>");
                    htmlBuilder.AppendLine("                    </tr>");
                }
            }
            else
            {
                htmlBuilder.AppendLine("                    <tr>");
                htmlBuilder.AppendLine("                        <td colspan=\"8\" class=\"text-center\">Aucune donnée disponible</td>");
                htmlBuilder.AppendLine("                    </tr>");
            }

            htmlBuilder.AppendLine("                </tbody>");
            htmlBuilder.AppendLine("            </table>");
            htmlBuilder.AppendLine("        </div>");
            htmlBuilder.AppendLine("    </div>");
            htmlBuilder.AppendLine("</body>");
            htmlBuilder.AppendLine("</html>");

            return htmlBuilder.ToString();
        }

        // Méthode publique pour accéder aux données factices depuis l'extérieur
        public List<UserCollection> GetDummyDataPublic()
        {
            return GetDummyData();
        }

        private List<UserCollection> GetDummyData()
        {
            // Données fictives pour test ou secours
            return new List<UserCollection>
            {
                new UserCollection
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    UserId = Guid.Parse("10000000-0000-0000-0000-000000000001"),
                    ModelId = 1,
                    Spotted = true,
                    CreatedAt = DateTime.Now.AddDays(-5),
                    Location = "Paris",
                    IsPublic = true,
                    Superspot = false
                },
                new UserCollection
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    UserId = Guid.Parse("10000000-0000-0000-0000-000000000002"),
                    ModelId = 2,
                    Spotted = false,
                    CreatedAt = DateTime.Now.AddDays(-2),
                    Location = "Lyon",
                    IsPublic = true,
                    Superspot = true
                },
                new UserCollection
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                    UserId = Guid.Parse("10000000-0000-0000-0000-000000000003"),
                    ModelId = 3,
                    Spotted = true,
                    CreatedAt = DateTime.Now.AddDays(-1),
                    Location = "Marseille",
                    IsPublic = false,
                    Superspot = false
                }
            };
        }
    }
}
