using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
  ┎━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┒
  ┃   Dedication Focus Discipline   ┃
  ┃        Practice more !!!        ┃
  ┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
*/
namespace Utils
{
    public class AutoPlaySpriteAnimation : MonoBehaviour
    {
        public List<Sprite> Sprites;
        [Range(0, 0.1f)] public float IntervalTime = 0.03f;

        private SpriteRenderer _render;
        private void Start()
        {
            _render = GetComponent<SpriteRenderer>();
            StartCoroutine(PlayAniamtion());
        }

        public IEnumerator PlayAniamtion()
        {
            int i = 0;
            while (i < Sprites.Count)
            {
                _render.sprite = Sprites[i++];
                if (i == Sprites.Count)
                {
                    i = 0;
                }
                yield return new WaitForSeconds(IntervalTime);
            }

        }

    }
}