using System;
using System.Runtime.InteropServices;

namespace RoboTwin.UI.Core.Video
{
    /// <summary>
    /// Zero-allocation, high-performance video frame unmarshaler.
    /// Uses native C++ interop (FFmpeg/WebRTC decoded frames) and spans to avoid GC pressure.
    /// </summary>
    public unsafe class ZeroCopyVideoFrame : IDisposable
    {
        private nint _nativeFramePtr;
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Stride { get; private set; }
        
        // Expose as Memory<byte> or Span<byte> to avoid copying buffer into C# managed arrays.
        private byte* _dataPtr;
        private int _bufferSize;

        public Span<byte> FrameData => new Span<byte>(_dataPtr, _bufferSize);

        [DllImport("robotwin_native_core", CallingConvention = CallingConvention.Cdecl)]
        private static extern void release_native_frame(nint framePtr);

        public ZeroCopyVideoFrame(nint nativeFramePtr, int width, int height, int stride, nint dataPtr, int bufferSize)
        {
            _nativeFramePtr = nativeFramePtr;
            Width = width;
            Height = height;
            Stride = stride;
            _dataPtr = (byte*)dataPtr;
            _bufferSize = bufferSize;
        }

        /// <summary>
        /// Reads frame directly to UI texture without allocations.
        /// </summary>
        public void BlitToTexture(nint destTexturePtr, int destStride)
        {
            // Fast unmanaged memory copy directly from C++ buffer to DirectX/Vulkan texture memory
            Buffer.MemoryCopy(
                source: _dataPtr,
                destination: (void*)destTexturePtr,
                destinationSizeInBytes: Height * destStride,
                sourceBytesToCopy: _bufferSize);
        }

        public void Dispose()
        {
            if (_nativeFramePtr != nint.Zero)
            {
                // Return buffer to native C++ pool
                release_native_frame(_nativeFramePtr);
                _nativeFramePtr = nint.Zero;
                _dataPtr = null;
            }
            GC.SuppressFinalize(this);
        }

        ~ZeroCopyVideoFrame()
        {
            Dispose();
        }
    }
}
