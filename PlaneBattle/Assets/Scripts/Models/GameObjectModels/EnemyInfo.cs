using PlaneBattle.Assets.Scripts.Models.SystemBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PlaneBattle.Assets.Scripts.Models.GameObjectModels
{
    [Serializable]
    public class EnemyInfo
    {
        public float Speed;

        public float AttackInterval;

        public float Health;

        public RocketInfo RocketData;

        public Vector3 Scale;

        public EnemyType Level;
    }
}
