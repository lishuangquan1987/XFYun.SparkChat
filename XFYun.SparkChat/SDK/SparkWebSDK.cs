using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace XFYun.SparkChat.SDK
{
    public class SparkWebSDK
    {
        private string _appId;
        private string _apiSecret;
        private string _apiKey;
        private SparkVersions _version;
        private ClientWebSocket _webSocketClient;
        public SparkWebSDK()
        {
            
        }
        public void Setup(string appId, string apiSecret, string apiKey, SparkVersions version = SparkVersions.V3_0)
        {
            this._apiKey = apiKey;
            this._apiSecret = apiSecret;
            this._appId = appId;
            this._version = version;
        }
        private string GetAuthUrl(string baseUrl, string apiSecret, string apiKey)
        {
            string date = DateTime.UtcNow.ToString("r");
            Uri uri = new Uri(baseUrl);
            var str = $"host: {uri.Host}\ndate: {date}\nGET {uri.LocalPath} HTTP/1.1";

            //使用apisecret,HMACSHA256算法加密str
            var sha256Bytes = new HMACSHA256(Encoding.UTF8.GetBytes(apiSecret)).ComputeHash(Encoding.UTF8.GetBytes(str));
            var sha256Str = Convert.ToBase64String(sha256Bytes);
            var authorization = $"api_key=\"{apiKey}\",algorithm=\"hmac-sha256\",headers=\"host date request-line\",signature=\"{sha256Str}\"";

            //date要做url处理
            date = Uri.EscapeDataString(date);
            string newUrl = $"ws://{uri.Host}{uri.LocalPath}?authorization={Convert.ToBase64String(Encoding.UTF8.GetBytes(authorization))}&date={date}&host={uri.Host}";
            return newUrl;
        }
        /// <summary>
        /// 询问问题，流式调用response
        /// 返回结果表示调用成功还是失败，如果调用失败，则返回失败原因
        /// </summary>
        /// <param name="question"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public async Task<(bool, string)> Ask(List<string> questions, CancellationToken token, Action<List<string>> responseHandler)
        {
            try
            {
                string url = "";
                string domain = "";
                switch (this._version)
                {
                    case SparkVersions.V1_5:
                        url = "ws://spark-api.xf-yun.com/v1.1/chat";
                        domain = "general";
                        break;
                    case SparkVersions.V2_0:
                        url = "ws://spark-api.xf-yun.com/v2.1/chat";
                        domain = "generalv2";
                        break;
                    case SparkVersions.V3_0:
                        url = "ws://spark-api.xf-yun.com/v3.1/chat";
                        domain = "generalv3";
                        break;
                }
                var newUrl = GetAuthUrl(url, this._apiSecret, this._apiKey);
                this._webSocketClient = new ClientWebSocket();
                await this._webSocketClient.ConnectAsync(new Uri(newUrl), token);

                var request = new JsonRequest()
                {
                    Header = new RequestHeader()
                    {
                        AppId = this._appId,
                        Uid = "123"
                    },
                    Parameter = new RequestParameter()
                    {
                        Chat = new RequestChat()
                        {
                            Domain = domain,
                            Temperature = 0.5,
                            MaxTokens = 1024,
                        }
                    },
                    Payload = new RequestPayload()
                    {
                        Message = new RequestMessage()
                        {
                            Text = questions.Select(x => new ReuqestContent()
                            {
                                Role = "user",
                                Content = x
                            }).ToList()
                        }
                    }
                };
                var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(request);

                await this._webSocketClient.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonStr)), WebSocketMessageType.Text, true, token);

                var recvBuffer = new byte[1024];

                while (true)
                {
                    WebSocketReceiveResult result = await this._webSocketClient.ReceiveAsync(new ArraySegment<byte>(recvBuffer), token);
                    if (result.CloseStatus.HasValue) return (true, "");
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        string recvMsg = Encoding.UTF8.GetString(recvBuffer, 0, result.Count);
                        var response = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonResponse>(recvMsg);
                        if (response.Header.Code != 0)
                        {
                            return (false, response.Header.Message);
                        }

                        if (response.Payload.Choices.Status == 2)//最后一个消息
                        {
                            responseHandler?.Invoke(response.Payload.Choices.Text.Select(x => x.Content).ToList());
                            return (true, "调用成功！");
                        }

                        responseHandler?.Invoke(response.Payload.Choices.Text.Select(x => x.Content).ToList());
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        return (false, result.CloseStatusDescription);
                    }
                }
            }
            catch (Exception e)
            {
                return (false, e.Message);
            }
            finally
            {
                await this._webSocketClient?.CloseAsync( WebSocketCloseStatus.NormalClosure,"client raise close request",token);
            }
        }
        public async void Close()
        {
            if (_webSocketClient != null)
            {
                await _webSocketClient.CloseAsync(WebSocketCloseStatus.NormalClosure, "正常关闭", new CancellationToken());
            }
        }
    }
    public enum SparkVersions
    {
        V1_5,
        V2_0,
        V3_0
    }


}
