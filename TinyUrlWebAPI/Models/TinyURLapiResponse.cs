﻿namespace TinyUrlWebAPI.Models
{
    public class TinyURLapiResponse
    {
        public int userid { get; set; }
        public string statusCode { get; set; }
        public string shortUrl { get; set; }
        public string destination { get; set; }
        public string slashtag { get; set; }
    }
}