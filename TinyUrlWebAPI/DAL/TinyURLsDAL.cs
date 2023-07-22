using System.Net;
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
                    LongUrl = WebUtility.UrlEncode(responseObj.destination.Trim()),
                    TinyUrl = responseObj.shortUrl.Trim(),
                    Alias = responseObj.slashtag.Trim(),
                    UserId = userId 
                };

                _context.UrlLinks.Add(urlLink);
                _context.SaveChanges();
            }

        }
        public List<UrlLink> GetUserData(int userId)
        {
            
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                using (var dbContext = new SennheiserTinyUrlsContext())
                {
                    var urlLinks = dbContext.UrlLinks.Where(link => link.UserId == userId).ToList();
                    return urlLinks;
                }
            }
            return null;

        }

    }
}
