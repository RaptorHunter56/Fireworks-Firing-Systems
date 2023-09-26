using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Fireworks_Firing_Systems
{
    public class Firework
    {
        public int ID { get; set; }
        [ShortHand("AuthorName")]
        public string FireworkName { get; set; }
        public FireworkType Type { get; set; }
        public Color Colour { get; set; }
        public decimal Delay { get; set; }
        public decimal Length { get; set; }

        /// var temp = new Firework(File.ReadAllText("Fireworks\\0.json"));
        /// temp.Colour = Color.Blue;
        /// File.WriteAllTextAsync("Fireworks\\1.json", temp.ToJson());
        private Firework() { }
        public Firework(int ID_, string Name_, FireworkType Type_, Color Colour_, decimal Delay_, decimal Length_)
        {
            ID = ID_;
            FireworkName = Name_;
            Type = Type_;
            Colour = Colour_;
            Delay = Delay_;
            Length = Length_;
        }

        public override string ToString() => $"[{ID.ToString()}]{FireworkName}:{Type}({Colour})";
        public string ToJson() => Newtonsoft.Json.JsonConvert.SerializeObject(this);
        public static Firework FromJson(string json) => Newtonsoft.Json.JsonConvert.DeserializeObject<Firework>(json);
        public string GetIcon() => $"Images\\icons8_Firework_{Type}_100px.png";

        public static void DataGridViewSetup(DataGridView gridView)
        {
            foreach (var propertyInfo in (new Firework()).GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (propertyInfo.PropertyType == typeof(Color))
                    gridView.Columns.Add(CreateIndicatorColoumn());
                gridView.Columns.Add(CreateColoumn(propertyInfo));
            }
        }
        private static DataGridViewColumn CreateColoumn(PropertyInfo propertyInfo)
        {
            return (propertyInfo.PropertyType == typeof(FireworkType)) ? 
                new DataGridViewComboBoxColumn()
                {
                    DataPropertyName = propertyInfo.Name,
                    Name = (propertyInfo.GetCustomAttributes(false).ToDictionary(a => a.GetType().Name, a => a).ContainsKey("ShortHand")) ? propertyInfo.GetCustomAttributes(false).ToDictionary(a => a.GetType().Name, a => a)["ShortHand"].ToString() : propertyInfo.Name,
                    AutoSizeMode = (propertyInfo.Name == "FireworkName") ? DataGridViewAutoSizeColumnMode.Fill : DataGridViewAutoSizeColumnMode.AllCells,
                    DataSource = Enum.GetValues(typeof(FireworkType))
                } :
                new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = propertyInfo.Name,
                    Name = (propertyInfo.GetCustomAttributes(false).ToDictionary(a => a.GetType().Name, a => a).ContainsKey("ShortHand")) ? propertyInfo.GetCustomAttributes(false).ToDictionary(a => a.GetType().Name, a => a)["ShortHand"].ToString() : propertyInfo.Name,
                    AutoSizeMode = (propertyInfo.Name == "FireworkName") ? DataGridViewAutoSizeColumnMode.Fill : DataGridViewAutoSizeColumnMode.AllCells,
                    Tag = (propertyInfo.PropertyType == typeof(Color)) ? "Color" : null
                };
        }
        private static DataGridViewColumn CreateIndicatorColoumn() => new DataGridViewTextBoxColumn() { Name = "🎨", ReadOnly = true, Width = 10 };

        public ListViewItem CreateListViewItem() => new ListViewItem(new string[] { FireworkName, $"{Type.ToString()} - {Colour.ToString()}"}, -1);

        public override bool Equals(object obj) => Equals(obj as Firework);
        public bool Equals(Firework obj) => obj != null && obj.ToJson() == this.ToJson();

        class ShortHandAttribute : Attribute
        {
            public string Name { get; }
            public ShortHandAttribute(string name) => Name = name;
        }
    }

    public class IgnitionObject
    {
        public Dictionary<int, Firework> fireworks = new Dictionary<int, Firework>();
        public decimal Length { get { return fireworks.Values.Select(x => x.Length + x.Delay).Max(); } }

        public TableLayoutPanel CreateTableLayoutPanel()
        {
            Label groupLabel = new Label()
            {
                Text = $"Group ({fireworks.Count})"
            };
            Label lengthLabel = new Label()
            {
                AutoSize = true,
                Text = $"M-Length: {fireworks.Values.Select(x => x.Length).Max()}, M-Delay: {fireworks.Values.Select(x => x.Delay).Max()}, Total: {Length}" 
            };
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel()
            {
                AutoSize = true,
                Dock = DockStyle.Fill
            };
            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel()
            {
                AllowDrop = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                AutoSize = true,
                BackColor = Color.WhiteSmoke,
                ColumnCount = 1,
                RowCount = 3,
                Size = new Size(279, 62),
                Tag = this
            };

            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel.Controls.Add(groupLabel, 0, 0);

            int i = 1;
            foreach (var firework in fireworks.OrderBy(x => x.Key))
            {
                Button fireworkButton = new Button()
                {
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    Dock = DockStyle.Top,
                    Text = $"[{firework.Key}] {firework.Value.FireworkName}",
                    Tag = firework
                };
                flowLayoutPanel.Controls.Add(fireworkButton);
                flowLayoutPanel.Controls.SetChildIndex(fireworkButton, i);
                i++;
            }
            tableLayoutPanel.Controls.Add(flowLayoutPanel, 0, 1);

            tableLayoutPanel.Controls.Add(lengthLabel, 0, 2);
            tableLayoutPanel.Controls.SetChildIndex(lengthLabel, 2);
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());

            return tableLayoutPanel;
        }
    }

    /// <summary>
    /// https://www.fireworkshawkesbay.co.nz/types-of-fireworks#
    /// </summary>
    public enum FireworkType
    {
        Sparkler,
        Fountain,
        [Description("Smoke Ball")]
        Smoke_Ball,
        Missile,
        Mine,
        Cake,
        [Description("Roman Candle")]
        Roman_Candle
    }
}
