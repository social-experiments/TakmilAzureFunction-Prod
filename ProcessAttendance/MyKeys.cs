using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessAttendance
{
    class MyKeys
    {
        string subscriptionkey = "";
        string faceEndpoint = "";

        public MyKeys(string cognitiveServicesKey, string endPoint)
        {
            //Replace your Azure Face API Subscription key and Faceendpoint location from portal instruction are here 
            this.Subscriptionkey = cognitiveServicesKey;
            this.FaceEndpoint = endPoint;

            // You must use the same region as you used to get your subscription
            // keys. For example, if you got your subscription keys from westus,
            // replace "westcentralus" with "westus".
            //
            // Free trial subscription keys are generated in the westcentralus
            // region. If you use a free trial subscription key, you can
            // use the region. ""https://westcentralus.api.cognitive.microsoft.com";
            // Specify the Azure region
            //this.FaceEndpoint = "https://eastus.api.cognitive.microsoft.com";
            //this.FaceEndpoint = "https://eastasia.api.cognitive.microsoft.com/";
        }

        public string Subscriptionkey
        {
            get
            {
                return this.subscriptionkey;
            }
            private set
            {
                this.subscriptionkey = value;
            }
        }

        public string FaceEndpoint
        {
            get
            {
                return this.faceEndpoint;
            }
            private set
            {
                this.faceEndpoint = value;
            }
        }
    }
}
