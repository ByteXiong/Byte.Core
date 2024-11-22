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
    public enum StateEnum { 
        /// <summary>
        /// 删除
        /// </summary>
        del = -1,
        /// <summary>
        /// 正常
        /// </summary>
        normal = 0,
        /// <summary>
        /// 新增
        /// </summary>
        add = 1,
        /// <summary>
        /// 更新
        /// </summary>
        update=2,
    }
    public enum OrderTypeEnum
    { 
        /// <summary>
        /// 正序
        /// </summary>
        asc = 1,
        /// <summary>
        /// 倒序
        /// </summary>
        desc = 2
    }

    public enum ViewTypeEnum
    {
         主页=1,
         编辑 =2,
         详情页=3,
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
        left = 1,
        center,
        right,

    }
    /// <summary>
    /// 固定方式
    /// </summary>
    public enum TableFixedEnum
    {
        left = 1,
        right = 3,

    }
    public enum WebSocketModelTypeEnum
    {
        发送心跳 = 0,
        在线用户 = 1,
        单聊 = 100,
        群聊 = 110,
    }
    /// <summary>
    /// 查询方式,不能删除同步给前端用
    /// </summary>
    public enum SearchTypeEnum
    {
        // 后端使用 SqlSuger.ConditionalType
        等于,
        模糊,
        大于,
        大于或等于,
        小于,
        小于或等于,
        区间,
     }

    public enum ColumnTypeEnum
    {
        整数 = 1,
        文本 = 2,
        枚举 = 3,
        字典 = 4,
        小数 = 5,
        日期 = 6,
        时间 = 7,
        时间戳转当地日期 = 8,
        时间戳转当地时间 = 9,
        单图 = 10,
        多图 = 11,
        文件 = 12,
        布尔 = 13,
        颜色 = 14,
        自定义 = 99
    }
    public enum LayoutTypeEnum
    {
        Base = 1,
        Blank=2,
    }

    public enum IconTypeEnum { 
       iconify图标=1,
       本地图标=2,
    
    }

}
