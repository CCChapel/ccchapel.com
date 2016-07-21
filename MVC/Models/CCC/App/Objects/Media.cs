using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CCC.Models.App.Objects
{
    public partial class Media
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum Formats
        {
            [EnumMember(Value = "aac")]
            AAC,

            [EnumMember(Value = "m3u")]
            M3U,

            [EnumMember(Value = "m4a")]
            M4A,

            [EnumMember(Value = "mp3")]
            MP3,

            [EnumMember(Value = "3gp")]
            _3GP,

            [EnumMember(Value = "m3u8")]
            M3U8,

            [EnumMember(Value = "m4v")]
            M4V,

            [EnumMember(Value = "mp4")]
            MP4,

            [EnumMember(Value = "rtsp")]
            RTSP
        }

        /// <summary>
        /// Specifies whether this media item can be downloaded and stored locally for offline consumption.
        /// Note: this value is overridden to false for streaming media.
        /// </summary>
        public bool Downloadable { get; set; }

        /// <summary>
        /// Specifies the total duration of this media item in milliseconds. Though optional, this property is important for media items with a variable bit rate, since the duration computation for VBR media is not handled correctly or uniformly by all operating systems and devices.
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Format for the provided media, which is often the same as the media file extension. Not all formats will be supported on every platform.
        /// 
        /// Explicit audio formats: aac, m3u, m4a, mp3
        /// Explicit video formats: 3gp, m3u8, m4v, mp4, rtsp
        /// 
        /// The app will attempt to play content with unrecognized formats, but because its type is unknown this content may appear as playable in both the audio player and the video player.
        /// 
        /// Dual-use formats such as m3u8 (which may contain audio+video streams, or audio only) are assumed to be video content.
        /// </summary>
        public Formats Format { get; set; }

        /// <summary>
        /// An array of IMAGE objects corresponding to the media item. This is commonly a screenshot or title photo (for video content), or album art (for audio content).
        /// </summary>
        public IEnumerable<Image> Images { get; set; }

        /// <summary>
        /// Arbitrary id string which identifies this media item. Using a GUID (Globally unique identifier) value is recommened (to guarantee proper tracking). While optional, to take advantage of Subsplash's play-tracking analytics system a sapId is required.
        /// </summary>
        public Guid SapID { get; set; }

        /// <summary>
        /// String describing the location of the media content.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Describes the size of video content. When multiple video MEDIA items are provided, an appropriately-sized video can be automatically selected for the current device and visual context.
        /// </summary>
        public int Width { get; set; }
    }
}