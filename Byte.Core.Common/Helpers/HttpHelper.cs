using Byte.Core.Common.Extensions;
using System.Text;

namespace Byte.Core.Common.Helpers
{
    public static class HttpHelper
    {
        public static async Task<string> PostAsync() {
            throw new NotImplementedException("系统提供http请求方法,此处作为Dome,参考");
            //using var httpClient = new HttpClient();
            //var requestContent = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
            //httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer my-token");
            //// send POST request
            //var response = await httpClient.PostAsync(url, requestContent);
           
            //// read response content
            //var responseContent = await response.Content.ReadAsStringAsync();
            ////获取里面内容
            //dynamic data = responseContent.ToObject<dynamic>();
            //return responseContent;

        }

        public static async Task<string> GetAsync(string url, Dictionary<string, string> headers = null,int timeout=60000)
        {
            using var httpClient = new HttpClient();
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
            httpClient.Timeout = TimeSpan.FromMilliseconds(timeout); 
             var response = await httpClient.GetAsync(url);
            // read response content
            var responseContent = await response.Content.ReadAsStringAsync();
            //获取里面内容
            dynamic data = responseContent.ToObject<dynamic>();
            return responseContent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramStr"> 使用tojson 转换</param>
        /// <param name="headers"></param>
        /// <param name="encoding"></param>
        /// <param name="mediaType"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static async Task<string> PostAsync(string url,string paramStr, Dictionary<string, string> headers = null, Encoding encoding = null, string mediaType = "application/json", int timeout = 60000)
        {
            using var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromMilliseconds(timeout);
            var requestContent = new StringContent(paramStr, encoding, mediaType);
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
        
            var response = await httpClient.PostAsync(url, requestContent);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
            }
            else
            {
                Console.WriteLine($"Request failed with status code {response.StatusCode}");
            }

            // read response content
            var responseContent = await response.Content.ReadAsStringAsync();
         
            return responseContent;
        }

        //#region 同步方法

        ///// <summary>
        ///// 使用Get方法获取字符串结果（加入Cookie）
        ///// </summary>
        ///// <param name="url"></param>
        ///// <param name="encoding"></param>
        ///// <returns></returns>
        //public static string HttpGet(string url, Encoding encoding = null, int timeOut = 60000)
        //{
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //    request.Method = "GET";
        //    request.Timeout = timeOut;

        //    //if (cookieContainer != null)
        //    //{
        //    //    request.CookieContainer = cookieContainer;
        //    //}

        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        //    //if (cookieContainer != null)
        //    //{
        //    //    response.Cookies = cookieContainer.GetCookies(response.ResponseUri);
        //    //}

        //    using (Stream responseStream = response.GetResponseStream())
        //    {
        //        using (StreamReader myStreamReader = new StreamReader(responseStream, encoding ?? Encoding.GetEncoding("utf-8")))
        //        {
        //            string retString = myStreamReader.ReadToEnd();
        //            return retString;
        //        }
        //    }
        //}
        ///// <summary>
        ///// (异步)使用Get方法获取字符串结果（加入Cookie）
        ///// </summary>
        ///// <param name="url"></param>
        ///// <param name="encoding"></param>
        ///// <returns></returns>
        //public static async Task<string> HttpGetAsync(string url, Encoding encoding = null, int timeOut = 60000)
        //{
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //    request.Method = "GET";
        //    request.Timeout = timeOut;

        //    //if (cookieContainer != null)
        //    //{
        //    //    request.CookieContainer = cookieContainer;
        //    //}

        //    HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync());

        //    //if (cookieContainer != null)
        //    //{
        //    //    response.Cookies = cookieContainer.GetCookies(response.ResponseUri);
        //    //}

        //    using (Stream responseStream = response.GetResponseStream())
        //    {
        //        using (StreamReader myStreamReader = new StreamReader(responseStream, encoding ?? Encoding.GetEncoding("utf-8")))
        //        {
        //            string retString = await myStreamReader.ReadToEndAsync();
        //            return retString;
        //        }
        //    }
        //}

        ///// <summary>
        ///// 使用Post方法获取字符串结果，常规提交
        ///// </summary>
        ///// <returns></returns>
        //public static string HttpPost(string url, Dictionary<string, string> formData = null, Encoding encoding = null, int timeOut = 60000, Dictionary<string, string> headers = null)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    formData.FillFormDataStream(ms);//填充formData
        //    return HttpPost(url, ms, "application/x-www-form-urlencoded", encoding, headers, timeOut);
        //}

