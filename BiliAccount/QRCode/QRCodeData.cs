using System.Collections;
using System.Collections.Generic;

namespace BiliAccount
{
    using System;
    using System.IO;
    using System.IO.Compression;

    public class QRCodeData : IDisposable
    {
        #region Public Constructors

        public QRCodeData(int version)
        {
            this.Version = version;
            var size = ModulesPerSideFromVersion(version);
            this.ModuleMatrix = new List<BitArray>();
            for (var i = 0; i < size; i++)
                this.ModuleMatrix.Add(new BitArray(size));
        }

        public QRCodeData(string pathToRawData, Compression compressMode) : this(File.ReadAllBytes(pathToRawData), compressMode)
        {
        }

        public QRCodeData(byte[] rawData, Compression compressMode)
        {
            var bytes = new List<byte>(rawData);

            //Decompress
            if (compressMode.Equals(Compression.Deflate))
            {
                using (var input = new MemoryStream(bytes.ToArray()))
                {
                    using (var output = new MemoryStream())
                    {
                        using (var dstream = new DeflateStream(input, CompressionMode.Decompress))
                        {
                            Stream4Methods.CopyTo(dstream, output);
                        }
                        bytes = new List<byte>(output.ToArray());
                    }
                }
            }
            else if (compressMode.Equals(Compression.GZip))
            {
                using (var input = new MemoryStream(bytes.ToArray()))
                {
                    using (var output = new MemoryStream())
                    {
                        using (var dstream = new GZipStream(input, CompressionMode.Decompress))
                        {
                            Stream4Methods.CopyTo(dstream, output);
                        }
                        bytes = new List<byte>(output.ToArray());
                    }
                }
            }

            if (bytes[0] != 0x51 || bytes[1] != 0x52 || bytes[2] != 0x52)
                throw new Exception("Invalid raw data file. Filetype doesn't match \"QRR\".");

            //Set QR code version
            var sideLen = (int)bytes[4];
            bytes.RemoveRange(0, 5);
            this.Version = (sideLen - 21 - 8) / 4 + 1;

            //Unpack
            var modules = new Queue<bool>();
            foreach (var b in bytes)
            {
                var bArr = new BitArray(new byte[] { b });
                for (int i = 7; i >= 0; i--)
                {
                    modules.Enqueue((b & (1 << i)) != 0);
                }
            }

            //Build module matrix
            this.ModuleMatrix = new List<BitArray>();
            for (int y = 0; y < sideLen; y++)
            {
                this.ModuleMatrix.Add(new BitArray(sideLen));
                for (int x = 0; x < sideLen; x++)
                {
                    this.ModuleMatrix[y][x] = modules.Dequeue();
                }
            }
        }

        #endregion Public Constructors

        #region Public Enums

        public enum Compression
        {
            Uncompressed,
            Deflate,
            GZip
        }

        #endregion Public Enums

        #region Public Properties

        public List<BitArray> ModuleMatrix { get; set; }
        public int Version { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void Dispose()
        {
            this.ModuleMatrix = null;
            this.Version = 0;
        }

        public byte[] GetRawData(Compression compressMode)
        {
            var bytes = new List<byte>();

            //Add header - signature ("QRR")
            bytes.AddRange(new byte[] { 0x51, 0x52, 0x52, 0x00 });

            //Add header - rowsize
            bytes.Add((byte)ModuleMatrix.Count);

            //Build data queue
            var dataQueue = new Queue<int>();
            foreach (var row in ModuleMatrix)
            {
                foreach (var module in row)
                {
                    dataQueue.Enqueue((bool)module ? 1 : 0);
                }
            }
            for (int i = 0; i < 8 - (ModuleMatrix.Count * ModuleMatrix.Count) % 8; i++)
            {
                dataQueue.Enqueue(0);
            }

            //Process queue
            while (dataQueue.Count > 0)
            {
                byte b = 0;
                for (int i = 7; i >= 0; i--)
                {
                    b += (byte)(dataQueue.Dequeue() << i);
                }
                bytes.Add(b);
            }
            var rawData = bytes.ToArray();

            //Compress stream (optional)
            if (compressMode.Equals(Compression.Deflate))
            {
                using (var output = new MemoryStream())
                {
                    using (var dstream = new DeflateStream(output, CompressionMode.Compress))
                    {
                        dstream.Write(rawData, 0, rawData.Length);
                    }
                    rawData = output.ToArray();
                }
            }
            else if (compressMode.Equals(Compression.GZip))
            {
                using (var output = new MemoryStream())
                {
                    using (GZipStream gzipStream = new GZipStream(output, CompressionMode.Compress, true))
                    {
                        gzipStream.Write(rawData, 0, rawData.Length);
                    }
                    rawData = output.ToArray();
                }
            }
            return rawData;
        }

        public void SaveRawData(string filePath, Compression compressMode)
        {
            File.WriteAllBytes(filePath, GetRawData(compressMode));
        }

        #endregion Public Methods

        #region Private Methods

        private static int ModulesPerSideFromVersion(int version)
        {
            return 21 + (version - 1) * 4;
        }

        #endregion Private Methods
    }
}