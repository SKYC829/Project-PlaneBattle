using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneBattle.Assets.Scripts
{
    /// <summary>
    /// 原抽奖控制器，在此用于可空概率的生成不同等级的敌人
    /// <para>原文:<see cref="https://www.cnblogs.com/qin160608/p/8145680.html"/></para>
    /// </summary>
    public class LotteryManager
    {
        /// <summary>
        /// 抽奖次数（单抽，十连抽）
        /// </summary>
        private int _Count;

        /// <summary>
        /// 抽奖次数（单抽，十连抽）
        /// </summary>
        public int Count { get { return _Count; } }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="count">抽奖次数，用uint是因为不允许抽负数的次数</param>
        public LotteryManager(uint count)
        {
            _Count = (int)count;
        }

        /// <summary>
        /// 不可控的抽奖（真·随机概率）
        /// </summary>
        /// <typeparam name="T">奖品类型</typeparam>
        /// <param name="rand">随机数</param>
        /// <param name="datas">奖池</param>
        /// <returns></returns>
        public T[] RandomLottery<T>(Random rand,List<T> datas)
        {
            List<T> result = new List<T>();
            if(rand != null)
            {
                //抽奖次数控制
                for (int i = 0; i < Count; i++)
                {
                    T item = datas[rand.Next(datas.Count - 1)]; //根据奖池的数量随机返回一个奖品
                    if (result.Contains(item))
                    {
                        continue; //防止获得重复的奖品，如果允许重复则这里可以注释
                    }
                    result.Add(item); //将获得的奖品添加到返回列表
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// 可空的抽奖（腾讯、网易云日常抽奖套路）
        /// </summary>
        /// <typeparam name="T">奖品类型</typeparam>
        /// <param name="rand">随机数</param>
        /// <param name="datas">奖池</param>
        /// <param name="weights">奖品权重</param>
        /// <returns></returns>
        public T[] ControlLottery<T>(Random rand, List<T> datas, List<uint> weights)
        {
            return ControlLottery<T>(rand, datas, dweights: weights.Cast<double>().ToList());
        }

        public T[] ControlLottery<T>(Random rand, List<T> datas, List<double> dweights)
        {
            List<T> result = new List<T>();
            if (rand != null)
            {
                //一个临时变量作为缓存，如果要根据抽奖次数增加概率，则这个变量要作为全局的静态变量，在分配权重的时候需要用到他
                Dictionary<T, double> cache = new Dictionary<T, double>();
                //给每一个奖品计算权重
                for (int i = 0; i < datas.Count; i++)
                {
                    //随机0~99乘以分配好的概率=奖品的中奖概率
                    cache.Add(datas[i], rand.Next(100) * dweights[i]);
                }
                //根据权重排序
                List<KeyValuePair<T, double>> sortResult = SortByValue<T>(cache);

                //取出排序后的集合里最前边的Count项
                foreach (KeyValuePair<T, double> item in sortResult.GetRange(0, Count))
                {
                    result.Add(item.Key);
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// 根据值排序集合
        /// </summary>
        /// <typeparam name="T">奖品</typeparam>
        /// <param name="vFrom">要排序的集合</param>
        /// <returns></returns>
        private List<KeyValuePair<T,uint>> SortByValue<T>(Dictionary<T, uint> vFrom)
        {
            List<KeyValuePair<T, uint>> result = new List<KeyValuePair<T, uint>>();
            if (vFrom != null)
            {
                result.AddRange(vFrom);
                result.Sort(delegate (KeyValuePair<T, uint> v1, KeyValuePair<T, uint> v2)
                {
                    return (int)(v2.Value - v1.Value);
                });
            }
            return result;
        }

        private List<KeyValuePair<T, double>> SortByValue<T>(Dictionary<T, double> dvFrom)
        {
            List<KeyValuePair<T, double>> result = new List<KeyValuePair<T, double>>();
            if (dvFrom != null)
            {
                result.AddRange(dvFrom);
                result.Sort(delegate (KeyValuePair<T, double> v1, KeyValuePair<T, double> v2)
                {
                    return (int)(v2.Value - v1.Value);
                });
            }
            return result;
        }
    }
}