        ///// <summary>
        ///// 发送HttpPost请求，使用JSON格式传输数据
        ///// </summary>
        ///// <param name="url"></param>
        ///// <param name="postData"></param>
        ///// <param name="encoding"></param>
        ///// <returns></returns>
        //public static string HttpPost(string url, string postData, Encoding encoding = null, Dictionary<string, string> headers = null)
        //{
        //    if (encoding == null)
        //        encoding = Encoding.UTF8;
        //    if (string.IsNullOrWhiteSpace(postData))
        //        throw new ArgumentNullException("postData");
        //    byte[] data = encoding.GetBytes(postData);
        //    MemoryStream stream = new MemoryStream();
        //    var formDataBytes = postData == null ? new byte[0] : Encoding.UTF8.GetBytes(postData);
        //    stream.Write(formDataBytes, 0, formDataBytes.Length);
        //    stream.Seek(0, SeekOrigin.Begin);//设置指针读取位置
        //    return HttpPost(url, stream, "application/json", encoding, headers);
        //}

        ///// <summary>
        ///// 使用POST请求数据，使用JSON传输数据
        ///// </summary>
        ///// <param name="url"></param>
        ///// <param name="dataObj">传输对象，转换为JSON传输</param>
        ///// <param name="encoding"></param>
        ///// <returns></returns>
        //public static string HttpPost(string url, object dataObj, Encoding encoding = null, Dictionary<string, string> headers = null)
        //{
        //    if (encoding == null)
        //        encoding = Encoding.UTF8;
        //    if (dataObj == null)
        //        throw new ArgumentNullException("dataObj");
        //    string postData = JsonConvert.SerializeObject(dataObj, new JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd HH:mm:ss" });
        //    byte[] data = encoding.GetBytes(postData);
        //    MemoryStream stream = new MemoryStream();
        //    var formDataBytes = postData == null ? new byte[0] : Encoding.UTF8.GetBytes(postData);
        //    stream.Write(formDataBytes, 0, formDataBytes.Length);
        //    stream.Seek(0, SeekOrigin.Begin);//设置指针读取位置
        //    return HttpPost(url, stream, "application/json", encoding, headers);
        //}

        ///// <summary>
        ///// 使用Post方法获取字符串结果
        ///// </summary>
        ///// <param name="url"></param>
        ///// <param name="postStream"></param>
        ///// <param name="contentType"></param>
        ///// <param name="encoding"></param>
        ///// <param name="timeOut"></param>
        ///// <returns></returns>
        //public static string HttpPost(string url, Stream postStream = null, string contentType = "application/x-www-form-urlencoded", Encoding encoding = null, Dictionary<string, string> headers = null, int timeOut = 60000)
        //{
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //    request.Method = "POST";
        //    request.Timeout = timeOut;

        //    request.ContentLength = postStream != null ? postStream.Length : 0;
        //    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
        //    request.KeepAlive = false;

        //    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.87 Safari/537.36 QQBrowser/9.2.5748.400";
        //    request.ContentType = contentType;

        //    //request.Headers.Add("Access-Control-Allow-Origin","http://517best.com");
        //    if (headers != null)
        //    {
        //        foreach (var header in headers)
        //        {
        //            request.Headers.Add(header.Key, header.Value);
        //        }
        //    }

        //    #region 输入二进制流
        //    if (postStream != null)
        //    {
        //        postStream.Position = 0;

        //        //直接写入流
        //        Stream requestStream = request.GetRequestStream();

        //        byte[] buffer = new byte[1024];
        //        int bytesRead = 0;
        //        while ((bytesRead = postStream.Read(buffer, 0, buffer.Length)) != 0)
        //        {
        //            requestStream.Write(buffer, 0, bytesRead);
        //        }

        //        postStream.Close();//关闭文件访问
        //    }
        //    #endregion

        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        //    using (Stream responseStream = response.GetResponseStream())
        //    {
        //        using (StreamReader myStreamReader = new StreamReader(responseStream, encoding ?? Encoding.GetEncoding("utf-8")))
        //        {
        //            string retString = myStreamReader.ReadToEnd();
        //            return retString;
        //        }
        //    }
        //}

