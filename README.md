# Shuttle.Core.Streams

## Installation

```bash
dotnet add package Shuttle.Core.Streams
```

Provides `Stream` extensions.

``` c#
Task<byte[]> ToBytesAsync(this Stream stream)
```

Creates an array of bytes from the given stream.  The stream position is reset once the operation has completed.

``` c#
Task<Stream> CopyAsync(this Stream stream)
```

Returns a copy of the given stream.  The underlying type used is a `MemoryStream`. If the given `stream` is a `MemoryStream` the operation will attempt to use the internal buffer and return a read-only stream; else a standard `MemoryStream` is used and the `stream` data copied to that. The copy will be at position 0 and the source `Stream` will remain at its original position.

