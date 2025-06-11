using UnityEngine;

public class BackgroundLooper : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.5f;
    [SerializeField] private float parallaxFactor = 0.2f;
    [SerializeField] private Transform cameraTransform;

    private float spriteWidth;
    private Transform[] backgrounds;

    void Start()
    {
        // Ambil semua child sprite (3 bg)
        backgrounds = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            backgrounds[i] = transform.GetChild(i);
        }

        // Ambil width dari 1 sprite (asumsi semua sama lebar)
        SpriteRenderer sr = backgrounds[0].GetComponent<SpriteRenderer>();
        spriteWidth = sr.bounds.size.x;
    }

    void Update()
    {
        // Scroll sesuai parallax & kamera
        float offsetX = cameraTransform.position.x * parallaxFactor;
        transform.position = new Vector3(offsetX, transform.position.y, transform.position.z);

        // Looping
        foreach (Transform bg in backgrounds)
        {
            if (cameraTransform.position.x - bg.position.x >= spriteWidth)
            {
                // Geser ke kanan 3 * lebar bg
                float rightMostX = GetRightMostX();
                bg.position = new Vector3(rightMostX + spriteWidth, bg.position.y, bg.position.z);
            }
        }
    }

    float GetRightMostX()
    {
        float maxX = backgrounds[0].position.x;
        foreach (var bg in backgrounds)
        {
            if (bg.position.x > maxX) maxX = bg.position.x;
        }
        return maxX;
    }
}
