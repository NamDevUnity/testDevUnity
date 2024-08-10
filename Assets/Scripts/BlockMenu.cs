using UnityEngine;
using UnityEngine.UI;

public class BlockMenu : MonoBehaviour
{
    public BlockPlacer blockPlacer;
    public Button button1;
    public Button button2;
    public Button button3;
    public int[] blockQuantities = { 10, 10, 10 };
      void Start()
    {
        updateButtonText();
    }

    void updateButtonText()
    {
        button1.GetComponentInChildren<Text>().text = blockQuantities[0].ToString();
        button2.GetComponentInChildren<Text>().text = blockQuantities[1].ToString();
        button3.GetComponentInChildren<Text>().text = blockQuantities[2].ToString();
    }

    public void DecreaseQuantity(int buttonIndex) 
    {
        if (blockQuantities[buttonIndex] > 0)
        {
            blockQuantities[buttonIndex]--;
            updateButtonText();
        }
    }

    public void IncreaseQuantity(int buttonIndex) 
    {
        blockQuantities[buttonIndex]++;
        updateButtonText();
    }

    // Phương thức mới để lấy số lượng khối
    public int GetQuantity(int buttonIndex)
    {
        return blockQuantities[buttonIndex];
    }

    public int[] GetBlockQuantities()
    {
        return blockQuantities;
    }
    public void SetQuantities(int[] quantities)
    {
        if (quantities.Length == blockQuantities.Length)
        {
            blockQuantities = quantities;
            updateButtonText();
        }
        else
        {
            Debug.LogError("Quantities length mismatch.");
        }
    }
    // Phương thức đặt số lượng khối về giá trị mặc định
    public void ResetQuantitiesToDefault()
    {
        blockQuantities = new int[] { 10, 10, 10 }; // Giá trị mặc định
        updateButtonText();
    }

    public void OnButtonBlock1Click()
    {
        FindObjectOfType<BlockPlacer>().selectBlock(0);
    }

    public void OnButtonBlock2Click()
    {
        FindObjectOfType<BlockPlacer>().selectBlock(1);
    }

    public void OnButtonBlock3Click()
    {
        FindObjectOfType<BlockPlacer>().selectBlock(2);
    }
}
