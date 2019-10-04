using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessAttendance
{
    public class ConnectionLog
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Location { get; set; }
        public string LatLong { get; set; }
        public string SchoolName { get; set; }
        public string ClassName { get; set; }
        public string TeacherName { get; set; }
        public int NoOfBoys { get; set; }
        public int NoOfGirls { get; set; }
        public int NoOfStudents { get; set; }
        public string PictureLocationURL { get; set; }
        public DateTime CreationTimestamp { get; set; }
        public DateTime UploadTimestamp { get; set; }
    }
}
