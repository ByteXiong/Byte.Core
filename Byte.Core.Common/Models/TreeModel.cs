using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Common.Models
{
    /// <summary>
    /// 树模型（可以作为父类）
    /// </summary>
    public class TreeModel<TKey>
    {
        /// <summary>
        /// 唯一标识Id
        /// </summary>
        public TKey Id { get; set; }

        /// <summary>
        /// 数据名
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// 父级Id
        /// </summary>
        public TKey ParentId { get; set; }

        /// <summary>
        /// 节点深度
        /// </summary>
        public int? Level { get; set; } = 1;

        /// <summary>
        /// 显示的内容
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 节点
        /// </summary>
        public List<object> Children { get; set; }
    }
}
