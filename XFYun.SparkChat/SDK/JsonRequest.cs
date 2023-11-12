using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFYun.SparkChat.SDK
{
    //构造请求体
    public class JsonRequest
    {
        [JsonProperty("header")]
        public RequestHeader Header { get; set; }
        [JsonProperty("parameter")]
        public RequestParameter Parameter { get; set; }
        [JsonProperty("payload")]
        public RequestPayload Payload { get; set; }
    }

    public class RequestHeader
    {
        /// <summary>
        /// 应用appid，从开放平台控制台创建的应用中获取
        /// </summary>
        [JsonProperty("app_id")]
        public string AppId { get; set; }
        /// <summary>
        /// 每个用户的id，用于区分不同用户
        /// </summary>
        [JsonProperty("uid")]
        public string Uid { get; set; }
    }

    public class RequestParameter
    {
        [JsonProperty("chat")]
        public RequestChat Chat { get; set; }
    }

    public class RequestChat
    {
        /// <summary>
        /// 指定访问的领域,general指向V1.5版本,generalv2指向V2版本,generalv3指向V3版本 。注意：不同的取值对应的url也不一样！
        /// </summary>
        [JsonProperty("domain")]
        public string Domain { get; set; }
        /// <summary>
        /// 核采样阈值。用于决定结果随机性，取值越高随机性越强即相同的问题得到的不同答案的可能性越高
        /// </summary>
        [JsonProperty("temperature")]
        public double Temperature { get; set; }
        /// <summary>
        /// 模型回答的tokens的最大长度
        /// </summary>
        [JsonProperty("max_tokens")]
        public int MaxTokens { get; set; }
    }

    public class RequestPayload
    {
        [JsonProperty("message")]
        public RequestMessage Message { get; set; }
    }

    public class RequestMessage
    {
        [JsonProperty("text")]
        public List<ReuqestContent> Text { get; set; }
    }

    public class ReuqestContent
    {
        /// <summary>
        /// user表示是用户的问题，assistant表示AI的回复
        /// </summary>
        [JsonProperty("role")]
        public string Role { get; set; }
        /// <summary>
        /// 用户和AI的对话内容
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }
    }
}
