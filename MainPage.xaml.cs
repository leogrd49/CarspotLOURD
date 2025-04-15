using CarspotLourd.Services;
using CarspotLourd.Models;
using System.IO;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration;

namespace CarspotLourd
{
    public partial class MainPage : ContentPage
    {
        private SupabaseDataService _dataService;
        private InstagramFeedService _instagramService;
        private bool _isInstagramView = true;

        public MainPage()
        {
            InitializeComponent();
            _dataService = new SupabaseDataService();
            _instagramService = new InstagramFeedService(_dataService);
            
            // S'abonner à l'événement de navigation terminée pour masquer l'indicateur de chargement
            webView.Navigated += WebView_Navigated;
            
            // Configuration du gestionnaire JavaScript
            ConfigureWebView();
            
            // Charger les données au démarrage (vue Instagram par défaut)
            LoadInstagramFeedAsync();
        }

        private void ConfigureWebView()
        {
            // S'abonner à l'événement Navigated pour injecter du JavaScript
            webView.Navigated += (sender, e) =>
            {
                // Injecter du JavaScript qui définit une fonction globale que nous pouvons appeler
                string script = @"
                function setupHandlers() {
                    var logoutBtn = document.getElementById('logout');
                    if (logoutBtn) {
                        logoutBtn.addEventListener('click', function() {
                            window.location.href = 'csp://logout';
                        });
                    }
                }
                if (document.readyState === 'complete') {
                    setupHandlers();
                } else {
                    window.addEventListener('load', setupHandlers);
                }";
                
                webView.EvaluateJavaScriptAsync(script);
            };

            // Gestionnaire de navigation pour intercepter les URLs csp://
            webView.Navigating += (sender, e) =>
            {
                if (e.Url.StartsWith("csp://logout"))
                {
                    // Annuler la navigation réelle
                    e.Cancel = true;
                    
                    // Gérer la déconnexion
                    MainThread.BeginInvokeOnMainThread(() => HandleLogout());
                }
            };
        }

        private void WebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            // Masquer l'indicateur de chargement une fois que le WebView a chargé
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
        }

        private void HandleLogout()
        {
            // Ici vous pourriez implémenter la logique de déconnexion
            // Par exemple, effacer les tokens, fermer la session, etc.
            DisplayAlert("Déconnexion", "Vous avez été déconnecté", "OK");
            
            // Vous pourriez rediriger vers une page de connexion ici
            // ou simplement rafraîchir la page actuelle
            RefreshCurrentView();
        }

        private void RefreshCurrentView()
        {
            LoadInstagramFeedAsync();
        }

        private async void LoadInstagramFeedAsync()
        {
            try
            {
                // Afficher l'indicateur de chargement
                loadingIndicator.IsRunning = true;
                loadingIndicator.IsVisible = true;

                // Générer le HTML du feed Instagram
                string htmlContent = await _instagramService.GenerateInstagramStyleFeedAsync();
                
                // Charger le HTML directement dans le WebView
                webView.Source = new HtmlWebViewSource 
                { 
                    Html = htmlContent
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement du feed Instagram: {ex.Message}");
                
                // En cas d'erreur, afficher un message à l'utilisateur
                await DisplayAlert("Erreur", "Une erreur est survenue lors du chargement des données.", "OK");
                
                // Masquer l'indicateur de chargement
                loadingIndicator.IsRunning = false;
                loadingIndicator.IsVisible = false;
            }
        }

        private async void LoadDataAsync()
        {
            try
            {
                // Afficher l'indicateur de chargement
                loadingIndicator.IsRunning = true;
                loadingIndicator.IsVisible = true;

                // Générer le HTML complet avec les trois tables
                string htmlContent = await _dataService.GenerateCompleteHtmlTableAsync();
                
                // Charger le HTML directement dans le WebView
                webView.Source = new HtmlWebViewSource 
                { 
                    Html = htmlContent
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement des données: {ex.Message}");
                
                // En cas d'erreur, utiliser des données factices
                var dummyCollections = _dataService.GetDummyDataPublic();
                var dummyModels = new List<Model>();
                var dummyBrands = new List<Brand>();
                string htmlContent = _dataService.GenerateHtmlTablesWithAllData(dummyCollections, dummyModels, dummyBrands);
                
                webView.Source = new HtmlWebViewSource 
                { 
                    Html = htmlContent
                };
                
                // Masquer l'indicateur de chargement
                loadingIndicator.IsRunning = false;
                loadingIndicator.IsVisible = false;
            }
        }
    }
}