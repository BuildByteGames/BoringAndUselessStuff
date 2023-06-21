using System.Collections.Generic;
using UnityEngine;
//By BuildByte 😁

public class RainbowFireworksMaker : MonoBehaviour
{
    [SerializeField] private uint maxCharacterLimit = 32500;
    [SerializeField] private float rainbowMin = 0.3f;
    [SerializeField,Multiline] private string preFirework = "/give @a minecraft:firework_rocket{Item:{id:\"minecraft:firework_rocket\",Count:-1},Enchantments:[{id:\"\",lvl:0s}],HideFlags:33,PickupDelay:32767,Age:-32768,Fireworks:{Flight:0,Explosions:["; 
    [SerializeField,Multiline] private string afterFirework = "]},display:{Name:'{\"text\":\"Bite Display Rocket\",\"color\":\"gray\",\"bold\":true,\"italic\":true}',Lore:['{\"text\":\"Color: \",\"color\":\"gray\",\"bold\":true,\"italic\":false}','{\"text\":\"rainbow\",\"color\":\"rainbow\",\"bold\":true,\"italic\":false,\"obfuscated\":true}','{\"text\":\"Fly Power 1\",\"color\":\"gray\",\"bold\":true,\"italic\":false}',]}} 1";
    
    private void Awake() => Showfirework();
    private void Showfirework()
    {
        List<uint> lightRainbow = RainbowColorsAsDecimal(200,rainbowMin);
        List<uint> rainbow = RainbowColorsAsDecimal(255,rainbowMin);

        List<string> fireworkColors = new List<string>();

        for (int i = 0; i < lightRainbow.Count; i++)
        {
            fireworkColors.Add("{Type:4,Flicker:0,Trail:1,"+$"Colors:[I;{rainbow[i]}],FadeColors:[I;{lightRainbow[i]}]"+"},");
        }
        string fireworkMiddle = string.Join("", fireworkColors);
        string firework = $"{preFirework}{fireworkMiddle}{afterFirework}";

        if(firework.ToCharArray().Length > maxCharacterLimit)
        {
            Debug.LogError($"Too much characters you need to remove {firework.ToCharArray().Length - maxCharacterLimit} characters");
            return;
        }

        MakeTxtFile(firework);
    }

    private static List<uint> RainbowColorsAsDecimal(byte Power = 255, float colorFulness = 1)
    {
        List<uint> decimalColors = new List<uint>();
        foreach (Color32 color in Rainbow(Power,colorFulness))
        {
            var hexRed = color.r.ToString("x"); 
            var hexGreen = color.g.ToString("x"); 
            var hexBlue = color.b.ToString("x"); 

            hexRed = $"{hexRed}0000";
            hexGreen = $"{hexGreen}00";

            uint decimalRed = uint.Parse(hexRed, System.Globalization.NumberStyles.HexNumber);
            uint decimalGreen = uint.Parse(hexGreen, System.Globalization.NumberStyles.HexNumber);
            uint decimalBlue = uint.Parse(hexBlue, System.Globalization.NumberStyles.HexNumber);

            decimalColors.Add((decimalRed + decimalGreen) + decimalBlue);
        }   
        return decimalColors;
    }

    private static List<Color32> Rainbow(byte Power = 255,float colorFullnes = 1)
    {
        List<Color32> rainbowColors = new List<Color32>();
        int ascending = 0;
        int descending = 0;
        for (int j = 0; j < 6; j++)
        {
            for (int i = 0; i < 255 * colorFullnes; i++)
            {
                ascending = i;
                descending = 255 - ascending;
            
                switch (j)
                {
                    case 0:
                        rainbowColors.Add(new Color32(Power, (byte)ascending, 0, 255));
                        break;
                    case 1:
                        rainbowColors.Add(new Color32((byte)descending, Power, 0, 255));
                        break;
                    case 2:
                        rainbowColors.Add(new Color32(0, Power, (byte)ascending, 255));
                        break;
                    case 3:
                        rainbowColors.Add(new Color32(0, (byte)descending, Power, 255));
                        break;
                    case 4:
                        rainbowColors.Add(new Color32((byte)ascending, 0, Power, 255));
                        break;
                    default: // case 5:
                        rainbowColors.Add(new Color32(Power, 0, (byte)descending, 255));
                        break;
                }
            }
        }
        return rainbowColors;
    }

    private void MakeTxtFile(string message)
    {
        string path = Application.dataPath + "/Fireworks_Command.txt";
        System.IO.StreamWriter file = new System.IO.StreamWriter(path);
        file.WriteLine(message);
        file.Close();
    }
}
