using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TinyUrlWebAPI.Models;

namespace TinyUrlWebAPI.DAL
{
    public class TinyURLsDAL
    {
        private readonly SennheiserTinyUrlsContext _context;

        public TinyURLsDAL(SennheiserTinyUrlsContext context)
        {
            _context = context;
        }
        public void AddUserData(TinyURLapiResponse responseObj, int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                UrlLink urlLink = new UrlLink
                {
                    LongUrl = responseObj.destination,
                    TinyUrl = responseObj.shortUrl,
                    Alias = responseObj.slashtag,
                    UserId = userId 
                };

                _context.UrlLinks.Add(urlLink);
                _context.SaveChanges();
            }

        }

    }
}
