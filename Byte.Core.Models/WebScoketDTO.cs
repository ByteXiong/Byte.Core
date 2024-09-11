using Byte.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Models
{
    public class WebSocketMsg
    {

        public WebSocketMsgTypeEnum Type { get; set; }
        public object Data { get; set; }
    }
}
