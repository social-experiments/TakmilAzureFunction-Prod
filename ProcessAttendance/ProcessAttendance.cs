using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ProcessAttendance
{
    public static class ProcessAttendance
    {
        private static string cognitiveServiceKey = System.Environment.GetEnvironmentVariable("CognitiveServiceKey");
        private static string endPoint = System.Environment.GetEnvironmentVariable("EndPoint");

        [FunctionName("ProcessAttendance")]
        public static async Task Run([BlobTrigger("takmil/{blobName}", 
            Connection = "AzureWebJobsStorage")]Stream inputBlob, string blobName,
            [Table("TakmilTable")] ICollector<ConnectionLog> outputTable,
            ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{blobName}\n Size: {inputBlob.Length} Bytes");

            blobName = Path.GetFileNameWithoutExtension(blobName);
            string[] nameParts = blobName.Split(new char[] { '_' });
            if (nameParts.Length != 5)
            {
                log.LogError("File name is in invalid format, expected A_B_C_D_E.json");
                return;
            }

            string schoolName = nameParts[0];
            string uploadTimeStr = nameParts[1] + "T" + nameParts[2];
            string orgName = nameParts[4];

            PictureData picture = GetPictureData(inputBlob, log);
            Tuple<int, int, int> t = ProcessPicture(picture.PictureURL, log);

            DateTime uploadTime = DateTime.UtcNow;
            DateTime createTime = DateTime.ParseExact(picture.PictureTimestamp, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

            ConnectionLog record = new ConnectionLog
            {
                PartitionKey = schoolName,
                RowKey = orgName + "-" + schoolName + "-" + uploadTimeStr,
                Location = picture.Location,
                LatLong = picture.LatLong,
                SchoolName = picture.SchoolName,
                ClassName = picture.ClassName,
                TeacherName = picture.TeacherName,
                NoOfBoys = t.Item1,
                NoOfGirls = t.Item2,
                NoOfStudents = t.Item3,
                PictureLocationURL = picture.PictureURL,
                CreationTimestamp = createTime,
                UploadTimestamp = uploadTime
            };

            outputTable.Add(record);
        }

        public static PictureData GetPictureData(Stream jsonBlob, ILogger log)
        {
            PictureData result = new PictureData();
            var serializer = new JsonSerializer();
            using (StreamReader reader = new StreamReader(jsonBlob))
            {
                using (var jsonTextReader = new JsonTextReader(reader))
                {
                    result = serializer.Deserialize<PictureData>(jsonTextReader);
                }
            }

            log.LogInformation($"Successfully parsed JSON blob");
            return result;
        }

        public static Tuple<int, int, int> ProcessPicture(string pictureURL, ILogger log)
        {
            return FaceDetector.Process(log, pictureURL, cognitiveServiceKey, endPoint);
        }
    }



    

    

    

    





}
