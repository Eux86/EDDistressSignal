using BusinessLayer.Interfaces;
using Entities.Interfaces;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class LocationService : ILocationService
    {
        Dictionary<string, ILocation> _playersLocation = new Dictionary<string, ILocation>();
        private IPlayerAuthService _authService;

        public LocationService()
        {
        }

        public LocationService(
            IPlayerAuthService authService)
        {
            _authService = authService;
        }

        public async void UpdatePlayerLocationAsync(string apiKey, ILocation location)
        {
            if (await _authService.AuthorizePlayerAsync(apiKey))
            {
                _playersLocation[apiKey] = location;
            }
            else
            {
                throw new Exception("Not authorized! Check API key");
            }
        }
    }
}
