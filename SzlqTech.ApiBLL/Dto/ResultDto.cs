using Newtonsoft.Json;


namespace SzlqTech.ApiBLL.Dto
{
    public class ResultDto : BaseDto
    {
        /// <summary>
        /// 999999 失败
        /// 000000 成功
        /// </summary>
        [JsonProperty("code")]
        public string? Code { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonProperty("result")]
        public object? Result { get; set; }
    }
}
