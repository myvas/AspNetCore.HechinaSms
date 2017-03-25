using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using AspNetCore.HechinaSmsService;

namespace test
{
    public class SmsManagerTests
    {
        [Fact]
        public async Task SendSms_Pass()
        {
            var optionsAccessor = new OptionsWrapper<HechinaSmsOptions>(
                new HechinaSmsOptions()
                {
                    AccountForCommon = new HechinaSmsAccountOptions()
                    {
                        Uid = "cix",
                        Pwd = "cix123456"
                    },
                    AccountForVerificationCode = new HechinaSmsAccountOptions()
                    {
                        Uid = "cix",
                        Pwd = "cix123456"
                    }
                });

            var loggerFactory = new LoggerFactory();

            var smsManager = new HechinaSmsManager(optionsAccessor, loggerFactory);
            var result = await smsManager.SendVerificationCodeAsync("15902059380", "56789");
            Assert.True(result);
        }

        [Fact]
        public async Task SendSms_Chs_Pass()
        {
            var optionsAccessor = new OptionsWrapper<HechinaSmsOptions>(
                new HechinaSmsOptions()
                {
                    AccountForCommon = new HechinaSmsAccountOptions()
                    {
                        Uid = "cix",
                        Pwd = "cix123456"
                    },
                    AccountForVerificationCode = new HechinaSmsAccountOptions()
                    {
                        Uid = "cix",
                        Pwd = "cix123456"
                    }
                });
            var loggerFactory = new LoggerFactory();

            var smsManager = new HechinaSmsManager(optionsAccessor, loggerFactory);
            var result = await smsManager.SendSmsAsync("15902059380", "您的验证码是654323");
            Assert.True(result);
        }
    }
}
