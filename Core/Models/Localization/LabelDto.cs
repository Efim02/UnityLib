namespace UnityLib.Core.Models.Localization
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// DTO надписи.
    /// </summary>
    [Serializable]
    public class LabelDto : IEquatable<LabelDto>
    {
        /// <summary>
        /// Ключ.
        /// </summary>
        [XmlAttribute]
        public string Key { get; set; }

        /// <summary>
        /// Надпись.
        /// </summary>
        [XmlAttribute]
        public string Label { get; set; }

        /// <inheritdoc />
        public bool Equals(LabelDto other)
        {
            return Key == other.Key;
        }
    }
}