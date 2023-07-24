using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Fireworks_Firing_Systems.OrderType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Fireworks_Firing_Systems
{
    public partial class Order : Form
    {
        public Order()
        {
            InitializeComponent();
        }
    }

    public abstract class OrderType
    {
        private static Random rng = new Random();
        public abstract void Order(ref Dictionary<int, IgnitionObject> ignitionObjects);

        public class OrderRandom : OrderType
        {
            public override void Order(ref Dictionary<int, IgnitionObject> ignitionObjects)
            {
                ignitionObjects = ignitionObjects.Values.OrderBy(a => rng.Next()).ToList().Select((s, i) => new { s, i }).ToDictionary(x => x.i, x => x.s);
            }
        }

        public class OrderWeighted : OrderType
        {
            public Dictionary<FireworkType, int> Weighted = new Dictionary<FireworkType, int>
            { 
                {FireworkType.Cake, 1 },
                {FireworkType.Fountain, 1 },
                {FireworkType.Mine, 1 },
                {FireworkType.Missile, 1 },
                {FireworkType.Roman_Candle, 1 },
                {FireworkType.Smoke_Ball, 1 },
                {FireworkType.Sparkler, 1 }
            };
            public int NullWeight => Weighted.Values.Sum() / Weighted.Count;
            public override void Order(ref Dictionary<int, IgnitionObject> ignitionObjects)
            {
                ignitionObjects = ignitionObjects.Values.SelectMany(x => Enumerable.Repeat(x, x.fireworks.Count() == 0 ? NullWeight : x.fireworks.Select(x => x.Type).Distinct().Select(x => Weighted[x]).DefaultIfEmpty(0).Sum() / x.fireworks.Count())).OrderBy(a => rng.Next()).ToList().Distinct().Select((s, i) => new { s, i }).ToDictionary(x => x.i, x => x.s);
            }
        }
    }
}
