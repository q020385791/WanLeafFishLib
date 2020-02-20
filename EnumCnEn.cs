using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WanLibrary
{
    public enum EnumCnEn
    {
        /// <summary>
        /// 卡片號碼
        /// </summary>
        [Description("姓名")]
        Name = 1
    }



    public static class EnumEx
    {
        /// <summary>
        /// 中文轉英文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="description"></param>
        /// <returns></returns>
        public static string GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);

            foreach (var field in type.GetFields())
            {
                //var test=field.CustomAttributes.Where(x => x.ConstructorArguments[0].ToString() ==  "{"+'"'+"卡片號碼"+'"'+"}");
                   var attributes = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                
                if (attributes!=null)
                {
                    if (attributes.Description== description)
                    {
                       return field.GetValue(null).ToString();
                    }
                    else
                    {
                        if (field.Name==description)
                        {
                            return field.GetValue(null).ToString();
                        }
                    }
                }
            }
            
            throw new ArgumentException("Not found.", description);
        }

       

    }

    
}
