using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adform.ScalaLab.Segmentation.Infrastructure
{
    public abstract class FileLoaderBase
    {
        protected readonly string _pathToFile;
        protected StreamReader _reader;

        protected FileLoaderBase(string pathToFile)
        {
            _pathToFile = pathToFile;
        }

        public void Open()
        {
            if (_reader != null)
            {
                throw new InvalidOperationException("It looks like the file has already been opened for read purposes");
            }

            _reader = File.OpenText(_pathToFile);
        }

        public void Close()
        {
            if (_reader != null)
            {
                _reader.Close();
                _reader.Dispose();
                _reader = null;
            }
        }

        protected long GetLongIpByStr(string ipStr)
        {
            var ipParts = ipStr.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < ipParts.Length; i++)
            {
                ipParts[i] = ipParts[i].PadLeft(3, '0');
            }
            return Int64.Parse(string.Join(string.Empty, ipParts));
        }
    }
}