        ///// <summary>
        ///// (异步)使用Post方法获取字符串结果，常规提交
        ///// </summary>
        ///// <returns></returns>
        //public static async Task<string> HttpPostAsync(string url, Dictionary<string, string> formData = null, string contentType = "application/json",  Encoding encoding = null, int timeOut = 60000, Dictionary<string, string> headers = null)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    formData.FillFormDataStream(ms);//填充formData
        //    return await HttpPostAsync(url, ms, contentType, encoding, headers, timeOut);
        //}

        ///// <summary>
        ///// (异步)发送HttpPost请求，使用JSON格式传输数据
        ///// </summary>
        ///// <param name="url"></param>
        ///// <param name="postData"></param>
        ///// <param name="encoding"></param>
        ///// <returns></returns>
        //public static async Task<string> HttpPostAsync(string url, string postData, string contentType = "application/json", Encoding encoding = null, Dictionary<string, string> headers = null)
        //{
        //    if (encoding == null)
        //        encoding = Encoding.UTF8;
        //    if (string.IsNullOrWhiteSpace(postData))
        //        throw new ArgumentNullException("postData");
        //    byte[] data = encoding.GetBytes(postData);
        //    MemoryStream stream = new MemoryStream();
        //    var formDataBytes = postData == null ? new byte[0] : Encoding.UTF8.GetBytes(postData);
        //    await stream.WriteAsync(formDataBytes, 0, formDataBytes.Length);
        //    stream.Seek(0, SeekOrigin.Begin);//设置指针读取位置
        //    return await HttpPostAsync(url, stream, contentType, encoding, headers);
        //}

        ///// <summary>
        ///// (异步)使用POST请求数据，使用JSON传输数据
        ///// </summary>
        ///// <param name="url"></param>
        ///// <param name="dataObj">传输对象，转换为JSON传输</param>
        ///// <param name="encoding"></param>
        ///// <returns></returns>
        //public static async Task<string> HttpPostAsync(string url, object dataObj, string contentType = "application/json", Encoding encoding = null, Dictionary<string, string> headers = null)
        //{
        //    if (encoding == null)
        //        encoding = Encoding.UTF8;
        //    if (dataObj == null)
        //        throw new ArgumentNullException("dataObj");
        //    string postData = JsonConvert.SerializeObject(dataObj, new JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd HH:mm:ss" });
        //    byte[] data = encoding.GetBytes(postData);
        //    MemoryStream stream = new MemoryStream();
        //    var formDataBytes = postData == null ? new byte[0] : Encoding.UTF8.GetBytes(postData);
        //    await stream.WriteAsync(formDataBytes, 0, formDataBytes.Length);
        //    stream.Seek(0, SeekOrigin.Begin);//设置指针读取位置
        //    return await HttpPostAsync(url, stream, contentType, encoding, headers);
        //}



        ///// <summary>
        ///// (异步)使用Post方法获取字符串结果
        ///// </summary>
        ///// <param name="url"></param>
        ///// <param name="postStream"></param>
        ///// <param name="contentType"></param>
        ///// <param name="encoding"></param>
        ///// <param name="timeOut"></param>
        ///// <returns></returns>
        //public static async Task<string>  HttpPostAsync(string url, Stream postStream = null, string contentType = "application/x-www-form-urlencoded", Encoding encoding = null, Dictionary<string, string> headers = null, int timeOut = 60000)
        //{
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.CreateHttp(url);
        //    request.Method = "POST";
        //    request.Timeout = timeOut;

        //    request.ContentLength = postStream != null ? postStream.Length : 0;
        //    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
        //    request.KeepAlive = false;

        //    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.87 Safari/537.36 QQBrowser/9.2.5748.400";
        //    request.ContentType = contentType;

        //    //request.Headers.Add("Access-Control-Allow-Origin","http://517best.com");
        //    if (headers != null)
        //    {
        //        foreach (var header in headers)
        //        {
        //            request.Headers.Add(header.Key, header.Value);
        //        }
        //    }

        //    #region 输入二进制流
        //    if (postStream != null)
        //    {
        //        postStream.Position = 0;

        //        //直接写入流
        //        Stream requestStream =await request.GetRequestStreamAsync();

