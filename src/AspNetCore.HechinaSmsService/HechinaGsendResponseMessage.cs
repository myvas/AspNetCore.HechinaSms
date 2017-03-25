using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.HechinaSmsService
{
    public class HechinaGsendResponseMessage
    {
        public HechinaGsendResponseMessage(string formUrlEncodedContent)
        {
            var dict = formUrlEncodedContent.Split('&')
                .ToDictionary(
                    c => c.Split('=')[0],
                    c => Uri.UnescapeDataString(c.Split('=')[1]));

            TotalCount = Convert.ToInt32(dict["num"]);
            SucceededMobiles = dict["success"];
            FailedMobiles = dict["faile"];
            ErrorCode = Convert.ToInt32(dict["errid"]);
            ErrorMsg = dict["err"];
        }

        public int TotalCount { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMsg { get; set; }
        public string SucceededMobiles { get; set; }
        public string FailedMobiles { get; set; }
    }
}
