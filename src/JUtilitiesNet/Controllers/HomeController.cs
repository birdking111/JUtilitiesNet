using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JUtilitiesNet.Models;
using YoutubeExtractor;
using System.Text.RegularExpressions;

namespace JUtilitiesNet.Controllers
{
    public class HomeController : Controller
    {
        public static List<Video> history = new List<Video>();

        public IActionResult Index()
        {
            Environment.GetEnvironmentVariables();

            return View("Index", history);
        }
        //[HttpPost]
        public IActionResult AddVideo(string vidURL)
        {
            //retrieve video info from url
            IEnumerable<VideoInfo> vInfo = DownloadUrlResolver.GetDownloadUrls(vidURL, false);

            //create video
            Video entry = new Video();
            entry.url = vidURL;
            entry.info = vInfo.Where(info => info.VideoType == VideoType.Mp4).First();

            /*Retireve video id from url*/
            entry.id = getID(vidURL);
            //add to the list
            history.Insert(0, entry);

            //trim to length of ten
            if (history.Count > 10)
            {
                history.RemoveAt(10);
                history.TrimExcess();
            }

            return RedirectToAction("Index");
        }

        //[HttpGet]
        //[Route("index")]
        public IActionResult RemoveVideo(int index)
        {
            history.RemoveAt(index);
            history.TrimExcess();

            return RedirectToAction("Index");
        }


        public IActionResult SelectVideo(int index)
        {
            //get the video specified
            Video selectedVid = history.ElementAt(index);

            //remove the video at the specified index
            history.RemoveAt(index);
            history.TrimExcess();

            //re-add the selected video to the top
            history.Insert(0, selectedVid);

            return RedirectToAction("Index");
        }

        private string getID(string url)
        {
            string vidID = null;

            //Created as Regex to process the url
            Regex idParser = new Regex("^.*((youtu.be\\/)|(v\\/)|(\\/u\\/\\w\\/)|(embed\\/)|(watch\\?))\\??v?=?([^#\\&\\?]*).*");

            if (idParser.IsMatch(url))
            {
                Match validUrl = idParser.Match(url);
                vidID = validUrl.Groups[7].Value;
            }

            return vidID;
        }

        /*public void downloadVideo()
        {
            //begin by getting the selected video, aka history at slot 0
            VideoInfo vid = history.ElementAt(0).info;

            //deal with any present decrypted signatures
            if(vid.RequiresDecryption)
            {
                DownloadUrlResolver.DecryptDownloadUrl(vid);
            }

            //set up a video downloader
            var videoDownloader = new VideoDownloader(vid, 
                Path.Combine( Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        }*/

        private static string RemoveIllegalPathCharacters(string path)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(path, "");

        }
    }
}
