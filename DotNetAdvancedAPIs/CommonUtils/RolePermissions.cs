using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils
{
    public class RolePermissions
    {
        public bool Create { get; set; } = false;
        public bool Update { get; set; } = false;
        public bool Delete { get; set; } = false;
        public bool Read { get; set; } = false;
    }
}
