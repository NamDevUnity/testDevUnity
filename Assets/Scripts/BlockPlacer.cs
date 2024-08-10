using UnityEngine;
using UnityEngine.EventSystems;

public class BlockPlacer : MonoBehaviour
{
    public GameObject[] blockPrefabs;
    private int selectedBlockIndex = -1;

    public AudioClip placeBlockSound;
    public AudioClip gatherBlockSound;
    private AudioSource audioSource;

    void Start()
    {
        // Kiểm tra nếu AudioSource không tồn tại, tạo mới và thêm vào đối tượng này
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Kiểm tra nếu chuột không trỏ vào UI
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                PlaceBlock();
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            GatherBlock();
        }
    }

    void PlaceBlock()
    {
        if (selectedBlockIndex != -1 && FindObjectOfType<BlockMenu>().GetQuantity(selectedBlockIndex) > 0)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 gridPosition = new Vector2(Mathf.Floor(mousePosition.x) + 0.5f, Mathf.Floor(mousePosition.y) + 0.5f);

            Collider2D[] colliders = Physics2D.OverlapBoxAll(gridPosition, new Vector2(0.9f, 0.9f), 0);
            if (colliders.Length == 0)
            {
                Instantiate(blockPrefabs[selectedBlockIndex], gridPosition, Quaternion.identity);
                FindObjectOfType<BlockMenu>().DecreaseQuantity(selectedBlockIndex);

                // Phát âm thanh khi đặt khối
                if (placeBlockSound != null)
                {
                    audioSource.PlayOneShot(placeBlockSound);
                }
            }
        }
    }

    void GatherBlock()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            string blockTag = hit.collider.tag;

            // Xác định loại khối dựa trên tag
            int blockType = -1;
            switch (blockTag)
            {
                case "Block1":
                    blockType = 0;
                    break;
                case "Block2":
                    blockType = 1;
                    break;
                case "Block3":
                    blockType = 2;
                    break;
            }

            if (blockType != -1 && blockType < blockPrefabs.Length)
            {
                Destroy(hit.collider.gameObject);
                FindObjectOfType<BlockMenu>().IncreaseQuantity(blockType);

                // Phát âm thanh khi thu thập khối
                if (gatherBlockSound != null)
                {
                    audioSource.PlayOneShot(gatherBlockSound);
                }
            }
            else
            {
                Debug.Log("Invalid block tag: " + blockTag);
            }
        }
        else
        {
            Debug.Log("No block detected under mouse.");
        }
    }

    public GameObject GetBlockPrefab(int type)
    {
        if (type >= 0 && type < blockPrefabs.Length)
        {
            return blockPrefabs[type];
        }
        Debug.LogError("Invalid block type: " + type);
        return null;
    }

    public void selectBlock(int blockIndex)
    {
        selectedBlockIndex = blockIndex;
    }

}
