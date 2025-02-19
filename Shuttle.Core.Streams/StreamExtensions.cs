using System;
using System.IO;
using System.Threading.Tasks;
using Shuttle.Core.Contract;

namespace Shuttle.Core.Streams;

public static class StreamExtensions
{
    /// <summary>
    ///     Returns a copy of the given stream.  The underlying type used is a `MemoryStream` and if the given `stream` is a
    ///     `MemoryStream` the operation will attempt to use internal buffer if exposed and return a read-only stream; else a
    ///     standard `MemoryStream` is used and the `stream` data copied to the that.
    /// </summary>
    /// <param name="stream">The `Stream` instance that contains the source data.</param>
    /// <returns>A new `MemoryStream` object.</returns>
    public static async Task<Stream> CopyAsync(this Stream stream)
    {
        Guard.AgainstNull(stream);

        MemoryStream result;

        if (stream is MemoryStream ms && ms.TryGetBuffer(out var buffer))
        {
            result = new(buffer.Array ?? throw new InvalidOperationException(Resources.CopyBufferArrayException), buffer.Offset, (int)ms.Length, false, true);
        }
        else
        {
            result = new() { Capacity = (int)stream.Length };

            var originalPosition = stream.Position;

            try
            {
                stream.Seek(0, SeekOrigin.Begin);

                await stream.CopyToAsync(result).ConfigureAwait(false);

                result.Seek(0, SeekOrigin.Begin);
            }
            finally
            {
                stream.Seek(originalPosition, SeekOrigin.Begin);
            }
        }

        return result;
    }

    /// <summary>
    ///     Creates an array of bytes from the given stream.  The stream position is reset once the operation has completed.
    /// </summary>
    /// <param name="stream">Input stream</param>
    /// <returns>An array of bytes</returns>
    public static async Task<byte[]> ToBytesAsync(this Stream stream)
    {
        using (var result = (MemoryStream)await Guard.AgainstNull(stream).CopyAsync())
        {
            return await Task.FromResult(result.ToArray());
        }
    }
}