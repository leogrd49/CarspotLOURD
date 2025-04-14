using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CarspotLourd.Models;
using Newtonsoft.Json;

namespace CarspotLourd.Services
{
    public class HttpSupabaseService
    {
        private readonly string _supabaseUrl;
        private readonly string _supabaseKey;
        private readonly HttpClient _httpClient;

        public HttpSupabaseService(string supabaseUrl, string supabaseKey)
        {
            _supabaseUrl = supabaseUrl;
            _supabaseKey = supabaseKey;

            // Configurer HttpClient pour ignorer les erreurs de certificat SSL
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
            
            _httpClient = new HttpClient(handler);
            
            // Ajouter les en-têtes par défaut pour toutes les requêtes
            _httpClient.DefaultRequestHeaders.Add("apikey", _supabaseKey);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_supabaseKey}");
        }

        public async Task<List<UserCollection>> GetUserCollectionsAsync()
        {
            try
            {
                Console.WriteLine("Tentative de récupération des collections utilisateur via HTTP direct...");
                
                // Utiliser l'API Supabase directement
                string requestUrl = $"{_supabaseUrl}/rest/v1/user_collections?select=*";
                
                // Utiliser le bon en-tête pour le format de retour
                using var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                request.Headers.Add("Prefer", "return=representation");
                
                Console.WriteLine($"Envoi de la requête à {requestUrl}");
                var response = await _httpClient.SendAsync(request);
                
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Réponse reçue: {response.StatusCode}");
                Console.WriteLine($"Contenu: {content.Substring(0, Math.Min(content.Length, 500))}...");
                
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var collections = JsonConvert.DeserializeObject<List<UserCollection>>(content);
                        Console.WriteLine($"Désérialisation réussie: {collections.Count} éléments");
                        return collections;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erreur de désérialisation: {ex.Message}");
                        throw new Exception($"Erreur de désérialisation: {ex.Message}", ex);
                    }
                }
                else
                {
                    throw new Exception($"Erreur API: {response.StatusCode} - {content}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur HTTP: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                throw;
            }
        }
    }
}