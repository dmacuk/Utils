using System.Windows;

namespace Utils.Window.Utils
{
    /// <summary>
    ///     Holder class for  windows position and size
    /// </summary>
    public class WindowLayout
    {
        /// <summary>
        ///     Window's height value
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        ///     Window's left value
        /// </summary>
        public double Left { get; set; }

        /// <summary>
        ///     Window's top value
        /// </summary>
        public double Top { get; set; }

        /// <summary>
        ///     Window's width value
        /// </summary>
        public double Width { get; set; }

        public WindowState WindowState { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((WindowLayout) obj);
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

        public override string ToString()
        {
            return $"Top: {Top}, Left: {Left}, Height: {Height}, Width: {Width}";
        }

        protected bool Equals(WindowLayout other)
        {
            return Top.Equals(other.Top) && Left.Equals(other.Left) && Height.Equals(other.Height) &&
                   Width.Equals(other.Width);
        }
    }
}