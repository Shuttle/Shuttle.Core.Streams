# Shuttle.Core.Streams

```
PM> Install-Package Shuttle.Core.Streams
```

Provides `Stream` extensions.

``` c#
byte[] ToBytes(this Stream stream)
Task<byte[]> ToBytesAsync(this Stream stream)
```

Returns the given `stream` as a `byte` array.

``` c#
Stream Copy(this Stream stream)
Task<Stream> CopyAsync(this Stream stream)
```

Creates a copyof the given `stream`.  THe copy will be at position 0 and the source `stream` will remain at its original position.

