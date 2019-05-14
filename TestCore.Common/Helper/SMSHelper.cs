using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace TestCore.Common.Helper
{
    public class SMSHelper
    {

        public static string Send(string smsUrl)
        {

            var request = WebRequest.Create(smsUrl) as HttpWebRequest;

            var result = string.Empty;

            if (request != null)
            {
                var task = request.GetResponseAsync();
                var wResp = task.Result;
                var respStream = wResp.GetResponseStream();
                using (var reader = new StreamReader(respStream))
                {
                    result = reader.ReadToEnd();
                }
                respStream.Dispose();
                wResp.Dispose();
            }

            return result;
        }

    }
     


}
