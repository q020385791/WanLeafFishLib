using EnumsNET;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.IO;

namespace WanLibrary
{
    //格式轉換
    public class DataFormat
    {
        //特定解碼頁面字串轉Byte
        public byte[] StringToByte(string Source, int CodePage)
        {
            try
            {
                return Encoding.GetEncoding(CodePage).GetBytes(Source);
            }
            catch (Exception ex)
            {
                return Encoding.GetEncoding(950).GetBytes(Source);
            }
        }
        //byte轉字串
        public static string ByToString950(byte[] bytes, int Start, int Num)
        {
            string data;
            data = "";
            data = System.Text.Encoding.GetEncoding(950).GetString(bytes, Start, Num);
            return data;
        }
        //物件轉JSON字串
        public string ObjectToJSON(object Source)
        {
            var result = Newtonsoft.Json.JsonConvert.SerializeObject(Source);
            return result;

            #region 參數使用方法
            //object newobject = new { param1 = "value1", param2 = "value2" }; 
            //儲入已經處理好的object 轉換成JSON字串輸出
            #endregion 
        }
        //從描述取得值(enum)
        public static string GetEnumValueFromDescription<T>(string description)
        {
            var type = typeof(T);

            foreach (var field in type.GetFields())
            {
                //var test=field.CustomAttributes.Where(x => x.ConstructorArguments[0].ToString() ==  "{"+'"'+"卡片號碼"+'"'+"}");
                var attributes = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

                if (attributes != null)
                {
                    if (attributes.Description == description)
                    {
                        return field.GetValue(null).ToString();
                    }
                    else
                    {
                        if (field.Name == description)
                        {
                            return field.GetValue(null).ToString();
                        }
                    }
                }
            }
            throw new ArgumentException("Not found.", description);

            #region  參數使用方法
            //Myclass.GetValueFromDescription<EnumFile>("出生日期")

            //由中文敘述轉成英文值
            //    public enum EnumFile
            //{
            //    [Description("出生日期")]
            //    Birthday = 3
            //}
            #endregion
        }

        /// <summary>
        /// 英文轉中文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static string GetDescriptionFromValue<T>(string Value)
        {
            if (Enum.IsDefined(typeof(EnumCnEn), Value))
            {
                var type = typeof(T);
                DescriptionAttribute EnumAttitute = (DescriptionAttribute)type.GetFields()[1].GetCustomAttributes(true)[0];
                string EnumDescription = EnumAttitute.Description;
                return EnumDescription;
            }
            else
            {
                return "NotExistEnum,Please Check";
            }
        }

        //向右邊填充
        public static string StringPadRight(string Target,int TotalLength,char paddingChar)
        {
            int Targetlength = Target.Length;
            if (TotalLength - Targetlength>0)
            {
                Target = Target.PadRight(TotalLength, paddingChar);
            }
            
            return Target;
        }
        //向左邊填充
        public static string StringPadLeft(string Target, int TotalLength, char paddingChar)
        {
            int Targetlength = Target.Length;
            if (TotalLength - Targetlength > 0)
            {
                Target = Target.PadLeft(TotalLength, paddingChar);
            }

            return Target;
        }

    }
    //格式與空值確認
    public class DataCheck
    {
        /// <summary>
        /// 判別 字串 是否為Null或空值
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="IfNullorEmpty"></param>
        public void CheckStringNullValue(string Source, ref bool IfNullorEmpty)
        {
            IfNullorEmpty = Source == null ? true : Source == "" ? true : false;
        }
        
        
        /// <summary>
        /// 判別 布林值 是否為Null或空值
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="IfNullorEmpty"></param>
        public void CheckBoolNullValue(bool? Source, ref bool IfNullorEmpty)
        {
            IfNullorEmpty = Source == null ? true : false;

        }

        /// <summary>
        /// 判別 int 是否為Null或空值
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="IfNullorEmpty"></param>
        public void CheckIntNullValue(int? Source, ref bool IfNullorEmpty)
        {
            IfNullorEmpty = Source == null ? true : false;
        }

