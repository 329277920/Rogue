using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Core
{
    /// <summary>
    /// 资源缓存列表
    /// </summary>
    public sealed class ResourceCacheManager 
    {
        private static Dictionary<ulong, Stream> Resources = new Dictionary<ulong, Stream>();

        public static Stream CreateResource(ulong key)
        {
            lock (Resources)
            {
                if (Resources.ContainsKey(key))
                {
                    return Resources[key];
                }
                var stream = new MemoryStream();
                Resources.Add(key, stream);
                return stream;
            }
        }

        public static Stream GetResource(ulong key)
        {
            lock (Resources)
            {
                if (Resources.ContainsKey(key))
                {
                    return Resources[key];
                }
                return null;
            }
        }

        public static void DeleteResource(ulong key)
        {
            lock (Resources)
            {
                if (!Resources.ContainsKey(key))
                {
                    return;
                }
                var stream = Resources[key];
                if (stream != null)
                {
                    stream.Dispose();
                }
                Resources.Remove(key);               
            }
        }
    }
}
