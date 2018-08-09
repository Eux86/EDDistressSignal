using BusinessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class FakePlayerAuthService : IPlayerAuthService
    {
        public async Task<bool> AuthorizePlayerAsync(string ApiKey)
        {
            return true;
        }
    }
}
