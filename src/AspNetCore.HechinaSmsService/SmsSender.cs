using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.HechinaSmsService
{
    public class SmsSender : IVerificationCodeSmsSender, ISmsSender
    {
        private readonly HechinaSmsManager _smsManager;
        public SmsSender(HechinaSmsManager smsManager)
        {
            _smsManager = smsManager ?? throw new ArgumentNullException(nameof(smsManager));
        }

        public async Task<bool> SendVerificationCodeAsync(string mobile, string verificationCode)
        {
            return await _smsManager.SendVerificationCodeAsync(mobile, verificationCode);
        }

        public async Task<bool> SendSmsAsync(string mobile, string content)
        {
            return await _smsManager.SendSmsAsync(mobile, content);
        }
    }
}
