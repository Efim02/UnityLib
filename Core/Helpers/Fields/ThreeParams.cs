namespace UnityLib.Core.Helpers.Fields
{
    using System;

    using UnityEngine;

    /// <summary>
    /// Серализуемый класс с тремя параметрами.
    /// </summary>
    /// <remarks> Пригодился, когда к объекту в зависимости от игрового режима подсоединяли разные объекты. </remarks>
    [Serializable]
    public class ThreeParams<TQ, TW, TE>
    {
        [SerializeField]
        private TE _param3;

        [SerializeField]
        private TQ _param1;

        [SerializeField]
        private TW _param2;

        public TQ Param1 => _param1;

        public TW Param2 => _param2;

        public TE Param3 => _param3;
    }
}