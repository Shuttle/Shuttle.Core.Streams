using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Shuttle.Core.Streams.Tests;

[TestFixture]
public class StreamExtensionsFixture
{
    [Test]
    public async Task Should_be_able_to_convert_a_stream_to_an_array_of_bytes_async()
    {
        var stream = new MemoryStream(new byte[] { 0, 1, 2, 3, 4 });
        var bytes = await stream.ToBytesAsync();

        Assert.That(bytes.Length, Is.EqualTo(5));
        Assert.That(bytes[0], Is.EqualTo(0));
        Assert.That(bytes[4], Is.EqualTo(4));
    }

    [Test]
    public async Task Should_be_able_to_make_a_copy_of_a_stream_async()
    {
        var bytes = new byte[] { 0, 1, 2, 3, 4 };
        var stream = new MemoryStream(bytes);
        var output = new MemoryStream();

        await stream.CopyToAsync(output);

        Assert.That(output.Length, Is.EqualTo(5));
        Assert.That(output.Position, Is.EqualTo(5));
        Assert.That(stream.Position, Is.EqualTo(5));

        var copy = await stream.CopyAsync();

        Assert.That(copy.Length, Is.EqualTo(5));
        Assert.That(copy.Position, Is.EqualTo(0));
        Assert.That(stream.Position, Is.EqualTo(5));
    }

    [Test]
    public async Task Should_be_able_to_make_a_readonly_copy_of_a_stream_async()
    {
        var bytes = new byte[] { 0, 1, 2, 3, 4 };
        var stream = new MemoryStream(bytes, 0, bytes.Length, false, true);
        var output = new MemoryStream();

        await stream.CopyToAsync(output);

        Assert.That(output.Length, Is.EqualTo(5));
        Assert.That(output.Position, Is.EqualTo(5));
        Assert.That(stream.Position, Is.EqualTo(5));

        var copy = await stream.CopyAsync();

        Assert.That(copy.Length, Is.EqualTo(5));
        Assert.That(copy.Position, Is.EqualTo(0));
        Assert.That(stream.Position, Is.EqualTo(5));
    }
}