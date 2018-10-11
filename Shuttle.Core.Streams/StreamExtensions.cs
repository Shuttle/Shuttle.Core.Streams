using System.IO;
using Shuttle.Core.Contract;

namespace Shuttle.Core.Streams
{
    public static class StreamExtensions
    {
        /// <summary>
        ///     Creates an array of bytes from the given stream.  The stream position is reset once the operation has completed.
        /// </summary>
        /// <param name="stream">Input stream</param>
        /// <returns>An array of bytes</returns>
        public static byte[] ToBytes(this Stream stream)
        {
            using (var result = (MemoryStream)stream.Copy())
            {
                return result.ToArray();
            }
        }

        /// <summary>
        ///     Copies the stream to a new MemoryStream object without copying the internal buffer.
        ///     This method returning the same result as the <see cref="Copy"/> method when the instance it not a <see cref="MemoryStream"/>
        /// </summary>
        /// <param name="stream">Input stream</param>
        /// <returns>Memory stream</returns>
        public static MemoryStream CopyMemoryStream(this Stream stream)
        {
            Guard.AgainstNull(stream, nameof(stream));

            if (stream is MemoryStream)
            {
                var ms = (MemoryStream) stream;
                return new MemoryStream(ms.GetBuffer(), 0, (int) ms.Position);
            }
        
            return (MemoryStream)Copy(stream);
        }

        public static Stream Copy(this Stream stream)
        {
            Guard.AgainstNull(stream, nameof(stream));

            var result = new MemoryStream {Capacity = (int) stream.Length};

            var originalPosition = stream.Position;

            try
            {
                stream.Seek(0, SeekOrigin.Begin);

                stream.CopyTo(result);

                result.Seek(0, SeekOrigin.Begin);
            }
            finally
            {
                stream.Seek(originalPosition, SeekOrigin.Begin);
            }

            return result;
        }
    }
}