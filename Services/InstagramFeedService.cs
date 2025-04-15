using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using CarspotLourd.Models;

namespace CarspotLourd.Services
{
    public class InstagramFeedService
    {
        private readonly SupabaseDataService _dataService;

        public InstagramFeedService(SupabaseDataService dataService = null)
        {
            _dataService = dataService ?? new SupabaseDataService();
        }

        /// <summary>
        /// Génère un feed HTML au style Instagram avec des données réelles
        /// </summary>
        public async Task<string> GenerateInstagramStyleFeedAsync()
        {
            try
            {
                // Tenter de récupérer les données réelles de la base
                var spots = await GetRealDataAsync();
                if (spots != null && spots.Count > 0)
                {
                    return GenerateHtmlFeed(spots);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des données réelles: {ex.Message}");
                // En cas d'erreur, on utilise des données fictives
            }

            // Utiliser des données fictives en fallback
            var mockData = GenerateMockPosts();
            return GenerateHtmlFeed(mockData);
        }

        /// <summary>
        /// Récupère les données réelles depuis la base de données Supabase
        /// </summary>
        private async Task<List<CarPost>> GetRealDataAsync()
        {
            try
            {
                // Utiliser le dataService ou faire un appel direct à Supabase ici
                // Cette implémentation dépend de votre structure de données
                List<UserCollection> collections = await _dataService.GetUserCollectionsPublicAsync();
                var models = await _dataService.GetModelsDataAsync();
                var brands = await _dataService.GetBrandsDataAsync();

                // Convertir les objets UserCollection, Model et Brand en objets CarPost
                var carPosts = new List<CarPost>();
                
                foreach (var collection in collections)
                {
                    // Trouver le modèle correspondant
                    var model = models.FirstOrDefault(m => m.Id.ToString() == collection.ModelId);
                    if (model == null) continue;
                    
                    // Trouver la marque correspondante
                    var brand = brands.FirstOrDefault(b => b.Id == model.BrandId);
                    if (brand == null) continue;
                    
                    // Créer un CarPost avec les données réelles
                    carPosts.Add(new CarPost
                    {
                        Username = "CarspotUser" + collection.UserId.Substring(0, 4),  // Simplification - dans un cas réel, vous récupéreriez le vrai nom d'utilisateur
                        UserProfilePic = "https://randomuser.me/api/portraits/" + (new Random().Next(2) == 0 ? "men" : "women") + "/" + new Random().Next(1, 99) + ".jpg",
                        ImageUrl = "https://source.unsplash.com/featured/?" + brand.Name + "," + model.Name,  // Image aléatoire d'Unsplash
                        CarModel = model.Name,
                        CarBrand = brand.Name,
                        Rarity = model.Rarity,
                        Location = collection.Location,
                        PostDate = collection.DateCreated ?? collection.CreatedAt,
                        Description = $"A beautiful {brand.Name} {model.Name} spotted in {collection.Location}!",
                        LikesCount = 0,  // Mettre les statistiques à 0 comme demandé
                        CommentsCount = 0,
                        SharesCount = 0
                    });
                }
                
                return carPosts;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des données réelles: {ex.Message}");
                throw; // Propager l'erreur pour la gérer dans la méthode appelante
            }
        }

        /// <summary>
        /// Génère des données fictives pour le feed
        /// </summary>
        private List<CarPost> GenerateMockPosts()
        {
            return new List<CarPost>
            {
                new CarPost
                {
                    Username = "Kingnathanael9",
                    UserProfilePic = "https://randomuser.me/api/portraits/men/32.jpg",
                    ImageUrl = "https://www.topgear.com/sites/default/files/2022/08/TopGear%20-%20Bentley%20Continental%20GT%20-%20001.jpg",
                    CarModel = "Continental GT",
                    CarBrand = "Bentley",
                    Rarity = "Rare",
                    Location = "Paris, France",
                    PostDate = new DateTime(2025, 3, 23),
                    Description = "Spotted this beauty in downtown Paris! The Continental GT is truly a masterpiece of automotive design.",
                    LikesCount = 0,
                    CommentsCount = 0,
                    SharesCount = 0
                },
                new CarPost
                {
                    Username = "CarEnthusiast",
                    UserProfilePic = "https://randomuser.me/api/portraits/women/44.jpg",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/97/2018_Lamborghini_Aventador_S_LP740-4_Roadster_6.5.jpg/1200px-2018_Lamborghini_Aventador_S_LP740-4_Roadster_6.5.jpg",
                    CarModel = "Aventador S",
                    CarBrand = "Lamborghini",
                    Rarity = "Legendary",
                    Location = "Monaco",
                    PostDate = new DateTime(2025, 4, 5),
                    Description = "The Lamborghini Aventador in Monaco - a perfect combo of exotic car and exotic location!",
                    LikesCount = 0,
                    CommentsCount = 0,
                    SharesCount = 0
                },
                new CarPost
                {
                    Username = "AutoSpotter",
                    UserProfilePic = "https://randomuser.me/api/portraits/men/59.jpg",
                    ImageUrl = "https://cdn.motor1.com/images/mgl/NGz9gK/s3/porsche-911-gt3-rs.jpg",
                    CarModel = "911 GT3 RS",
                    CarBrand = "Porsche",
                    Rarity = "Rare",
                    Location = "Nürburgring, Germany",
                    PostDate = new DateTime(2025, 4, 8),
                    Description = "The ultimate track weapon - Porsche 911 GT3 RS spotted at the Green Hell!",
                    LikesCount = 0,
                    CommentsCount = 0,
                    SharesCount = 0
                }
            };
        }

        /// <summary>
        /// Génère le HTML pour le feed Instagram
        /// </summary>
        private string GenerateHtmlFeed(List<CarPost> posts)
        {
            var sb = new StringBuilder();
            
            // Entête HTML avec styles CSS
            sb.Append(@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Carspot Feed</title>
                <link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css'>
                <link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.0/font/bootstrap-icons.css'>
                <style>
                    body {
                        font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Helvetica, Arial, sans-serif;
                        background-color: #fafafa;
                        padding: 0;
                        margin: 0;
                    }
                    .header {
                        position: sticky;
                        top: 0;
                        background-color: white;
                        border-bottom: 1px solid #dbdbdb;
                        padding: 10px 15px;
                        display: flex;
                        justify-content: space-between;
                        align-items: center;
                        z-index: 1000;
                    }
                    .header h1 {
                        font-size: 24px;
                        font-weight: bold;
                        margin: 0;
                    }
                    .container {
                        max-width: 550px;
                        margin: 0 auto;
                        padding: 0;
                    }
                    .post {
                        background-color: white;
                        border: 1px solid #dbdbdb;
                        border-radius: 8px;
                        margin-bottom: 15px;
                        overflow: hidden;
                    }
                    .post-header {
                        display: flex;
                        align-items: center;
                        padding: 14px;
                        border-bottom: 1px solid #efefef;
                    }
                    .profile-pic {
                        width: 40px;
                        height: 40px;
                        border-radius: 50%;
                        margin-right: 10px;
                        object-fit: cover;
                    }
                    .username {
                        font-weight: 600;
                        color: #262626;
                    }
                    .location {
                        font-size: 12px;
                        color: #8e8e8e;
                        margin-top: 2px;
                    }
                    .post-image {
                        width: 100%;
                        height: auto;
                        object-fit: cover;
                        max-height: 500px;
                    }
                    .post-content {
                        padding: 12px;
                    }
                    .post-title {
                        display: flex;
                        justify-content: space-between;
                        align-items: center;
                        margin-bottom: 8px;
                    }
                    .car-model {
                        font-weight: 600;
                        font-size: 18px;
                        color: #262626;
                    }
                    .rarity-badge {
                        padding: 4px 12px;
                        border-radius: 20px;
                        font-size: 12px;
                        font-weight: 600;
                        color: white;
                    }
                    .rare {
                        background-color: #0095f6;
                    }
                    .legendary {
                        background: linear-gradient(45deg, #f09433, #e6683c, #dc2743, #cc2366, #bc1888);
                    }
                    .uncommon {
                        background-color: #00bf63;
                    }
                    .post-description {
                        margin-bottom: 12px;
                        color: #262626;
                    }
                    .post-date {
                        color: #8e8e8e;
                        font-size: 12px;
                        margin-bottom: 12px;
                    }
                    .post-actions {
                        display: flex;
                        border-top: 1px solid #efefef;
                        padding: 10px 0;
                    }
                    .action-button {
                        flex: 1;
                        text-align: center;
                        padding: 8px;
                        color: #262626;
                        display: flex;
                        justify-content: center;
                        align-items: center;
                    }
                    .action-count {
                        margin-left: 6px;
                    }
                    .action-icon {
                        font-size: 22px;
                    }
                    .create-button {
                        position: fixed;
                        bottom: 20px;
                        right: 20px;
                        width: 56px;
                        height: 56px;
                        border-radius: 28px;
                        background: linear-gradient(45deg, #f09433, #e6683c, #dc2743, #cc2366, #bc1888);
                        color: white;
                        display: flex;
                        align-items: center;
                        justify-content: center;
                        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.2);
                        border: none;
                        font-size: 24px;
                        z-index: 1000;
                    }
                </style>
            </head>
            <body>
                <div class='header'>
                    <h1>Carspot</h1>
                    <div class='header-actions'>
                        <i class='bi bi-box-arrow-right' id='logout' style='font-size: 24px; cursor: pointer;' title='Déconnexion'></i>
                    </div>
                </div>
                <div class='container'>
            ");

            // Générer chaque post
            foreach (var post in posts)
            {
                string rarityClass = post.Rarity.ToLower();
                sb.Append($@"
                    <div class='post'>
                        <div class='post-header'>
                            <img src='{post.UserProfilePic}' alt='{post.Username}' class='profile-pic'>
                            <div>
                                <div class='username'>{post.Username}</div>
                                <div class='location'>{post.Location}</div>
                            </div>
                            <div style='margin-left: auto;'>
                                <i class='bi bi-three-dots'></i>
                            </div>
                        </div>
                        <img src='{post.ImageUrl}' alt='{post.CarBrand} {post.CarModel}' class='post-image'>
                        <div class='post-content'>
                            <div class='post-title'>
                                <div class='car-model'>{post.CarBrand} {post.CarModel}</div>
                                <div class='rarity-badge {rarityClass}'>{post.Rarity}</div>
                            </div>
                            <div class='post-description'>{post.Description}</div>
                            <div class='post-date'>{post.PostDate.ToString("dd/MM/yyyy")}</div>
                        </div>
                    </div>
                ");
            }

            // Bouton de création et pied de page
            sb.Append(@"
                </div>
                <script>
                    // JavaScript pour le bouton de déconnexion
                    document.addEventListener('DOMContentLoaded', function() {
                        // Bouton de déconnexion
                        document.getElementById('logout').addEventListener('click', function() {
                            try {
                                window.location.href = 'csp://logout';
                            } catch (e) {
                                console.error('Erreur lors de la déconnexion:', e);
                            }
                        });
                    });
                </script>
            </body>
            </html>
            ");

            return sb.ToString();
        }
    }

    /// <summary>
    /// Classe représentant un post dans le feed Instagram
    /// </summary>
    public class CarPost
    {
        public string Username { get; set; }
        public string UserProfilePic { get; set; }
        public string ImageUrl { get; set; }
        public string CarModel { get; set; }
        public string CarBrand { get; set; }
        public string Rarity { get; set; }
        public string Location { get; set; }
        public DateTime PostDate { get; set; }
        public string Description { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public int SharesCount { get; set; }
    }
}