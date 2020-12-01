
namespace SeppukuStudio
{

    public class PlayerSound : AudioObject
    {
        private void Start()
        {
            GetComponentInParent<PlayerHit>().OnHit += PlayHitSound;
            GetComponentInParent<Player>().OnJump += PlayJumpSound;
            GetComponentInParent<PlayerHit>().OnCharging += PlayChargingSound;

        }

        private void PlayHitSound()
        {
            Play("Hit");
        }

        private void PlayJumpSound()
        {
            Play("Jump");
        }

        private void PlayChargingSound(bool charging)
        {
            if (charging && !IsPlaying("Charging"))
                Play("Charging");
            else if (!charging)
            {
                if (IsPlaying("Charging"))
                    Stop("Charging");

                Play("Swing");
            }
        }

    }
}