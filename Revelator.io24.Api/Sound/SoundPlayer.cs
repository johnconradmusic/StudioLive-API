using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Sound
{
    public class SoundPlayer
    {
        public static void PlaySound(string file)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(file); 
            player.Play();
        }
    }
}
