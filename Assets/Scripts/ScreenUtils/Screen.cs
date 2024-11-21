using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Screen : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private Image targetImage;

        private Texture2D imageTexture;

        private void Start()
        {
            if (targetImage != null && targetImage.sprite != null && targetImage.sprite.texture != null)
            {
                imageTexture = targetImage.sprite.texture;
            }
        }

        public Color GetPixel()
        {
            if (imageTexture == null)
            {
                return Color.black;
            }

            Vector2 localCursor;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                targetImage.rectTransform,
                Input.mousePosition,
                null,
                out localCursor
            );

            Rect rect = targetImage.rectTransform.rect;
            float u = (localCursor.x - rect.x) / rect.width;
            float v = (localCursor.y - rect.y) / rect.height;

            if (u >= 0 && u <= 1 && v >= 0 && v <= 1)
            {
                int x = Mathf.FloorToInt(u * imageTexture.width);
                int y = Mathf.FloorToInt(v * imageTexture.height);

                Color pixelColor = imageTexture.GetPixel(x, y);
                return pixelColor;
            }
            else
            {
                return Color.black;
            }
        }
    }
}
