using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    public Sprite closedBox;
    public Sprite openBox;
    public GameObject starPrefab;
    public int starForce = 5;

    private bool isTouchingBox = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isTouchingBox)
        {
            Debug.Log("E Pressed");
            this.gameObject.GetComponent<SpriteRenderer>().sprite = openBox;
            //call shootstar every half second
            InvokeRepeating("shootStar", 0.0f, 0.7f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isTouchingBox = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isTouchingBox = false;
    }

    private void shootStar()
    {
        GameObject star = Instantiate(starPrefab, this.transform.position, Quaternion.identity);
        int random = Random.Range(-3, 3);
        star.GetComponent<Rigidbody2D>().AddForce(new Vector2(random, 1* starForce),  ForceMode2D.Impulse);
    }
}