        /// <summary>
        /// 確認字串是否每一個字元都是數字
        /// </summary>
        /// <param name="sNumericStr"></param>
        /// <returns></returns>
        public static bool IsNumeric(string sNumericStr)
        {
            int iCharIndex = 0;
            bool Result = false;
            bool bNumeric = false;
            for (iCharIndex = 1; iCharIndex <= sNumericStr.Length; iCharIndex++)
            {
                char subsNumericStr = ' ';
                try
                {
                    subsNumericStr = Convert.ToChar(sNumericStr.Substring(iCharIndex - 1, 1));

                }
                catch (Exception)
                {
                    return false;

                }

                if (iCharIndex == 1)
                {

                    bNumeric = (subsNumericStr >= '0' && subsNumericStr <= '9');
                }
                else
                {
                    bNumeric = bNumeric && (subsNumericStr >= '0' && subsNumericStr <= '9');
                }
            }

            return bNumeric;

        }

    }

    //動態連接
    public class CustomDLLInvoke
    {
        [DllImport("kernel32.dll")]
        public extern static IntPtr LoadLibrary(string path);
        [DllImport("kernel32.dll")]
        public extern static IntPtr GetProcAddress(IntPtr lib, String funcName);
        [DllImport("Kernel32.dll")]
        public extern static bool FreeLibrary(IntPtr lib);
        private IntPtr MLib;
        public CustomDLLInvoke(string dllPath)
        {
            var MLib = LoadLibrary(dllPath);
        }
        ~CustomDLLInvoke()
        {
            FreeLibrary(MLib);
        }
        public Delegate Invoke(string APIName, Type t)
        { IntPtr api = GetProcAddress(MLib, APIName); return (Delegate)Marshal.GetDelegateForFunctionPointer(api, t); }

        #region LoadLib使用方法
        //static IntPtr IntptrDll=CustomDLLInvoke.LoadLibrary("dll_Path");
        // static IntPtr pAddressOfFunctionToCall = CustomDLLInvoke.GetProcAddress(IntptrDll, "Dll_Function_EntryPoint");

        //byte[] ArrayInStr = Encoding.GetEncoding(950).GetBytes(sInStr);
        //byte[] ArraysOutStr = new byte[200];

        //[UnmanagedFunctionPointer(CallingConvention.StdCall)]
        //public delegate int THISDelgate(byte[] InBuffer, byte[] OutBuffer);

        //THISDelgate DelGate = (THISDelgate)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(THISDelgate));
        //var Result = DelGate.Invoke(ArrayInStr, ArraysOutStr);
        //sOutStr = (string) Encoding.GetEncoding(950).GetString(ArraysOutStr).TrimEnd('\0');
        //CustomDLLInvoke.FreeLibrary(hCSHIS);

        #endregion
    }

    //檔案建立
    public class CreateFile
    {
        public static void CreateXmlFile(string XmlString)
        {

            // 新增檔案
            XmlDocument DocForOutput = new XmlDocument();
            DocForOutput.LoadXml(XmlString);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            // Save the document to a file and auto-indent the output.
            // Save the document to a file and auto-indent the output.
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "Save an  File";
            saveFileDialog1.Filter = "XML-File | *.xml";
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                XmlWriter writer = XmlWriter.Create(saveFileDialog1.FileName, settings);
                DocForOutput.Save(writer);
                writer.Close();
            }
        }
        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return false;
        }
    }

    //日期格式轉換
    public class DateFormat
    {
        /// <summary>
        /// 1.公元年->民國年
        /// </summary>
        /// <param name="ADYear"></param>
        /// <returns></returns>
        public static string ADToROCYear(string ADYear)
        {
            //預設值，如果結果為-999代表參數錯誤，無轉換
            int intADYear = -999;
            int RocYear = -999;
            bool IsNum=int.TryParse(ADYear, out intADYear);
            
            if (IsNum)
            {
                RocYear = intADYear - 1911;
            }
            return RocYear.ToString();
        }

        /// <summary>
        /// 2.民國年->公元年
        /// </summary>
        /// <param name="ROCYear"></param>
        /// <returns></returns>
        public static string RocYearToAD(string ROCYear)
        {
            int intROCYear = -999;
            int ADYear = -999;
            bool IsNum = int.TryParse(ROCYear, out intROCYear);

            if (IsNum)
            {
                ADYear = intROCYear + 1911;
            }
            return ADYear.ToString();
        }
    }
}
