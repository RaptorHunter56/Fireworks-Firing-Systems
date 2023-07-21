using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public abstract void Order();

        public class OrderRandom : OrderType
        {
            public override void Order()
            {
                throw new NotImplementedException();
            }
        }

        public class OrderWeighted : OrderType
        {
            public override void Order()
            {
                throw new NotImplementedException();
            }
        }
    }
}
