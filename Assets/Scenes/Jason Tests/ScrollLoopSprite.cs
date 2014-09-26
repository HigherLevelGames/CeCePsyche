using UnityEngine;
using System.Collections;

public class ScrollLoopSprite : MonoBehaviour
{
		public SpriteRenderer Render1, Render2;
		public float Speed = 0.3f;
		float offsetx1, offsetx2;
		float reset;

		void Start ()
		{
				if (Speed > 0) {
						offsetx2 = -0.5f;
						reset = 0.5f;

				} else {
						offsetx2 = 0.5f;
						reset = -0.5f;
				}
		}

		void Update ()
		{
				offsetx1 += Time.deltaTime * Speed;
				offsetx2 += Time.deltaTime * Speed;
				if (Speed > 0) {
						reset = 0.5f;
						if (offsetx2 > reset)
								offsetx2 -= reset * 2;
						if (offsetx1 > reset)
								offsetx1 -= reset * 2;
				} else {
						reset = -0.5f;
						if (offsetx2 < reset)
							offsetx2 -= reset * 2;
						if (offsetx1 < reset)
							offsetx1 -= reset * 2;
				}
				Render1.material.mainTextureOffset = new Vector2 (offsetx1, 0);
				Render2.material.mainTextureOffset = new Vector2 (offsetx2, 0);
		}
}