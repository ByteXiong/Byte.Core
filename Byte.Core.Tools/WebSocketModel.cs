using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Tools
{
    public class WebSocketModel
    {

        /// <summary>
        /// 生产者
        /// </summary>
        public  Guid? ProId { get; set; }
        /// <summary>
        /// 消费者
        /// </summary>
        public Guid[]? ConIds{ get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public WebSocketModelTypeEnum Type { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; set; }
       

    }
}
