using System;
using System.Text;
using GameBase;

namespace GameNetwork
{
    public class ShortConnectByteBuffer
    {
        public static ShortConnectByteBuffer RequestByteBuffer(int reqUid, byte[] data)
        {
            // size : 【rid, 数据】
            int size = ShortConnectDefines.ReqUidLen + data.Length;
            // TODO: BufferPool
            // byte[] bytes = BufferPool.Get(size, false);
            // byte[] bytes = new byte[size];
            byte[] bytes = ShortConnectStreamBufferPool.GetBuffer(size);
            ShortConnectByteBuffer byteBuffer = new ShortConnectByteBuffer();
            byteBuffer.buffer = bytes;

            byteBuffer.WriteInt(reqUid);
            byteBuffer.WriteBytes(data);

            return byteBuffer;
        }

        public static ShortConnectByteBuffer GateRequestByteBuffer(byte[] data)
        {
            // size : 【rid, 数据】
            int size = data.Length;
            // TODO: BufferPool
            // byte[] bytes = BufferPool.Get(size, false);
            // byte[] bytes = new byte[size];
            byte[] bytes = ShortConnectStreamBufferPool.GetBuffer(size);
            ShortConnectByteBuffer byteBuffer = new ShortConnectByteBuffer();
            byteBuffer.buffer = bytes;

            byteBuffer.WriteBytes(data);

            return byteBuffer;
        }

        public static ShortConnectByteBuffer ResponseByteBuffer(byte[] data)
        {
            ShortConnectByteBuffer byteBuffer = new ShortConnectByteBuffer();
            byteBuffer.buffer = data;
            return byteBuffer;
        }

        private static readonly int INIT_SIZE = 128;
        private static readonly int MAX_SIZE = 655537;

        private int writeOffset = 0;
        private int readOffset = 0;
        public byte[] buffer;
        private bool isBigEndian = true;

        // -------------------------------------------------get/set-------------------------------------------------
        public int WriteOffset()
        {
            return writeOffset;
        }

        public void SetWriteOffset(int writeIndex)
        {
            if (writeIndex > buffer.Length)
            {
                throw new Exception("writeIndex[" + writeIndex + "] out of bounds exception: readerIndex: " +
                                    readOffset +
                                    ", writerIndex: " + writeOffset +
                                    "(expected: 0 <= readerIndex <= writerIndex <= capacity:" + buffer.Length);
            }

            writeOffset = writeIndex;
        }

        public void SetReadOffset(int readIndex)
        {
            if (readIndex > writeOffset)
            {
                throw new Exception("readIndex[" + readIndex + "] out of bounds exception: readerIndex: " + readOffset +
                                    ", writerIndex: " + writeOffset +
                                    "(expected: 0 <= readerIndex <= writerIndex <= capacity:" + buffer.Length);
            }

            readOffset = readIndex;
        }

        public byte[] ToBytes()
        {
            var bytes = new byte[writeOffset];
            Array.Copy(buffer, 0, bytes, 0, writeOffset);
            return bytes;
        }

        public bool IsReadable()
        {
            return writeOffset > readOffset;
        }


        public void WriteByte(byte value)
        {
            EnsureCapacity(1);
            buffer[writeOffset] = value;
            writeOffset++;
        }

        public byte ReadByte()
        {
            var byteValue = buffer[readOffset];
            readOffset++;
            return byteValue;
        }


        public int GetCapacity()
        {
            return buffer.Length - writeOffset;
        }

        public void EnsureCapacity(int capacity)
        {
            while (capacity - GetCapacity() > 0)
            {
                var newSize = buffer.Length * 2;
                if (newSize > MAX_SIZE)
                {
                    throw new Exception("Bytebuf max size is [655537], out of memory error");
                }

                var newBytes = new byte[newSize];
                Array.Copy(buffer, 0, newBytes, 0, buffer.Length);
                this.buffer = newBytes;
            }
        }

        public void WriteBytes(byte[] bytes)
        {
            EnsureCapacity(bytes.Length);
            var length = bytes.Length;
            Array.Copy(bytes, 0, buffer, writeOffset, length);
            writeOffset += length;
        }

