using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public BlockMenu blockMenu;
    public BlockPlacer blockPlacer;
    private string saveFilePath;

    void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "savefile.json");
        LoadGameState();
    }

    void Update()
    {
        // Kiểm tra nếu phím "ESC" được nhấn
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SaveGameState();
            Debug.Log("Exiting game.");
            Application.Quit(); // Thoát ứng dụng
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Dùng trong trình biên tập Unity
#endif
        }
    }

    public void SaveGameState()
    {
        if (blockMenu == null || blockPlacer == null)
        {
            Debug.LogError("BlockMenu or BlockPlacer is not assigned.");
            return;
        }

        GameData gameData = new GameData
        {
            blockQuantities = blockMenu.GetBlockQuantities()
        };

        // Lưu thông tin khối đã đặt
        foreach (Transform child in blockPlacer.transform)
        {
            BlockData blockData = new BlockData
            {
                position = child.position,
                rotation = child.rotation,
                type = GetBlockType(child.gameObject)
            };
            gameData.blocks.Add(blockData);
        }

        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game state saved.");
    }

    public void LoadGameState()
    {
        if (blockMenu == null || blockPlacer == null)
        {
            Debug.LogError("BlockMenu or BlockPlacer is not assigned.");
            return;
        }

        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            GameData gameData = JsonUtility.FromJson<GameData>(json);

            // Tải số lượng khối
            blockMenu.SetQuantities(gameData.blockQuantities);

            // Xóa các khối hiện tại trong scene
            foreach (Transform child in blockPlacer.transform)
            {
                Destroy(child.gameObject);
            }

            // Tạo lại các khối từ dữ liệu lưu
            foreach (BlockData blockData in gameData.blocks)
            {
                GameObject blockPrefab = blockPlacer.GetBlockPrefab(blockData.type);
                if (blockPrefab != null)
                {
                    Instantiate(blockPrefab, blockData.position, blockData.rotation, blockPlacer.transform);
                }
                else
                {
                    Debug.LogError("Block prefab not found for type: " + blockData.type);
                }
            }

            Debug.Log("Game state loaded.");
        }
        else
        {
            // Không có dữ liệu lưu, đặt số lượng khối về giá trị mặc định
            blockMenu.ResetQuantitiesToDefault();
            Debug.LogWarning("Save file does not exist. Setting block quantities to default.");
        }
    }

    int GetBlockType(GameObject blockPrefab)
    {
        for (int i = 0; i < blockPlacer.blockPrefabs.Length; i++)
        {
            if (blockPlacer.blockPrefabs[i] == blockPrefab)
            {
                return i;
            }
        }
        return -1;
    }

    string GetBlockTag(int type)
    {
        switch (type)
        {
            case 0: return "Block1";
            case 1: return "Block2";
            case 2: return "Block3";
            default: return "Untagged";
        }
    }
}
