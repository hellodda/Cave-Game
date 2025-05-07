namespace Cave_Game.Core
{
    public class FloatBuffer
    {
        private readonly float[] _buffer;
        private int _position;
        private int _limit;
        private readonly int _capacity;

        public int Position => _position;
        public int Limit => _limit;
        public int Capacity => _capacity;
        public bool HasRemaining => _position < _limit;
        public int Remaining => _limit - _position;

        public FloatBuffer(int capacity)
        {
            _buffer = new float[capacity];
            Array.Clear(_buffer, 0, capacity); 
            _capacity = capacity;
            _limit = capacity;
            _position = 0;
        }

        public FloatBuffer Put(float value)
        {
            if (_position >= _limit)
                throw new InvalidOperationException("Buffer overflow");

            _buffer[_position++] = value;
            return this;
        }

        public FloatBuffer Put(int index, float value)
        {
            if (index < 0 || index >= _limit)
                throw new ArgumentOutOfRangeException(nameof(index));

            _buffer[index] = value;
            return this;
        }

        public float Get()
        {
            if (_position >= _limit)
                throw new InvalidOperationException("Buffer underflow");

            return _buffer[_position++];
        }

        public float Get(int index)
        {
            if (index < 0 || index >= _limit)
                throw new ArgumentOutOfRangeException(nameof(index));

            return _buffer[index];
        }

        public FloatBuffer Flip()
        {
            _limit = _position;
            _position = 0;
            return this;
        }

        public FloatBuffer Clear()
        {
            _position = 0;
            _limit = _capacity;
            return this;
        }

        public FloatBuffer Rewind()
        {
            _position = 0;
            return this;
        }

        public FloatBuffer Clip()
        {
            int newSize = Remaining;
            float[] clipped = new float[newSize];
            Array.Copy(_buffer, _position, clipped, 0, newSize);
            return Wrap(clipped);
        }

        public float[] ToArray()
        {
            float[] result = new float[_limit];
            Array.Copy(_buffer, 0, result, 0, _limit);
            return result;
        }

        public Span<float> AsSpan() => _buffer.AsSpan(_position, Remaining);

        public static FloatBuffer Wrap(float[] array)
        {
            var buf = new FloatBuffer(array.Length);
            Array.Copy(array, buf._buffer, array.Length);
            return buf;
        }
    }

}
