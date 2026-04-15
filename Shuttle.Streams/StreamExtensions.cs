using Shuttle.Contract;

namespace Shuttle.Streams;

public static class StreamExtensions
{
    extension(Stream stream)
    {
        /// <summary>
        ///     Returns a copy of the given stream.  The underlying type used is a `MemoryStream` and if the given `stream` is a
        ///     `MemoryStream` the operation will attempt to use internal buffer if exposed and return a read-only stream; else a
        ///     standard `MemoryStream` is used and the `stream` data copied to the that.
        /// </summary>
        /// <returns>A new `MemoryStream` object.</returns>
        public async Task<MemoryStream> CopyAsync(CancellationToken cancellationToken = default)
        {
            Guard.AgainstNull(stream);

            if (stream is MemoryStream ms && ms.TryGetBuffer(out var buffer))
            {
                return new(buffer.Array ?? throw new InvalidOperationException(Resources.CopyBufferArrayException), buffer.Offset, (int)ms.Length, false, true);
            }

            var result = new MemoryStream(stream is { CanSeek: true, Length: <= int.MaxValue } ? (int)stream.Length : 0);

            if (stream.CanSeek)
            {
                var originalPosition = stream.Position;
                try
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    await stream.CopyToAsync(result, cancellationToken).ConfigureAwait(false);
                    result.Seek(0, SeekOrigin.Begin);
                }
                finally
                {
                    stream.Seek(originalPosition, SeekOrigin.Begin);
                }
            }
            else
            {
                await stream.CopyToAsync(result, cancellationToken).ConfigureAwait(false);
                result.Seek(0, SeekOrigin.Begin);
            }

            return result;
        }

        /// <summary>
        ///     Creates an array of bytes from the given stream.  The stream position is reset once the operation has completed.
        /// </summary>
        /// <returns>An array of bytes</returns>
        public async Task<byte[]> ToBytesAsync()
        {
            using var result = await Guard.AgainstNull(stream).CopyAsync();
            return result.ToArray();
        }
    }
}