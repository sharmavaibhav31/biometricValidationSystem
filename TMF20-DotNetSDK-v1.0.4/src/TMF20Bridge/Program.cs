using System;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace TMF20Bridge
{
    class Program
    {
        public class MatchRequest
        {
            public string ReferenceTemplate { get; set; }
            public string ClaimedTemplate { get; set; }
        }
        static void Main(string[] args)
        {
            string prefix = "http://127.0.0.1:5010/";
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("HttpListener not supported.");
                return;
            }

            var listener = new HttpListener();
            listener.Prefixes.Add(prefix);
            listener.Start();
            Console.WriteLine("TMF20Bridge listening on " + prefix);

            var fp = new TMF20FPLibrary();
            var serializer = new JavaScriptSerializer();

            while (true)
            {
                var ctx = listener.GetContext();
                try
                {
                    string path = ctx.Request.Url.AbsolutePath.Trim('/').ToLowerInvariant();
                    if (path == "device/check")
                    {
                        var connected = fp.isDeviceConnected();
                        WriteJson(ctx, serializer.Serialize(new { connected }));
                    }
                    else if (path == "device/info")
                    {
                        var info = new DeviceInfo();
                        int code = fp.getDeviceInfo(info);
                        if (code != 0)
                        {
                            WriteError(ctx, 500, "code=" + code);
                        }
                        else
                        {
                            WriteJson(ctx, serializer.Serialize(info));
                        }
                    }
                    else if (path.StartsWith("fingerprint/capture"))
                    {
                        int timeoutMs = 10000;
                        var result = new CaptureResult();
                        int code = fp.captureFingerprint(result, timeoutMs);
                        if (code != 0)
                        {
                            WriteError(ctx, 500, result.errorString ?? ("code=" + code));
                        }
                        else
                        {
                            var tplBytes = result.fmrBytes ?? result.rawImageBytes;
                            var tpl = Convert.ToBase64String(tplBytes);
                            WriteJson(ctx, serializer.Serialize(new { success = true, template = tpl }));
                        }
                    }
                    else if (path == "fingerprint/match")
                    {
                        string body;
                        using (var reader = new System.IO.StreamReader(ctx.Request.InputStream, ctx.Request.ContentEncoding))
                        {
                            body = reader.ReadToEnd();
                        }
                        var obj = serializer.Deserialize<MatchRequest>(body);
                        var refTpl = Convert.FromBase64String(obj.ReferenceTemplate);
                        var claimTpl = Convert.FromBase64String(obj.ClaimedTemplate);
                        bool matched = fp.matchIsoTemplates(refTpl, claimTpl);
                        WriteJson(ctx, serializer.Serialize(new { matched }));
                    }
                    else
                    {
                        WriteError(ctx, 404, "Not found");
                    }
                }
                catch (Exception ex)
                {
                    WriteError(ctx, 500, ex.Message);
                }
            }
        }

        static void WriteJson(HttpListenerContext ctx, string json)
        {
            var bytes = Encoding.UTF8.GetBytes(json);
            ctx.Response.ContentType = "application/json";
            ctx.Response.ContentEncoding = Encoding.UTF8;
            ctx.Response.ContentLength64 = bytes.Length;
            ctx.Response.OutputStream.Write(bytes, 0, bytes.Length);
            ctx.Response.OutputStream.Close();
        }

        static void WriteError(HttpListenerContext ctx, int code, string message)
        {
            ctx.Response.StatusCode = code;
            WriteJson(ctx, new JavaScriptSerializer().Serialize(new { error = message }));
        }
    }
}


