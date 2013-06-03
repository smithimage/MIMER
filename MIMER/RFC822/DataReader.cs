using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MIMER.RFC822
{
    public class DataReader
    {
        public event DataReadEventHandler DataRead = null;

        public IList<IEndCriteriaStrategy> Criterias { get; set; }
        
        protected long m_BytesRead;
        private long m_UpdateInterval = 1;

        public DataReader(params IEndCriteriaStrategy[] endCriterias)
        {
            Criterias = endCriterias;
        }

        public Result ReadData(ref Stream dataStream)
        {
            int fulfilledCritera = -1;
            int size, pos, c;
            char[] buffer, data;

            size = 1;
            pos = 0;
            buffer = new char[size];

            while ((c = dataStream.ReadByte()) != -1)
            {
                m_BytesRead++;
                if ((m_BytesRead % m_UpdateInterval) == 0 && pos > 0)
                {
                    DataReadEventArgs args = new DataReadEventArgs();
                    args.AmountRead = m_BytesRead;
                    OnDataRead(this, args);
                }

                if (pos >= (size - 1))
                {
                    size = size * 2;
                    char[] tmpBuffer = new char[size];
                    buffer.CopyTo(tmpBuffer, 0);
                    buffer = null;
                    buffer = tmpBuffer;
                }
                buffer[pos] = (char)c;

                if (pos > 0)
                {
                    int i = 0;
                    foreach (IEndCriteriaStrategy criteria in Criterias)
                    {
                        if (criteria.IsEndReached(buffer, pos))
                        {
                            fulfilledCritera = i;
                            break;
                        }
                        i++;
                    }
                }

                if (fulfilledCritera > -1)
                {
                    break;
                }
                pos++;
            }

            data = new char[pos + 1];
            Array.Copy(buffer, data, pos + 1);
            buffer = null;
            return new Result() { Data = data, FulfilledCritera = fulfilledCritera };
        }

        protected void OnDataRead(object sender, DataReadEventArgs args)
        {
            if (DataRead != null)
            {
                DataRead(sender, args);
            }
        }

        public class Result
        {
            public char[] Data { get; internal set; }
            public int FulfilledCritera { get; internal set; }
        }
    }
}
