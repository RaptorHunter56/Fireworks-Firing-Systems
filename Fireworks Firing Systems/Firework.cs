using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Serialization;
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
        public List<Firework> fireworks = new List<Firework>();
        public int Length = 0;
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