        //        byte[] buffer = new byte[1024];
        //        int bytesRead = 0;
        //        while ((bytesRead =await postStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
        //        {
        //            requestStream.Write(buffer, 0, bytesRead);
        //        }

        //        postStream.Close();//关闭文件访问
        //    }
        //    #endregion

        //    HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync());

        //    using (Stream responseStream = response.GetResponseStream())
        //    {
        //        using (StreamReader myStreamReader = new StreamReader(responseStream, encoding ?? Encoding.GetEncoding("utf-8")))
        //        {
        //            string retString = await myStreamReader.ReadToEndAsync();
        //            return retString;
        //        }
        //    }
        //}

        //public static string HttpPostFile(UploadParameterType parameter)
        //{
        //    using (MemoryStream memoryStream = new MemoryStream())
        //    {

        //        // 1.分界线
        //        string boundary = string.Format("----{0}", DateTime.Now.Ticks.ToString("x")),       // 分界线可以自定义参数
        //            beginBoundary = string.Format("--{0}\r\n", boundary),
        //            endBoundary = string.Format("\r\n--{0}--\r\n", boundary);
        //        byte[] beginBoundaryBytes = parameter.Encoding.GetBytes(beginBoundary),
        //            endBoundaryBytes = parameter.Encoding.GetBytes(endBoundary);
        //        // 2.组装开始分界线数据体 到内存流中
        //        memoryStream.Write(beginBoundaryBytes, 0, beginBoundaryBytes.Length);
        //        // 3.组装 上传文件附加携带的参数 到内存流中
        //        if (parameter.PostParameters != null && parameter.PostParameters.Count > 0)
        //        {
        //            foreach (KeyValuePair<string, string> keyValuePair in parameter.PostParameters)
        //            {
        //                string parameterHeaderTemplate = string.Format("Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}\r\n{2}", keyValuePair.Key, keyValuePair.Value, beginBoundary);
        //                byte[] parameterHeaderBytes = parameter.Encoding.GetBytes(parameterHeaderTemplate);

        //                memoryStream.Write(parameterHeaderBytes, 0, parameterHeaderBytes.Length);
        //            }
        //        }

        //        // 4.组装文件头数据体 到内存流中
        //        string fileHeaderTemplate = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n", parameter.FileNameKey, parameter.FileNameValue);
        //        byte[] fileHeaderBytes = parameter.Encoding.GetBytes(fileHeaderTemplate);
        //        memoryStream.Write(fileHeaderBytes, 0, fileHeaderBytes.Length);
        //        // 5.组装文件流 到内存流中
        //        byte[] buffer = new byte[1024 * 1024 * 1];
        //        int size = parameter.UploadStream.Read(buffer, 0, buffer.Length);
        //        while (size > 0)
        //        {
        //            memoryStream.Write(buffer, 0, size);
        //            size = parameter.UploadStream.Read(buffer, 0, buffer.Length);
        //        }



        //        // 6.组装结束分界线数据体 到内存流中
        //        memoryStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
        //        // 7.获取二进制数据
        //        byte[] postBytes = memoryStream.ToArray();
        //        memoryStream.Close();
        //        GC.Collect();
        //        // 8.HttpWebRequest 组装
        //        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(parameter.Url, UriKind.RelativeOrAbsolute));
        //        webRequest.AllowWriteStreamBuffering = false;
        //        webRequest.Method = "POST";
        //        webRequest.Timeout = 1800000;
        //        webRequest.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
        //        webRequest.ContentLength = postBytes.Length;

        //        // 9.写入上传请求数据
        //        using (Stream requestStream = webRequest.GetRequestStream())
        //        {
        //            requestStream.Write(postBytes, 0, postBytes.Length);
        //            requestStream.Close();
        //        }
        //        // 10.获取响应
        //        using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
        //        {
        //            using (StreamReader reader = new StreamReader(webResponse.GetResponseStream(), parameter.Encoding))
        //            {
        //                string body = reader.ReadToEnd();
        //                reader.Close();
        //                return body;
        //            }
        //        }
        //    }
        //}

        //#endregion

        //public static void FillFormDataStream(this Dictionary<string, string> formData, Stream stream)
        //{
        //    string dataString = GetQueryString(formData);
        //    var formDataBytes = formData == null ? new byte[0] : Encoding.UTF8.GetBytes(dataString);
        //    stream.Write(formDataBytes, 0, formDataBytes.Length);
        //    stream.Seek(0, SeekOrigin.Begin);//设置指针读取位置
        //}

