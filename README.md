# Shuttle.Core.Streams

`Shuttle.Core.Streams` provides essential `Stream` extension methods to simplify common operations like copying and byte array conversion.

## Installation

```bash
dotnet add package Shuttle.Core.Streams
```

## Stream Extensions

```csharp
Task<byte[]> ToBytesAsync()
```

Creates an array of bytes from the given stream. The source stream's position is preserved throughout the operation (it is restored to its original state once the operation completes).

```csharp
Task<Stream> CopyAsync()
```

Returns a copy of the given stream as a `MemoryStream`.

**Optimizations:**

If the source stream is a `MemoryStream` and its internal buffer is accessible, `CopyAsync` will return a read-only `MemoryStream` over the same buffer (a zero-copy operation). Otherwise, a standard `MemoryStream` is used and the source data is copied.

**Positioning:**

- The returned copy will be positioned at `0`.
- The source stream's position is **preserved** (restored to its original state).

## Usage

```csharp
using Shuttle.Core.Streams;

// ... inside an async method ...

// Convert a stream to a byte array
byte[] data = await myStream.ToBytesAsync();

// Create an efficient copy of a stream
using (Stream copy = await myStream.CopyAsync())
{
    // work with the copy, myStream position remains unchanged
}
```

