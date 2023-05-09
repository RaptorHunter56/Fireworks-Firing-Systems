using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;

namespace Fireworks_Firing_Systems
{
    public class Firework
    {
        public int ID { get; set; }
        public string FireworkName { get; set; }
        public FireworkType Type { get; set; }
        public Color Colour { get; set; }
        public int Delay { get; set; }
        public int Length { get; set; }

        /// var temp = new Firework(File.ReadAllText("Fireworks\\0.json"));
        /// temp.Colour = Color.Blue;
        /// File.WriteAllTextAsync("Fireworks\\1.json", temp.ToJson());
        public Firework(int ID_, string Name_, FireworkType Type_, Color Colour_, int Delay_, int Length_)
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
        public string GetIcon() => $"Images\\icons8_Firework_{Type}_100px.png";
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
