using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneBattle.Assets.Scripts.Models.SystemBase
{
    /// <summary>
    /// 敌人类型，越高越难
    /// </summary>
    public enum EnemyType
    {
        /// <summary>
        /// 普通
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 稀有
        /// </summary>
        Rare = 2,
        /// <summary>
        /// 史诗
        /// </summary>
        Epic = 3,
        /// <summary>
        /// 传说
        /// </summary>
        Legend = 4,
        /// <summary>
        /// 大Boss
        /// </summary>
        Boss = 5,
    }
}
