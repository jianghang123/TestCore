using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.AspNetCore.Mvc.Abstractions
{
    public static class ActionDescriptorExtension
    {

        /// <summary>
        ///获取 action 方法上的属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        public static T GetActionAttribute<T>(this ActionDescriptor descriptor)
        {
            try
            {
                if (descriptor.ActionConstraints != null)
                {
                    var list = descriptor.ActionConstraints.OfType<T>();

                    if (list != null && list.Any())
                    {
                        return list.FirstOrDefault();
                    }
                }
                return default(T);
            }
            catch
            {
                return default(T);
            }

        }
 

    }
}
