using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SzlqTech.ApiBLL.Dto;

namespace SzlqTech.ApiBLL.Service
{
    public class BaseApiService
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        protected const string ResultSuccessCode = "000000";

        protected const string ResultFailCode = "999999";

        protected string WebApiBaseUrl { get; } //= DataSyncConfig.Config.TraceApiUrl;

        protected string LoginPath { get; }// = DataSyncConfig.Config.LoginPath;

        protected string ApiUsername { get; }// = DbAndApiAuthConfig.Config.Username;

        protected string ApiPassword { get; } //= DbAndApiAuthConfig.Config.Password;

        protected string ApiType { get; } //= DataSyncConfig.Config.WebApiType;

        protected Dictionary<string, string> RequestHeaders { get; } = new Dictionary<string, string>();

        protected const string Authorization = "Authorization";

        // protected string Token { get; set; }


        public static DateTime LoginTime { get; set; }

        protected ResultDto EnsureResultSuccess(ResultDto? resultDto)
        {
            if (resultDto == null)
            {
                throw new Exception("服务器返回解析为空");
            }

            if (resultDto.Code == ResultSuccessCode && (resultDto.Result == null || string.IsNullOrEmpty(resultDto.Result.ToString())))
            {
                throw new Exception("服务器返回成功编码, 但是获取结果为空");
            }

            if (resultDto.Code == ResultFailCode)
            {
                throw new Exception($"服务器返回失败编码{resultDto.Code}-{resultDto.Message}");
            }

            if (resultDto.Code != ResultSuccessCode)
            {
                throw new Exception($"服务器返回未知编码{resultDto.Code}-{resultDto.Message}");
            }

            return resultDto;
        }

        public async Task Login()
        {
            // httpStatusCode = default;
            Dictionary<string, string> requestParams = new Dictionary<string, string>();
            requestParams.Add("username", ApiUsername);
            requestParams.Add("password", ApiPassword);
            requestParams.Add("type", ApiType);
            string url = WebApiBaseUrl + LoginPath;
            try
            {
                string result = await HttpHelper.HttpRequestTokenAsync(url, requestParams);
                var resultDto = JsonConvert.DeserializeObject<ResultDto>(result);
                EnsureResultSuccess(resultDto);
                GlobalContext.Token = resultDto!.Result!.ToString();
                RequestHeaders.Clear();
                RequestHeaders.Add(Authorization, GlobalContext.Token);
            }
            catch (Exception ex)
            {
                throw new Exception($"登录服务器[{url}]异常, 异常信息: {ex.Message}. {ex.InnerException?.Message}", ex);
            }
        }

        /// <summary>
        /// 重新登录
        /// </summary>
        /// <returns></returns>
        protected async Task Relogin()
        {
            string url = WebApiBaseUrl + LoginPath;
            await Login();
            logger.Info($"API登录超时或未登录，重新登录【{url}】");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="getParams"></param>
        /// <param name="tips"></param>
        // <returns></returns>
        protected async Task<ResultDto> GetAsync(string url, Dictionary<string, object>? getParams, string tips = "")
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    throw new ArgumentException("URL不可为空");
                }
                string json = await HttpHelper.GetJsonAsync(url, getParams, RequestHeaders);
                if (json == HttpStatusCode.Unauthorized.ToString())
                {
                    await Relogin();
                    return await GetAsync(url, getParams, tips);
                }
                var resultDto = JsonConvert.DeserializeObject<ResultDto>(json);

                return EnsureResultSuccess(resultDto);
            }
            catch (Exception ex)
            {
                throw new Exception($"GET服务器[{url}]{tips}信息异常, 异常消息: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 返回string类型结果
        /// </summary>
        /// <param name="url"></param>
        /// <param name="getParams"></param>
        /// <param name="tips"></param>
         // <returns></returns>
        protected async Task<string> GetStringAsync(string url, Dictionary<string, object>? getParams, string tips = "")
        {
            var resultDto = await GetAsync(url, getParams, tips);
            return resultDto!.Result!.ToString();
        }

        protected async Task<T?> GetObjectAsync<T>(string url, Dictionary<string, object>? getParams, string tips = "") where T : BaseDto
        {
            var resultDto = await GetAsync(url, getParams, tips);
            var result = resultDto!.Result!.ToString();
            return JsonConvert.DeserializeObject<T>(result);
        }

        protected async Task<List<T>?> GetListAsync<T>(string url, Dictionary<string, object>? getParams, string tips = "") where T : BaseDto
        {
            var resultDto = await GetAsync(url, getParams, tips);
            var result = resultDto!.Result!.ToString();
            return JsonConvert.DeserializeObject<List<T>>(result);
        }


        protected async Task<ResultDto> PostFormAsync(string url, Dictionary<string, string>? postForm, string tips = "")
        {
            try
            {
                if (postForm == null)
                {
                    postForm = new Dictionary<string, string>();
                }
                if (string.IsNullOrEmpty(url))
                {
                    throw new ArgumentException("URL不可为空");
                }
                string json = await HttpHelper.PostFormAsync(url, postForm, RequestHeaders);
                if (json == HttpStatusCode.Unauthorized.ToString())
                {
                    await Relogin();
                    return await PostFormAsync(url, postForm, tips);
                }
                var resultDto = JsonConvert.DeserializeObject<ResultDto>(json);

                return EnsureResultSuccess(resultDto);

            }
            catch (Exception ex)
            {
                throw new Exception($"POST服务器[{url}]{tips}信息失败, {ex.Message}", ex);
            }
        }

        protected async Task<T?> PostFormToObjectAsync<T>(string url, Dictionary<string, string>? postForm, string tips = "") where T : BaseDto
        {
            var resultDto = await PostFormAsync(url, postForm, tips);
            var result = resultDto!.Result!.ToString();
            return JsonConvert.DeserializeObject<T>(result);
        }

        protected async Task<List<T>?> PostFormToListAsync<T>(string url, Dictionary<string, string>? postForm, string tips = "") where T : BaseDto
        {
            var resultDto = await PostFormAsync(url, postForm, tips);
            var result = resultDto!.Result!.ToString();
            return JsonConvert.DeserializeObject<List<T>>(result);
        }



        protected async Task<ResultDto> PostJsonAsync(string url, string postJson, string tips = "")
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    throw new ArgumentException("postJson不可为空");
                }
                if (string.IsNullOrEmpty(url))
                {
                    throw new ArgumentException("URL不可为空");
                }
                string json = await HttpHelper.PostJsonAsync(url, postJson, RequestHeaders);
                if (json == HttpStatusCode.Unauthorized.ToString())
                {
                    await Relogin();
                    return await PostJsonAsync(url, postJson, tips);
                }
                var resultDto = JsonConvert.DeserializeObject<ResultDto>(json);

                return EnsureResultSuccess(resultDto);

            }
            catch (Exception ex)
            {
                throw new Exception($"POST服务器[{url}]{tips}信息失败, {ex.Message}", ex);
            }
        }

        protected async Task<T?> PostJsonToObjectAsync<T>(string url, string postJson, string tips = "") where T : BaseDto
        {
            var resultDto = await PostJsonAsync(url, postJson, tips);
            var result = resultDto!.Result!.ToString();
            return JsonConvert.DeserializeObject<T>(result);
        }

        protected async Task<List<T>?> PostJsonToListAsync<T>(string url, string postJson, string tips = "") where T : BaseDto
        {
            var resultDto = await PostJsonAsync(url, postJson, tips);
            var result = resultDto!.Result!.ToString();
            return JsonConvert.DeserializeObject<List<T>>(result);
        }

    }
}