        public byte[] ReadBytes(int count, bool isPool = false)
        {
            byte[] bytes = null;
            if (count == 0)
            {
                GameLog.WarringInfo("Respone data size is 0 !!! ");
                bytes = Array.Empty<byte>();
            }
            else
            {
                if (isPool)
                {
                    // TODO: BufferPool
                    // bytes = BufferPool.Get(count, false);
                    // bytes = new byte[count];
                    bytes = ShortConnectStreamBufferPool.GetBuffer(count);
                }
                else
                {
                    bytes = new byte[count];
                }

                Array.Copy(buffer, readOffset, bytes, 0, count);
            }

            readOffset += count;
            return bytes;
        }

        public void WriteUint(uint value)
        {
            if (isBigEndian)
            {
                WriteByte((byte)((value >> 24) & 0xFF)); // 写入高字节
                WriteByte((byte)((value >> 16) & 0xFF)); // 写入次高字节
                WriteByte((byte)((value >> 8) & 0xFF)); // 写入次低字节
                WriteByte((byte)(value & 0xFF)); // 写入低字节
            }
            else
            {
                WriteByte((byte)(value & 0xFF)); // 写入低字节
                WriteByte((byte)((value >> 8) & 0xFF)); // 写入次低字节
                WriteByte((byte)((value >> 16) & 0xFF)); // 写入次高字节
                WriteByte((byte)((value >> 24) & 0xFF)); // 写入高字节
            }
        }

        public uint ReadUint()
        {
            if (isBigEndian)
            {
                uint result = 0;

                // 读取高字节
                byte byte1 = ReadByte();
                result |= ((uint)byte1 << 24);

                // 读取次高字节
                byte byte2 = ReadByte();
                result |= ((uint)byte2 << 16);

                // 读取次低字节
                byte byte3 = ReadByte();
                result |= ((uint)byte3 << 8);

                // 读取低字节
                byte byte4 = ReadByte();
                result |= byte4;

                return result;
            }
            else
            {
                uint result = 0;

                // 读取低字节
                byte byte1 = ReadByte();
                result |= byte1;

                // 读取次低字节
                byte byte2 = ReadByte();
                result |= ((uint)byte2 << 8);

                // 读取次高字节
                byte byte3 = ReadByte();
                result |= ((uint)byte3 << 16);

                // 读取高字节
                byte byte4 = ReadByte();
                result |= ((uint)byte4 << 24);

                return result;
            }
        }

        public void WriteInt(int value)
        {
            if (isBigEndian)
            {
                WriteByte((byte)((value >> 24) & 0xFF)); // 写入高字节
                WriteByte((byte)((value >> 16) & 0xFF)); // 写入次高字节
                WriteByte((byte)((value >> 8) & 0xFF)); // 写入次低字节
                WriteByte((byte)(value & 0xFF)); // 写入低字节
            }
            else
            {
                WriteByte((byte)(value & 0xFF)); // 写入低字节
                WriteByte((byte)((value >> 8) & 0xFF)); // 写入次低字节
                WriteByte((byte)((value >> 16) & 0xFF)); // 写入次高字节
                WriteByte((byte)((value >> 24) & 0xFF)); // 写入高字节
            }
        }

        public int ReadInt()
        {
            if (isBigEndian)
            {
                int result = 0;
                result |= (ReadByte() << 24); // 读取高字节
                result |= (ReadByte() << 16); // 读取次高字节
                result |= (ReadByte() << 8); // 读取次低字节
                result |= ReadByte(); // 读取低字节
                return result;
            }
            else
            {
                int result = 0;
                result |= ReadByte(); // 读取低字节
                result |= (ReadByte() << 8); // 读取次低字节
                result |= (ReadByte() << 16); // 读取次高字节
                result |= (ReadByte() << 24); // 读取高字节
                return result;
            }
        }


        // -------------------------------------------------Converter-------------------------------------------------
        private static readonly byte[] EMPTY_BYTE_ARRAY = new byte[] { };


        /// <summary>
        /// 以 UTF-8 字节数组的形式返回指定的字符串。
        /// </summary>
        /// <param name="value">要转换的字符串。</param>
        /// <returns>UTF-8 字节数组。</returns>
        public byte[] GetBytes(string value)
        {
            if (value == null)
            {
                return EMPTY_BYTE_ARRAY;
            }

            return Encoding.UTF8.GetBytes(value);
        }

        /// <summary>
        /// 返回由 UTF-8 字节数组转换来的字符串。
        /// </summary>
        /// <param name="value">UTF-8 字节数组。</param>
        /// <returns>字符串。</returns>
        public string GetString(byte[] value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            return Encoding.UTF8.GetString(value, 0, value.Length);
        }
    }
}
