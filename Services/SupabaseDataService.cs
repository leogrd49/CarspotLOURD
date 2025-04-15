using CarspotLourd.Models;
using Supabase;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Linq;

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

        // Récupération des collections
        public async Task<List<UserCollection>> GetUserCollectionsAsync()
        {
            try
            {
                if (_supabaseClient == null)
                {
                    InitializeClient();
                    if (_supabaseClient == null)
                    {
                        return GetDummyCollections(); // Retourner des données fictives si l'initialisation échoue
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
                return GetDummyCollections(); // Retourner des données fictives en cas d'erreur
            }
        }
        
        // Récupérer les collections publiques uniquement
        public async Task<List<UserCollection>> GetUserCollectionsPublicAsync()
        {
            try
            {
                if (_supabaseClient == null)
                {
                    InitializeClient();
                    if (_supabaseClient == null)
                    {
                        // Retourner uniquement les collections publiques depuis les données fictives
                        return GetDummyCollections().Where(c => c.IsPublic).ToList();
                    }
                }

                var response = await _supabaseClient
                    .From<UserCollection>()
                    .Select("*")
                    .Where(c => c.IsPublic == true)
                    .Get();

                Console.WriteLine($"Récupération réussie! {response.Models.Count} collections publiques trouvées.");
                return response.Models;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des collections publiques: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                // Retourner uniquement les collections publiques depuis les données fictives
                return GetDummyCollections().Where(c => c.IsPublic).ToList();
            }
        }
        
        // Récupération des modèles
        public async Task<List<Model>> GetModelsAsync()
        {
            try
            {
                if (_supabaseClient == null)
                {
                    InitializeClient();
                    if (_supabaseClient == null)
                    {
                        return GetDummyModels(); // Retourner des données fictives si l'initialisation échoue
                    }
                }

                var response = await _supabaseClient
                    .From<Model>()
                    .Select("*")
                    .Get();

                Console.WriteLine($"Récupération réussie! {response.Models.Count} modèles trouvés.");
                return response.Models;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des modèles: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                return GetDummyModels(); // Retourner des données fictives en cas d'erreur
            }
        }
        
        // Récupérer les modèles pour l'affichage Instagram
        public async Task<List<Model>> GetModelsDataAsync()
        {
            return await GetModelsAsync();
        }

        // Récupération des marques
        public async Task<List<Brand>> GetBrandsAsync()
        {
            try
            {
                if (_supabaseClient == null)
                {
                    InitializeClient();
                    if (_supabaseClient == null)
                    {
                        return GetDummyBrands(); // Retourner des données fictives si l'initialisation échoue
                    }
                }

                var response = await _supabaseClient
                    .From<Brand>()
                    .Select("*")
                    .Get();

                Console.WriteLine($"Récupération réussie! {response.Models.Count} marques trouvées.");
                return response.Models;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des marques: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                return GetDummyBrands(); // Retourner des données fictives en cas d'erreur
            }
        }
        
        // Récupérer les marques pour l'affichage Instagram
        public async Task<List<Brand>> GetBrandsDataAsync()
        {
            return await GetBrandsAsync();
        }

        public async Task<string> GenerateCompleteHtmlTableAsync()
        {
            // Récupérer les données nécessaires
            var collections = await GetUserCollectionsAsync();
            var models = await GetModelsAsync();
            var brands = await GetBrandsAsync();
            
            // Générer le HTML
            return GenerateHtmlTablesWithAllData(collections, models, brands);
        }

        public string GenerateHtmlTablesWithAllData(List<UserCollection> collections, List<Model> models, List<Brand> brands)
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
            htmlBuilder.AppendLine("            margin-bottom: 30px;");
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

            // Table des collections d'utilisateurs
            htmlBuilder.AppendLine("    <div class=\"container\">");
            htmlBuilder.AppendLine("        <h3>Collections d'utilisateurs</h3>");
            htmlBuilder.AppendLine("        ");
            htmlBuilder.AppendLine("        <div class=\"table-responsive\">");
            htmlBuilder.AppendLine("            <table class=\"table table-striped\">");
            htmlBuilder.AppendLine("                <thead>");
            htmlBuilder.AppendLine("                    <tr>");
            htmlBuilder.AppendLine("                        <th>ID</th>");
            htmlBuilder.AppendLine("                        <th>Modèle</th>");
            htmlBuilder.AppendLine("                        <th>Marque</th>");
            htmlBuilder.AppendLine("                        <th>Rareté</th>");
            htmlBuilder.AppendLine("                        <th>Repéré</th>");
            htmlBuilder.AppendLine("                        <th>Date de création</th>");
            htmlBuilder.AppendLine("                        <th>Localisation</th>");
            htmlBuilder.AppendLine("                        <th>Public</th>");
            htmlBuilder.AppendLine("                        <th>Superspot</th>");
            htmlBuilder.AppendLine("                    </tr>");
            htmlBuilder.AppendLine("                </thead>");
            htmlBuilder.AppendLine("                <tbody>");

            if (collections != null && collections.Count > 0)
            {
                foreach (var item in collections)
                {
                    // Trouver le modèle correspondant
                    var model = models?.FirstOrDefault(m => m.Id.ToString() == item.ModelId);
                    // Trouver la marque correspondante
                    var brand = model != null && model.BrandId.HasValue 
                        ? brands?.FirstOrDefault(b => b.Id == model.BrandId.Value)
                        : null;

                    htmlBuilder.AppendLine("                    <tr>");
                    htmlBuilder.AppendLine($"                        <td>{item.Id}</td>");
                    htmlBuilder.AppendLine($"                        <td>{(model?.Name ?? $"Modèle {item.ModelId}")}</td>");
                    htmlBuilder.AppendLine($"                        <td>{(brand?.Name ?? "Inconnue")}</td>");
                    htmlBuilder.AppendLine($"                        <td>{(model?.Rarity ?? "Inconnue")}</td>");
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
                htmlBuilder.AppendLine("                        <td colspan=\"9\" class=\"text-center\">Aucune donnée disponible</td>");
                htmlBuilder.AppendLine("                    </tr>");
            }

            htmlBuilder.AppendLine("                </tbody>");
            htmlBuilder.AppendLine("            </table>");
            htmlBuilder.AppendLine("        </div>");
            htmlBuilder.AppendLine("    </div>");

            // Table des modèles
            htmlBuilder.AppendLine("    <div class=\"container\">");
            htmlBuilder.AppendLine("        <h3>Modèles</h3>");
            htmlBuilder.AppendLine("        ");
            htmlBuilder.AppendLine("        <div class=\"table-responsive\">");
            htmlBuilder.AppendLine("            <table class=\"table table-striped\">");
            htmlBuilder.AppendLine("                <thead>");
            htmlBuilder.AppendLine("                    <tr>");
            htmlBuilder.AppendLine("                        <th>ID</th>");
            htmlBuilder.AppendLine("                        <th>Marque</th>");
            htmlBuilder.AppendLine("                        <th>Nom</th>");
            htmlBuilder.AppendLine("                        <th>Série</th>");
            htmlBuilder.AppendLine("                        <th>Génération</th>");
            htmlBuilder.AppendLine("                        <th>Moteur</th>");
            htmlBuilder.AppendLine("                        <th>Puissance</th>");
            htmlBuilder.AppendLine("                        <th>Rareté</th>");
            htmlBuilder.AppendLine("                    </tr>");
            htmlBuilder.AppendLine("                </thead>");
            htmlBuilder.AppendLine("                <tbody>");

            if (models != null && models.Count > 0)
            {
                foreach (var model in models)
                {
                    // Trouver la marque correspondante
                    var brand = model.BrandId.HasValue 
                        ? brands?.FirstOrDefault(b => b.Id == model.BrandId.Value)
                        : null;

                    htmlBuilder.AppendLine("                    <tr>");
                    htmlBuilder.AppendLine($"                        <td>{model.Id}</td>");
                    htmlBuilder.AppendLine($"                        <td>{(brand?.Name ?? "Inconnue")}</td>");
                    htmlBuilder.AppendLine($"                        <td>{model.Name}</td>");
                    htmlBuilder.AppendLine($"                        <td>{model.Series}</td>");
                    htmlBuilder.AppendLine($"                        <td>{model.GenerationName}</td>");
                    htmlBuilder.AppendLine($"                        <td>{model.Engine}</td>");
                    htmlBuilder.AppendLine($"                        <td>{model.Horsepower}</td>");
                    htmlBuilder.AppendLine($"                        <td>{model.Rarity}</td>");
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

            // Table des marques
            htmlBuilder.AppendLine("    <div class=\"container\">");
            htmlBuilder.AppendLine("        <h3>Marques</h3>");
            htmlBuilder.AppendLine("        ");
            htmlBuilder.AppendLine("        <div class=\"table-responsive\">");
            htmlBuilder.AppendLine("            <table class=\"table table-striped\">");
            htmlBuilder.AppendLine("                <thead>");
            htmlBuilder.AppendLine("                    <tr>");
            htmlBuilder.AppendLine("                        <th>ID</th>");
            htmlBuilder.AppendLine("                        <th>Nom</th>");
            htmlBuilder.AppendLine("                        <th>Pays</th>");
            htmlBuilder.AppendLine("                        <th>Date de création</th>");
            htmlBuilder.AppendLine("                    </tr>");
            htmlBuilder.AppendLine("                </thead>");
            htmlBuilder.AppendLine("                <tbody>");

            if (brands != null && brands.Count > 0)
            {
                foreach (var brand in brands)
                {
                    htmlBuilder.AppendLine("                    <tr>");
                    htmlBuilder.AppendLine($"                        <td>{brand.Id}</td>");
                    htmlBuilder.AppendLine($"                        <td>{brand.Name}</td>");
                    htmlBuilder.AppendLine($"                        <td>{brand.Country}</td>");
                    htmlBuilder.AppendLine($"                        <td>{brand.CreatedAt.ToString("yyyy-MM-dd")}</td>");
                    htmlBuilder.AppendLine("                    </tr>");
                }
            }
            else
            {
                htmlBuilder.AppendLine("                    <tr>");
                htmlBuilder.AppendLine("                        <td colspan=\"4\" class=\"text-center\">Aucune donnée disponible</td>");
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

        // Pour la rétrocompatibilité avec l'ancienne méthode
        public string GenerateHtmlTable(List<UserCollection> collections)
        {
            // Utiliser des données dummy pour les modèles et marques dans ce cas
            return GenerateHtmlTablesWithAllData(collections, GetDummyModels(), GetDummyBrands());
        }

        // Méthode publique pour accéder aux données factices depuis l'extérieur
        public List<UserCollection> GetDummyDataPublic()
        {
            return GetDummyCollections();
        }

        // Données fictives pour les collections
        private List<UserCollection> GetDummyCollections()
        {
            return new List<UserCollection>
            {
                new UserCollection
                {
                    Id = Guid.Parse("590ed93e-f03a-4974-8f51-b6c350d58141"),
                    UserId = "10000000-0000-0000-0000-000000000001",
                    ModelId = "1",     // 911 Carrera (Porsche)
                    Spotted = true,
                    CreatedAt = DateTime.Now.AddDays(-5),
                    DateCreated = DateTime.Now.AddDays(-5),
                    Location = "Paris, France",
                    IsPublic = true,
                    Superspot = false
                },
                new UserCollection
                {
                    Id = Guid.Parse("e7074003-e4c2-481c-a2f2-3ff2ce8b4738"),
                    UserId = "10000000-0000-0000-0000-000000000002",
                    ModelId = "2",     // M3 Competition (BMW)
                    Spotted = false,
                    CreatedAt = DateTime.Now.AddDays(-2),
                    DateCreated = DateTime.Now.AddDays(-2),
                    Location = "Monaco",
                    IsPublic = true,
                    Superspot = true
                },
                new UserCollection
                {
                    Id = Guid.Parse("d4c9a006-80b8-46af-ab6c-53902b155b94"),
                    UserId = "10000000-0000-0000-0000-000000000003",
                    ModelId = "3",     // Huracán Evo (Lamborghini)
                    Spotted = true,
                    CreatedAt = DateTime.Now.AddDays(-1),
                    DateCreated = DateTime.Now.AddDays(-1),
                    Location = "Milan, Italie",
                    IsPublic = false,
                    Superspot = false
                }
            };
        }

        // Données fictives pour les modèles
        private List<Model> GetDummyModels()
        {
            return new List<Model>
            {
                new Model
                {
                    Id = 1,
                    BrandId = 1,
                    Name = "911 Carrera",
                    Series = "911",
                    GenerationName = "992",
                    Engine = "3.0L Flat-6",
                    Horsepower = 385,
                    Rarity = "Common",
                    CreatedAt = DateTime.Now.AddDays(-120)
                },
                new Model
                {
                    Id = 2,
                    BrandId = 2,
                    Name = "M3 Competition",
                    Series = "M3",
                    GenerationName = "G80",
                    Engine = "3.0L Twin-Turbo",
                    Horsepower = 503,
                    Rarity = "Uncommon",
                    CreatedAt = DateTime.Now.AddDays(-90)
                },
                new Model
                {
                    Id = 3,
                    BrandId = 3,
                    Name = "Huracán Evo",
                    Series = "Huracán",
                    GenerationName = "Evo",
                    Engine = "5.2L V10",
                    Horsepower = 640,
                    Rarity = "Rare",
                    CreatedAt = DateTime.Now.AddDays(-60)
                }
            };
        }

        // Données fictives pour les marques
        private List<Brand> GetDummyBrands()
        {
            return new List<Brand>
            {
                new Brand
                {
                    Id = 1,
                    Name = "Porsche",
                    Country = "Allemagne",
                    CreatedAt = DateTime.Now.AddDays(-200)
                },
                new Brand
                {
                    Id = 2,
                    Name = "BMW",
                    Country = "Allemagne",
                    CreatedAt = DateTime.Now.AddDays(-190)
                },
                new Brand
                {
                    Id = 3,
                    Name = "Lamborghini",
                    Country = "Italie",
                    CreatedAt = DateTime.Now.AddDays(-180)
                }
            };
        }
    }
}