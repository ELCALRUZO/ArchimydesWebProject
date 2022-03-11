using Newtonsoft.Json;
using Newtonsoft.Json.Linq; 
using ArchimydesWeb.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ArchimydesWeb.Helpers
{
    public class General
    { 

        private readonly static string _errorFolder;
        //private readonly static string _Ips;
        

        static General()
        {
            var subDirectory = Directory.GetCurrentDirectory();

            _errorFolder = Path.Combine(subDirectory + @"\\AppLog\\");

            if (!Directory.Exists(_errorFolder))
            {
                Directory.CreateDirectory(_errorFolder);
            }
        }

        //public static string strPath;
        //public static string[] currentFileName;
        //public static string[] incomingFolderName;
        //public static string[] outgoingFolderName;
        //public static string[] ExpectedFolderList;
        //public static string FromFileLocation;
        //public static string ToFilelocation;
        //public static string ArchiveName;
        //public static string expectedFolderList;
        //public static string Server;
        //public static string Subject;
        //public static string MessageBody;
        //public static string From;
        //public static string To;
        //public static string Cc;
        //public static long Counter;
        //public static string GeneralReportSummary;



        //public static string GetIPAddressLocal()
        //{
        //    IPHostEntry Host = default(IPHostEntry);
        //    string Hostname = null;
        //    string IpAddress = null;
        //    Hostname = System.Environment.MachineName;
        //    Host = Dns.GetHostEntry(Hostname);
        //    foreach (IPAddress IP in Host.AddressList)
        //    {
        //        if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        //        {
        //            IpAddress = Convert.ToString(IP);
        //        }
        //    }
        //    return IpAddress;
        //}

        public static string GetCompleteExceptionMessage(Exception EX)
        {
            Exception exception = EX;
            string str = exception.Message;
            for (; exception.InnerException != null; exception = exception.InnerException)
                str = str + " because " + exception.InnerException.Message;
            return str;
        }


        protected static string BuildErrorMsg(Exception EX)
        {
            string str1 = "";
            string str2 = "";
            Exception baseException = EX.GetBaseException();
            if (EX == null)
                return "";
            string str3 = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss");
            string exceptionMessage = GetCompleteExceptionMessage(EX);
            if (baseException.TargetSite != (MethodBase)null)
                str1 = baseException.TargetSite.Name;
            if (baseException.StackTrace != null)
                str2 = EX.GetBaseException().StackTrace;
            return string.Format("\r\n\r\n[{0}]\r\n Subject: \t{1}\r\n Page Request: \t{2}\r\n Stack Trace : \t{3}", (object)str3, (object)exceptionMessage, (object)str1, (object)str2);
        }

        protected static string BuildStringErrorMsg(string lenghtErrorText)
        {
            string str = lenghtErrorText;
            if (lenghtErrorText == null)
                return "";
            return string.Format("\r\n\r\n[{0}]\r\n Stack Trace : \t{1}", new object[2]
            {
        (object) DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss"),
        (object) str
            });
        }

        public static void LogToFile(Exception EX)
        {
            try
            {
                File.AppendAllText(_errorFolder + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", BuildErrorMsg(EX));
            }
            catch (NullReferenceException ex)
            {
                LogToFileLenght(ex.ToString());
            }
            catch (FileNotFoundException ex)
            {
                LogToFileLenght(ex.ToString());
            }
            catch (Exception ex)
            {
                LogToFile(ex);
            }
        }



        //public static void LogToFile(string message, string path = "")
        //{
        //    try
        //    {
        //        string _path = _errorFolder + (string.IsNullOrEmpty(path) ? "\\Others" : $"\\{path}\\");
        //        if (!Directory.Exists(_path))
        //        {
        //            Directory.CreateDirectory(_path);
        //        }
        //        File.AppendAllText(_path + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", message);
        //    }
        //    catch (NullReferenceException ex)
        //    {
        //        LogToFileLenght(ex.ToString());
        //    }
        //    catch (FileNotFoundException ex)
        //    {
        //        LogToFileLenght(ex.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        LogToFile(ex);
        //    }
        //}

        public static void LogToFileLenght(string lenghtErrorText)
        {
            try
            {
                File.AppendAllText(_errorFolder + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", BuildStringErrorMsg(lenghtErrorText));
            }
            catch (NullReferenceException ex)
            {
                LogToFile(ex);
            }
            catch (FileNotFoundException ex)
            {
                LogToFile(ex);
            }
            catch (Exception ex)
            {
                LogToFile(ex);
            }
        }



        //-------Logging Errors------------------//

        public static string GetHash(string[] param)
        {
            try
            {
                string finalHashedString = string.Empty;
                string hashedString = string.Empty;
                foreach (var prm in param)
                {
                    hashedString += prm;
                }
                using (SHA512Managed shaManager = new SHA512Managed())
                {
                    Byte[] encryptedString = shaManager.ComputeHash(Encoding.UTF8.GetBytes(hashedString));
                    finalHashedString = BitConverter.ToString(encryptedString).Replace("-", "").ToUpper();
                }
                return finalHashedString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // SHA-512 Hash Code Generator Method
        public static string Get512Hash(string strInput)
        {
            SHA512 sha512 = new SHA512CryptoServiceProvider();

            //provide the string in byte format to the ComputeHash method. 
            //This method returns the SHA-512 hash code in byte array
            byte[] arrHash = sha512.ComputeHash(Encoding.UTF8.GetBytes(strInput));

            // use a Stringbuilder to append the bytes from the array to create a SHA-512 hash code string.
            StringBuilder sbHash = new StringBuilder();

            // Loop through byte array of the hashed code and format each byte as a hexadecimal code.
            for (int i = 0; i < arrHash.Length; i++)
            {
                sbHash.Append(arrHash[i].ToString("x2"));
            }

            // Return the hexadecimal SHA-512 hash code string.
            return sbHash.ToString();
        }

        

        public static string MakeRequest(string RequestURL, string RequestString,string RequestMethod)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(RequestURL) as HttpWebRequest;
                request.Method = RequestMethod;
                request.ContentType = "text";
                request.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR1.0.3705;)");
                //request.Headers.Add("Content-Type", "text/xml");
                //    byte[] bytes = Encoding.UTF8.GetBytes(RequestString);
                //    Stream requestStream = request.GetRequestStream();
                //    requestStream.Write(bytes, 0, bytes.Length);
                //    requestStream.Close();
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    return new StreamReader(response.GetResponseStream()).ReadToEnd();
                }

            }
            catch (Exception exception)
            {
                //WriteLog(exception.Message + exception.StackTrace, @"c:\Logs\Errors\");
                return "500";
            }
        }

        public static string MakeVFDRequest(string RequestURL, string RequestString = null, string RequestMethod = null,string token = null,string bodyRequest = null )
        {
            string baseRequestURL = "";
            if (RequestString == null)
            {
                baseRequestURL = RequestURL;
            }
            else
            {
                baseRequestURL = RequestURL + RequestString;
            }
            var client = new RestClient(baseRequestURL);
            RestRequest request = new RestRequest();
            
            if (RequestMethod.ToLower() == "get")
            {
                request = new RestRequest(Method.GET);
            }
            else if (RequestMethod.ToLower() == "post")
            {
                request = new RestRequest(Method.POST);
            }

            if (token != null && token != "")
            {
                request.AddHeader("Authorization", "Bearer " + token);
            }

            if (bodyRequest != null && bodyRequest != "")
            {
                request.AddParameter("application/json", bodyRequest, ParameterType.RequestBody);
            }

            request.AddHeader("Accept", "application/json");

            var response = client.Execute(request);

            var content = response.Content;

            return content;

        }

        public static string GetInvoiceRef(string transactionRef)
        {
            string refNumber = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 16).ToUpper();//"0000000000000000";
            //string refNumber = "00000000";// 00000000";
            //refNumber.Re
            if (!string.IsNullOrEmpty(transactionRef))
            {
                return refNumber.Substring(0, (refNumber.Length - transactionRef.ToString().Length) - 1);
            }
            return $"INV{refNumber}N".ToUpper();
        }

        public static DataTable JsonToDataTable(string jsonString)
        {
            var jsonLinq = JObject.Parse(jsonString);

            // Find the first array using Linq  
            var srcArray = jsonLinq.Descendants().Where(d => d is JArray).First();
            var trgArray = new JArray();
            foreach (JObject row in srcArray.Children<JObject>())
            {
                var cleanRow = new JObject();
                foreach (JProperty column in row.Properties())
                {
                    // Only include JValue types  
                    if (column.Value is JValue)
                    {
                        cleanRow.Add(column.Name, column.Value);
                    }
                }
                trgArray.Add(cleanRow);
            }

            return JsonConvert.DeserializeObject<DataTable>(trgArray.ToString());
        }
         

    }
}
