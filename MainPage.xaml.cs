using CarspotLourd.Services;
using System.IO;

namespace CarspotLourd
{
    public partial class MainPage : ContentPage
    {
        private SupabaseDataService _dataService;

        public MainPage()
        {
            InitializeComponent();
            _dataService = new SupabaseDataService();
            
            // S'abonner à l'événement de navigation terminée pour masquer l'indicateur de chargement
            webView.Navigated += WebView_Navigated;
            
            // Charger les données au démarrage
            LoadDataAsync();
        }

        private void WebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            // Masquer l'indicateur de chargement une fois que le WebView a chargé
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
        }

        private async void LoadDataAsync()
        {
            try
            {
                // Afficher l'indicateur de chargement
                loadingIndicator.IsRunning = true;
                loadingIndicator.IsVisible = true;

                // Récupérer les données depuis Supabase
                var collections = await _dataService.GetUserCollectionsAsync();
                
                // Générer le HTML avec les données
                string htmlContent = _dataService.GenerateHtmlTable(collections);
                
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
                var dummyData = _dataService.GetDummyDataPublic();
                string htmlContent = _dataService.GenerateHtmlTable(dummyData);
                
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
