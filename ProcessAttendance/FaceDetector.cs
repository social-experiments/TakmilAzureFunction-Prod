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
    class FaceDetector
    {
        // You must use the same region as you used to get your subscription
        // keys. For example, if you got your subscription keys from westus,
        // replace "westcentralus" with "westus".
        //
        // Free trial subscription keys are generated in the westcentralus
        // region. If you use a free trial subscription key, you shouldn't
        // need to change the region.
        // Specify the Azure region

        // localImagePath = @"C:\Documents\LocalImage.jpg"
        //private const string localImagePath = @"C:\Users\yadavm\Pictures\takmildemo.jpg";

        //private const string remoteImageUrl =
        //    "https://upload.wikimedia.org/wikipedia/commons/3/37/Dagestani_man_and_woman.jpg";

        //"https://takmil.org/wp-content/uploads/2018/03/img-20180203-wa0004.jpg";
        //"https://upload.wikimedia.org/wikipedia/commons/3/37/Dagestani_man_and_woman.jpg";

        private static readonly FaceAttributeType[] faceAttributes = { FaceAttributeType.Age, FaceAttributeType.Gender };

        private static int totalFaceDetected = 0;
        private static int femaleDetected = 0;
        private static int maleDetected = 0;

        public static Tuple<int, int, int> Process(ILogger log, string remoteImageUrl, string cognitiveServicesKey, string endPoint)
        {
            log.LogInformation("Welcome to Face API...");
            MyKeys mykey = new MyKeys(cognitiveServicesKey, endPoint);
            string subscriptionkey = mykey.Subscriptionkey;
            string faceEndpoint = mykey.FaceEndpoint;
            log.LogInformation(string.Format("subsKey {0}", subscriptionkey));
            log.LogInformation(string.Format("EndKey {0}", faceEndpoint));

            FaceClient faceClient = new FaceClient(
                new ApiKeyServiceClientCredentials(subscriptionkey),
                new System.Net.Http.DelegatingHandler[] { });
            faceClient.Endpoint = faceEndpoint;
            log.LogInformation("Face being detected ...");
            var t1 = DetectedRemoteAsync(log, faceClient, remoteImageUrl);
            //var t2 = DetectLocalAsync(faceClient, localImagePath);

            Task.WhenAll(t1).Wait(5000);
            //Task.WhenAll(t2).Wait(5000);
            //Console.WriteLine("Press any key to exit");
            //Console.ReadLine();

            return new Tuple<int, int, int>(maleDetected, femaleDetected, totalFaceDetected);
        }

        // It uses the Face service client to detect faces in a remote image, referenced by a URL. 
        // Note that it uses the faceAttributes field—the DetectedFace objects added to faceList will have the specified attributes (in this case, age and gender)
        private static async Task DetectedRemoteAsync(ILogger log, FaceClient faceClient, string ImageUrl)
        {
            log.LogInformation("Image URL: " + ImageUrl);
            if (!Uri.IsWellFormedUriString(ImageUrl, UriKind.Absolute))
            {
                log.LogError(string.Format("\n Invalid remote Imageurl : \n {0} \n", ImageUrl));
                return;
            }

            try
            {
                IList<DetectedFace> faceList =
                    await faceClient.Face.DetectWithUrlAsync(
                        ImageUrl, true, false, faceAttributes);
                DisplayAttributes(log, GetFaceAttributes(faceList, ImageUrl), ImageUrl);
                log.LogInformation(string.Format("Total Face detected :: {0}", totalFaceDetected));
                log.LogInformation(string.Format("Total Female detected :: {0}", femaleDetected));
                log.LogInformation(string.Format("Total Male detected :: {0}", maleDetected));
            }
            catch (APIErrorException e)
            {
                log.LogError("Error Processing the Image File");
                log.LogError(ImageUrl + ":" + e.Message);
            }
        }



        // the GetFaceAttributes method. It returns a string with the relevant attribute information.
        private static string GetFaceAttributes(IList<DetectedFace> faceList, string imagePath)
        {
            string attributes = string.Empty;
            maleDetected = 0;
            femaleDetected = 0;
            totalFaceDetected = 0;

            foreach (DetectedFace face in faceList)
            {
                double? age = face.FaceAttributes.Age;
                string gender = face.FaceAttributes.Gender.ToString();
                attributes += gender + " " + age + "   " + "\n";
                if (gender == "Female")
                    femaleDetected++;
                else
                    maleDetected++;
                totalFaceDetected++;
            }

            return attributes;
        }

        private static void DisplayAttributes(ILogger log, string attributes, string imageUri)
        {
            log.LogInformation(imageUri);
            log.LogInformation(attributes + "\n");
        }
    }
}
