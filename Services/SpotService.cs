using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CarspotLourd.Models;
using Supabase.Interfaces;
using Postgrest.Responses;
using Supabase.Gotrue;
using Supabase.Realtime;
using Supabase.Storage;

namespace CarspotLourd.Services
{
    public class SpotService
    {
        private readonly Supabase.Client _supabaseClient;

        public SpotService(Supabase.Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        // Récupérer tous les spots
        public async Task<List<Spot>> GetAllSpots()
        {
            var response = await _supabaseClient
                .From<Spot>()
                .Get();
            return response.Models.ToList();
        }

        // Récupérer un spot par son ID
        public async Task<Spot> GetSpotById(Guid id)
        {
            var response = await _supabaseClient
                .From<Spot>()
                .Where(s => s.Id == id)
                .Single();
            return response;
        }

        // Récupérer les spots d'un utilisateur spécifique
        public async Task<List<Spot>> GetSpotsByUserId(Guid userId)
        {
            var response = await _supabaseClient
                .From<Spot>()
                .Where(s => s.UserId == userId)
                .Get();
            return response.Models.ToList();
        }

        // Récupérer les spots pour un modèle spécifique
        public async Task<List<Spot>> GetSpotsByModelId(long modelId)
        {
            var response = await _supabaseClient
                .From<Spot>()
                .Where(s => s.ModelId == modelId)
                .Get();
            return response.Models.ToList();
        }

        // Créer un nouveau spot
        public async Task<Spot> CreateSpot(Spot spot)
        {
            spot.CreatedAt = DateTime.UtcNow;
            var response = await _supabaseClient
                .From<Spot>()
                .Insert(spot);
            return response.Models.FirstOrDefault();
        }

        // Supprimer un spot
        public async Task DeleteSpot(Guid id)
        {
            await _supabaseClient
                .From<Spot>()
                .Where(s => s.Id == id)
                .Delete();
        }

        // Compter les likes pour un spot
        public async Task<int> GetLikesCount(Guid spotId)
        {
            var likes = await _supabaseClient
                .From<SpotLike>()
                .Where(l => l.SpotId == spotId)
                .Get();
            return likes.Models.Count();
        }

        // Compter les commentaires pour un spot
        public async Task<int> GetCommentsCount(Guid spotId)
        {
            var comments = await _supabaseClient
                .From<SpotComment>()
                .Where(c => c.SpotId == spotId)
                .Get();
            return comments.Models.Count();
        }

        // Compter les partages pour un spot
        public async Task<int> GetSharesCount(Guid spotId)
        {
            var shares = await _supabaseClient
                .From<SpotShare>()
                .Where(s => s.SpotId == spotId)
                .Get();
            return shares.Models.Count();
        }

        // Vérifier si un utilisateur a aimé un spot
        public async Task<bool> HasUserLikedSpot(Guid spotId, Guid userId)
        {
            var likes = await _supabaseClient
                .From<SpotLike>()
                .Where(l => l.SpotId == spotId && l.UserId == userId)
                .Get();
            return likes.Models.Any();
        }

        // Ajouter un like
        public async Task AddLike(Guid spotId, Guid userId)
        {
            var like = new SpotLike
            {
                Id = Guid.NewGuid(),
                SpotId = spotId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _supabaseClient
                .From<SpotLike>()
                .Insert(like);
        }

        // Supprimer un like
        public async Task RemoveLike(Guid spotId, Guid userId)
        {
            await _supabaseClient
                .From<SpotLike>()
                .Where(l => l.SpotId == spotId && l.UserId == userId)
                .Delete();
        }
    }
}