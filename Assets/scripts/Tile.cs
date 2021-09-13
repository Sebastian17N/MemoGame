using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool Uncovered = true;
    public bool Active = true;
    public Sprite frontFace;
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = GetTargetRotation();
        var frontObject = transform.Find("front");
        var spriteRenderer = frontObject.transform.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = frontFace;
    }

    // Update is called once per frame
    void Update()
    {
        var targetRotation = GetTargetRotation();
        transform.rotation = Quaternion.Lerp(
            transform.rotation, 
            targetRotation, 
            Time.deltaTime*5f);

        if (Active == false)
            Destroy(gameObject);
    }
    Quaternion GetTargetRotation()
    {
        var rotation = Uncovered ? Vector3.zero : (Vector3.up * 180f);
        return Quaternion.Euler(rotation);
    }

    private void OnMouseDown()
    {
        var board = FindObjectOfType<Board>();
        if (board.CanMove == false)
            return;

        Uncovered = !Uncovered;
        board.CheckPair();
    }
}
