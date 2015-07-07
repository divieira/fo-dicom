using System;
using System.IO;
using Dicom.IO.Buffer;

namespace Dicom.IO
{
    public class FileByteTarget : IDisposable, IByteTarget
    {
        private FileReference _file;
        private Stream _stream;
        private Endian _endian;
        private object _lock;

        private MemoryStream _stream_buffer;
        private BinaryWriter _stream_buffer_writer;

        public FileByteTarget(FileReference file)
        {
            _file = file;
            _stream = _file.OpenWrite();
            _endian = Endian.LocalMachine;
            _lock = new object();

            _stream_buffer = new MemoryStream();
            _stream_buffer_writer = new EndianBinaryWriter(_stream_buffer, _endian);
        }

        public Endian Endian
        {
            get { return _endian; }
            set
            {
                if (_endian != value)
                {
                    lock (_lock)
                    {
                        _endian = value;
                        _stream_buffer_writer = EndianBinaryWriter.Create(_stream_buffer, _endian);
                    }
                }
            }
        }

        public long Position
        {
            get { return _stream_buffer.Position; }
        }

        public void Write(byte v)
        {
            _stream_buffer_writer.Write(v);
        }

        public void Write(short v)
        {
            _stream_buffer_writer.Write(v);
        }

        public void Write(ushort v)
        {
            _stream_buffer_writer.Write(v);
        }

        public void Write(int v)
        {
            _stream_buffer_writer.Write(v);
        }

        public void Write(uint v)
        {
            _stream_buffer_writer.Write(v);
        }

        public void Write(long v)
        {
            _stream_buffer_writer.Write(v);
        }

        public void Write(ulong v)
        {
            _stream_buffer_writer.Write(v);
        }

        public void Write(float v)
        {
            _stream_buffer_writer.Write(v);
        }

        public void Write(double v)
        {
            _stream_buffer_writer.Write(v);
        }

        public void Write(byte[] buffer, uint offset = 0, uint count = uint.MaxValue, ByteTargetCallback callback = null, object state = null)
        {
            if (count == uint.MaxValue)
                count = (uint)buffer.Length - offset;

            if (callback != null)
                _stream_buffer.BeginWrite(buffer, (int)offset, (int)count, OnEndWrite, new Tuple<ByteTargetCallback, object>(callback, state));
            else
            {
                _stream_buffer.Write(buffer, (int)offset, (int)count);
            }
        }
        private void OnEndWrite(IAsyncResult result)
        {
            try
            {
                _stream_buffer.EndWrite(result);
            }
            catch
            {
            }
            finally
            {
                if (result.AsyncState != null)
                {
                    Tuple<ByteTargetCallback, object> state = result.AsyncState as Tuple<ByteTargetCallback, object>;
                    state.Item1(this, state.Item2);
                }
            }
        }

        public void Close()
        {
            try
            {
                _stream_buffer.Close();
                _stream_buffer = null;
            }
            catch
            {
            }
        }

        public void Dispose()
        {
            _stream.Write(_stream_buffer.ToArray(), 0, (int)_stream_buffer.Length);
            Close();
        }
    }
}
