using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.HechinaSmsService
{
    public class HechinaSmsOptions
    {
        public string VerificationCodeFormat { get; set; } = "Your verification code is {0}";
        public HechinaSmsAccountOptions AccountForCommon { get; set; }
        public HechinaSmsAccountOptions AccountForVerificationCode { get; set; }
        public HechinaSmsAccountOptions AccountForAdvertising { get; set; }
    }

    public class HechinaSmsAccountOptions
    {
        public string Uid { get; set; }
        public string Pwd { get; set; }
    }
}
