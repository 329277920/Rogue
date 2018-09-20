using CefSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Core
{
    public class ResourceLoadCompleteEventArgs
    {
        public ResourceLoadCompleteEventArgs(string uri, ResourceType resourceType, long contentLength, Stream content, UrlRequestStatus status)
        {
            this.Uri = uri;
            this.ResourceType = resourceType;
            this.ContentLength = contentLength;
            this.Content = content;
            this.Status = status;
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
        /// 资源大小
        /// </summary>
        public long ContentLength { get; private set; }

        /// <summary>
        /// 请求状态
        /// </summary>
        public UrlRequestStatus Status { get; private set; }

        /// <summary>
        /// 资源流
        /// </summary>
        public Stream Content { get; private set; }
    }
}
