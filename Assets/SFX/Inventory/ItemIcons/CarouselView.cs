using UnityEngine;

public class CarouselView : MonoBehaviour
{
    public RectTransform[] images;
    public RectTransform ViewWindow;

    private float image_width;
    private float lerpTimer;
    private float lerpPosition;
    private float screenPosition;
    private float lastScreenPosition;

    public float ImageGap = 30;

    public int m_currentIndex;

    public int CurrentIndex { get { return m_currentIndex; } }

    public RectTransform CurrentItem;
    public CraftItemIcon CraftItemIcon;

    void Start()
    {
        image_width = ViewWindow.rect.width;
        CurrentItem = images[0];
        CraftItemIcon = CurrentItem.GetComponent<CraftItemIcon>();

        for (int i = 1; i < images.Length; i++)
        {
            images[i].anchoredPosition = new Vector2(((image_width + ImageGap) * i), 0);
        }
    }

    void Update()
    {
        UpdateCarouselView();
        CraftItemIcon = CurrentItem.GetComponent<CraftItemIcon>();
    }

    void UpdateCarouselView()
    {
        lerpTimer = lerpTimer + Time.deltaTime;

        if (lerpTimer < 0.333f)
        {
            screenPosition = Mathf.Lerp(lastScreenPosition, lerpPosition * -1, lerpTimer * 3);
            lastScreenPosition = screenPosition;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            OnSwipeComplete(false);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            OnSwipeComplete(true);
        }

        for (int i = 0; i < images.Length; i++)
        {
            images[i].anchoredPosition = new Vector2(screenPosition + ((image_width + ImageGap) * i), 0);
            if (i == m_currentIndex)
            {
                images[i].localScale = Vector3.Lerp(images[i].localScale, new Vector3(1.2f, 1.2f, 1.2f), Time.deltaTime * 5);
            }
            else
            {
                images[i].localScale = Vector3.Lerp(images[i].localScale, new Vector3(0.7f, 0.7f, 0.7f), Time.deltaTime * 5);
            }
        }
    }

    void OnSwipeComplete(bool swipeRight)
    {
        if (swipeRight)
        {
            if (m_currentIndex < images.Length - 1)
            {
                m_currentIndex++;
            }
            else
            {
                m_currentIndex = 0;
            }
        }
        else
        {
            if (m_currentIndex > 0)
            {
                m_currentIndex--;
            }
            else
            {
                m_currentIndex = images.Length - 1;
            }
        }

        lerpTimer = 0;
        lerpPosition = (image_width + ImageGap) * m_currentIndex;
        screenPosition = lerpPosition * -1;
        lastScreenPosition = screenPosition;
        CurrentItem = images[m_currentIndex];

        for (int i = 0; i < images.Length; i++)
        {
            images[i].anchoredPosition = new Vector2(screenPosition + ((image_width + ImageGap) * i), 0);
            if (i == m_currentIndex)
            {
                images[i].localScale = Vector3.Lerp(images[i].localScale, new Vector3(1.2f, 1.2f, 1.2f), Time.deltaTime * 5);
            }
            else
            {
                images[i].localScale = Vector3.Lerp(images[i].localScale, new Vector3(0.7f, 0.7f, 0.7f), Time.deltaTime * 5);
            }
        }
    }

    #region public methods
    public void MoveLeft()
    {
        if (m_currentIndex > 0)
        {
            m_currentIndex--;
        }
        else
        {
            m_currentIndex = images.Length - 1;
        }

        lerpTimer = 0;
        lerpPosition = (image_width + ImageGap) * m_currentIndex;
        screenPosition = lerpPosition * -1;
        lastScreenPosition = screenPosition;
    }

    public void MoveRight()
    {
        if (m_currentIndex < images.Length - 1)
        {
            m_currentIndex++;
        }
        else
        {
            m_currentIndex = 0;
        }

        lerpTimer = 0;
        lerpPosition = (image_width + ImageGap) * m_currentIndex;
        screenPosition = lerpPosition * -1;
        lastScreenPosition = screenPosition;
    }
    #endregion
}
