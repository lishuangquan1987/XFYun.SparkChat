using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFYun.SparkChat.SDK
{
    public class JsonResponse
    {
        [JsonProperty("header")]
        public ResponseHeader Header { get; set; }
        [JsonProperty("payload")]
        public ResponsePayload Payload { get; set; }
    }
    public class ResponseHeader
    {
        /// <summary>
        /// 错误码，0表示正常，非0表示出错；详细释义可在接口说明文档最后的错误码说明了解
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; set; }
        /// <summary>
        /// 会话是否成功的描述信息
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
        /// <summary>
        /// 会话的唯一id，用于讯飞技术人员查询服务端会话日志使用,出现调用错误时建议留存该字段
        /// </summary>
        [JsonProperty("sid")]
        public string Sid { get; set; }
        /// <summary>
        /// 会话状态，取值为[0,1,2]；0代表首次结果；1代表中间结果；2代表最后一个结果
        /// </summary>
        [JsonProperty("status")]
        public int Status { get; set; }
    }
    public class ResponsePayload
    {
        [JsonProperty("choices")]
        public ResponseChoices Choices { get; set; }
        [JsonProperty("useage")]
        public ResponseUsage Usage { get; set; }
    }
    public class ResponseChoices
    {
        /// <summary>
        /// 文本响应状态，取值为[0,1,2]; 0代表首个文本结果；1代表中间文本结果；2代表最后一个文本结果
        /// </summary>
        [JsonProperty("status")]
        public int Status { get; set; }
        /// <summary>
        /// 返回的数据序号，取值为[0,9999999]
        /// </summary>
        [JsonProperty("seq")]
        public int Seq { get; set; }
        [JsonProperty("text")]
        public List<ReponseContent> Text { get; set; }
    }
    public class ReponseContent
    {
        /// <summary>
        /// AI的回答内容
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }
        /// <summary>
        /// 角色标识，固定为assistant，标识角色为AI
        /// </summary>
        [JsonProperty("role")]
        public string Role { get; set; }
        /// <summary>
        /// 结果序号，取值为[0,10]; 当前为保留字段，开发者可忽略
        /// </summary>
        [JsonProperty("index")]
        public int Index { get; set; }
    }
    public class ResponseUsage
    {
        [JsonProperty("text")]
        public ResponseUsageDetails Text { get; set; }
    }
    public class ResponseUsageDetails
    {
        /// <summary>
        /// 保留字段，可忽略
        /// </summary>
        [JsonProperty("question_tokens")]
        public int QuestionTokens { get; set; }
        /// <summary>
        /// 包含历史问题的总tokens大小
        /// </summary>
        [JsonProperty("prompt_tokens")]
        public int PromptTokens { get; set; }
        /// <summary>
        /// 回答的tokens大小
        /// </summary>
        [JsonProperty("completion_tokens")]
        public int CompletionTokens { get; set; }
        /// <summary>
        /// prompt_tokens和completion_tokens的和，也是本次交互计费的tokens大小
        /// </summary>
        [JsonProperty("total_tokens")]
        public int TotalTokens { get; set; }
    }
}
