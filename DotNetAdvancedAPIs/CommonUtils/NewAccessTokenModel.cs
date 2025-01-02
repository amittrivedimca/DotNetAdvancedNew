using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils
{
    public class NewAccessTokenModel
    {
        public bool IsSuccess { get; set; }
        public string NewAccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
