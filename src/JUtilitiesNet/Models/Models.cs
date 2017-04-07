using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExtractor;

namespace JUtilitiesNet.Models
{
    public class Video
    {
        public string url { get; set; }
        public string title { get; set; }
        public string id { get; set; }

        public VideoInfo info { get; set; }
    }

    
}
