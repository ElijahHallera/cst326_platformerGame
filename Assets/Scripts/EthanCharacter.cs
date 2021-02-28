using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class EthanCharacter : MonoBehaviour
{
  private Animator animator;
  public Rigidbody rb;
  public LevelParserStarter level;
  public float modifier = 1;
  public float jumpForce = 1;
  [Range(-2, 2)] public float speed = 0;
  private bool jump = false;
  


  void Awake()
  {
    animator = GetComponent<Animator>();
  }

  void Update()
  {
    float horizontal = Input.GetAxis("Horizontal");
    jump = (Input.GetKeyDown(KeyCode.Space)) ? true : false;

    //horizontal = speed;

    //Set character rotation
    float y = (horizontal < 0) ? -90 : 90;
    Quaternion newRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, y, transform.rotation.eulerAngles.z);
    transform.rotation = newRotation;

    //Set character animation
    animator.SetFloat("Speed", Mathf.Abs(horizontal));

    //move character
    transform.Translate((transform.right * -1) * horizontal * modifier * Time.deltaTime);
    
  }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.name == "Brick(Clone)")
        {
            Destroy(collision.collider.gameObject);
            level.GetComponent<LevelParserStarter>().increaseScore();
        }

        if (collision.collider.gameObject.name == "QuestionBox(Clone)")
        {
            level.GetComponent<LevelParserStarter>().increaseScore();
            level.GetComponent<LevelParserStarter>().increaseCoins();
        }

        if (collision.collider.gameObject.name == "Goal(Clone)")
        {
            level.GetComponent<LevelParserStarter>().increaseScore();
            level.GetComponent<LevelParserStarter>().WinGame();
        }

        if(collision.collider.gameObject.name == "Lava(Clone)")
        {
            level.GetComponent<LevelParserStarter>().QuitGame();
        }
    }

    void FixedUpdate()
  {
    if (jump) rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
  }
}
