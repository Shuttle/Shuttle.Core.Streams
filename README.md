# Shuttle.Core.Streams

```
PM> Install-Package Shuttle.Core.Streams
```

Provides `Stream` extensions.

``` c#
Task<byte[]> ToBytesAsync(this Stream stream)
```

Returns the given `Stream` as a `byte` array.

``` c#
Task<Stream> CopyAsync(this Stream stream)
```

Creates a copy of the given `Stream`.  THe copy will be at position 0 and the source `Stream` will remain at its original position.

