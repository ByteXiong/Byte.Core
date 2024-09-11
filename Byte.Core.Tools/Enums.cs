using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Tools
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

    public enum RoleTypeEnum
    {
        系统角色 = 10,
        公司角色 = 20,
        部门角色 = 30,
        个人角色 = 40,
    }

    public enum MenuTypeEnum
    {
        目录 = 1,
        菜单 = 2,
        按钮 = 3,
        外链 = 4,
    }

    public enum DeptTypeEnum
    {
        平台 = 10,

        公司 = 20,

        部门 = 30,
    }
    /// <summary>
    /// 对齐方式
    /// </summary>
    public enum TableAlignEnum
    {
        left=1,
        center,
        right,
    
    }
    /// <summary>
    /// 固定方式
    /// </summary>
    public enum TableFixedEnum
    {
        left = 1,
        right =3,

    }
    public enum WebSocketMsgTypeEnum
    { 
     发送心跳=0,
     在线用户=1,
     单聊=100,
     群聊 = 110,
    }
}
