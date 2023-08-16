namespace UnityLib.Core.Models.Localization
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// DTO хранилища надписей.
    /// </summary>
    [Serializable]
    [XmlRoot("LabelStorage")]
    public class LabelStorageDto
    {
        public LabelStorageDto()
        {
            Labels = new List<LabelDto>();
        }

        public LabelStorageDto(List<LabelDto> labelDtos)
        {
            Labels = labelDtos;
        }

        /// <summary>
        /// Надписи.
        /// </summary>
        [XmlArray("Labels")]
        [XmlArrayItem("Label")]
        public List<LabelDto> Labels { get; set; }
    }
}