        ///// <summary>
        ///// 组装QueryString的方法
        ///// 参数之间用&amp;连接，首位没有符号，如：a=1&amp;b=2&amp;c=3
        ///// </summary>
        ///// <param name="formData"></param>
        ///// <returns></returns>
        //public static string GetQueryString(this Dictionary<string, string> formData)
        //{
        //    if (formData == null || formData.Count == 0)
        //    {
        //        return "";
        //    }
        //    StringBuilder sb = new StringBuilder();
        //    var i = 0;
        //    foreach (var kv in formData)
        //    {
        //        i++;
        //        sb.AppendFormat("{0}={1}", kv.Key, kv.Value);
        //        if (i < formData.Count)
        //        {
        //            sb.Append("&");
        //        }
        //    }
        //    return sb.ToString();
        //}



        /////// <summary>
        /////// 判断是否为AJAX请求
        /////// </summary>
        /////// <param name="req"></param>
        /////// <returns></returns>
        ////public static bool IsAjaxRequest(this HttpRequest req)
        ////{
        ////    bool result = false;

        ////    var xreq = req.Headers.ContainsKey("x-requested-with");
        ////    if (xreq)
        ////    {
        ////        result = req.Headers["x-requested-with"] == "XMLHttpRequest";
        ////    }

        ////    return result;
        ////}

        ///// <summary>
        ///// 获取去掉查询参数的Url
        ///// </summary>
        ///// <param name="req">请求</param>
        ///// <returns></returns>
        //public static string GetDisplayUrlNoQuery(this HttpRequest req)
        //{
        //    var queryStr = req.QueryString.ToString();
        //    var displayUrl = req.GetDisplayUrl();

        //    return queryStr.IsNullOrEmpty() ? displayUrl : displayUrl.Replace(queryStr, "");
        //}

        ///// <summary>
        ///// 获取Token
        ///// </summary>
        ///// <param name="req">请求</param>
        ///// <returns></returns>
        //public static string GetToken(this HttpRequest req)
        //{
        //    string tokenHeader = req.Headers["Authorization"].ToString();
        //    if (tokenHeader.IsNullOrEmpty())
        //        return null;

        //    string pattern = "^Bearer (.*?)$";
        //    if (!Regex.IsMatch(tokenHeader, pattern))
        //        throw new Exception("token格式不对!格式为:Bearer {token}");

        //    string token = Regex.Match(tokenHeader, pattern).Groups[1]?.ToString();
        //    if (token.IsNullOrEmpty())
        //        throw new Exception("token不能为空!");

        //    return token;
        //}

        ///// <summary>
        ///// 获取Token中的Payload
        ///// </summary>
        ///// <param name="req">请求</param>
        ///// <returns></returns>
        //public static JWTPayload GetJWTPayload(this HttpRequest req)
        //{
        //    string token = req.GetToken();
        //    var payload = JWTHelper.GetPayload<JWTPayload>(token);

        //    return payload;
        //}
        ///// <summary>
        ///// 上传文件 - 请求参数类
        ///// </summary>
        //public class UploadParameterType
        //{
        //    public UploadParameterType()
        //    {
        //        FileNameKey = "fileName";
        //        Encoding = Encoding.UTF8;
        //        PostParameters = new Dictionary<string, string>();
        //    }
        //    /// <summary>
        //    /// 上传地址
        //    /// </summary>
        //    public string Url { get; set; }
        //    /// <summary>
        //    /// 文件名称key
        //    /// </summary>
        //    public string FileNameKey { get; set; }
        //    /// <summary>
        //    /// 文件名称value
        //    /// </summary>
        //    public string FileNameValue { get; set; }
        //    /// <summary>
        //    /// 编码格式
        //    /// </summary>
        //    public Encoding Encoding { get; set; }
        //    /// <summary>
        //    /// 上传文件的流
        //    /// </summary>
        //    public Stream UploadStream { get; set; }
        //    /// <summary>
        //    /// 上传文件 携带的参数集合
        //    /// </summary>
        //    public IDictionary<string, string> PostParameters { get; set; }
        //}
    }
}
