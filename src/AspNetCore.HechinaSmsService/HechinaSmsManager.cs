using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AspNetCore;

namespace AspNetCore.HechinaSmsService
{
    public class HechinaSmsManager
    {
        private readonly ILogger _logger;
        private readonly HechinaSmsOptions _options;
        protected readonly HttpClient _backchannel;

        public HechinaSmsManager(
            IOptions<HechinaSmsOptions> optionsAccessor,
            ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory?.CreateLogger<HechinaSmsManager>()??throw new ArgumentNullException(nameof(loggerFactory));

            //检查入参
            _options = optionsAccessor?.Value?? throw new ArgumentNullException(nameof(optionsAccessor));

            if (_options.AccountForCommon == null)
            {
                throw new ArgumentNullException(nameof(_options.AccountForCommon));
            }

            //用户便利性处理
            if (_options.AccountForVerificationCode == null)
            {
                _options.AccountForVerificationCode = _options.AccountForCommon;
            }
            if (_options.AccountForAdvertising == null)
            {
                _options.AccountForAdvertising = _options.AccountForCommon;
            }

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            _backchannel = new HttpClient(new HttpClientHandler());
            _backchannel.DefaultRequestHeaders.UserAgent.ParseAdd("HechinaSmsAgent/1.0");
            _backchannel.Timeout = TimeSpan.FromSeconds(5);
            _backchannel.MaxResponseContentBufferSize = 1024 * 2; // 2 KB

        }

        public async Task<bool> SendSmsAsync(string mobile, string content)
        {
            return await SendAsync(_options.AccountForCommon, mobile, content);
        }

        public async Task SendAdvertisingAsync(string[] mobiles, string content)
        {
            foreach (var mobile in mobiles)
            {
                await SendAsync(_options.AccountForAdvertising, mobile, content);
            };
        }

        public async Task<bool> SendVerificationCodeAsync(string mobile, string verificationCode)
        {
            var content = FormatVerificationCode(verificationCode);
            return await SendAsync(_options.AccountForCommon, mobile, content);
        }

        public async Task<bool> SendAsync(HechinaSmsAccountOptions options, string mobile, string content)
        {
            //处理不合法字符
            content = content.Replace("&", "_");

            var smsEndpoint = "http://203.81.21.34/send/gsend.asp";
            var smsRequestParameters = new Dictionary<string, string>
            {
                {"dst", mobile},
                {"msg", content },
                {"name", options.Uid },
                {"pwd", options.Pwd }
            };
            //var requestContent = new FormUrlEncodedContent(smsRequestParameters);
            var s = smsRequestParameters.Select(x => string.Format("{0}={1}", x.Key, x.Value)).JoinAsString("&");
            _logger.LogDebug("{0}?{1}", smsEndpoint, s);

            HttpContent requestContent = new ByteArrayContent(Encoding.GetEncoding("gb2312").GetBytes(s));
            requestContent.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            //requestContent.Headers.Add("Content-Type", "application/x-www-form-urlencoded; charset=gb2312");

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, smsEndpoint);
            requestMessage.Headers.AcceptCharset.AddIfNotContains(new StringWithQualityHeaderValue("GB2312"));
            //requestMessage.Headers.CacheControl.NoCache = true;
            requestMessage.Content = requestContent;

            var responseMessage = await _backchannel.SendAsync(requestMessage);
            responseMessage.EnsureSuccessStatusCode();

            var responseBytes = await responseMessage.Content.ReadAsByteArrayAsync();
            var responseContent = Encoding.GetEncoding("GB2312").GetString(responseBytes);
            Debug.WriteLine(responseContent);
            var message = new HechinaGsendResponseMessage(responseContent);
            return message.ErrorCode == 0;
        }

        private string FormatVerificationCode(string verificationCode)
        {
            //return $"您的验证码是{verificationCode}";
            return string.Format(_options.VerificationCodeFormat, verificationCode);
        }

        private string ConvertToEncodedGb2312(string text)
        {
            var result = WebUtility.UrlEncode(text);
            return result;
        }
    }
}
