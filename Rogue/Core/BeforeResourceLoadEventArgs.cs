using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Core
{
    /// <summary>
    /// 在请求某个资源前触发
    /// </summary>
    public class BeforeResourceLoadEventArgs
    {
        public BeforeResourceLoadEventArgs(string uri, ResourceType resourceType)
        {
            this.Uri = uri;
            this.ResourceType = resourceType;
            this.CacheToQueue = false;
            this.Cancel = false;
        }

        /// <summary>
        /// 资源路径
        /// </summary>
        public string Uri { get; private set; }

        /// <summary>
        /// 资源类型
        /// </summary>
        public ResourceType ResourceType { get; private set; }

        /// <summary>
        /// 是否缓存到队列中
        /// </summary>
        public bool CacheToQueue { get; set; }

        /// <summary>
        /// 是否取消加载
        /// </summary>
        public bool Cancel { get; set; }
    }
}
