using System.Drawing;
using System.Xml.Linq;
using System;

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

        public Firework(string Json)
        {
            Firework input = Newtonsoft.Json.JsonConvert.DeserializeObject<Firework>(Json);
            ID = input.ID;
            FireworkName = input.FireworkName;
            Type = input.Type;
            Colour = input.Colour;
            Delay = input.Delay;
            Length = input.Length;
        }
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
        public string GetImage() => $"Images\\icons8_Firework_{Type}_100px.png";
    }

    /// <summary>
    /// https://www.fireworkshawkesbay.co.nz/types-of-fireworks#
    /// </summary>
    public enum FireworkType
    {
        Sparkler,
        Fountain,
        Smoke_Ball,
        Missile,
        Mine,
        Cake,
        Roman_Candle
    }
}
