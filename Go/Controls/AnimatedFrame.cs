using Xamarin.Forms;

namespace Go.Controls
{
    public class AnimatedFrame : Frame
    {
        public AnimatedFrame()
        {
            new Animation
            (
                callback: e => {

                    if (Opacity == 1)
                    {
                        this.FadeTo(0.4);
                    }

                    if (Opacity <= 0.4)
                    {
                        this.FadeTo(1);
                    }
                }
            ).Commit(this, "Fade", length: 500, repeat: () => true);
        }
    }
}