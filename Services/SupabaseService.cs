using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CarspotLourd.Models;
using Supabase;
using Newtonsoft.Json;

namespace CarspotLourd.Services
{
    public class SupabaseService
    {
        private readonly Supabase.Client _supabaseClient;

        public SupabaseService(string supabaseUrl, string supabaseKey)
        {
            if (string.IsNullOrEmpty(supabaseUrl) || string.IsNullOrEmpty(supabaseKey))
            {
                throw new ArgumentException("Supabase URL et Supabase Key ne peuvent pas être vides.");
            }

            Console.WriteLine($"Connexion à Supabase: URL={supabaseUrl}");

            // Configurer TLS
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

            // Contournement des vérifications SSL pour l'environnement de développement
            ServicePointManager.ServerCertificateValidationCallback =
                (sender, certificate, chain, sslPolicyErrors) => true;

            var options = new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true
            };

            try
            {
                _supabaseClient = new Supabase.Client(supabaseUrl, supabaseKey, options);
                Console.WriteLine("Client Supabase initialisé avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'initialisation du client Supabase: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                throw;
            }
        }

        public async Task<List<UserCollection>> GetUserCollectionsAsync()
        {
            try
            {
                Console.WriteLine("Tentative de récupération des collections utilisateur...");

                var response = await _supabaseClient
                    .From<UserCollection>()
                    .Select("*")
                    .Get();

                Console.WriteLine($"Succès! {response.Models.Count} collections récupérées.");
                return response.Models;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des collections: {ex.Message}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    Console.WriteLine($"Inner exception stack trace: {ex.InnerException.StackTrace}");
                }

                // Retourner une liste vide au lieu de planter
                return new List<UserCollection>();
            }
        }
    }
}