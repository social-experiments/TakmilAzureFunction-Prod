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
    public static class TrainModel
    {
        private static string cognitiveServiceKey = System.Environment.GetEnvironmentVariable("CognitiveServiceKey");
        private static string endPoint = System.Environment.GetEnvironmentVariable("EndPoint");

        [FunctionName("TrainingPhoto")]
        public static async Task Run([BlobTrigger("takmiltraining/{blobName}",
            Connection = "AzureWebJobsStorage")]Stream inputBlob, string blobName,
            [Table("TakmilTable")] ICollector<ConnectionLog> outputTable,
            ILogger log)
        {
            await Task.CompletedTask;
            return;
        }
    }
}
