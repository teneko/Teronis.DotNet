using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;
using Teronis.Json.Converters;

namespace Teronis.Drawing
{
    public struct RGBColor
    {
        public static readonly RGBColor Empty = new RGBColor();

        /// <summary>
        /// bgr-order
        /// </summary>
        [JsonConverter(typeof(ByteArrayConverter)), JsonProperty(PropertyName = "bgr")]
        public byte[] PixelArray { get; private set; }
        [JsonIgnore]
        public byte G { get { return PixelArray[1]; } }
        [JsonIgnore]
        public byte B { get { return PixelArray[0]; } }
        [JsonIgnore]
        public byte R { get { return PixelArray[2]; } }

        /// <summary>
        /// 0x00bbggrr
        /// </summary>
        [JsonIgnore]
        public uint BGR => ((uint)B << 8) | ((uint)G << 16) | ((uint)(R << 24));

        /// <summary>
        /// bgr-order
        /// </summary>
        /// <param name="pixelArray"></param>
        [JsonConstructor]
        public RGBColor(byte[] pixelArray)
        {
            PixelArray = pixelArray;
        }

        public RGBColor(byte r, byte g, byte b) : this(new byte[] { b, g, r }) { }

        /// <summary>
        /// b=g=r
        /// </summary>
        /// <param name="bgr"></param>
        public RGBColor(byte bgr) : this(bgr, bgr, bgr) { }

        public RGBColor(uint bgr) : this((byte)(bgr >> 24), (byte)(bgr >> 16), (byte)(bgr >> 8)) { }

        public bool Compare(byte r, byte g, byte b)
        {
            return (R == r && G == g && B == b);
        }

        public IEnumerable<RGBColor> AsEnumerable()
        {
            return new[] { this };
        }

        public static implicit operator Color(RGBColor color)
        {
            return Color.FromArgb(color.R, color.G, color.B);
        }
    }
}
