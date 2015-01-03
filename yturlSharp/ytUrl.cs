using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.Collections;

namespace yturlSharp
{
    /// <summary>
    /// This struct represents an itag reference. Thanks to https://github.com/cdown/yturl for the inspiration, and the itag data!
    /// </summary>
    struct ITagValue
    {
        public double vDimensions;
        public double vBitrate;
        public double aBitrate;
        public double aSamplerate;
        public string vEncoding;
    }

    /// <summary>
    /// This class provides a basic method of acquiring a streamable YouTube URL from a standard YouTube URL.
    /// </summary>
    class ytUrl
    {
        //This autoprop contains the dictionary of itags created in the ctor
        private Dictionary<String, ITagValue> itags { get; set; }

        /// <summary>
        /// Constructor which defines the itag collection
        /// </summary>
        public ytUrl()
        {
            //Instantiate collection of itag data
            itags = new Dictionary<string, ITagValue>() 
            { 
                { "5", new ITagValue() { vDimensions = 400*240, vBitrate=0.25,aBitrate=64,aSamplerate=22.05, vEncoding="h263" } },
                { "6", new ITagValue() { vDimensions = 480*270, vBitrate=0.8, aBitrate=64,aSamplerate=22.05, vEncoding="h263" } },
                { "13", new ITagValue() { vDimensions = 176*144, vBitrate=0.5, aBitrate=64,aSamplerate=22.05, vEncoding="mp4v" } },
                { "17", new ITagValue() { vDimensions = 176*144, vBitrate= 2, aBitrate = 64, aSamplerate=22.05, vEncoding= "mp4v" } },
                { "18", new ITagValue() { vDimensions = 640*360, vBitrate= 0.5, aBitrate = 96, aSamplerate =44.1, vEncoding= "h264" } },
                { "22", new ITagValue() { vDimensions = 1280*720, vBitrate= 2.9, aBitrate = 192, aSamplerate =44.1, vEncoding= "h264" } },
                { "34", new ITagValue() { vDimensions = 640*360, vBitrate= 0.5, aBitrate = 128, aSamplerate =44.1, vEncoding= "h264" } },
                { "35", new ITagValue() { vDimensions = 854*480, vBitrate= 1, aBitrate = 128, aSamplerate =44.1, vEncoding= "h264" } },
                { "36", new ITagValue() { vDimensions = 320*240, vBitrate= 0.17, aBitrate = 38, aSamplerate =44.1, vEncoding= "mp4v" } },
                { "37", new ITagValue() { vDimensions = 1920*1080, vBitrate= 2.9, aBitrate = 192, aSamplerate =44.1, vEncoding= "h264" } },
                { "38", new ITagValue() { vDimensions = 4096*3072, vBitrate= 5, aBitrate = 192, aSamplerate =44.1, vEncoding= "h264" } },                                                                                                                            
                { "43", new ITagValue() { vDimensions = 640*360, vBitrate= 0.5, aBitrate = 128, aSamplerate =44.1, vEncoding= "vp8" } },
                { "44", new ITagValue() { vDimensions = 854*480, vBitrate= 1, aBitrate = 128, aSamplerate =44.1, vEncoding= "vp8" } },
                { "45", new ITagValue() { vDimensions = 1280*720, vBitrate= 2, aBitrate = 192, aSamplerate =44.1, vEncoding= "vp8" } },                
                { "46", new ITagValue() { vDimensions = 1920*1080, vBitrate= 2, aBitrate = 192, aSamplerate =44.1, vEncoding= "vp8" } },
            };
        }

        /// <summary>
        /// Grabs a dictionary of ItagValue references, with their associated streaming URL for the provided YouTube URL.
        /// </summary>
        /// <param name="youtubeUrl">Any YouTube URL</param>
        /// <returns></returns>
        public Dictionary<ITagValue, Uri> GetStreamUrl(string youtubeUrl)
        {
            //Initialize the return collection
            var results = new Dictionary<ITagValue, Uri>();

            //Fetch the videoID from the query string
            string VideoID = VideoIDFromUrl(new Uri(youtubeUrl));

            //Fetch the raw youtubeinfo content from the 'api'
            var RawYouTubeInfo = new WebClient().DownloadString(new Uri("http://www.youtube.com/get_video_info?hl=en&video_id=" + VideoID + "&asv=3&el=detailpage&hl=en_US").ToString());
            
            //for each stream: 
            HttpUtility.ParseQueryString(RawYouTubeInfo).Get("url_encoded_fmt_stream_map").Split(',').ForEach(x =>
            {
                //Parse the query
                var QueryParse = HttpUtility.ParseQueryString(x);

                //Add a dictionary entry with a reference to the associated ITag value, and the Uri from the query
                results.Add(itags.Single(y => y.Key == QueryParse.Get("itag")).Value, new Uri(QueryParse.Get("url")));
            });

            //Return our collection
            return results;
        }

        /// <summary>
        /// Isolates the video id from the url's query string
        /// </summary>
        /// <param name="uri">YouTube Uri</param>
        /// <returns></returns>
        private string VideoIDFromUrl(Uri uri)
        {
            //Get the video ID from a youtube query string
            return HttpUtility.ParseQueryString(uri.Query).Get("v");
        }
    }

    /// <summary>
    /// Extension methods -- foreach grabbed from: http://extensionmethod.net/csharp/ienumerable-t/foreach-3
    /// </summary>
    public static class ExtensionMethods
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> array, Action<T> act)
        {
            foreach (var i in array)
                act(i);
            return array;
        }
    }
}
