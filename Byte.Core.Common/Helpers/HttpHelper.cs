using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using Byte.Core.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Byte.Core.Common.Helpers
{
    public static class HttpHelper
    {
        /// <summary>
        /// 获取所有请求的参数（包括get参数和post参数）
        /// </summary>
        /// <param name="context">请求上下文</param>
        /// <returns></returns>
        public static Dictionary<string, object> GetAllRequestParams(HttpContext context)
        {
            Dictionary<string, object> allParams = new Dictionary<string, object>();

            var request = context.Request;
            List<string> paramKeys = new List<string>();
            var getParams = request.Query.Keys.ToList();
            var postParams = new List<string>();
            try
            {
                if (request.Method.ToLower() != "get")
                    postParams = request.Form.Keys.ToList();
            }
            catch
            {
                // ignored
            }

            paramKeys.AddRange(getParams);
            paramKeys.AddRange(postParams);

            paramKeys.ForEach(aParam =>
            {
                object value = null;
                if (request.Query.ContainsKey(aParam))
                {
                    value = request.Query[aParam].ToString();
                }
                else if (request.Form.ContainsKey(aParam))
                {
                    value = request.Form[aParam].ToString();
                }

                if (aParam == "Password")
                {
                    allParams.Add("Password", "********");
                }
                else
                {
                    allParams.Add(aParam, value);
                }
            });

            string contentType = request.ContentType?.ToLower() ?? "";

            //若为POST的application/json
            if (contentType.Contains("application/json"))
            {
                var stream = request.Body;
                string str = stream.ReadToString(Encoding.UTF8);

                if (!str.IsNullOrEmpty())
                {
                    var obj = str.ToJObject();
                    foreach (var aProperty in obj)
                    {
                        allParams[aProperty.Key] = aProperty.Value;
                    }
                }
            }

            return allParams;
        }
        #region 同步方法

        /// <summary>
        /// 使用Get方法获取字符串结果（加入Cookie）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string HttpGet(string url, Encoding encoding = null, int timeOut = 60000)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Timeout = timeOut;

            //if (cookieContainer != null)
            //{
            //    request.CookieContainer = cookieContainer;
            //}

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //if (cookieContainer != null)
            //{
            //    response.Cookies = cookieContainer.GetCookies(response.ResponseUri);
            //}

            using (Stream responseStream = response.GetResponseStream())
            {
                using (StreamReader myStreamReader = new StreamReader(responseStream, encoding ?? Encoding.GetEncoding("utf-8")))
                {
                    string retString = myStreamReader.ReadToEnd();
                    return retString;
                }
            }
        }

        /// <summary>
        /// 使用Post方法获取字符串结果，常规提交
        /// </summary>
        /// <returns></returns>
        public static string HttpPost(string url, Dictionary<string, string> formData = null, Encoding encoding = null, int timeOut = 60000, Dictionary<string, string> headers = null, Stream fileStream = null)
        {
            MemoryStream ms = new MemoryStream();
            formData.FillFormDataStream(ms);//填充formData
            return HttpPost(url, ms, ContentType.Form, encoding, headers, timeOut, fileStream);
        }

        /// <summary>
        /// 发送HttpPost请求，使用JSON格式传输数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string HttpPost(string url, string postData, Encoding encoding = null, Dictionary<string, string> headers = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            if (string.IsNullOrWhiteSpace(postData))
                throw new ArgumentNullException("postData");
            byte[] data = encoding.GetBytes(postData);
            MemoryStream stream = new MemoryStream();
            var formDataBytes = postData == null ? new byte[0] : Encoding.UTF8.GetBytes(postData);
            stream.Write(formDataBytes, 0, formDataBytes.Length);
            stream.Seek(0, SeekOrigin.Begin);//设置指针读取位置
            return HttpPost(url, stream, ContentType.Json, encoding, headers);
        }

        /// <summary>
        /// 使用POST请求数据，使用JSON传输数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dataObj">传输对象，转换为JSON传输</param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string HttpPost(string url, object dataObj, Encoding encoding = null, Dictionary<string, string> headers = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            if (dataObj == null)
                throw new ArgumentNullException("dataObj");
            string postData = JsonConvert.SerializeObject(dataObj, new JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd HH:mm:ss" });
            byte[] data = encoding.GetBytes(postData);
            MemoryStream stream = new MemoryStream();
            var formDataBytes = postData == null ? new byte[0] : Encoding.UTF8.GetBytes(postData);
            stream.Write(formDataBytes, 0, formDataBytes.Length);
            stream.Seek(0, SeekOrigin.Begin);//设置指针读取位置
            return HttpPost(url, stream, ContentType.Json, encoding, headers);
        }

        /// <summary>
        /// 使用Post方法获取字符串结果
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postStream"></param>
        /// <param name="contentType"></param>
        /// <param name="encoding"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static string HttpPost(string url, Stream postStream = null, ContentType type= ContentType.Json, Encoding encoding = null, Dictionary<string, string> headers = null, int timeOut = 60000, Stream fileStream =null)
        {
            var mapping = new Dictionary<ContentType, string>(){
            { ContentType.Form, "application/x-www-form-urlencoded" },
            { ContentType.Json, "application/json"}
            };

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Timeout = timeOut;

            request.ContentLength = postStream != null ? postStream.Length : 0;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.KeepAlive = false;

            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.87 Safari/537.36 QQBrowser/9.2.5748.400";
            request.ContentType = mapping[type];

            //request.Headers.Add("Access-Control-Allow-Origin","http://517best.com");
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            //直接写入流
            Stream requestStream = request.GetRequestStream();
            #region 输入二进制流
            if (postStream != null)
            {
                postStream.Position = 0;

                

                byte[] buffer = new byte[1024];
                int bytesRead = 0;
                while ((bytesRead = postStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    requestStream.Write(buffer, 0, bytesRead);
                }

                postStream.Close();//关闭文件访问
            }


                if (fileStream != null)
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead = 0;
                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        requestStream.Write(buffer, 0, bytesRead);
                    }
                    fileStream.Close();
                }
            
           

            #endregion

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream responseStream = response.GetResponseStream())
            {
                using (StreamReader myStreamReader = new StreamReader(responseStream, encoding ?? Encoding.GetEncoding("utf-8")))
                {
                    string retString = myStreamReader.ReadToEnd();
                    return retString;
                }
            }
        }

        #endregion

        public static void FillFormDataStream(this Dictionary<string, string> formData, Stream stream)
        {
            string dataString = GetQueryString(formData);
            var formDataBytes = formData == null ? new byte[0] : Encoding.UTF8.GetBytes(dataString);
            stream.Write(formDataBytes, 0, formDataBytes.Length);
            stream.Seek(0, SeekOrigin.Begin);//设置指针读取位置
        }

        /// <summary>
        /// 组装QueryString的方法
        /// 参数之间用&amp;连接，首位没有符号，如：a=1&amp;b=2&amp;c=3
        /// </summary>
        /// <param name="formData"></param>
        /// <returns></returns>
        public static string GetQueryString(this Dictionary<string, string> formData)
        {
            if (formData == null || formData.Count == 0)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            var i = 0;
            foreach (var kv in formData)
            {
                i++;
                sb.AppendFormat("{0}={1}", kv.Key, kv.Value);
                if (i < formData.Count)
                {
                    sb.Append("&");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// HttpWebRequest发送文件
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="paramName">文件参数名</param>
        /// <param name="contentType">contentType</param>
        /// <param name="nameValueCollection">其余要附带的参数键值对</param>
        public static void HttpUploadFile(string url, string filePath, string paramName, string contentType, NameValueCollection nameValueCollection)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";
            request.KeepAlive = true;
            request.Credentials = CredentialCache.DefaultCredentials;
            Stream requestStream = request.GetRequestStream();
            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in nameValueCollection.Keys)
            {
                requestStream.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, nameValueCollection[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                requestStream.Write(formitembytes, 0, formitembytes.Length);
            }
            requestStream.Write(boundarybytes, 0, boundarybytes.Length);
            string header = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n", paramName, filePath, contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            requestStream.Write(headerbytes, 0, headerbytes.Length);
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                requestStream.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();
            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            requestStream.Write(trailer, 0, trailer.Length);
            requestStream.Close();
            WebResponse webResponse = null;
            try
            {
                webResponse = request.GetResponse();
                Stream responseStream = webResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream);
                string result = streamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                if (webResponse != null)
                {
                    webResponse.Close();
                    webResponse = null;
                }
            }
            finally
            {
                request = null;
            }
        }

    }

    #region 类型定义

    /// <summary>
    /// Http请求方法定义
    /// </summary>
    public enum HttpMethod
    {
        Get,
        Post,
        Put,
        Delete,
        Head,
        Options,
        Trace,
        Connect
    }

    public enum ContentType
    {
        /// <summary>
        /// 传统Form表单,即application/x-www-form-urlencoded
        /// </summary>
        Form,

        /// <summary>
        /// 使用Json,即application/json
        /// </summary>
        Json
    }

    #endregion
}