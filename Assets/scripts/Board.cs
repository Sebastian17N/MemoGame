using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Board : MonoBehaviour
{
    public GameObject TilePrefabs;

    public Sprite[] sprites;
    Tile[] Tiles;

    public Vector2 TilesOffset = Vector2.one;
    public int Width = 6;
    public int Height = 4;

    public bool CanMove = false;

    public TextMesh WinText;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        WinText.GetComponent<Renderer>().enabled = false;
        CreateTiles();
        ShuffleTiles();
        PlaceTiles();
        CanMove = false;
        yield return new WaitForSeconds(2f);
        CanMove = true;
        HideTiles();
        
    }
    void CreateTiles()
    {
        var lenght = Width * Height;
        Tiles = new Tile[lenght];
        for (int i = 0; i < lenght; i++)
        {
            var sprite = sprites[i / 2];
            Tiles[i] = CreateTile(sprite);
        }
       
    }   
    Tile CreateTile(Sprite faceSprite)
    {
        var gameobject = Instantiate(TilePrefabs);
        gameobject.transform.parent = transform;

        var tile = gameobject.GetComponent<Tile>();
        tile.Uncovered = true;
        tile.frontFace = faceSprite;

        return tile;
    }

    void ShuffleTiles()
    {
        for (int i = 0; i < 1000; i++)
        {
            int index1 = Random.Range(0, Tiles.Length);
            int index2 = Random.Range(0, Tiles.Length);

            var tile1 = Tiles[index1];
            var tile2 = Tiles[index2];

            Tiles[index1] = tile2;
            Tiles[index2] = tile1;
        }
    }

    void PlaceTiles()
    {
        for (int i = 0; i < Width * Height; i++)
        {
            int x = i % Width;
            int y = i / Width;

            Tiles[i].transform.localPosition = new Vector3(x*TilesOffset.x, y*TilesOffset.y,0);
        }
    }
   
    void HideTiles()
    {
        Tiles.ToList().ForEach(tile => tile.Uncovered = false);
    }
    bool CheckIfEnd()
    {
        return Tiles.All(tile => tile.Active == false);
    }
    public void CheckPair()
    {
        StartCoroutine(CheckPairCoroutine());
    }

    IEnumerator CheckPairCoroutine()
    {
        var tilesUncovered = Tiles
            .Where(tile => tile.Active)
            .Where(tile => tile.Uncovered)
            .ToArray();

        if (tilesUncovered.Length != 2)
            yield break;

        var tile1 = tilesUncovered[0];
        var tile2 = tilesUncovered[1];

        CanMove = false;
        yield return new WaitForSeconds(0.5f);
        CanMove = true;

        if (tile1.frontFace == tile2.frontFace)
        {
            tile1.Active = false;
            tile2.Active = false;
        }
        else
        {
            tile1.Uncovered = false;
            tile2.Uncovered = false;
        }
        if (CheckIfEnd())
        {
            CanMove = true;
            WinText.GetComponent<Renderer>().enabled = true;
            yield return new WaitForSeconds(3f);
            Application.Quit();
        }
    }
        
}
