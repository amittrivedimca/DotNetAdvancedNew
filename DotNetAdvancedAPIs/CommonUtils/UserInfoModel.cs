using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils
{
    public class UserInfoModel
    {
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public AppRole Role { get; set; }
        public string ErrorMessage { get; set; }
    }
}
