using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Common
{
 // 写在同一文件夹内 方便前端拷贝
    public enum VersionEnum
    {

        [Description("公共/第三方")]
        def = 0,
        /// <summary>
        /// Pc端
        /// </summary>
        [Description("Web端网站")]
        Pc = 1,
        /// <summary>
        /// 
        /// </summary>
        [Description("移动端/微信小程序/钉钉/H5")]
        App = 2,
    }
    public enum MenuType
    {
        目录 = 1,
        菜单 = 2,
        按钮 = 3,
        外链 = 4,
    }
    public enum UserType
    {
        系统 = 1,

        用户 = 50,
    }

}
