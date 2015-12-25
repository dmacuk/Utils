namespace Utils.Window
{
    /// <summary>
    /// Holder class for  windows position and size
    /// </summary>
    public class WindowSettings
    {
        /// <summary>
        /// Window's top value
        /// </summary>
        public double Top { get; set; }
        /// <summary>
        /// Window's left value
        /// </summary>
        public double Left { get; set; }
        /// <summary>
        /// Window's height value
        /// </summary>
        public double Height { get; set; }
        /// <summary>
        /// Window's width value
        /// </summary>
        public double Width { get; set; }

        public override string ToString()
        {
            return string.Format("Top: {0}, Left: {1}, Height: {2}, Width: {3}", Top, Left, Height, Width);
        }

        protected bool Equals(WindowSettings other)
        {
            return Top.Equals(other.Top) && Left.Equals(other.Left) && Height.Equals(other.Height) && Width.Equals(other.Width);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WindowSettings) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Top.GetHashCode();
                hashCode = (hashCode*397) ^ Left.GetHashCode();
                hashCode = (hashCode*397) ^ Height.GetHashCode();
                hashCode = (hashCode*397) ^ Width.GetHashCode();
                return hashCode;
            }
        }
    }
}