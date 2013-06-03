
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MIMER.RFC2045.Pattern
{
   public class ApplicationSubTypePatern : ICompiledPattern
   {
      private IList<string> m_SubTypes;
      private string m_TextPattern;
      private Regex m_Regex;

      public ApplicationSubTypePatern()
      {
         SubTypes = new List<string>();
         SubTypes.Add("gif");
         SubTypes.Add("jpeg");
         SubTypes.Add("msword");
         SubTypes.Add("octet-stream");
         SubTypes.Add("PostScript");
         SubTypes.Add("pdf");
         SubTypes.Add("bmp");
         SubTypes.Add("msaccess");
         SubTypes.Add("msexcel");
         SubTypes.Add("vnd.ms-excel");
         SubTypes.Add("X-MS-Excel");
         SubTypes.Add("vnd.ms-powerpoint");
         SubTypes.Add("x-zip-compressed");
         SubTypes.Add("vnd.openxmlformats-officedocument.spreadsheetml.sheet");

         Compile();
      }

      public string TextPattern
      {
         get { return m_TextPattern; }
      }

      public Regex RegularExpression
      {
         get { return m_Regex; }
      }

      public IList<string> SubTypes
      {
         get { return m_SubTypes; }
         set { m_SubTypes = value; }
      }

      public void Compile()
      {
         StringBuilder builder = new StringBuilder();
         builder.Append("(");
         for (int i = 0; i < SubTypes.Count; i++)
         {
            builder.Append(SubTypes[i]);
            if (i < SubTypes.Count)
            {
               builder.Append("|");
            }
         }
         builder.Append(")");

         m_TextPattern = builder.ToString();
         m_Regex = new Regex(m_TextPattern, RegexOptions.Compiled);
      }
   }
}
