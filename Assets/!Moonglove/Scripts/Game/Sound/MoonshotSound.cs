using System.Collections;
using UnityEngine;
namespace SeppukuStudio
{
    public class MoonshotSound : AudioObject
    {
        private void Start()
        {
            MoonshotManager.Instance.OnEndRound += PlayFailSound;
            MoonshotManager.Instance.OnFlagTaken += PlayFlagPickUp;
            MoonshotManager.Instance.OnLevelCompleted += PlayWin;

            StartCoroutine(MusicRoutine());
        }

        private IEnumerator MusicRoutine()
        {
            Play("Start");
            yield return new WaitForSeconds(GetLength("Start"));

            int music = GameManager.instance.LevelIndex;
            if (music == 0)
                Play("Music1");
            else if (music == 1 || music == 3)
                Play("Music2");
            else
                Play("Music3");
        }

        private void PlayFailSound(float resetRoundTime)
        {
            Play("Fail");
        }

        private void PlayFlagPickUp(int flag)
        {
            Play("FlagPickUp");
        }

        private void PlayWin()
        {
            Play("Win");

        }
    }
}