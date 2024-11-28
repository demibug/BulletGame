using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

namespace GameNetwork.UnityWebSocket
{
    public sealed class UnityWebSocketStreamBufferPool
    {
        private const int MAX_TOTAL_POOL_SIZE = 1000; // 总缓存池大小限制

        private static readonly ConcurrentDictionary<int, ConcurrentQueue<StreamBuffer>> streamPool = new();
        private static readonly ConcurrentDictionary<int, ConcurrentQueue<byte[]>> bufferPool = new();

        private static volatile int streamCount = 0;
        private static volatile int totalPoolCount = 0;

        public static StreamBuffer GetStream(int expectedSize, bool canWrite, bool canRead)
        {
            if (expectedSize <= 0)
                throw new ArgumentException("expectedSize must be greater than 0.");

            var streamCache = streamPool.GetOrAdd(expectedSize, _ => new ConcurrentQueue<StreamBuffer>());

            if (streamCache.TryDequeue(out var streamBuffer))
            {
                Interlocked.Decrement(ref streamCount);
                Interlocked.Decrement(ref totalPoolCount);
                streamBuffer.SetOperate(canWrite, canRead);
                return streamBuffer;
            }

            return new StreamBuffer(expectedSize, canWrite, canRead);
        }

        public static void RecycleStream(StreamBuffer stream)
        {
            if (stream == null || stream.size == 0 || totalPoolCount >= MAX_TOTAL_POOL_SIZE)
                return;

            var streamCache = streamPool.GetOrAdd(stream.size, _ => new ConcurrentQueue<StreamBuffer>());

            stream.ClearBuffer();
            stream.ResetStream();
            streamCache.Enqueue(stream);

            Interlocked.Increment(ref streamCount);
            Interlocked.Increment(ref totalPoolCount);
        }

        public static byte[] GetBuffer(int expectedSize)
        {
            if (expectedSize <= 0)
                throw new ArgumentException("expectedSize must be greater than 0.");

            var bufferCache = bufferPool.GetOrAdd(expectedSize, _ => new ConcurrentQueue<byte[]>());

            if (bufferCache.TryDequeue(out var buffer))
            {
                Interlocked.Decrement(ref totalPoolCount);
                return buffer;
            }

            return new byte[expectedSize];
        }

        public static void RecycleBuffer(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0 || totalPoolCount >= MAX_TOTAL_POOL_SIZE)
                return;

            var bufferCache = bufferPool.GetOrAdd(buffer.Length, _ => new ConcurrentQueue<byte[]>());
            Array.Clear(buffer, 0, buffer.Length);
            bufferCache.Enqueue(buffer);

            Interlocked.Increment(ref totalPoolCount);
        }

        public static byte[] DeepCopy(byte[] bytes)
        {
            if (bytes == null)
                return null;

            var newBytes = GetBuffer(bytes.Length);
            Buffer.BlockCopy(bytes, 0, newBytes, 0, bytes.Length);
            return newBytes;
        }

        public static void Cleanup()
        {
            foreach (var key in streamPool.Keys)
            {
                if (streamPool.TryGetValue(key, out var queue) && queue.IsEmpty)
                {
                    streamPool.TryRemove(key, out _);
                }
            }

            foreach (var key in bufferPool.Keys)
            {
                if (bufferPool.TryGetValue(key, out var queue) && queue.IsEmpty)
                {
                    bufferPool.TryRemove(key, out _);
                }
            }
        }
    }

    public sealed class StreamBuffer : IDisposable
    {
        private byte[] mBuffer;
        private MemoryStream mMemStream;
        private BinaryReader mBinaryReader;
        private BinaryWriter mBinaryWriter;

        public StreamBuffer(int bufferSize, bool canWrite, bool canRead)
        {
            if (bufferSize <= 0)
                throw new ArgumentException("bufferSize must be greater than 0.");

            mBuffer = new byte[bufferSize];
            SetOperate(canWrite, canRead);
        }

        internal void SetOperate(bool canWrite, bool canRead)
        {
            if (!canWrite && size <= 0)
                throw new ArgumentException("bufferSize must be greater than 0 when cannot write.");

            this.canWrite = canWrite;
            this.canRead = canRead;
        }

        public bool canWrite { get; private set; }
        public bool canRead { get; private set; }

        public MemoryStream memStream
        {
            get
            {
                if (!canRead && !canWrite)
                    throw new InvalidOperationException("The stream buffer cannot read and cannot write!");

                if (mMemStream == null)
                    mMemStream = new MemoryStream(mBuffer, 0, mBuffer.Length, true, true);

                return mMemStream;
            }
        }

        public BinaryReader binaryReader
        {
            get
            {
                if (!canRead)
                    throw new InvalidOperationException("The stream buffer cannot read!");

                if (mBinaryReader == null)
                    mBinaryReader = new BinaryReader(memStream);

                return mBinaryReader;
            }
        }

        public BinaryWriter binaryWriter
        {
            get
            {
                if (!canWrite)
                    throw new InvalidOperationException("The stream buffer cannot write!");

                if (mBinaryWriter == null)
                    mBinaryWriter = new BinaryWriter(memStream);

                return mBinaryWriter;
            }
        }

        public int size => mBuffer.Length;

        public void CopyFrom(byte[] src, int srcOffset, int dstOffset, int length)
        {
            Buffer.BlockCopy(src, srcOffset, mBuffer, dstOffset, length);
        }

        public void CopyTo(byte[] dst, int srcOffset, int dstOffset, int length)
        {
            Buffer.BlockCopy(mBuffer, srcOffset, dst, dstOffset, length);
        }

        public long Position() => mMemStream?.Position ?? 0;

        public byte[] ToArray() => UnityWebSocketStreamBufferPool.DeepCopy(mBuffer);

        public void ClearBuffer() => Array.Clear(mBuffer, 0, mBuffer.Length);

        public void ResetStream() => mMemStream?.Seek(0, SeekOrigin.Begin);

        public void Dispose()
        {
            mBinaryReader?.Close();
            mBinaryWriter?.Close();
            mMemStream?.Dispose();
            mBinaryReader = null;
            mBinaryWriter = null;
            mMemStream = null;
        }
    }
}
