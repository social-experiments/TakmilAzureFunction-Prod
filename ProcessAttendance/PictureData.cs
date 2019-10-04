using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ProcessAttendance
{
    public class PictureData
    {
        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("latlong")]
        public string LatLong { get; set; }

        [JsonProperty("schoolName")]
        public string SchoolName { get; set; }

        [JsonProperty("className")]
        public string ClassName { get; set; }

        [JsonProperty("teacherName")]
        public string TeacherName { get; set; }

        [JsonProperty("pictureURL")]
        public string PictureURL { get; set; }

        [JsonProperty("pictureTimestamp")]
        public string PictureTimestamp { get; set; }
    }
}
